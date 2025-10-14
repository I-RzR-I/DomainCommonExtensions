// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2025-10-14 13:10
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-10-14 14:04
// ***********************************************************************
//  <copyright file="TEAEncryptionExtensions.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using DomainCommonExtensions.Helpers;
using DomainCommonExtensions.Resources.Enums;
using DomainCommonExtensions.Utilities.Ensure;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

#endregion

namespace DomainCommonExtensions.CommonExtensions
{
    public static partial class CryptoExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A string extension method that TEA encrypt.
        /// </summary>
        /// <param name="stringToEncrypt">The stringToEncrypt to act on.</param>
        /// <param name="key">The key.</param>
        /// <param name="passCode">[out] The pass code.</param>
        /// <returns>
        ///     A string.
        /// </returns>
        /// =================================================================================================
        public static string TEAEncrypt(this string stringToEncrypt, string key, out string passCode)
        {
            DomainEnsure.IsNotNullOrEmpty(stringToEncrypt, nameof(stringToEncrypt));
            DomainEnsure.IsNotNullOrEmpty(key, nameof(key));

            passCode = TinyEncryptionAlgorithmHelper.GeneratePassKeyFromPassCode(key, out var passKey);
            uint[] data = null;
            var encryptedRawData = string.Empty;

            TinyEncryptionAlgorithmHelper.ConvertTextToData(ref stringToEncrypt, ref data);
            TinyEncryptionAlgorithmHelper.XxTeaEncrypt(ref data, passKey);
            TinyEncryptionAlgorithmHelper.WriteData(ref data, ref encryptedRawData);

            return encryptedRawData;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A string extension method that TEA decrypt.
        /// </summary>
        /// <param name="stringToDecrypt">The stringToDecrypt to act on.</param>
        /// <param name="key">The key.</param>
        /// <returns>
        ///     A string.
        /// </returns>
        /// =================================================================================================
        public static string TEADecrypt(this string stringToDecrypt, string key)
        {
            DomainEnsure.IsNotNullOrEmpty(stringToDecrypt, nameof(stringToDecrypt));
            DomainEnsure.IsNotNullOrEmpty(key, nameof(key));
            DomainEnsure.ThrowExceptionIfFuncIsTrue(ExceptionType.ArgumentException, 
                () => key.Length < 16, "Key does not meet the requirements (min length 16)", nameof(key));

            TinyEncryptionAlgorithmHelper.GeneratePassKeyFromPassCode(key, out var passKey);
            uint[] data = null;
            var decryptedRawData = string.Empty;

            TinyEncryptionAlgorithmHelper.ReadData(ref stringToDecrypt, ref data);
            TinyEncryptionAlgorithmHelper.XxTeaDecrypt(ref data, passKey);
            TinyEncryptionAlgorithmHelper.ConvertDataToText(ref decryptedRawData, data);

            return decryptedRawData;
        }
    }
}