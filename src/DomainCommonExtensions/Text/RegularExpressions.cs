// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-08-10 21:39
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-12 23:40
// ***********************************************************************
//  <copyright file="RegularExpressions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Text.RegularExpressions;

// ReSharper disable InconsistentNaming

#endregion

namespace RzR.Extensions.Domain.Text
{
    /// <summary>
    ///     Regular expressions REGEX
    /// </summary>
    public struct RegularExpressions
    {
        private const int RegexMatchTimeoutMilliseconds = 2000;

        /// <summary>
        ///     Is only numbers
        /// </summary>
        /// <remarks>Accepts a trailing newline; not a sanitizer.</remarks>
        public const string NUMBER = "^[0-9]*$";

        /// <summary>
        ///     Is guid format
        /// </summary>
        public const string GUID =
            @"^(\{{0,1}([0-9a-fA-F]){8}-{0,1}([0-9a-fA-F]){4}-{0,1}([0-9a-fA-F]){4}-{0,1}([0-9a-fA-F]){4}-{0,1}([0-9a-fA-F]){12}\}{0,1})$";

        /// <summary>
        ///     Is email format
        /// </summary>
        public const string EMAIL =
            @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        /// <summary>
        ///     Is credit card format
        /// </summary>
        public const string CREDIT_CARD =
            @"^(?:4[0-9]{12}(?:[0-9]{3})?|[25][1-7][0-9]{14}|6(?:011|5[0-9][0-9])[0-9]{12}|3[47][0-9]{13}|3(?:0[0-5]|[68][0-9])[0-9]{11}|(?:2131|1800|35\d{3})\d{11})$";

        /// <summary>
        ///     URL check regex
        /// </summary>
        public const string URL = @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?";

        /// <summary>
        ///     IP regex
        /// </summary>
        public const string IP =
            @"\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b";

        /// <summary>
        ///     BASE 64 regex
        /// </summary>
        public const string BASE64 = @"^[a-zA-Z0-9\+/]*={0,3}$";

        /// <summary>
        ///     BASE 32 regex
        /// </summary>
        public const string BASE32 = @"^[A-Z2-7]+(?:={1,6})?$";

        /// <summary>
        ///     (Immutable) the is all upper case string RegEx.
        /// </summary>
        /// <remarks>ASCII letters only; accepts a trailing newline; not a sanitizer.</remarks>
        public static readonly Regex IsUpperCaseStringRegex = CreateRegex("^[A-Z]*$", RegexOptions.Compiled);

        /// <summary>
        ///     (Immutable) the is all lower case string RegEx.
        /// </summary>
        /// <remarks>ASCII letters only; accepts a trailing newline; not a sanitizer.</remarks>
        public static readonly Regex IsLowerCaseStringRegex = CreateRegex("^[a-z]*$", RegexOptions.Compiled);

        /// <summary>
        ///     (Immutable) the is all letters string RegEx.
        /// </summary>
        /// <remarks>ASCII letters only; accepts a trailing newline; not a sanitizer.</remarks>
        public static readonly Regex IsAllLettersStringRegex = CreateRegex("^[a-zA-Z]*$", RegexOptions.Compiled);

        /// <summary>
        ///     (Immutable) the web URL expression.
        /// </summary>
        public static readonly Regex IsWebUrlRegex = CreateRegex(@"((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+(:[0-9]+)?|(?:www.|[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)", RegexOptions.Singleline | RegexOptions.Compiled);

        /// <summary>
        ///     Create a regex with the library match timeout where the target framework supports one.
        /// </summary>
        /// <param name="pattern">Regular expression pattern.</param>
        /// <param name="options">Regular expression options.</param>
        /// <returns></returns>
        internal static Regex CreateRegex(string pattern, RegexOptions options)
        {
#if NET45_OR_GREATER || NET || NETSTANDARD1_0_OR_GREATER
            return new Regex(pattern, options, TimeSpan.FromMilliseconds(RegexMatchTimeoutMilliseconds));
#else
            return new Regex(pattern, options);
#endif
        }

        /// <summary>
        ///     Replace every match of <paramref name="pattern"/> through the static (cached) Regex.Replace, with the
        ///     library match timeout where the target framework supports one.
        /// </summary>
        /// <param name="input">Input text.</param>
        /// <param name="pattern">Regular expression pattern.</param>
        /// <param name="replacement">Replacement pattern.</param>
        /// <returns></returns>
        internal static string Replace(string input, string pattern, string replacement)
        {
#if NET45_OR_GREATER || NET || NETSTANDARD1_0_OR_GREATER
            return Regex.Replace(input, pattern, replacement, RegexOptions.None, TimeSpan.FromMilliseconds(RegexMatchTimeoutMilliseconds));
#else
            return Regex.Replace(input, pattern, replacement);
#endif
        }

        /// <summary>
        ///     Answer whether <paramref name="input"/> matches, failing closed (false) when the match times out.
        /// </summary>
        /// <param name="regex">Regex to evaluate.</param>
        /// <param name="input">Input to check.</param>
        /// <returns></returns>
        internal static bool IsMatchOrFalseOnTimeout(Regex regex, string input)
        {
#if NET45_OR_GREATER || NET || NETSTANDARD1_0_OR_GREATER
            try
            {
                return regex.IsMatch(input);
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
#else
            return regex.IsMatch(input);
#endif
        }
    }
}
