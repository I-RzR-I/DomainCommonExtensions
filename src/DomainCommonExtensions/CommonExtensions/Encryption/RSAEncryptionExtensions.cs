// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2023-09-18 17:52
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-09-18 17:52
// ***********************************************************************
//  <copyright file="RSAEncryptionExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using DomainCommonExtensions.DataTypeExtensions;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace DomainCommonExtensions.CommonExtensions
{
    /// -------------------------------------------------------------------------------------------------
    /// <content>
    ///     A crypto extensions.
    /// </content>
    /// =================================================================================================
    public static partial class CryptoExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Encrypt a string using the supplied key. Encoding is done using RSA encryption.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Occurs when stringToEncrypt or key is null or empty.
        /// </exception>
        /// <param name="stringToEncrypt">String that must be encrypted.</param>
        /// <param name="key">Encryption key.</param>
        /// <returns>
        ///     A string representing a byte array separated by a minus sign.
        /// </returns>
        /// =================================================================================================
        public static string EncryptWithRSA(this string stringToEncrypt, string key)
        {
            stringToEncrypt.ThrowArgIfNullOrEmpty("An empty string value cannot be encrypted.");
            key.ThrowArgIfNullOrEmpty("Cannot encrypt using an empty key. Please supply an encryption key.");

            var cspp = new CspParameters { KeyContainerName = key };
            var rsa = new RSACryptoServiceProvider(cspp) { PersistKeyInCsp = true };
            var bytes = rsa.Encrypt(Encoding.Unicode.GetBytes(stringToEncrypt), true);
            rsa.Dispose();

            return BitConverter.ToString(bytes);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Decrypt a string using the supplied key. Decoding is done using RSA encryption.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Occurs when stringToDecrypt or key is null or empty.
        /// </exception>
        /// <param name="stringToDecrypt">String that must be decrypted.</param>
        /// <param name="key">Decryption key.</param>
        /// <returns>
        ///     The decrypted string or null if decryption failed.
        /// </returns>
        /// =================================================================================================
        public static string DecryptWithRSA(this string stringToDecrypt, string key)
        {
            string result = null;
            stringToDecrypt.ThrowArgIfNullOrEmpty("An empty string value cannot be decrypted.");
            key.ThrowArgIfNullOrEmpty("Cannot decrypt using an empty key. Please supply a decryption key.");

            try
            {
                var cspp = new CspParameters { KeyContainerName = key };
                var rsa = new RSACryptoServiceProvider(cspp) { PersistKeyInCsp = true };
                var decryptArray = stringToDecrypt.Split(new[] { "-" }, StringSplitOptions.None);
                var decryptByteArray = Array.ConvertAll(decryptArray, s => Convert.ToByte(byte.Parse(s,
                    NumberStyles.HexNumber)));
                var bytes = rsa.Decrypt(decryptByteArray, true);
                rsa.Dispose();

                result = Encoding.Unicode.GetString(bytes);
            }
            catch
            {
                // no need for further processing
            }

            return result;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Encrypt a string using the supplied key. Encoding is done using RSA encryption.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Occurs when stringToEncrypt or key is null or empty.
        /// </exception>
        /// <param name="stringToEncrypt">String that must be encrypted.</param>
        /// <param name="keySize">Decryption key size.</param>
        /// <param name="publicKey">Encryption key (public key).</param>
        /// <param name="isKeyBase64Encoded">
        ///     (Optional) Indicate if the public key is XML encoded to BASE64 string or not.
        /// </param>
        /// <returns>
        ///     The encrypted string or null if decryption failed.
        /// </returns>
        /// =================================================================================================
        public static string EncryptRSAXmlKey(this string stringToEncrypt, int keySize, string publicKey, bool isKeyBase64Encoded = false)
        {
            stringToEncrypt.ThrowArgIfNullOrEmpty("An empty string value cannot be encrypted.");
            publicKey.ThrowArgIfNullOrEmpty("Cannot encrypt using an empty key. Please supply an encryption key.");


            using var rsa = new RSACryptoServiceProvider(keySize);
            rsa.FromXmlString(isKeyBase64Encoded.IsTrue() ? publicKey.Base64Decode() : publicKey);
            var encryptedData = rsa.Encrypt(Encoding.Unicode.GetBytes(stringToEncrypt), true);

            return encryptedData.ToBase64String();
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Decrypt a string using the supplied key. Decoding is done using RSA encryption.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Occurs when stringToDecrypt or key is null or empty.
        /// </exception>
        /// <param name="stringToDecrypt">String that must be decrypted.</param>
        /// <param name="keySize">Decryption key size.</param>
        /// <param name="privateKey">Decryption key (private key).</param>
        /// <param name="isKeyBase64Encoded">
        ///     (Optional) Indicate if the private key is XML encoded to BASE64 string or not.
        /// </param>
        /// <returns>
        ///     The decrypted string or null if decryption failed.
        /// </returns>
        /// =================================================================================================
        public static string DecryptRSAXmlKey(this string stringToDecrypt, int keySize, string privateKey, bool isKeyBase64Encoded = false)
        {
            stringToDecrypt.ThrowArgIfNullOrEmpty("An empty string value cannot be decrypted.");
            privateKey.ThrowArgIfNullOrEmpty("Cannot decrypt using an empty key. Please supply a decryption key.");

            try
            {
                using var rsa = new RSACryptoServiceProvider(keySize);
                rsa.FromXmlString(isKeyBase64Encoded.IsTrue() ? privateKey.Base64Decode() : privateKey);
                var decryptedData = rsa.Decrypt(stringToDecrypt.DeCodeBytesFromBase64(), true);

                return Encoding.Unicode.GetString(decryptedData);
            }
            catch
            {
                return null;
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A string extension method that encrypts a rsa certificate.
        /// </summary>
        /// <param name="stringToEncrypt">String that must be encrypted.</param>
        /// <param name="publicKey">Encryption key (public key).</param>
        /// <returns>
        ///     The encrypted string or null if decryption failed.
        /// </returns>
        /// =================================================================================================
        public static string EncryptRSACertificate(this string stringToEncrypt, RSACryptoServiceProvider publicKey)
        {
            try
            {
                if (stringToEncrypt.IsMissing() || publicKey.IsNull())
                    return null;

                var byteData = Encoding.Unicode.GetBytes(stringToEncrypt);
                var encrypted = publicKey.Encrypt(byteData, false);

                return Convert.ToBase64String(encrypted);
            }
            catch
            {
                return null;
            }

        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A string extension method that decrypt rsa certificate.
        /// </summary>
        /// <param name="stringToDecrypt">String that must be decrypted.</param>
        /// <param name="privateKey">Decryption key (private key).</param>
        /// <returns>
        ///     The decrypted string or null if decryption failed.
        /// </returns>
        /// =================================================================================================
        public static string DecryptRSACertificate(this string stringToDecrypt, RSACryptoServiceProvider privateKey)
        {
            try
            {
                if (stringToDecrypt.IsMissing() || privateKey.IsNull())
                    return null;

                var encrypted = Convert.FromBase64String(stringToDecrypt);
                var decrypted = privateKey.Decrypt(encrypted, false);

                return Encoding.Unicode.GetString(decrypted);
            }
            catch
            {
                return null;
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A string extension method that encrypts a rsa certificate.
        /// </summary>
        /// <param name="stringToEncrypt">String that must be encrypted.</param>
        /// <param name="certificate">The certificate.</param>
        /// <returns>
        ///     The encrypted string or null if decryption failed.
        /// </returns>
        /// =================================================================================================
        public static string EncryptRSACertificate(this string stringToEncrypt, X509Certificate2 certificate)
        {
            try
            {
                if (stringToEncrypt.IsMissing() || certificate.IsNull())
                    return null;

                var byteData = Encoding.Unicode.GetBytes(stringToEncrypt);
                var rsaKeyProvider = (RSACryptoServiceProvider)certificate.PublicKey.Key;
                var encrypted = rsaKeyProvider.Encrypt(byteData, false);

                return Convert.ToBase64String(encrypted);
            }
            catch
            {
                return null;
            }

        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A string extension method that decrypt rsa certificate.
        /// </summary>
        /// <param name="stringToDecrypt">String that must be decrypted.</param>
        /// <param name="certificate">The certificate.</param>
        /// <returns>
        ///     The decrypted string or null if decryption failed.
        /// </returns>
        /// =================================================================================================
        public static string DecryptRSACertificate(this string stringToDecrypt, X509Certificate2 certificate)
        {
            try
            {
                if (stringToDecrypt.IsMissing() || certificate.IsNull())
                    return null;

                var encrypted = Convert.FromBase64String(stringToDecrypt);
                var rsaKeyProvider = (RSACryptoServiceProvider)certificate.PrivateKey;
                var decrypted = rsaKeyProvider.Decrypt(encrypted, false);

                return Encoding.Unicode.GetString(decrypted);
            }
            catch
            {
                return null;
            }
        }
    }
}