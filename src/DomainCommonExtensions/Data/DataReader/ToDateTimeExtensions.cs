// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2023-10-18 15:06
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-10-18 15:06
// ***********************************************************************
//  <copyright file="ToDateTimeExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Data;
using RzR.Extensions.Domain.Internal.Data;

// ReSharper disable RedundantCast

#endregion

namespace RzR.Extensions.Domain.Data.DataReader
{
    /// <summary>
    ///     DataReader to DateTime extensions
    /// </summary>
    public static class ToDateTimeExtensions
    {
        /// <summary>
        ///     Convert to DateTime from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns>
        ///     Return converted DateTime value or DateTime.MinValue if the value is DBNull, null, empty or whitespace.
        /// </returns>
        /// <remarks>
        ///     A missing column throws the data provider's exception. A typed DateTime is returned as is
        ///     (milliseconds and Kind kept); a DateTimeOffset returns its LocalDateTime. Strings are parsed
        ///     with the ISO 8601 formats and the invariant culture first, then with the current culture.
        ///     Malformed values throw <see cref="FormatException"/>.
        /// </remarks>
        public static DateTime ToDateTime(this IDataReader dataReader, string columnName)
            => DataValueConverter.TryRead(dataReader, columnName, out var value)
                ? DataValueConverter.ToDateTime(value, columnName)
                : DateTime.MinValue;

        /// <summary>
        ///     Convert to nullable DateTime from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns>Return converted DateTime value or NULL if the value is DBNull, null, empty or whitespace.</returns>
        /// <remarks>
        ///     Converts like <see cref="ToDateTime"/>. A missing column throws the data provider's exception.
        /// </remarks>
        public static DateTime? ToNullDateTime(this IDataReader dataReader, string columnName)
            => DataValueConverter.TryRead(dataReader, columnName, out var value)
                ? DataValueConverter.ToDateTime(value, columnName)
                : (DateTime?)null;

        /// <summary>
        ///     DateTime.ParseExact from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <param name="format">DateTime format</param>
        /// <returns>
        ///     Return parsed DateTime value or DateTime.MinValue if the value is DBNull, null, empty or whitespace.
        /// </returns>
        /// <remarks>
        ///     A missing column throws the data provider's exception. A typed DateTime is returned as is and a
        ///     DateTimeOffset returns its LocalDateTime; the format is not used for them. Other values are
        ///     parsed with <paramref name="format"/> and the invariant culture.
        /// </remarks>
        public static DateTime ToDateTimeParseExact(this IDataReader dataReader, string columnName, string format)
            => DataValueConverter.TryRead(dataReader, columnName, out var value)
                ? DataValueConverter.ToDateTimeExact(value, columnName, format)
                : DateTime.MinValue;

        /// <summary>
        ///     Nullable DateTime.ParseExact from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <param name="format">DateTime format</param>
        /// <returns>Return parsed DateTime value or NULL if the value is DBNull, null, empty or whitespace.</returns>
        /// <remarks>
        ///     Converts like <see cref="ToDateTimeParseExact"/>. A missing column throws the data provider's exception.
        /// </remarks>
        public static DateTime? ToNullDateTimeParseExact(this IDataReader dataReader, string columnName, string format)
            => DataValueConverter.TryRead(dataReader, columnName, out var value)
                ? DataValueConverter.ToDateTimeExact(value, columnName, format)
                : (DateTime?)null;

        /// <summary>
        ///     DateTime.Parse from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns>
        ///     Return parsed DateTime value or DateTime.MinValue if the value is DBNull, null, empty or whitespace.
        /// </returns>
        /// <remarks>
        ///     Converts like <see cref="ToDateTime"/>. A missing column throws the data provider's exception.
        /// </remarks>
        public static DateTime ToDateTimeParse(this IDataReader dataReader, string columnName)
            => DataValueConverter.TryRead(dataReader, columnName, out var value)
                ? DataValueConverter.ToDateTime(value, columnName)
                : DateTime.MinValue;

        /// <summary>
        ///     Nullable DateTime.Parse from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns>Return parsed DateTime value or NULL if the value is DBNull, null, empty or whitespace.</returns>
        /// <remarks>
        ///     Converts like <see cref="ToDateTime"/>. A missing column throws the data provider's exception.
        /// </remarks>
        public static DateTime? ToNullDateTimeParse(this IDataReader dataReader, string columnName)
            => DataValueConverter.TryRead(dataReader, columnName, out var value)
                ? DataValueConverter.ToDateTime(value, columnName)
                : (DateTime?)null;
    }
}
