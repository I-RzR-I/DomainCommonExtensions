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

// ReSharper disable CheckNamespace

#region U S A G E S

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using DomainCommonExtensions.DataTypeExtensions;
using DomainCommonExtensions.Utilities.Ensure;

#endregion

namespace DomainCommonExtensions.CommonExtensions
{
    public static partial class CryptoExtensions
    {
        /// <summary>
        ///     Encrypt a string. Encoding is done using AES encryption.
        /// </summary>
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