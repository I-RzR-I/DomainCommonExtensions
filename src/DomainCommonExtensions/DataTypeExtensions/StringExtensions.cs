// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-08-10 21:39
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-12 23:39
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
using System.Linq;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using DomainCommonExtensions.CommonExtensions;
using DomainCommonExtensions.Resources;

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
            if (string.IsNullOrEmpty(input) || input.Length < 2) return input;

            return input.First().ToString().ToUpper() + input.Substring(1);
        }

        /// <summary>
        ///     First char to lower
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FirstCharToLower(this string input)
        {
            if (string.IsNullOrEmpty(input) || input.Length < 2) return input;

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
            if (str.IsNull())
                throw new ArgumentNullException(nameof(str));

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
            var truncatedString = text;

            if (maxLength <= 0) return truncatedString;
            var strLength = maxLength - (useDots.Equals(true) ? suffix.Length : 0);

            if (strLength <= 0) return truncatedString;

            if (text == null || text.Length <= maxLength) return truncatedString;

            truncatedString = text.Substring(0, strLength);
            truncatedString = truncatedString.TrimEnd();

            if (useDots.Equals(true))
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
            if (string.IsNullOrEmpty(value)) return value;

            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        /// <summary>
        ///     Is valid url
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsValidUrl(this string text)
        {
            var rx = new Regex(RegularExpressions.URL);

            return rx.IsMatch(text);
        }

        /// <summary>
        ///     Check if is valid ip address
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsValidIpAddress(this string s)
        {
            return Regex.IsMatch(s,
                RegularExpressions.IP);
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
        /// <returns></returns>
        /// <remarks></remarks>
        public static string TrimAndReduceSpace(this string value)
        {
            return Regex.Replace(value, @"\s+", " ").TrimIfNotNull();
        }

        /// <summary>
        ///     Validate email address with (MailAddress - System.Net.Mail)
        /// </summary>
        /// <param name="emailAddress">Email address</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsValidEmailWithMailAddress(this string emailAddress)
        {
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
            try
            {
                var parse = DateTime.TryParseExact(inputStringDate,
                    DateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out var date);
                if (!parse)
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
            if (string.IsNullOrEmpty(str)) return str;

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
        /// <returns></returns>
        /// <remarks></remarks>
        public static string TrimAndReplaceSpecialCharacters(this string value)
        {
            return value.TrimAndReduceSpace().RemoveSpecialChars();
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
            if (content.IsNull())
                throw new ArgumentNullException(nameof(content));

            var buffer = Encoding.UTF8.GetBytes(content);
            var sha1 = new SHA1CryptoServiceProvider();

            return sha1.ComputeHash(buffer);
        }

        /// <summary>
        ///     Get hash SHA256 from string with Crypto service provider
        /// </summary>
        /// <param name="content">Input string</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static byte[] GetHashSha256CryptoProv(this string content)
        {
            if (content.IsNull())
                throw new ArgumentNullException(nameof(content));

            var buffer = Encoding.UTF8.GetBytes(content);
            var sha1 = new SHA256CryptoServiceProvider();

            return sha1.ComputeHash(buffer);
        }

        /// <summary>
        ///     Get hash SHA256
        /// </summary>
        /// <param name="inputString">Input data</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static byte[] GetHashSha256(this string inputString)
        {
            if (inputString.IsNull())
                throw new ArgumentNullException(nameof(inputString));

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
            if (inputString.IsNull())
                throw new ArgumentNullException(nameof(inputString));

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
            if (encoded.IsNull())
                throw new ArgumentNullException(nameof(encoded));

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
            if (string.IsNullOrEmpty(base64String)) return false;

            base64String = base64String.TrimIfNotNull();

            return base64String.Length % 4 == 0 &&
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
            if (data is null) return null;

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
            if (value.IsNull())
                throw new ArgumentNullException(nameof(value));
            if (next.IsNull())
                throw new ArgumentNullException(nameof(next));

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
            if (string.IsNullOrEmpty(unicodeString)) return unicodeString;

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
            if (string.IsNullOrEmpty(val))
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
            if (value.IsNull())
                throw new ArgumentNullException(nameof(value));

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
            if (str.IsNull())
                throw new ArgumentNullException(nameof(str));

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
            if (a.IsNull())
                throw new ArgumentNullException(nameof(a));
            if (b.IsNull())
                throw new ArgumentNullException(nameof(b));

            if (a == b)
                return 100;
            if (a.Length == 0 || b.Length == 0) return 0;
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
            if (input.IsNull())
                throw new ArgumentNullException(nameof(input));
            if (character.IsNull())
                throw new ArgumentNullException(nameof(character));
            if (separator.IsNull())
                throw new ArgumentNullException(nameof(separator));

            var output = "";

            var tagsArray = input.Split(separator);
            var emptyElement = tagsArray.Select((value, index) => new { index, value })
                .Where(element => string.IsNullOrEmpty(element.value)).Select(element => element.index).ToList();

            for (var i = 0; i < emptyElement.Count; i++)
            {
                if (emptyElement[i] == 0 || emptyElement[i] == tagsArray.Length - 1)
                    continue;
                tagsArray[emptyElement[i]] = character;
            }

            output = string.Join(separator.ToString(), tagsArray);
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
            if (inputData.IsNull())
                throw new ArgumentNullException(nameof(inputData));

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
            if (source.IsNull())
                throw new ArgumentNullException(nameof(source));

            var match = pull.Count(source.StartsWith);

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
            if (image.IsNull())
                throw new ArgumentNullException(nameof(image));

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
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            try
            {
                return (T)Enum.Parse(typeof(T), value, true);
            }
            catch
            {
                return default(T);
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
            return Regex.Replace(input, $@"\b{oldValue}\b", newValue);
        }
    }
}