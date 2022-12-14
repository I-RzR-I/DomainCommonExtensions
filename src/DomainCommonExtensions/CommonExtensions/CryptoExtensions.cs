// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-08-10 21:39
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-12 23:39
// ***********************************************************************
//  <copyright file="CryptoExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

#endregion

namespace DomainCommonExtensions.CommonExtensions
{
    /// <summary>
    ///     String crypto extensions
    /// </summary>
    /// <remarks></remarks>
    public static class CryptoExtensions
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
            if (string.IsNullOrEmpty(stringToEncrypt))
                throw new ArgumentException("An empty string value cannot be encrypted.");

            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Cannot encrypt using an empty key. Please supply an encryption key.");

            var cspp = new CspParameters
            {
                KeyContainerName = key
            };

            var rsa = new RSACryptoServiceProvider(cspp) { PersistKeyInCsp = true };

            var bytes = rsa.Encrypt(Encoding.UTF8.GetBytes(stringToEncrypt), true);

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

            if (string.IsNullOrEmpty(stringToDecrypt))
                throw new ArgumentException("An empty string value cannot be encrypted.");

            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Cannot decrypt using an empty key. Please supply a decryption key.");

            try
            {
                var cspp = new CspParameters { KeyContainerName = key };

                var rsa = new RSACryptoServiceProvider(cspp) { PersistKeyInCsp = true };

                var decryptArray = stringToDecrypt.Split(new[] { "-" }, StringSplitOptions.None);
                var decryptByteArray = Array.ConvertAll(decryptArray, s => Convert.ToByte(byte.Parse(s,
                    NumberStyles.HexNumber)));

                var bytes = rsa.Decrypt(decryptByteArray, true);

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