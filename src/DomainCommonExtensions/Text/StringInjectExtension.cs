// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-08-10 21:39
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-12 23:40
// ***********************************************************************
//  <copyright file="StringInjectExtension.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using RzR.Extensions.Domain.Primitives;

// ReSharper disable RedundantAssignment

#endregion

namespace RzR.Extensions.Domain.Text
{
    /// <summary>
    ///     String inject extensions
    /// </summary>
    /// <remarks>
    ///     Placeholders are "{key}" or "{key:format}". Substitution is single-pass: a value that itself
    ///     contains a placeholder is not injected again. Keys are matched literally (ordinal) by their
    ///     ToString() value (when two keys have the same ToString() the first one enumerated wins), keys
    ///     containing '{' or '}' are not supported, and placeholders whose key is not supplied are left
    ///     unchanged. On .NET Framework 4.5+ and .NET Standard a placeholder scan
    ///     that exceeds the match timeout throws <see cref="T:System.Text.RegularExpressions.RegexMatchTimeoutException"/>.
    /// </remarks>
    public static class StringInjectExtension
    {
        private static readonly Regex PlaceholderRegex = RegularExpressions.CreateRegex(@"\{([^{}]+)\}", RegexOptions.None);

        /// <summary>
        ///     Inject string.
        /// </summary>
        /// <remarks>
        ///     On .NET Framework 4.5+ and .NET Standard throws <see cref="T:System.Text.RegularExpressions.RegexMatchTimeoutException"/>
        ///     when the regex match times out.
        /// </remarks>
        /// <param name="formatString">String.</param>
        /// <param name="injectionObject">Data.</param>
        /// <returns>
        ///     Null when <paramref name="formatString"/> is null; an empty or white-space string is
        ///     returned unchanged.
        /// </returns>
        public static string Inject(this string formatString, object injectionObject)
        {
            if (formatString.IsNull()) return null;
            if (injectionObject.IsNull()) return formatString;

            return formatString.Inject(injectionObject.GetPropertyHash());
        }

        /// <summary>
        ///     Inject string.
        /// </summary>
        /// <remarks>
        ///     On .NET Framework 4.5+ and .NET Standard throws <see cref="T:System.Text.RegularExpressions.RegexMatchTimeoutException"/>
        ///     when the regex match times out.
        /// </remarks>
        /// <param name="formatString">String.</param>
        /// <param name="dictionary">Data.</param>
        /// <returns>
        ///     Null when <paramref name="formatString"/> is null; an empty or white-space string is
        ///     returned unchanged.
        /// </returns>
        public static string Inject(this string formatString, IDictionary dictionary)
        {
            if (formatString.IsNull()) return null;
            if (dictionary.IsNull()) return formatString;

            return InjectValues(formatString, ToLookup(dictionary));
        }

        /// <summary>
        ///     Inject string.
        /// </summary>
        /// <remarks>
        ///     On .NET Framework 4.5+ and .NET Standard throws <see cref="T:System.Text.RegularExpressions.RegexMatchTimeoutException"/>
        ///     when the regex match times out.
        /// </remarks>
        /// <param name="formatString">String.</param>
        /// <param name="attributes">Data.</param>
        /// <returns>
        ///     Null when <paramref name="formatString"/> is null; an empty or white-space string is
        ///     returned unchanged.
        /// </returns>
        public static string Inject(this string formatString, Hashtable attributes)
        {
            if (formatString.IsNull()) return null;
            if (attributes.IsNull()) return formatString;

            return InjectValues(formatString, ToLookup(attributes));
        }

        /// <summary>
        ///     Inject string value.
        /// </summary>
        /// <remarks>
        ///     On .NET Framework 4.5+ and .NET Standard throws <see cref="T:System.Text.RegularExpressions.RegexMatchTimeoutException"/>
        ///     when the regex match times out.
        /// </remarks>
        /// <param name="formatString">String.</param>
        /// <param name="key">Format.</param>
        /// <param name="replacementValue">Replace.</param>
        /// <returns>
        ///     Null when <paramref name="formatString"/> is null; an empty or white-space string is
        ///     returned unchanged.
        /// </returns>
        public static string InjectSingleValue(this string formatString, string key, object replacementValue)
        {
            if (formatString.IsNull()) return null;
            if (key.IsMissing()) return formatString;

            var values = new Dictionary<string, object>(StringComparer.Ordinal) { { key, replacementValue } };

            return InjectValues(formatString, values);
        }

        /// <summary>
        ///     Replace every placeholder of <paramref name="formatString"/> in a single pass.
        /// </summary>
        /// <param name="formatString">String</param>
        /// <param name="values">Values by key</param>
        /// <returns></returns>
        private static string InjectValues(string formatString, IDictionary<string, object> values)
        {
            return PlaceholderRegex.Replace(formatString, match => ResolvePlaceholder(match, values));
        }

        /// <summary>
        ///     Resolve one placeholder: the whole content as a key first ("{key}"), then the content split
        ///     at each ':' from the left ("{key:format}"); an unknown key keeps the placeholder.
        /// </summary>
        /// <param name="match">Placeholder match</param>
        /// <param name="values">Values by key</param>
        /// <returns></returns>
        private static string ResolvePlaceholder(Match match, IDictionary<string, object> values)
        {
            var content = match.Groups[1].Value;
            object value;
            if (values.TryGetValue(content, out value))
                return (value ?? string.Empty).ToString();

            for (var colon = content.IndexOf(':', 1); colon > 0 && colon < content.Length - 1; colon = content.IndexOf(':', colon + 1))
            {
                if (!values.TryGetValue(content.Substring(0, colon), out value))
                    continue;

                //do a double string.Format - first to build the proper format string, and then to format the replacement value
                var attributeFormatString = string.Format(CultureInfo.InvariantCulture, "{{0:{0}}}", content.Substring(colon + 1));

                return string.Format(CultureInfo.CurrentCulture, attributeFormatString, value);
            }

            return match.Value;
        }

        /// <summary>
        ///     Copy the entries into an ordinal lookup keyed by each key's ToString(); missing keys are skipped
        ///     and, for keys with the same ToString(), the first one enumerated wins.
        /// </summary>
        /// <param name="dictionary">Data</param>
        /// <returns></returns>
        private static IDictionary<string, object> ToLookup(IDictionary dictionary)
        {
            var values = new Dictionary<string, object>(dictionary.Count, StringComparer.Ordinal);
            var entry = dictionary.GetEnumerator();
            while (entry.MoveNext())
            {
                var key = entry.Key.ToString();
                if (key.IsMissing() || values.ContainsKey(key)) continue;

                values.Add(key, entry.Value);
            }

            return values;
        }
    }
}
