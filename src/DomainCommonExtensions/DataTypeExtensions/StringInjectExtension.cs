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

using System.Collections;
using System.Globalization;
using System.Text.RegularExpressions;
using DomainCommonExtensions.CommonExtensions;
// ReSharper disable RedundantAssignment

#endregion

namespace DomainCommonExtensions.DataTypeExtensions
{
    /// <summary>
    ///     String inject extensions
    /// </summary>
    /// <remarks></remarks>
    public static class StringInjectExtension
    {
        /// <summary>
        ///     Inject string
        /// </summary>
        /// <param name="formatString">String</param>
        /// <param name="injectionObject">Data</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string Inject(this string formatString, object injectionObject)
        {
            if (formatString.IfNullThenEmpty().IsMissing()) return null;
            if (injectionObject.IsNull()) return formatString;

            return formatString.Inject(injectionObject.GetPropertyHash());
        }

        /// <summary>
        ///     Inject string
        /// </summary>
        /// <param name="formatString">String</param>
        /// <param name="dictionary">Data</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string Inject(this string formatString, IDictionary dictionary)
        {
            if (formatString.IfNullThenEmpty().IsMissing()) return null;
            if (dictionary.IsNull()) return formatString;

            return formatString.Inject(new Hashtable(dictionary));
        }

        /// <summary>
        ///     Inject string
        /// </summary>
        /// <param name="formatString">String</param>
        /// <param name="attributes">Data</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string Inject(this string formatString, Hashtable attributes)
        {
            if (formatString.IfNullThenEmpty().IsMissing()) return null;
            if (attributes.IsNull()) return formatString;

            var result = formatString;
            if (attributes.IsNull() || formatString.IsNull())
                return result;

            foreach (string attributeKey in attributes.Keys)
                result = result.InjectSingleValue(attributeKey, attributes[attributeKey]);

            return result;
        }

        /// <summary>
        ///     Inject string value
        /// </summary>
        /// <param name="formatString">String</param>
        /// <param name="key">Format</param>
        /// <param name="replacementValue">Replace</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string InjectSingleValue(this string formatString, string key, object replacementValue)
        {
            if (formatString.IfNullThenEmpty().IsMissing()) return null;
            if (key.IfNullThenEmpty().IsMissing()) return formatString;
            if (key.IsNull()) return formatString;

            var result = formatString;
            var attributeRegex = new Regex("{(" + key + ")(?:}|(?::(.[^}]*)}))");
            foreach (Match m in attributeRegex.Matches(formatString))
            {
                var replacement = m.ToString();
                if (m.Groups[2].Length > 0) //matched {foo:SomeFormat}
                {
                    //do a double string.Format - first to build the proper format string, and then to format the replacement value
                    var attributeFormatString = string.Format(CultureInfo.InvariantCulture, "{{0:{0}}}", m.Groups[2]);
                    replacement = string.Format(CultureInfo.CurrentCulture, attributeFormatString, replacementValue);
                }
                else //matched {foo}
                {
                    replacement = (replacementValue ?? string.Empty).ToString();
                }

                //perform replacements, one match at a time
                result = result.Replace(m.ToString(), replacement); //attributeRegex.Replace(result, replacement, 1);
            }

            return result;
        }
    }
}