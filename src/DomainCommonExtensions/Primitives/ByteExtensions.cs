// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-08-10 21:39
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-12-08 22:59
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
using RzR.Extensions.Domain.Collections;
using RzR.Extensions.Domain.Cryptography;
using RzR.Extensions.Domain.Validation;

#if NET45_OR_GREATER || NET || NETSTANDARD1_0_OR_GREATER

using System.IO;
using System.IO.Compression;

#endif

#endregion

namespace RzR.Extensions.Domain.Primitives
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
            DomainEnsure.IsNotNull(bytes, nameof(bytes));

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
            DomainEnsure.IsNotNull(bytes, nameof(bytes));

            var sb = new StringBuilder();
            foreach (var b in bytes)
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        /// <summary>
        ///     Convert a byte array to a hexadecimal string.
        /// </summary>
        /// <param name="bytes">Input bytes.</param>
        /// <param name="upperCase">When <see langword="true"/> emits upper-case hex (e.g. <c>FF</c>); otherwise lower-case (e.g. <c>ff</c>). Default is lower-case.</param>
        /// <returns>Hexadecimal string representation of <paramref name="bytes"/>.</returns>
        public static string ToHexString(this byte[] bytes, bool upperCase = false)
        {
            DomainEnsure.IsNotNull(bytes, nameof(bytes));

            var format = upperCase ? "X2" : "x2";
            var sb = new StringBuilder(bytes.Length * 2);
            foreach (var b in bytes)
                sb.Append(b.ToString(format));

            return sb.ToString();
        }

        /// <summary>
        ///     Compute the SHA-256 checksum of a byte array and return it as a lower-case hexadecimal string.
        ///     Useful for content-addressable identifiers, integrity verification and ETag-like values.
        /// </summary>
        /// <param name="bytes">Input bytes.</param>
        /// <returns>64-character lower-case hexadecimal SHA-256 digest.</returns>
        public static string Sha256Hex(this byte[] bytes)
        {
            DomainEnsure.IsNotNull(bytes, nameof(bytes));

            using var sha = SHA256.Create();
            var hash = sha.ComputeHash(bytes);

            return hash.ToHexString(false);
        }

        /// <summary>
        ///     Get SHA1 hash from bytes
        /// </summary>
        /// <param name="bytes">Bytes</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetHashSha1String(this byte[] bytes)
        {
            DomainEnsure.IsNotNull(bytes, nameof(bytes));

#pragma warning disable SCS0006
            using var sha1 = new SHA1Managed();
#pragma warning restore SCS0006
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
            DomainEnsure.IsNotNull(bytes, nameof(bytes));

            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        ///     Gets string for bytes array
        /// </summary>
        /// <param name="bytes">The bytes array</param>
        /// <returns>The string</returns>
        public static string GetString(this byte[] bytes)
        {
            DomainEnsure.IsNotNull(bytes, nameof(bytes));

            return bytes.Aggregate(string.Empty, (current, b) => current + (char) b);
        }

        /// <summary>
        ///     Get string from byte stored as UNICODE
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToStringFromByteUnicode(this byte[] input)
        {
            DomainEnsure.IsNotNull(input, nameof(input));

            return Encoding.Unicode.GetString(input);
        }

        /// <summary>
        ///     Convert to hexadecimal from byte array
        /// </summary>
        /// <param name="clearText">Required. </param>
        /// <param name="withSpace">Optional. The default value is false.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToHexByte(this byte[] clearText, bool withSpace = false)
        {
            DomainEnsure.IsNotNull(clearText, nameof(clearText));

            var hex = new StringBuilder(clearText.Length * 2);
            foreach (var b in clearText)
                hex.AppendFormat("{0:X2}{1}", b, withSpace ? " " : "");
            var result = hex.ToString();

            return withSpace ? result.TrimEnd(' ') : result;
        }

        /// <summary>
        ///     Compare byte[]
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool CompareTo(this byte[] a, byte[] b)
        {
            if (ReferenceEquals(a, b))
                return true;
            else if (ReferenceEquals(a, null))
                return false;
            else if (ReferenceEquals(b, null))
                return false;
            else if (a.Length != b.Length)
                return false;
            else
            {
                return !a.Where((t, i) => t != b[i]).Any();
            }
        }

#if NET45_OR_GREATER || NET || NETSTANDARD1_0_OR_GREATER
        /// <summary>
        ///     Compress bytes with GZip
        /// </summary>
        /// <param name="source">Input bytes source</param>
        /// <param name="compressionLevel">GZip compression level</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static byte[] GZipCompress(this byte[] source, CompressionLevel compressionLevel)
        {
            DomainEnsure.IsNotNull(source, nameof(source));

            using var memory = new MemoryStream();
            using (var gzip = new GZipStream(memory, compressionLevel, true))
            {
                gzip.Write(source, 0, source.Length);
            }

            return memory.ToArray();
        }
#endif

        /// <summary>
        ///     Convert byte[] to BASE32 string
        /// </summary>
        /// <param name="sourceBytes">The sourceBytes to act on.</param>
        /// <returns>
        ///     A BASE32 string.
        /// </returns>
        public static string Base32BytesToString(this byte[] sourceBytes)
        {
            if(sourceBytes.IsNullOrEmptyEnumerable())
                DomainEnsure.IsNotNull(sourceBytes, nameof(sourceBytes));

            var charCount = (int)Math.Ceiling(sourceBytes.Length / 5D) * 8;
            var returnArray = new char[charCount];

            byte nextChar = 0, bitsRemaining = 5;
            var arrayIndex = 0;

            foreach (var byteItem in sourceBytes)
            {
                nextChar = (byte)(nextChar | (byteItem >> (8 - bitsRemaining)));
                returnArray[arrayIndex++] = Base32EncodingHelper.ByteToChar(nextChar);

                if (bitsRemaining < 4)
                {
                    nextChar = (byte)((byteItem >> (3 - bitsRemaining)) & 31);
                    returnArray[arrayIndex++] = Base32EncodingHelper.ByteToChar(nextChar);
                    bitsRemaining += 5;
                }

                bitsRemaining -= 3;
                nextChar = (byte)((byteItem << bitsRemaining) & 31);
            }

            if (arrayIndex.Equals(charCount).IsFalse())
            {
                returnArray[arrayIndex++] = Base32EncodingHelper.ByteToChar(nextChar);
                while (arrayIndex != charCount) 
                    returnArray[arrayIndex++] = '=';
            }

            return new string(returnArray);
        }

        /// <summary>
        ///     A byte[] extension method that query if 'sourceBytes' is entirely null.
        /// </summary>
        /// <param name="sourceBytes">The sourceBytes to act on.</param>
        /// <returns>
        ///     True if entirely null, false if not.
        /// </returns>
        public static bool IsEntirelyNull(this byte[] sourceBytes)
        {
            return sourceBytes.All(b => b == 0);
        }
    }
}
