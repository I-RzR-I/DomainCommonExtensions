// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2025-08-13 00:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-08-13 00:16
// ***********************************************************************
//  <copyright file="Base32EncodingHelper.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using DomainCommonExtensions.DataTypeExtensions;

#endregion

namespace DomainCommonExtensions.Helpers.Internal
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A base 32 encoding helper.
    /// </summary>
    /// =================================================================================================
    internal static class Base32EncodingHelper
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the base 32 alphabet.
        /// </summary>
        /// =================================================================================================
        internal static readonly string Base32Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A char extension method that character to int 32.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Thrown when one or more arguments have unsupported or illegal values.
        /// </exception>
        /// <param name="sourceChar">The sourceChar to act on.</param>
        /// <returns>
        ///     An int.
        /// </returns>
        /// =================================================================================================
        internal static int CharToInt32(char sourceChar)
        {
            var value = (int)sourceChar;

            return value switch
            {
                //65-90 == uppercase letters
                < 91 and > 64 => value - 65,
                //50-55 == numbers 2-7
                < 56 and > 49 => value - 24,
                //97-122 == lowercase letters
                < 123 and > 96 => value - 97,
                _ => throw new ArgumentException("Supplied character is not a valid value of BASE32.", nameof(sourceChar))
            };
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A byte extension method that byte to character.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Thrown when one or more arguments have unsupported or illegal values.
        /// </exception>
        /// <param name="sourceByte">The sourceByte to act on.</param>
        /// <returns>
        ///     A char.
        /// </returns>
        /// =================================================================================================
        internal static char ByteToChar(byte sourceByte)
        {
            return sourceByte switch
            {
                < 26 => (char)(sourceByte + 65),
                < 32 => (char)(sourceByte + 24),
                _ => throw new ArgumentException("Supplied byte is not a valid value of BASE32.", nameof(sourceByte))
            };
        }
    }
}