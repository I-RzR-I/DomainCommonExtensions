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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using RzR.Extensions.Domain.Collections;
using RzR.Extensions.Domain.Reflection.TypeParam;
using RzR.Extensions.Domain.Primitives;
using RzR.Extensions.Domain.Text;

// ReSharper disable AccessToModifiedClosure

#endregion

namespace RzR.Extensions.Domain.Cryptography.Passwords
{
    /// <summary>
    ///     A password generate utilities.
    /// </summary>
    public static class PasswordGenerateUtils
    {
        /// <summary>
        ///     (Immutable) array of alphabets.
        /// </summary>
        private static readonly string[] AlphabetArray =
        {
            "ABCDEFGHJKLMNOPQRSTUVWXYZ",
            "abcdefghijkmnopqrstuvwxyz",
            "0123456789",
            "&!#@$_?-"
        };

        /// <summary>
        ///     Generates password by specified options.
        /// </summary>
        /// <remarks>
        ///     Every random draw comes from <see cref="RandomNumberGenerator"/> with rejection sampling,
        ///     so each single draw is unbiased. The result is not uniform over the combined alphabet: a
        ///     character class is picked first and then a character from it. The alphabet is built for
        ///     each call: custom characters passed in
        ///     <paramref name="ownNonAlphanumericChars"/> are added to the default special characters for
        ///     this call only and never affect other calls. This is a convenience generator with a fixed,
        ///     small set of special characters. Its strength depends on the length you choose. For
        ///     credential issuance, prefer a vetted policy library. When <paramref name="requiredUniqueChar"/>
        ///     is larger than the number of distinct characters the selected character classes can
        ///     supply, the call never returns.
        /// </remarks>
        /// <param name="requiredMinimumLength">(Optional) Length of the required minimum.</param>
        /// <param name="requiredUniqueChar">(Optional) The required unique character.</param>
        /// <param name="requiredDigits">(Optional) True to required digits.</param>
        /// <param name="requiredLowerChars">(Optional) True to required lower characters.</param>
        /// <param name="requiredUpperChars">(Optional) True to required upper characters.</param>
        /// <param name="requiredNonAlphanumeric">(Optional) True to required non-alphanumeric.</param>
        /// <param name="ownNonAlphanumericChars">(Optional) The own non-alphanumeric characters.</param>
        /// <returns>
        ///     A generated password/pass code.
        /// </returns>
        public static string Generate(int requiredMinimumLength = 8, int requiredUniqueChar = 4, bool requiredDigits = true,
            bool requiredLowerChars = true, bool requiredUpperChars = true, bool requiredNonAlphanumeric = true, string[] ownNonAlphanumericChars = null
        )
        {
            var passCodeLength = ValidatePassCodeLength(requiredMinimumLength, requiredUniqueChar, requiredDigits,
                requiredLowerChars, requiredUpperChars, requiredNonAlphanumeric);

            var alphabet = (string[])AlphabetArray.Clone();
            if (ownNonAlphanumericChars.IsNullOrEmptyEnumerable().IsFalse())
            {
                var newNonAlphaNum = alphabet[3].ToStringArray().AppendIfNotExists(ownNonAlphanumericChars);
                alphabet[3] = newNonAlphaNum.ArrayToString();
            }

            var resultChars = new List<char>();
            var allowedPool = new[] { 0, 1, 2, 3 };
            using var rng = RandomNumberGenerator.Create();

            if (requiredUpperChars.IsTrue())
                resultChars.Insert(NextInclusive(rng, 0, resultChars.Count), alphabet[0][NextInclusive(rng, 0, alphabet[0].Length - 1)]);
            else allowedPool = allowedPool.RemoveItem(0);

            if (requiredLowerChars.IsTrue())
                resultChars.Insert(NextInclusive(rng, 0, resultChars.Count), alphabet[1][NextInclusive(rng, 0, alphabet[1].Length - 1)]);
            else allowedPool = allowedPool.RemoveItem(1);

            if (requiredDigits.IsTrue())
                resultChars.Insert(NextInclusive(rng, 0, resultChars.Count), alphabet[2][NextInclusive(rng, 0, alphabet[2].Length - 1)]);
            else allowedPool = allowedPool.RemoveItem(2);

            if (requiredNonAlphanumeric.IsTrue())
                resultChars.Insert(NextInclusive(rng, 0, resultChars.Count), alphabet[3][NextInclusive(rng, 0, alphabet[3].Length - 1)]);
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

                var rndAlphabetIdx = allowedPool[NextInclusive(rng, 0, allowedPool.Length - 1)];
                var resource = alphabet[rndAlphabetIdx];
                var rndIdx = NextInclusive(rng, 0, resultChars.Count - 1);
                var rndValue = resource[NextInclusive(rng, 0, resource.Length - 1)];
                resultChars.Insert(rndIdx, rndValue);
            }

            return new string(resultChars.ToArray());
        }

        /// <summary>
        ///     Draws a uniformly distributed integer in the inclusive range 
        ///     [<paramref name="min"/>, <paramref name="max"/>] from a cryptographic random number generator.
        /// </summary>
        /// <remarks>
        ///     Returns <paramref name="min"/> without drawing when <paramref name="max"/> is not greater
        ///     than <paramref name="min"/>. That keeps the behaviour of <see cref="Generate"/> for an empty
        ///     pool, which throws <see cref="IndexOutOfRangeException"/> when every character class is turned
        ///     off. Rejection sampling discards the values at or above the largest multiple of the range
        ///     size, so the modulo that follows carries no bias. <paramref name="max"/> minus
        ///     <paramref name="min"/> must be less than <see cref="uint.MaxValue"/>, which always holds
        ///     for the string and list lengths used here.
        /// </remarks>
        /// <param name="rng">
        ///     The random number generator shared by one <see cref="Generate"/> call.
        /// </param>
        /// <param name="min">The inclusive lower bound.</param>
        /// <param name="max">The inclusive upper bound.</param>
        /// <returns>
        ///     A value from <paramref name="min"/> to <paramref name="max"/>, both inclusive.
        /// </returns>
        private static int NextInclusive(RandomNumberGenerator rng, int min, int max)
        {
            if (max <= min) return min;

            Debug.Assert(unchecked((uint)(max - min)) < uint.MaxValue, "The range must not span every uint value.");

            var range = unchecked((uint)(max - min)) + 1;
            var limit = uint.MaxValue - uint.MaxValue % range;
            var buffer = new byte[sizeof(uint)];
            uint value;
            do
            {
                rng.GetBytes(buffer);
                value = BitConverter.ToUInt32(buffer, 0);
            } while (value >= limit);

            return unchecked(min + (int)(value % range));
        }

        /// <summary>
        ///     Validates the pass code length.
        /// </summary>
        /// <param name="requiredMinimumLength">Length of the required minimum.</param>
        /// <param name="requiredUniqueChar">The required unique character.</param>
        /// <param name="requiredDigits">True to required digits.</param>
        /// <param name="requiredLowerChars">True to required lower characters.</param>
        /// <param name="requiredUpperChars">True to required upper characters.</param>
        /// <param name="requiredNonAlphanumeric">True to required non-alphanumeric.</param>
        /// <returns>
        ///     An int.
        /// </returns>
        private static int ValidatePassCodeLength(int requiredMinimumLength, int requiredUniqueChar, bool requiredDigits,
            bool requiredLowerChars, bool requiredUpperChars, bool requiredNonAlphanumeric)
        {
            var passCodeLength = requiredMinimumLength.IfFuncIsTrue(requiredUniqueChar, () => requiredMinimumLength < requiredUniqueChar);
            passCodeLength = passCodeLength.IfFuncIsTrue(4, () => requiredMinimumLength < 4);

            return passCodeLength;
        }
    }
}
