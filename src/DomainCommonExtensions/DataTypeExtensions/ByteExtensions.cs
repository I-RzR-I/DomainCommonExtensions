// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-08-10 21:39
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-12 23:39
// ***********************************************************************
//  <copyright file="ByteExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

#endregion

namespace DomainCommonExtensions.DataTypeExtensions
{
    /// <summary>
    ///     Byte extensions
    /// </summary>
    /// <remarks></remarks>
    public static class ByteExtensions
    {
        /// <summary>
        ///     Convert byte[] to string BASE64
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToBase64String(this byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));

            return Convert.ToBase64String(bytes, 0, bytes.Length);
        }

        /// <summary>
        ///     Get hash string SHA256
        /// </summary>
        /// <param name="bytes">Input data</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetHashSha256String(this byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));

            var sb = new StringBuilder();
            foreach (var b in bytes)
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        /// <summary>
        ///     Get SHA1 hash from bytes
        /// </summary>
        /// <param name="bytes">Bytes</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetHashSha1String(this byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));

            using var sha1 = new SHA1Managed();
            var hash = sha1.ComputeHash(bytes);

            return Convert.ToBase64String(hash);
        }

        /// <summary>
        ///     Get string from byte array
        /// </summary>
        /// <param name="bytes">input bytes</param>
        /// <returns></returns>
        /// <remarks></remarks>
        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once InconsistentNaming
        public static string GetStringUTF8(this byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));

            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        ///     Gets string for bytes array
        /// </summary>
        /// <param name="bytes">The bytes array</param>
        /// <returns>The string</returns>
        public static string GetString(this byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));

            return bytes.Aggregate(string.Empty, (current, b) => current + (char)b);
        }

        /// <summary>
        ///     Get string from byte stored as UNICODE
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToStringFromByteUnicode(this byte[] input)
        {
            return Encoding.Unicode.GetString(input);
        }

        /// <summary>
        ///     Convert to hexadecimal from byte array
        /// </summary>
        /// <param name="clearText">Required. </param>
        /// <param name="withSpace">Optional. The default value is false.If set to <see langword="true" />, then ; otherwise, .</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToHexByte(this byte[] clearText, bool withSpace = false)
        {
            var hex = new StringBuilder(clearText.Length * 2);
            foreach (var b in clearText)
                hex.AppendFormat("{0:X2}{1}", b, withSpace ? " " : "");
            var result = hex.ToString();

            return withSpace ? result.TrimEnd(' ') : result;
        }
    }
}