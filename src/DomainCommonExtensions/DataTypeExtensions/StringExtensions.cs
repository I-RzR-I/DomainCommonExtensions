// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-08-10 21:39
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-12-08 21:24
// ***********************************************************************
//  <copyright file="StringExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Cryptography;
using System.Text;

#if NET || NETSTANDARD2_0_OR_GREATER
using System.Text.Encodings.Web;
using System.Text.Json;
#endif

using System.Text.RegularExpressions;
using CodeSource;
using DomainCommonExtensions.ArraysExtensions;
using DomainCommonExtensions.CommonExtensions;
using DomainCommonExtensions.Helpers.Internal;
using DomainCommonExtensions.Resources;
using DomainCommonExtensions.Resources.Enums;
using DomainCommonExtensions.Utilities.Ensure;

#endregion

namespace DomainCommonExtensions.DataTypeExtensions
{
    /// <summary>
    ///     String extensions
    /// </summary>
    /// <remarks></remarks>
    public static class StringExtensions
    {
        private static readonly string[] DateFormats =
            {"yyyy-MM-dd", "MM/dd/yyyy", "M/dd/yyyy", "M/d/yyyy", "MM/d/yyyy", "yyyyMMdd"};

        /// <summary>
        ///     Split string
        /// </summary>
        /// <param name="str"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static string[] Split(this string str, string delimiter)
        {
            return string.IsNullOrEmpty(str)
                ? new[] { str }
                : str.Split(new[] { delimiter }, StringSplitOptions.None);
        }

        /// <summary>
        ///     First char to upper
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FirstCharToUpper(this string input)
        {
            if (input.IsNullOrEmpty() || input.Length < 2) return input;

            return input.First().ToString().ToUpper() + input.Substring(1);
        }

        /// <summary>
        ///     First char to lower
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FirstCharToLower(this string input)
        {
            if (input.IsNullOrEmpty() || input.Length < 2) return input;

            return input.First().ToString().ToLowerInvariant() + input.Substring(1);
        }

        /// <summary>
        ///     Is Null or empty snippet
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        ///     Is valid email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsValidEmail(this string email)
        {
            return !email.IsNullOrEmpty() && Regex.IsMatch(email, RegularExpressions.EMAIL);
        }

        /// <summary>
        ///     Converts a string into a "SecureString"
        /// </summary>
        /// <param name="str">Input String</param>
        /// <returns></returns>
        public static SecureString ToSecureString(this string str)
        {
            DomainEnsure.ThrowExceptionIfFuncIsTrue(ExceptionType.ArgumentNullException,
                str.IsMissing, str, nameof(str));

            var secureString = new SecureString();
            foreach (var c in str)
                secureString.AppendChar(c);

            return secureString;
        }

        /// <summary>
        ///     Remove any html tags from string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string StripHtml(this string input)
        {
            input ??= string.Empty;
            var tagsExpression = new Regex(@"</?.+?>");

            return tagsExpression.Replace(input, " ");
        }

        /// <summary>
        ///     Truncates the string to a specified length and replace the truncated to a ...
        /// </summary>
        /// <param name="text">string that will be truncated</param>
        /// <param name="maxLength">total length of characters to maintain before the truncate happens</param>
        /// <param name="useDots">Use 3 dots(...) in the end of string</param>
        /// <returns>truncated string</returns>
        public static string Truncate(this string text, int maxLength, bool useDots = false)
        {
            // replaces the truncated string to a ...
            const string suffix = "...";
            var truncatedString = text ?? string.Empty;

            if (maxLength.IsLessOrEqualZero()) return truncatedString;
            var strLength = maxLength - (useDots.Equals(true) ? suffix.Length : 0);

            if (strLength.IsLessOrEqualZero()) return truncatedString;

            if (text.IsNullOrEmpty() || text!.Length <= maxLength) return truncatedString;

            truncatedString = text.Substring(0, strLength);
            truncatedString = truncatedString.TrimEnd();

            if (useDots.IsTrue())
                truncatedString += suffix;

            return truncatedString;
        }

        /// <summary>
        ///     Truncate string by custom length
        /// </summary>
        /// <param name="value">Input string/plain text</param>
        /// <param name="maxLength">Maxim number of characters to return</param>
        /// <returns></returns>
        public static string TruncateExactLength(this string value, int maxLength)
        {
            if (value.IfNullThenEmpty().IsNullOrEmpty()) return value;

            return value!.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        /// <summary>
        ///     Is valid url
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsValidUrl(this string text)
        {
            var rx = new Regex(RegularExpressions.URL);

            return rx.IsMatch(text.IfNullThenEmpty());
        }

        /// <summary>
        ///     Check if is valid ip address
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsValidIpAddress(this string s)
        {
            return Regex.IsMatch(s, RegularExpressions.IP);
        }

        /// <summary>
        ///     To bytes
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] ToBytes(this string str)
        {
            return str.IsNull() ? default : Encoding.ASCII.GetBytes(str);
        }

        /// <summary>
        ///     Utf8 to bytes
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] Utf8ToBytes(this string str)
        {
            return str.IsNull() ? default : Encoding.UTF8.GetBytes(str);
        }

        /// <summary>
        ///     Decode string from UTF8
        /// </summary>
        /// <param name="utf8String"></param>
        /// <returns></returns>
        public static string DecodeFromUtf8(this string utf8String)
        {
            if (utf8String.IfNullThenEmpty().IsMissing()) return null;

            var utf8Bytes = new byte[utf8String.Length];
            for (var i = 0; i < utf8String.Length; ++i) utf8Bytes[i] = (byte)utf8String[i];

            return Encoding.UTF8.GetString(utf8Bytes, 0, utf8Bytes.Length);
        }

        /// <summary>
        ///     Convert string to Hexadecimal value
        /// </summary>
        /// <param name="clearText">Required. Text in clear format</param>
        /// <param name="withSpace">Optional. The default value is false.If set to <see langword="true" />, then ; otherwise, .</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToHexString(this string clearText, bool withSpace = false)
        {
            if (clearText.IfNullThenEmpty().IsMissing()) return null;

            var sb = new StringBuilder();
            var crs = clearText.ToCharArray();
            foreach (var c in crs) sb.Append($"{Convert.ToInt32(c):X}{(withSpace ? " " : "")}");

            var result = sb.ToString();

            return withSpace ? result.TrimEnd(' ') : result;
        }

        /// <summary>
        ///     Convert from Hexadecimal to string
        /// </summary>
        /// <param name="hexString">Required. Hexadecimal value</param>
        /// <param name="withSpace">Optional. The default value is false.If set to <see langword="true" />, then ; otherwise, .</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToStringHex(this string hexString, bool withSpace = false)
        {
            if (hexString.IfNullThenEmpty().IsMissing()) return null;
            var sb = new StringBuilder();

            if (withSpace)
            {
                foreach (var hex in hexString.Split(' '))
                {
                    var value = Convert.ToInt32(hex, 16);
                    var charValue = (char)value;
                    sb.Append(charValue);
                }
            }
            else
            {
                var bytes = new byte[hexString.Length / 2];
                for (var i = 0; i < bytes.Length; i++)
                {
                    var value = Convert.ToInt32(hexString.Substring(i * 2, 2), 16);
                    var charValue = (char)value;
                    sb.Append(charValue);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        ///     Convert to byte array from hexadecimal
        /// </summary>
        /// <param name="hexString">Required. </param>
        /// <param name="withSpace">Optional. The default value is false.If set to <see langword="true" />, then ; otherwise, .</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static byte[] ToByteHex(this string hexString, bool withSpace = false)
        {
            if (hexString.IfNullThenEmpty().IsMissing()) return null;
            var bytes = new List<byte>();
            if (withSpace)
            {
                var hexArray = hexString.Split(' ');
                foreach (var hex in hexArray)
                {
                    var value = Convert.ToByte(hex, 16);
                    bytes.Add(value);
                }

                return bytes.ToArray();
            }

            for (var i = 0; i < hexString.Length / 2; i++)
            {
                var value = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
                bytes.Add(value);
            }

            return bytes.ToArray();
        }

        /// <summary>
        ///     Trim string if is not null
        /// </summary>
        /// <param name="value">Data to trim</param>
        /// <returns></returns>
        public static string TrimIfNotNull(this string value)
        {
            return value?.Trim();
        }

        /// <summary>
        ///     Trim and reduce space from string
        /// </summary>
        /// <param name="value">Input value</param>
        /// <param name="reduceAllSpaces">
        ///     Reduce/remove all spaces from source string data.
        ///     Default value is 'false'.
        /// </param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string TrimAndReduceSpace(this string value, bool reduceAllSpaces = false)
        {
            if (value.IfNullThenEmpty().IsMissing()) return null;

            return Regex.Replace(value, @"\s+", reduceAllSpaces.IsTrue() ? "" : " ").TrimIfNotNull();
        }

        /// <summary>
        ///     Validate email address with (MailAddress - System.Net.Mail)
        /// </summary>
        /// <param name="emailAddress">Email address</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsValidEmailWithMailAddress(this string emailAddress)
        {
            if (emailAddress.IfNullThenEmpty().IsMissing()) return false;
            try
            {
                _ = new MailAddress(emailAddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        /// <summary>
        ///     Get file name and extension separately as array
        /// </summary>
        /// <param name="fullName">Full file name (added by user)</param>
        /// <returns>Array of 2 string array[0] => filename, array[1] => extension</returns>
        public static string[] GetFileNameAndExtenstion(this string fullName)
        {
            if (fullName.IfNullThenEmpty().IsMissing()) return null;

            var extension = fullName.Split('.')[fullName.Split('.').Length - 1];
            var fileName = fullName.Replace($".{extension}", "");

            return new[] { fileName, extension };
        }

        /// <summary>
        ///     Encode string to BASE 64
        /// </summary>
        /// <param name="plainText">Plain text</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string Base64Encode(this string plainText)
        {
            if (plainText.IfNullThenEmpty().IsMissing()) return null;

            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            return Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        ///     Decode string from BASE 64
        /// </summary>
        /// <param name="base64EncodedData">Encoded string</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string Base64Decode(this string base64EncodedData)
        {
            if (base64EncodedData.IfNullThenEmpty().IsMissing()) return null;

            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);

            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        /// <summary>
        ///     Move up in directory by set level (append level in front of current path)
        /// </summary>
        /// <param name="currentPath">Current path</param>
        /// <param name="noOfLevels">Nr of levels to set up</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string FrontAppendMoveUpInDirectory(this string currentPath, int noOfLevels)
        {
            if (currentPath.IfNullThenEmpty().IsMissing()) return null;

            var path = new StringBuilder();
            for (var i = 0; i < noOfLevels; i++)
                path.Append(@"..\");
            path.Append(currentPath);

            return path.ToString();
        }

        /// <summary>
        ///     Move up in directory by set level (append level to back of current path)
        /// </summary>
        /// <param name="currentPath">Current path</param>
        /// <param name="noOfLevels">Nr of levels to set up</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string MoveUpInDirectoryBackAppend(this string currentPath, int noOfLevels)
        {
            if (currentPath.IfNullThenEmpty().IsMissing()) return null;

            var path = new StringBuilder();
            path.Append(currentPath);
            for (var i = 0; i < noOfLevels; i++)
                path.Append(@"..\");

            return path.ToString();
        }

        /// <summary>
        ///     Format to DateTime input string
        /// </summary>
        /// <param name="inputStringDate"> DateTime as string</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DateTime? FormatToDate(this string inputStringDate)
        {
            if (inputStringDate.IfNullThenEmpty().IsMissing()) return null;
            try
            {
                var parseResult = DateTime.TryParseExact(inputStringDate,
                    DateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out var date);
                if (parseResult.IsFalse())
                    return null;

                return date;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///     Replace special characters
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string RemoveSpecialChars(this string str)
        {
            if (str.IsNullOrEmpty()) return str;

            var chars = new[]
            {
                ",", ".", "/", "!", "@", "#", "$", "%", "^", "&", "*", "'", "\"", ";", "_", "(", ")", ":", "|", "[",
                "]", "<", ">"
            };

            foreach (var t in chars)
                if (str.Contains(t))
                    str = str.Replace(t, "");

            return str;
        }

        /// <summary>
        ///     Trim and replace special characters in string
        /// </summary>
        /// <param name="value">Input string</param>
        /// <param name="reduceAllSpaces">
        ///     Reduce/remove all spaces from source string data.
        ///     Default value is 'false'.
        /// </param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string TrimAndReplaceSpecialCharacters(this string value, bool reduceAllSpaces = false)
        {
            return value.TrimAndReduceSpace(reduceAllSpaces).RemoveSpecialChars();
        }

        /// <summary>
        ///     Trim in the end of string last space and specified char.
        ///     Like: old => "****, ", new => "****"
        /// </summary>
        /// <param name="value">string value</param>
        /// <param name="reduce">char to remove</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string TrimEndCharAndReduceLastWhiteSpace(this string value, char reduce)
        {
            return value.TrimIfNotNull().TrimEnd(' ').TrimEnd(reduce);
        }

        /// <summary>
        ///     Trim in the end of string last space and specified char.
        ///     Like: old => "****, ", new => "****"
        /// </summary>
        /// <param name="value">string value</param>
        /// <param name="reduce">chars array to remove</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string TrimEndCharAndReduceLastWhiteSpace(this string value, char[] reduce)
        {
            var result = string.Empty;
            foreach (var i in reduce) result = value.TrimIfNotNull().TrimEnd(' ').TrimEnd(i);

            return result;
        }

        /// <summary>
        ///     Get hash SHA1 from string with Crypto service provider
        /// </summary>
        /// <param name="content">Input string</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static byte[] GetHashSha1CryptoProv(this string content)
        {
            DomainEnsure.IsNotNull(content, nameof(content));

            var buffer = Encoding.UTF8.GetBytes(content);
#pragma warning disable SCS0006
            var sha1 = new SHA1CryptoServiceProvider();
#pragma warning restore SCS0006
            var bytes = sha1.ComputeHash(buffer);
            sha1.Dispose();

            return bytes;
        }

        /// <summary>
        ///     Get hash SHA256 from string with Crypto service provider
        /// </summary>
        /// <param name="content">Input string</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static byte[] GetHashSha256CryptoProv(this string content)
        {
            DomainEnsure.IsNotNull(content, nameof(content));

            var buffer = Encoding.UTF8.GetBytes(content);
            var sha1 = new SHA256CryptoServiceProvider();
            var bytes = sha1.ComputeHash(buffer);
            sha1.Dispose();

            return bytes;
        }

        /// <summary>
        ///     Get hash SHA256
        /// </summary>
        /// <param name="inputString">Input data</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static byte[] GetHashSha256(this string inputString)
        {
            DomainEnsure.IsNotNull(inputString, nameof(inputString));

            using HashAlgorithm algorithm = SHA256.Create();

            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        /// <summary>
        ///     Get hash string SHA256
        /// </summary>
        /// <param name="inputString">Input data</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetHashSha256String(this string inputString)
        {
            DomainEnsure.IsNotNull(inputString, nameof(inputString));

            var sb = new StringBuilder();
            foreach (var b in GetHashSha256(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        /// <summary>
        ///     Decode byte[] from BASE64 string
        /// </summary>
        /// <param name="encoded">Encoded byte[]</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static byte[] DeCodeBytesFromBase64(this string encoded)
        {
            DomainEnsure.IsNotNull(encoded, nameof(encoded));

            return Convert.FromBase64String(encoded);
        }

        /// <summary>
        ///     Check if string is in BASE64 format
        /// </summary>
        /// <param name="base64String">Encoded string</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsBase64String(this string base64String)
        {
            if (base64String.IsNullOrEmpty()) return false;

            base64String = base64String.TrimIfNotNull();

            return (base64String.Length % 4).IsZero() &&
                   Regex.IsMatch(base64String, RegularExpressions.BASE64, RegexOptions.None);
        }

        /// <summary>
        ///     Replace special characters
        /// </summary>
        /// <param name="data">Input string to format</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ReplaceSpecialCharacters(this string data)
        {
            if (data.IsNull()) return null;

            var normalizedString = data.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();
            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark) stringBuilder.Append(c);
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        /// <summary>
        ///     This method returns the next combination of the string. Thus for the string "C" the next combination will
        ///     be "D", for "BG" - "BH" and for "ZZ" the result will be "AAA". Only for uppercase strings of english alphabet!!!
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Next(this string value)
        {
            if (value.IfNullThenEmpty().IsMissing()) return null;
            var result = new StringBuilder(value);

            var isConditionSatisfied = false;
            for (var index = value.Length; index-- > 0 && !isConditionSatisfied;)
            {
                if (result[index] == 'Z') continue;
                isConditionSatisfied = true;
                result[index]++;
                for (var right = index + 1; right < value.Length; right++) result[right] = 'A';
            }

            if (isConditionSatisfied)
                return result.ToString();

            var alternative = new StringBuilder();
            for (var i = 0; i <= value.Length; i++) alternative.Append('A');

            return alternative.ToString();
        }

        /// <summary>
        ///     Move to next
        /// </summary>
        /// <param name="value">Input string</param>
        /// <param name="next">Next index</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string Next(this string value, uint next)
        {
            DomainEnsure.IsNotNull(value, nameof(value));
            DomainEnsure.IsNotNull(next, nameof(next));

            var result = new StringBuilder(value).ToString();
            for (var i = 0; i < next; i++) result = result.Next();

            return result;
        }

        /// <summary>
        ///     Get string from UNICODE -> UTF8
        /// </summary>
        /// <param name="unicodeString">Input string</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string FromUnicode(this string unicodeString)
        {
            if (unicodeString.IsNullOrEmpty()) return unicodeString;

            var bytes = Encoding.Unicode.GetBytes(unicodeString);
            var encoded = Encoding.Convert(Encoding.Unicode, Encoding.UTF8, bytes);

            return Encoding.UTF8.GetString(encoded, 0, encoded.Length);
        }

        /// <summary>
        ///     Try to parse nullable GUID
        /// </summary>
        /// <param name="val">Input string guid</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Guid? TryParseGuidNullable(this string val)
        {
            if (val.IsNullOrEmpty())
                return null;
            if (Guid.TryParse(val, out var guidValue))
                return guidValue;

            return null;
        }

        /// <summary>
        ///     Uppercase for each words for string
        /// </summary>
        /// <param name="value">The string</param>
        /// <returns>The string each words are uppercase</returns>
        public static string UppercaseWords(this string value)
        {
            DomainEnsure.IsNotNull(value, nameof(value));

            var array = value.ToCharArray();
            if (array.Length >= 1)
                if (char.IsLower(array[0]))
                    array[0] = char.ToUpper(array[0]);

            for (var i = 1; i < array.Length; i++)
                if (array[i - 1] == ' ')
                    if (char.IsLower(array[i]))
                        array[i] = char.ToUpper(array[i]);

            return new string(array);
        }

        /// <summary>
        ///     Gets bytes for string
        /// </summary>
        /// <param name="str">The string</param>
        /// <returns>The array of bytes</returns>
        public static byte[] GetBytes(this string str)
        {
            DomainEnsure.IsNotNull(str, nameof(str));

            var bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);

            return bytes;
        }

        /// <summary>
        ///     Filter XML
        /// </summary>
        /// <param name="xml">XML string</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string FilterXml(this string xml)
        {
            if (xml.IfNullThenEmpty().IsMissing()) return null;

            var output = SplitXmlWithCharSpecial(xml, @"&amp;", '&');
            output = SplitXmlWithCharSpecial(output, @"&amp;lt;", '<');
            output = SplitXmlWithCharSpecial(output, @"&amp;gt;", '>');

            return output;
        }

        /// <summary>
        ///     Simple compare two (2) strings and detect similarity in %
        /// </summary>
        /// <param name="a">The string A</param>
        /// <param name="b">The string B</param>
        /// <returns>Return similarity of two words in percentage</returns>
        public static double SimpleCompareTwoString(this string a, string b)
        {
            DomainEnsure.IsNotNull(a, nameof(a));
            DomainEnsure.IsNotNull(b, nameof(b));

            if (a == b)
                return 100;
            if (a.Length.IsZero() || b.Length.IsZero()) return 0;
            double maxLen = a.Length > b.Length ? a.Length : b.Length;
            var minLen = a.Length < b.Length ? a.Length : b.Length;
            var sameCharAtIndex = 0;
            for (var i = 0; i < minLen; i++)
                if (a[i] == b[i])
                    sameCharAtIndex++;

            return sameCharAtIndex / maxLen * 100;
        }

        /// <summary>
        ///     Split XML string
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="character">Chars</param>
        /// <param name="separator">Separator</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static string SplitXmlWithCharSpecial(string input, string character, char separator)
        {
            DomainEnsure.IsNotNull(input, nameof(input));
            DomainEnsure.IsNotNull(character, nameof(character));
            DomainEnsure.IsNotNull(separator, nameof(separator));

            var tagsArray = input.Split(separator);
            var emptyElement = tagsArray.Select((value, index) => new { index, value })
                .Where(element => string.IsNullOrEmpty(element.value)).Select(element => element.index).ToList();

            for (var i = 0; i < emptyElement.Count; i++)
            {
                if (emptyElement[i] == 0 || emptyElement[i] == tagsArray.Length - 1)
                    continue;
                tagsArray[emptyElement[i]] = character;
            }

            var output = string.Join(separator.ToString(), tagsArray);
            output = output.Replace(separator + character, character);

            return output;
        }

        /// <summary>
        ///     Generate list of permutation from input words
        /// </summary>
        /// <param name="inputData">input string separated by ' ' (space) or ',' (coma)</param>
        /// <returns>List of all permutations</returns>
        public static List<string> GenerateStringPermutation(this string inputData)
        {
            if (inputData.IsNull())
                throw new ArgumentNullException(nameof(inputData));

            var result = new List<string>();
            var splittedWord = inputData.Split(new[] { " ", ",", ", " }, StringSplitOptions.None).ToList();
            var factorial = 1;
            for (var i = 2; i <= splittedWord.Count; i++)
                factorial *= i;

            for (var v = 0; v < factorial; v++)
            {
                var s = splittedWord.ToArray();
                var k = v;
                for (var j = 2; j <= splittedWord.Count; j++)
                {
                    var other = k % j;
                    var temp = s[j - 1];
                    s[j - 1] = s[other];
                    s[other] = temp;
                    k = k / j;
                }

                var itemPermutation = string.Empty;
                foreach (var item in s) itemPermutation += item + " ";
                result.Add(itemPermutation.Trim());
            }

            return result;
        }

        /// <summary>
        ///     Generate sting ordered list
        /// </summary>
        /// <param name="inputData">Input string</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<string> GenerateStringOrderedList(this string inputData)
        {
            DomainEnsure.IsNotNull(inputData, nameof(inputData));

            var result = new List<string>();
            var splittedWord = inputData.Split(new[] { " ", ",", ", " }, StringSplitOptions.None).ToList();

            var lastCombination = string.Empty;
            foreach (var word in splittedWord)
                if (word.Length > 2)
                    result.Add(lastCombination += word);

            return result;
        }

        /// <summary>
        ///     Verify if source is in specified pull of data
        /// </summary>
        /// <param name="source">Source string</param>
        /// <param name="pull">Pull for validation</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool ValidatePull(this string source, IEnumerable<string> pull)
        {
            DomainEnsure.IsNotNull(source, nameof(source));

            var match = (pull ?? new List<string>()).Count(source.StartsWith);

            return match > 0;
        }

        /// <summary>
        ///     Fix BASE64 image
        /// </summary>
        /// <param name="image">Image source code</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string FixBase64ForImage(this string image)
        {
            DomainEnsure.IsNotNull(image, nameof(image));

            var sbText = new StringBuilder(image, image.Length);
            sbText.Replace("\r\n", string.Empty);
            sbText.Replace(" ", string.Empty);

            return sbText.ToString();
        }

        /// <summary>
        ///     Cast string to enum value
        /// </summary>
        /// <param name="value">Input string to cast</param>
        /// <returns></returns>
        /// <typeparam name="T">Type of Enum</typeparam>
        /// <remarks></remarks>
        public static T ToEnum<T>(this string value)
        {
            DomainEnsure.IsNotNull(value, nameof(value));

            try
            {
                return (T)Enum.Parse(typeof(T), value, true);
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        ///     To bytes UNICODE
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] ToBytesUnicode(this string str)
        {
            return str.IsNull() ? default : Encoding.Unicode.GetBytes(str);
        }

        /// <summary>
        ///     Replace exact word in string
        /// </summary>
        /// <param name="input">Current strings</param>
        /// <param name="oldValue">Value for search</param>
        /// <param name="newValue">Value to replace</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ReplaceExact(this string input, string oldValue, string newValue)
        {
            if (input.IfNullThenEmpty().IsMissing()) return null;

            return Regex.Replace(input, $@"\b{oldValue}\b", newValue);
        }

        /// <summary>
        ///     Return current value or default value in case when input value is null or is white space character
        /// </summary>
        /// <param name="input">Input text to check</param>
        /// <param name="defaultValue">Default value to return</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string IfNullOrWhiteSpace(this string input, string defaultValue)
        {
            return !string.IsNullOrWhiteSpace(input) ? input : defaultValue;
        }

        /// <summary>
        ///     Return current value or default value in case when input value is null or is empty character
        /// </summary>
        /// <param name="input">Input text to check</param>
        /// <param name="defaultValue">Default value to return</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string IfNullOrEmpty(this string input, string defaultValue)
        {
            return !input.IsNullOrEmpty() ? input : defaultValue;
        }

        /// <summary>
        ///     Translates the (multiline) text to an array of lines
        /// </summary>
        /// <param name="source">Input data</param>
        /// <returns>(A\r\nB) => new string[2] { "A", "B" }</returns>
        /// <remarks></remarks>
        public static string[] ToLines(this string source)
        {
            if (source.IsNull()) return new string[0];

            var temp = source;
            temp = temp.Replace("\r\n", "\n");
            temp = temp.Replace('\r', '\n');

            return temp.Split('\n');
        }

        /// <summary>
        ///     Returns string chunks of equal size (except for the first or last part that could be shorter).
        /// </summary>
        /// <param name="source">Input data to chunk.</param>
        /// <param name="size">The size of chunk.</param>
        /// <param name="isRightAlign">
        ///     Whether to align left or right. Right, alignment means the first chunk is allowed to be shorter,
        ///     while left alignment means the last piece is allowed to be shorter.
        /// </param>
        /// <returns>Chunks of the given string.</returns>
        public static IEnumerable<string> Chunked(this string source, int size, bool isRightAlign = false)
        {
            if (source.IsNull()) yield break;
            if (source.Length.IsZero()) yield break;
            if (size.IsLessOrEqualZero())
            {
                yield return source;
                yield break;
            }

            var lastLen = source.Length % size;
            if (isRightAlign && lastLen.IsGreaterThanZero())
            {
                yield return source.Substring(0, lastLen);
                for (var i = lastLen; i < source.Length; i += size) yield return source.Substring(i, size);
            }
            else
            {
                var upTo = source.Length - lastLen - 1;
                for (var i = 0; i <= upTo; i += size) yield return source.Substring(i, size);
                if (lastLen.IsGreaterThanZero())
                    yield return source.Substring(source.Length - lastLen);
            }
        }

        /// <summary>
        ///     Is empty snippet
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsEmpty(this string str)
        {
            return (str ?? "") == "";
        }

        /// <summary>
        ///     XML encode
        /// </summary>
        /// <param name="xml">Source xml</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string XmlEncode(this string xml)
        {
            if (xml.IsNullOrEmpty()) return xml;

            xml = xml.Replace("&", "&amp;");
            xml = xml.Replace("<", "&lt;");
            xml = xml.Replace(">", "&gt;");
            xml = xml.Replace("\"", "&quot;");
            xml = xml.Replace("'", "&apos;");

            return xml;

        }

        /// <summary>
        ///     XML decode
        /// </summary>
        /// <param name="xml">Decode XML to normal value</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string XmlDecode(this string xml)
        {
            if (xml.IsNullOrEmpty()) return xml;

            xml = xml.Replace("&lt;", "<");
            xml = xml.Replace("&gt;", ">");
            xml = xml.Replace("&quot;", "\"");
            xml = xml.Replace("&apos;", "'");
            xml = xml.Replace("&amp;", "&");

            return xml;
        }

        /// <summary>
        ///     Deserialize XML to object
        /// </summary>
        /// <param name="xml">Source xml</param>
        /// <param name="toType">Type to deserialize</param>
        /// <returns></returns>
        /// <remarks></remarks>
        [CodeSource("https://learn.microsoft.com/en-us/dotnet/api/system.runtime.serialization.datacontractserializer?view=netstandard-2.0", "", "MS", "2022-12-28", "Reference source")]
        public static object DeserializeToObject(this string xml, Type toType)
        {
            using Stream stream = new MemoryStream();
            var data = Encoding.UTF8.GetBytes(xml);
            stream.Write(data, 0, data.Length);
            stream.Position = 0;
            var deserializer = new DataContractSerializer(toType);

            return deserializer.ReadObject(stream);
        }

        /// <summary>
        ///     Check if string is contains in source
        /// </summary>
        /// <param name="source">Source data</param>
        /// <param name="check">Data to check</param>
        /// <param name="comp">String comparison</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool Contains(this string source, string check, StringComparison comp)
        {
            return source?.IndexOf(check, comp) >= 0;
        }

        /// <summary>
        ///     Parse string as int
        /// </summary>
        /// <param name="source">Source string</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int ParseToInt(this string source)
        {
            return int.Parse(source);
        }

        /// <summary>
        ///     Parse string as nullable int
        /// </summary>
        /// <param name="source">Source string</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int? ParseNullableInt(this string source)
        {
            if (!int.TryParse(source, out var result))
                return null;

            return result;
        }

        /// <summary>
        ///     Try parse string as int
        /// </summary>
        /// <param name="source">Source string</param>
        /// <param name="result">Parse result</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool TryParseInt(this string source, out int result)
        {
            return int.TryParse(source, out result);
        }

        /// <summary>
        /// Creates a SHA512 hash of the specified input.
        /// </summary>
        /// <param name="source">The input.</param>
        /// <returns>A hash</returns>
        public static string GetHashSha512String(this string source)
        {
            if (source.IsNull())
                throw new ArgumentNullException(nameof(source));

            using var sha = SHA512.Create();
            var bytes = Encoding.UTF8.GetBytes(source);
            var hash = sha.ComputeHash(bytes);

            return Convert.ToBase64String(hash);
        }

        /// <summary>
        ///     Generate list from source string separated by space
        /// </summary>
        /// <param name="source">The input source string</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static IEnumerable<string> FromSpaceSeparatedString(this string source)
        {
            if (source.IsNull())
                throw new ArgumentNullException(nameof(source));

            source = source.Trim();

            return source.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        /// <summary>
        ///     Is missing source string
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsMissing(this string source)
        {
            return string.IsNullOrWhiteSpace(source);
        }

        /// <summary>
        ///     Is present source string
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsPresent(this string source)
        {
            return !string.IsNullOrWhiteSpace(source);
        }

        /// <summary>
        ///     Add query string
        /// </summary>
        /// <param name="url">Source URL</param>
        /// <param name="query">Query string</param>
        /// <remarks></remarks>
        public static string AddQueryString(this string url, string query)
        {
            if (url.IfNullThenEmpty().IsMissing()) return null;
            if (query.IfNullThenEmpty().IsMissing()) return url;

            if (!url.Contains("?"))
            {
                url += "?";
            }
            else if (!url.EndsWith("&"))
            {
                url += "&";
            }

            return url + query;
        }

#if NET || NETSTANDARD2_0_OR_GREATER

        /// <summary>
        ///     Add query string
        /// </summary>
        /// <param name="url">Source URL</param>
        /// <param name="name">Query param name</param>
        /// <param name="value">Query param value</param>
        /// <remarks></remarks>
        public static string AddQueryString(this string url, string name, string value)
        {
            if (url.IfNullThenEmpty().IsMissing()) return null;
            if (name.IfNullThenEmpty().IsMissing()) return url;

            return url.AddQueryString(name + "=" + UrlEncoder.Default.Encode(value.IfNullThenEmpty()));
        }
#endif

        /// <summary>
        ///     Add hash fragment to URL
        /// </summary>
        /// <param name="url">Source URL</param>
        /// <param name="query">Query string</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string AddHashFragment(this string url, string query)
        {
            if (url.IfNullThenEmpty().IsMissing()) return null;
            if (query.IfNullThenEmpty().IsMissing()) return url;

            if (!url.Contains("#"))
            {
                url += "#";
            }

            return url + query;
        }

        /// <summary>
        ///     Get origin from URL
        /// </summary>
        /// <param name="url">Source URL</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetOrigin(this string url)
        {
            if (url.IsNull()) return null;

            Uri uri;
            try
            {
                uri = new Uri(url);
            }
            catch (Exception)
            {
                return null;
            }

            if (uri.Scheme == "http" || uri.Scheme == "https")
            {
                return $"{uri.Scheme}://{uri.Authority}";
            }

            return null;
        }

        /// <summary>
        ///     Obfuscate source string
        /// </summary>
        /// <param name="source">Source string</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string Obfuscate(this string source)
        {
            var last4Chars = "****";
            if (source.IsPresent() && source.Length > 4)
            {
                last4Chars = source.Substring(source.Length - 4);
            }

            return "****" + last4Chars;
        }

        /// <summary>
        ///     Return string as redacted value
        /// </summary>
        /// <param name="source">Source string</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string AsRedacted(this string source)
            => "***REDACTED***";

        /// <summary>
        ///     Return string prefix trimmed
        /// </summary>
        /// <param name="source">Source string</param>
        /// <param name="prefix">Prefix to be checked</param>
        /// <param name="stringComparision">String comparision type. Default OrdinalIgnoreCase.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string TrimPrefix(this string source, string prefix, StringComparison stringComparision = StringComparison.OrdinalIgnoreCase)
        {
            if (source.IsMissing() || prefix.IsMissing()) return source;

            if (source.StartsWith(prefix, stringComparision))
            {
                return source.Substring(prefix.Length);
            }

            return source;
        }

        /// <summary>
        ///     Return string suffix trimmed
        /// </summary>
        /// <param name="source">Source string</param>
        /// <param name="suffix">Suffix to be checked</param>
        /// <param name="stringComparision">String comparision type. Default OrdinalIgnoreCase.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string TrimSuffix(this string source, string suffix, StringComparison stringComparision = StringComparison.OrdinalIgnoreCase)
        {
            if (source.IsMissing() || suffix.IsMissing()) return source;

            if (source.EndsWith(suffix, stringComparision))
            {
                return source.Substring(0, source.Length - suffix.Length);
            }

            return source;
        }

        /// <summary>
        ///     Return empty source string is null
        /// </summary>
        /// <param name="source">Source string</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string IfNullThenEmpty(this string source) => source ?? string.Empty;

#if NET || NETSTANDARD2_0_OR_GREATER

        /// <summary>
        ///     Check if provided source is as valid JSON
        /// </summary>
        /// <param name="source">Source string</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsValidJson(this string source)
        {
            try
            {
                if (source.IsMissing()) return false;
                if ((source.TrimIfNotNull().StartsWith("{") && source.TrimIfNotNull().EndsWith("}")) ||
                    (source.TrimIfNotNull().StartsWith("[") && source.TrimIfNotNull().EndsWith("]")))
                {
                    using var jsonDoc = JsonDocument.Parse(source);

                    return true;
                }
                else return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     Check if provided source is as valid JSON object
        /// </summary>
        /// <param name="source">Source string</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsValidJsonObject(this string source)
        {
            try
            {
                if (source.IsMissing()) return false;
                if (source.TrimIfNotNull().StartsWith("{")
                    && source.TrimIfNotNull().EndsWith("}"))
                {
                    return source.IsValidJson();
                }
                else return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     Check if provided source is as valid JSON array
        /// </summary>
        /// <param name="source">Source string</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsValidJsonArray(this string source)
        {
            try
            {
                if (source.IsMissing()) return false;
                if (source.TrimIfNotNull().StartsWith("[")
                    && source.TrimIfNotNull().EndsWith("]"))
                {
                    return source.IsValidJson();
                }
                else return false;
            }
            catch
            {
                return false;
            }
        }

#endif

        /// <summary>
        ///     Escape backslash from source string
        /// </summary>
        /// <param name="source">Source string</param>
        /// <param name="customCharEscape">Custom char to be escaped in source. Default value is '|'.</param>
        /// <returns></returns>
        /// <remarks>Replace by default '\' => '\\', and custom char(string value) from 'x' => '\x'</remarks>
        public static string EscapeBackSlash(this string source, string customCharEscape = "|")
        {
            source = source.Replace(@"\", @"\\");
            source = source.Replace(customCharEscape, @$"\{customCharEscape}");

            return source;
        }

        /// <summary>
        ///     Cast source string to array of letters.
        /// </summary>
        /// <param name="source">Source string</param>
        /// <param name="returnEmptyIfMissing">Indicate if source is empty return empty array, otherwise return null.</param>
        /// <returns>Returns array of strings.</returns>
        /// <remarks></remarks>
        public static string[] ToStringArray(this string source, bool returnEmptyIfMissing = false)
        {
            if (source.IsMissing()) return returnEmptyIfMissing.IsTrue() ? new string[0] : null;

            return source.ToCharArray().Select(c => c.ToString()).ToArray();
        }

        /// <summary>
        ///     Cast source array of letters/string value to one string.
        /// </summary>
        /// <param name="source">Source string array.</param>
        /// <param name="returnEmptyIfMissing">Indicate if source is empty return string.Empty, otherwise return null.</param>
        /// <returns>Returns string.</returns>
        /// <remarks></remarks>
        public static string ArrayToString(this string[] source, bool returnEmptyIfMissing = false)
        {
            if (source.IsNullOrEmptyEnumerable()) return returnEmptyIfMissing.IsTrue() ? string.Empty : null;

            return string.Concat(source);
        }

        /// <summary>
        ///     Check if is string is value
        /// </summary>
        /// <param name="source">Source string value to check.</param>
        /// <returns></returns>
        public static bool IsGuid(this string source)
        {
            if (source.IsNullOrEmpty()) return false;

            var options = RegexOptions.IgnoreCase | RegexOptions.Multiline;
            var guidRegEx = new Regex(RegularExpressions.GUID, options);

            return guidRegEx.IsMatch(source);
        }

        /// <summary>
        ///     Parse string to Guid
        /// </summary>
        /// <param name="source">Source string value to cast as Guid.</param>
        /// <returns>Returns Guid.Empty if source is empty or parse error.</returns>
        public static Guid ToGuid(this string source)
        {
            if (source.IsNullOrEmpty()) return Guid.Empty;
            try
            {
                return Guid.Parse(source);
            }
            catch
            {
                return Guid.Empty;
            }
        }

        /// <summary>
        ///     Parse string to Guid from format:  "\"9ffd2c3-6er456ds\""
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Guid FromDoubleQuotesWithBackSlashesToGuid(this string source)
        {
            if (source.IsNullOrEmpty()) return Guid.Empty;
            try
            {
                return Guid.ParseExact(source.Replace("-", "").Replace("\"", ""), "N");
            }
            catch
            {
                return Guid.Empty;
            }
        }

        /// <summary>
        ///     Format source string values with parameters
        /// </summary>
        /// <param name="source">Parameterized source string value.</param>
        /// <param name="args">Parameters to supply source string.</param>
        /// <returns></returns>
        public static string FormatWith(this string source, params object[] args)
        {
            if (source.IsMissing()) return string.Empty;

            return string.Format(CultureInfo.CurrentCulture, source, args);
        }

        /// <summary>
        ///     A string extension method that check if source contains.
        /// </summary>
        /// <param name="source">The input.</param>
        /// <param name="searchValue">The search value.</param>
        /// <param name="resultValue">Default value to return.</param>
        /// <returns>
        ///     A string.
        /// </returns>
        public static string IfContains(this string source, string searchValue, string resultValue)
        {
            return source.Contains(searchValue).IsTrue() ? resultValue : source;
        }

        /// <summary>
        ///     A string extension method that check if source not contains.
        /// </summary>
        /// <param name="source">The input.</param>
        /// <param name="searchValue">The search value.</param>
        /// <param name="resultValue">Default value to return.</param>
        /// <returns>
        ///     A string.
        /// </returns>
        public static string IfNotContains(this string source, string searchValue, string resultValue)
        {
            return source.Contains(searchValue).IsFalse() ? resultValue : source;
        }

        /// <summary>
        ///     A string extension method that check if source starts with.
        /// </summary>
        /// <param name="source">The input.</param>
        /// <param name="searchValue">The search value.</param>
        /// <param name="resultValue">Default value to return.</param>
        /// <returns>
        ///     A string.
        /// </returns>
        public static string IfStartsWith(this string source, string searchValue, string resultValue)
        {
            return source.StartsWith(searchValue).IsTrue() ? resultValue : source;
        }

        /// <summary>
        ///     A string extension method that check if source not starts with.
        /// </summary>
        /// <param name="source">The input.</param>
        /// <param name="searchValue">The search value.</param>
        /// <param name="resultValue">Default value to return.</param>
        /// <returns>
        ///     A string.
        /// </returns>
        public static string IfNotStartsWith(this string source, string searchValue, string resultValue)
        {
            return source.StartsWith(searchValue).IsFalse() ? resultValue : source;
        }


        /// <summary>
        ///     Check if string is in BASE32 format
        /// </summary>
        /// <param name="base32String">Encoded BASE32 string</param>
        /// <param name="paddingCheck">Check for padding data.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsBase32String(this string base32String, bool paddingCheck = true)
        {
            if (base32String.IsNullOrEmpty()) return false;

            base32String = base32String.TrimIfNotNull();

            if (paddingCheck.IsTrue())
                return (base32String.Length % 8).IsZero() &&
                    Regex.IsMatch(base32String, RegularExpressions.BASE32, RegexOptions.None);
            else
                return Regex.IsMatch(base32String, RegularExpressions.BASE32, RegexOptions.None);
        }

        /// <summary>
        ///     Convert BASE32 string to byte[]
        /// </summary>
        /// <param name="base32String">Encoded BASE32 string.</param>
        /// <returns>
        ///     A byte[].
        /// </returns>
        public static byte[] Base32ToBytes(this string base32String)
        {
            DomainEnsure.IsNotNullOrEmptyArgNull(base32String, nameof(base32String));

            base32String = base32String.TrimEnd('=');
            var byteCount = base32String.Length * 5 / 8;
            var returnArray = new byte[byteCount];

            byte curByte = 0, bitsRemaining = 8;
            var arrayIndex = 0;

            foreach (var character in base32String)
            {
                var charValue = Base32EncodingHelper.CharToInt32(character);

                int mask;
                if (bitsRemaining > 5)
                {
                    mask = charValue << (bitsRemaining - 5);
                    curByte = (byte)(curByte | mask);
                    bitsRemaining -= 5;
                }
                else
                {
                    mask = charValue >> (5 - bitsRemaining);
                    curByte = (byte)(curByte | mask);
                    returnArray[arrayIndex++] = curByte;
                    curByte = (byte)(charValue << (3 + bitsRemaining));
                    bitsRemaining += 3;
                }
            }

            if (arrayIndex.Equals(byteCount).IsFalse())
                returnArray[arrayIndex] = curByte;

            return returnArray;
        }

        /// <summary>
        ///     A string extension method that encode clear text to BASE32 text.
        /// </summary>
        /// <param name="sourceString">The sourceString to act on.</param>
        /// <param name="includePadding">
        ///     (Optional) True to include, false to exclude the padding.
        /// </param>
        /// <returns>
        ///     A string encoded to BASE32.
        /// </returns>
        public static string Base32Encode(this string sourceString, bool includePadding = true)
        {
            if (sourceString.IsMissing())
                return string.Empty;

            var byteArray = Encoding.UTF8.GetBytes(sourceString);
            var encodedResult = new StringBuilder((byteArray.Length + 4) / 5 * 8);

            var bitBuffer = 0;
            var bitsInBuffer = 0;

            foreach (var currentByte in byteArray)
            {
                bitBuffer = (bitBuffer << 8) | currentByte;
                bitsInBuffer += 8;

                while (bitsInBuffer >= 5)
                {
                    bitsInBuffer -= 5;
                    var index = (bitBuffer >> bitsInBuffer) & 0x1F;
                    encodedResult.Append(Base32EncodingHelper.Base32Alphabet[index]);
                }
            }

            if (bitsInBuffer > 0)
            {
                var index = (bitBuffer << (5 - bitsInBuffer)) & 0x1F;
                encodedResult.Append(Base32EncodingHelper.Base32Alphabet[index]);
            }

            if (includePadding.IsTrue())
            {
                while (encodedResult.Length % 8 != 0)
                    encodedResult.Append("=");
            }

            return encodedResult.ToString();
        }

        /// <summary>
        ///     A string extension method that decode BASE32 text to clear text.
        /// </summary>
        /// <param name="sourceString">The base32 to act on.</param>
        /// <returns>
        ///     A string decoded from BASE32.
        /// </returns>
        public static string Base32Decode(this string sourceString)
        {
            if (string.IsNullOrEmpty(sourceString))
                return string.Empty;

            var cleanEncodedText = sourceString.TrimEnd('=').ToUpperInvariant();

            var bitBuffer = 0;
            var bitsInBuffer = 0;
            var tempResult = new byte[cleanEncodedText.Length * 5 / 8];
            var byteIndex = 0;

            foreach (var currentChar in cleanEncodedText)
            {
                var charValue = Base32EncodingHelper.Base32Alphabet.IndexOf(currentChar);
                DomainEnsure.ThrowExceptionIfFuncIsTrue(ExceptionType.ArgumentException,
                    () => charValue < 0, $"Invalid BASE32 character: {currentChar}", nameof(sourceString));

                bitBuffer = (bitBuffer << 5) | charValue;
                bitsInBuffer += 5;

                if (bitsInBuffer >= 8)
                {
                    bitsInBuffer -= 8;
                    tempResult[byteIndex++] = (byte)((bitBuffer >> bitsInBuffer) & 0xFF);
                }
            }

            return Encoding.UTF8.GetString(tempResult, 0, byteIndex);
        }
    }
}