// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2026-01-18 17:01
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-01-18 22:22
// ***********************************************************************
//  <copyright file="CharExtensions.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Globalization;

// ReSharper disable MergeIntoLogicalPattern
// ReSharper disable MergeIntoPattern

#endregion

namespace DomainCommonExtensions.DataTypeExtensions
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A character extensions.
    /// </summary>
    /// =================================================================================================
    public static class CharExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A char extension method that query if 'source' is missing.
        /// </summary>
        /// <param name="source">The source to act on.</param>
        /// <returns>
        ///     True if missing, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsMissing(this char source)
        {
            return char.IsWhiteSpace(source);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A char extension method that query if 'source' is present.
        /// </summary>
        /// <param name="source">The source to act on.</param>
        /// <returns>
        ///     True if present, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsPresent(this char source)
        {
            return !char.IsWhiteSpace(source);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A char extension method that query if 'source' is digit.
        /// </summary>
        /// <param name="source">The source to act on.</param>
        /// <returns>
        ///     True if digit, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsDigit(this char source)
        {
            return source >= '0' && source <= '9';
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A char extension method that query if 'source' is ASCII.
        /// </summary>
        /// <param name="source">The source to act on.</param>
        /// <returns>
        ///     True if ascii, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsAscii(this char source)
        {
            return source <= 127;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A char extension method that query if 'source' is ASCII letter.
        /// </summary>
        /// <param name="source">The source to act on.</param>
        /// <returns>
        ///     True if ASCII letter, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsAsciiLetter(this char source)
        {
            return (source >= 'A' && source <= 'Z')
                   || (source >= 'a' && source <= 'z');
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A char extension method that query if 'source' is ASCII upper.
        /// </summary>
        /// <param name="source">The source to act on.</param>
        /// <returns>
        ///     True if ASCII upper, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsAsciiUpper(this char source)
        {
            return source >= 'A' && source <= 'Z';
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A char extension method that query if 'source' is ASCII lower.
        /// </summary>
        /// <param name="source">The source to act on.</param>
        /// <returns>
        ///     True if ASCII lower, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsAsciiLower(this char source)
        {
            return source >= 'a' && source <= 'z';
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A char extension method that query if 'source' is new line.
        /// </summary>
        /// <param name="source">The source to act on.</param>
        /// <returns>
        ///     True if new line, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsNewLine(this char source)
        {
            return source == '\n' || source == '\r';
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A char extension method that query if 'source' is quote value.
        /// </summary>
        /// <param name="source">The source to act on.</param>
        /// <returns>
        ///     True if is quote value, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsQuote(this char source)
        {
            return source == '"' || source == '\'';
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A char extension method that query if 'source' is bracket value.
        /// </summary>
        /// <param name="source">The source to act on.</param>
        /// <returns>
        ///     True if is bracket value, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsBracket(this char source)
        {
            return source == '(' || source == ')' || source == '[' || source == ']' || source == '{' || source == '}';
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A char extension method that query if 'source' is operator value.
        /// </summary>
        /// <param name="source">The source to act on.</param>
        /// <returns>
        ///     True if is operator value, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsOperator(this char source)
        {
            return source == '+' || source == '-' || source == '*' || source == '/' || source == '%' || source == '^';
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A char extension method that query if 'source' is mathematics sign.
        /// </summary>
        /// <param name="source">The source to act on.</param>
        /// <returns>
        ///     True if is mathematics sign, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsMathSign(this char source)
        {
            return source == '+' || source == '-' || source == '=' || source == '<' || source == '>' || source == '*' ||
                   source == '/';
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A char extension method that query if 'source' is white space fast.
        /// </summary>
        /// <param name="source">The source to act on.</param>
        /// <returns>
        ///     True if white space fast, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsWhiteSpaceFast(this char source)
        {
            return source == ' ' || source == '\t' || source == '\n' || source == '\r';
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A char extension method that query if 'source' is symbol or punctuation.
        /// </summary>
        /// <param name="source">The source to act on.</param>
        /// <returns>
        ///     True if symbol or punctuation, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsSymbolOrPunctuation(this char source)
        {
            return char.IsSymbol(source) || char.IsPunctuation(source);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A char extension method that query if 'source' is binary digit.
        /// </summary>
        /// <param name="source">The source to act on.</param>
        /// <returns>
        ///     True if binary digit, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsBinaryDigit(this char source)
        {
            return source == '0' || source == '1';
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A char extension method that query if 'source' is octal digit.
        /// </summary>
        /// <param name="source">The source to act on.</param>
        /// <returns>
        ///     True if octal digit, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsOctalDigit(this char source)
        {
            return source >= '0' && source <= '7';
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A char extension method that query if 'source' is hexadecimal digit.
        /// </summary>
        /// <param name="source">The source to act on.</param>
        /// <returns>
        ///     True if hexadecimal digit, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsHexDigit(this char source)
        {
            return (source >= '0' && source <= '9')
                   || (source >= 'A' && source <= 'F')
                   || (source >= 'a' && source <= 'f');
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A char extension method that converts a source to a hexadecimal value.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when the requested operation is invalid.
        /// </exception>
        /// <param name="source">The source to act on.</param>
        /// <returns>
        ///     Source as an int.
        /// </returns>
        /// =================================================================================================
        public static int ToHexValue(this char source)
        {
            if (source >= '0' && source <= '9') 
                return source - '0';
            if (source >= 'A' && source <= 'F') 
                return source - 'A' + 10;
            if (source >= 'a' && source <= 'f')
                return source - 'a' + 10;
            throw new InvalidOperationException("Not a hex digit");
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A char extension method that toggle case.
        /// </summary>
        /// <param name="source">The source to act on.</param>
        /// <returns>
        ///     A char.
        /// </returns>
        /// =================================================================================================
        public static char ToggleCase(this char source)
        {
            if (char.IsUpper(source)) 
                return char.ToLowerInvariant(source);
            if (char.IsLower(source)) 
                return char.ToUpperInvariant(source);

            return source;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A char extension method that query if 'source' is printable.
        /// </summary>
        /// <param name="source">The source to act on.</param>
        /// <returns>
        ///     True if printable, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsPrintable(this char source)
        {
            return !char.IsControl(source);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A char extension method that move to the next item in the collection.
        /// </summary>
        /// <param name="source">The source to act on.</param>
        /// <returns>
        ///     A char.
        /// </returns>
        /// =================================================================================================
        public static char Next(this char source)
        {
            return (char)(source + 1);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A char extension method that move to the previous item in the collection.
        /// </summary>
        /// <param name="source">The source to act on.</param>
        /// <returns>
        ///     A char.
        /// </returns>
        /// =================================================================================================
        public static char Previous(this char source)
        {
            return (char)(source - 1);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A char extension method that query if 'source' is emoji.
        /// </summary>
        /// <param name="source">The source to act on.</param>
        /// <returns>
        ///     True if emoji, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsEmoji(this char source)
        {
            return char.GetUnicodeCategory(source) == UnicodeCategory.OtherSymbol;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A char? extension method that query if 'source' has value.
        /// </summary>
        /// <param name="source">The source to act on.</param>
        /// <returns>
        ///     True if value is present, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool HasValue(this char? source)
        {
            return source.HasValue;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A char? extension method that default if null.
        /// </summary>
        /// <param name="source">The source to act on.</param>
        /// <param name="defaultChar">The default character.</param>
        /// <returns>
        ///     A char.
        /// </returns>
        /// =================================================================================================
        public static char DefaultIfNull(this char? source, char defaultChar)
        {
            return source ?? defaultChar;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A char? extension method that query if 'source' is null or white space.
        /// </summary>
        /// <param name="source">The source to act on.</param>
        /// <returns>
        ///     True if null or white space, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsNullOrWhiteSpace(this char? source)
        {
            return !source.HasValue || char.IsWhiteSpace(source.Value);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A char? extension method that space if null.
        /// </summary>
        /// <param name="source">The source to act on.</param>
        /// <returns>
        ///     A char.
        /// </returns>
        /// =================================================================================================
        public static char SpaceIfNull(this char? source)
        {
            return source ?? ' ';
        }
    }
}