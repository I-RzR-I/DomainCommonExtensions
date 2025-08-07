// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2025-02-15 13:31
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-02-17 21:13
// ***********************************************************************
//  <copyright file="PasswordGenerateUtils.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Collections.Generic;
using System.Linq;
using DomainCommonExtensions.ArraysExtensions;
using DomainCommonExtensions.CommonExtensions.TypeParam;
using DomainCommonExtensions.DataTypeExtensions;
using DomainCommonExtensions.Helpers;

// ReSharper disable AccessToModifiedClosure

#endregion

namespace DomainCommonExtensions.Utilities
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A password generate utilities.
    /// </summary>
    /// =================================================================================================
    public static class PasswordGenerateUtils
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) array of alphabets.
        /// </summary>
        /// =================================================================================================
        private static readonly string[] AlphabetArray =
        {
            "ABCDEFGHJKLMNOPQRSTUVWXYZ",
            "abcdefghijkmnopqrstuvwxyz",
            "0123456789",
            "&!#@$_?-"
        };

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Generates password by specified options.
        /// </summary>
        /// <param name="requiredMinimumLength">(Optional) Length of the required minimum.</param>
        /// <param name="requiredUniqueChar">(Optional) The required unique character.</param>
        /// <param name="requiredDigits">(Optional) True to required digits.</param>
        /// <param name="requiredLowerChars">(Optional) True to required lower characters.</param>
        /// <param name="requiredUpperChars">(Optional) True to required upper characters.</param>
        /// <param name="requiredNonAlphanumeric">(Optional) True to required non alphanumeric.</param>
        /// <param name="ownNonAlphanumericChars">(Optional) The own non alphanumeric characters.</param>
        /// <returns>
        ///     A generated password/pass code.
        /// </returns>
        /// =================================================================================================
        public static string Generate(int requiredMinimumLength = 8, int requiredUniqueChar = 4, bool requiredDigits = true,
            bool requiredLowerChars = true, bool requiredUpperChars = true, bool requiredNonAlphanumeric = true, string[] ownNonAlphanumericChars = null
        )
        {
            var passCodeLength = ValidatePassCodeLength(requiredMinimumLength, requiredUniqueChar, requiredDigits,
                requiredLowerChars, requiredUpperChars, requiredNonAlphanumeric);

            if (ownNonAlphanumericChars.IsNullOrEmptyEnumerable().IsFalse())
            {
                var newNonAlphaNum = AlphabetArray[3].ToStringArray().AppendIfNotExists(ownNonAlphanumericChars);
                AlphabetArray[3] = newNonAlphaNum.ArrayToString();
            }

            var resultChars = new List<char>();
            var allowedPool = new[] { 0, 1, 2, 3 };
            var rnd = RandomHelper.Instance;

            if (requiredUpperChars.IsTrue())
                resultChars.Insert(rnd.Number(0, resultChars.Count), AlphabetArray[0][rnd.Number(0, AlphabetArray[0].Length - 1)]);
            else allowedPool = allowedPool.RemoveItem(0);

            if (requiredLowerChars.IsTrue())
                resultChars.Insert(rnd.Number(0, resultChars.Count), AlphabetArray[1][rnd.Number(0, AlphabetArray[1].Length - 1)]);
            else allowedPool = allowedPool.RemoveItem(1);

            if (requiredDigits.IsTrue())
                resultChars.Insert(rnd.Number(0, resultChars.Count), AlphabetArray[2][rnd.Number(0, AlphabetArray[2].Length - 1)]);
            else allowedPool = allowedPool.RemoveItem(2);

            if (requiredNonAlphanumeric.IsTrue())
                resultChars.Insert(rnd.Number(0, resultChars.Count), AlphabetArray[3][rnd.Number(0, AlphabetArray[3].Length - 1)]);
            else allowedPool = allowedPool.RemoveItem(3);

            for (var i = resultChars.Count;
                 i < passCodeLength || resultChars.Distinct().Count() < requiredUniqueChar;
                 i++)
            {
                if (resultChars.Distinct().Count() < requiredUniqueChar && resultChars.Count == passCodeLength)
                {
                    resultChars.GetDuplicates()
                        .ForEachAndReturn(x =>
                        {
                            resultChars.Remove(x);
                            i--;
                        });
                }

                var rndAlphabetIdx = allowedPool[rnd.Number(0, allowedPool.Length - 1)];
                var resource = AlphabetArray[rndAlphabetIdx];
                var rndIdx = rnd.Number(0, resultChars.Count - 1);
                var rndValue = resource[rnd.Number(0, resource.Length - 1)];
                resultChars.Insert(rndIdx, rndValue);
            }

            return new string(resultChars.ToArray());
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Validates the pass code length.
        /// </summary>
        /// <param name="requiredMinimumLength">Length of the required minimum.</param>
        /// <param name="requiredUniqueChar">The required unique character.</param>
        /// <param name="requiredDigits">True to required digits.</param>
        /// <param name="requiredLowerChars">True to required lower characters.</param>
        /// <param name="requiredUpperChars">True to required upper characters.</param>
        /// <param name="requiredNonAlphanumeric">True to required non alphanumeric.</param>
        /// <returns>
        ///     An int.
        /// </returns>
        /// =================================================================================================
        private static int ValidatePassCodeLength(int requiredMinimumLength, int requiredUniqueChar, bool requiredDigits,
            bool requiredLowerChars, bool requiredUpperChars, bool requiredNonAlphanumeric)
        {
            var passCodeLength = requiredMinimumLength.IfFuncIsTrue(requiredUniqueChar, () => requiredMinimumLength < requiredUniqueChar);
            passCodeLength = passCodeLength.IfFuncIsTrue(4, () => requiredMinimumLength < 4);

            return passCodeLength;
        }
    }
}