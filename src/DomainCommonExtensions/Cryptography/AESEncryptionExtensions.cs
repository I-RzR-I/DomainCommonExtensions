// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2023-09-18 17:54
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-09-18 18:06
// ***********************************************************************
//  <copyright file="AESEncryptionExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using RzR.Extensions.Domain.Primitives;
using RzR.Extensions.Domain.Validation;

// ReSharper disable CheckNamespace

#endregion

namespace RzR.Extensions.Domain.Cryptography
{
    public static partial class CryptoExtensions
    {
        /// <summary>
        ///     Encrypt a string. Encoding is done using AES encryption.
        /// </summary>
        /// <remarks>
        ///     AES-CBC with PKCS7 padding over the UTF-8 plain text; the output is Base64 of the ciphertext only (the IV
        ///     is not included).
        ///     When isLongStringKey is false the key is the raw UTF-8 bytes of <paramref name="key"/>, which must be
        ///     exactly 16, 24 or 32 bytes (AES-128/192/256), otherwise a CryptographicException is thrown.
        ///     When isLongStringKey is true the key is an unsalted, single-pass MD5 hash of the UTF-8 key (16 bytes), so
        ///     the cipher is always AES-128 and <paramref name="keySize"/> = 256 has no effect. This is not a password
        ///     key derivation function.
        ///     In both cases the effective key size comes from the key bytes, not from <paramref name="keySize"/>;
        ///     keySize must still be 128, 192 or 256 and blockSize must be 128, otherwise a CryptographicException is thrown.
        ///     The caller supplies the IV: reusing an IV with the same key reveals when two messages share a prefix.
        ///     Use a new random 16-byte IV for every message and store it with the ciphertext.
        ///     There is no authentication (MAC): ciphertext can be altered undetected, and if decryption errors are
        ///     observable by an attacker the scheme is open to padding-oracle attacks.
        ///     Not suitable for password storage or new designs; prefer authenticated encryption (e.g. AES-GCM).
        /// </remarks>
        /// <param name="plainText">Plaint text to be encrypted.</param>
        /// <param name="key">Encryption key.</param>
        /// <param name="iv">Initialization vector.</param>
        /// <param name="isLongStringKey">Indicate if is provided by user long key or not. Default value = false.</param>
        /// <param name="keySize">Key size. Default value = 256.</param>
        /// <param name="blockSize">Block size. Default value = 128.</param>
        /// <returns></returns>
        public static string AesEncryptString(this string plainText, string key, byte[] iv, bool isLongStringKey = false, int keySize = 256, int blockSize = 128)
        {
            key.ThrowIfArgNullOrEmpty(nameof(key));
            plainText.ThrowIfArgNull(nameof(plainText));
            iv.ThrowIfArgNull(nameof(iv));

            byte[] array;
            using (var aes = Aes.Create())
            {
                aes.KeySize = keySize;
                aes.BlockSize = blockSize;
                aes.Padding = PaddingMode.PKCS7;
                aes.Mode = CipherMode.CBC;
                aes.Key = isLongStringKey.IsTrue() ? GetHashByteKey(key) : Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (var streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        /// <summary>
        ///     Decrypt a string. Decoding is done using AES encryption.
        /// </summary>
        /// <remarks>
        ///     Reverses <see cref="AesEncryptString"/>: AES-CBC with PKCS7 padding, UTF-8 plain text, and the same key
        ///     handling (raw UTF-8 key of 16, 24 or 32 bytes, or an unsalted MD5 hash of the key, i.e. always AES-128,
        ///     when isLongStringKey is true). The IV must be the one used for encryption.
        ///     There is no authentication (MAC): tampered ciphertext is not detected, and a CryptographicException is
        ///     thrown on bad padding. Do not expose decryption errors to untrusted callers (padding-oracle risk).
        ///     Not suitable for password storage or new designs; prefer authenticated encryption (e.g. AES-GCM).
        /// </remarks>
        /// <param name="cipherText">Encrypted text to be decrypted</param>
        /// <param name="key">Decryption key.</param>
        /// <param name="iv">Initialization vector.</param>
        /// <param name="isLongStringKey">Indicate if is provided by user long key or not. Default value = false.</param>
        /// <param name="keySize">Key size. Default value = 256.</param>
        /// <param name="blockSize">Block size. Default value = 128.</param>
        /// <returns></returns>
        public static string AesDecryptString(this string cipherText, string key, byte[] iv, bool isLongStringKey = false, int keySize = 256, int blockSize = 128)
        {
            key.ThrowIfArgNullOrEmpty(nameof(key));
            cipherText.ThrowIfArgNullOrEmpty(nameof(cipherText));
            iv.ThrowIfArgNull(nameof(iv));

            var buffer = Convert.FromBase64String(cipherText);
            using (var aes = Aes.Create())
            {
                aes.KeySize = keySize;
                aes.BlockSize = blockSize;
                aes.Padding = PaddingMode.PKCS7;
                aes.Mode = CipherMode.CBC;
                aes.Key = isLongStringKey.IsTrue() ? GetHashByteKey(key) : Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using (var memoryStream = new MemoryStream(buffer))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (var streamReader = new StreamReader(cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Get AES key byte from user key hash.
        /// </summary>
        /// <param name="userKey">User custom long key.</param>
        /// <returns></returns>
        private static byte[] GetHashByteKey(string userKey)
        {
            var keyBytes = Encoding.UTF8.GetBytes(userKey);
            using var md5 = MD5.Create();

            return md5.ComputeHash(keyBytes);
        }
    }
}
