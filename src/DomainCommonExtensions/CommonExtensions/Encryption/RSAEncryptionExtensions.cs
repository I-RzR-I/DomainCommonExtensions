// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2023-09-18 17:52
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-09-18 17:52
// ***********************************************************************
//  <copyright file="RSAEcryptionExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using DomainCommonExtensions.DataTypeExtensions;

// ReSharper disable CheckNamespace

namespace DomainCommonExtensions.CommonExtensions
{
    public static partial class CryptoExtensions
    {
        /// <summary>
        ///     Encrypt a string using the supplied key. Encoding is done using RSA encryption.
        /// </summary>
        /// <param name="stringToEncrypt">String that must be encrypted.</param>
        /// <param name="key">Encryption key.</param>
        /// <returns>A string representing a byte array separated by a minus sign.</returns>
        /// <exception cref="ArgumentException">Occurs when stringToEncrypt or key is null or empty.</exception>
        // ReSharper disable once InconsistentNaming
        public static string EncryptWithRSA(this string stringToEncrypt, string key)
        {
            stringToEncrypt.ThrowArgIfNullOrEmpty("An empty string value cannot be encrypted.");
            key.ThrowArgIfNullOrEmpty("Cannot encrypt using an empty key. Please supply an encryption key.");

            var cspp = new CspParameters { KeyContainerName = key };
            var rsa = new RSACryptoServiceProvider(cspp) { PersistKeyInCsp = true };
            var bytes = rsa.Encrypt(Encoding.UTF8.GetBytes(stringToEncrypt), true);
            rsa.Dispose();

            return BitConverter.ToString(bytes);
        }

        /// <summary>
        ///     Decrypt a string using the supplied key. Decoding is done using RSA encryption.
        /// </summary>
        /// <param name="stringToDecrypt">String that must be decrypted.</param>
        /// <param name="key">Decryption key.</param>
        /// <returns>The decrypted string or null if decryption failed.</returns>
        /// <exception cref="ArgumentException">Occurs when stringToDecrypt or key is null or empty.</exception>
        // ReSharper disable once InconsistentNaming
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

                result = Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                // no need for further processing
            }

            return result;
        }
    }
}