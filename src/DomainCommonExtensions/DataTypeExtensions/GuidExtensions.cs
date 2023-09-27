// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-08-10 21:39
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-12 23:39
// ***********************************************************************
//  <copyright file="GuidExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Text.RegularExpressions;
using DomainCommonExtensions.Resources;

#endregion

namespace DomainCommonExtensions.DataTypeExtensions
{
    /// <summary>
    ///     Guid extensions
    /// </summary>
    /// <remarks></remarks>
    public static class GuidExtensions
    {
        /// <summary>
        ///     Check if is string is value
        /// </summary>
        /// <param name="source"></param>
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
        /// <param name="source"></param>
        /// <returns></returns>
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
        ///     Parse string to Guid
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Guid? TryToGuid(this string source)
        {
            if (source.IsNullOrEmpty()) return null;
            try
            {
                return Guid.Parse(source);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///     Guid to int 32
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public static int ToInt32(this Guid uuid)
        {
            var gb = uuid.ToByteArray();

            return BitConverter.ToInt32(gb, 0);
        }

        /// <summary>
        ///     Guid to long
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public static long ToLong(this Guid uuid)
        {
            var gb = uuid.ToByteArray();

            return BitConverter.ToInt64(gb, 0);
        }
    }
}