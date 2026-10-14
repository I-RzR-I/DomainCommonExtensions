// ***********************************************************************
//  Assembly          : RzR.Shared.Extensions.DomainCommonExtensions
//  Author            : RzR
//  Created On        : 2026-10-10 22:10
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-10-14 11:16
//  ***********************************************************************
//  <copyright file="ToDateTimeOffsetExtensions.cs" company="RzR SOFT & TECH">
//      Copyright (c) RzR. All rights reserved.
//  </copyright>
//  <contact>
//      https://iamrzr.dev/contact
//  </contact>
//  <summary></summary>
//  ***********************************************************************

#region U S I N G

using System;
using System.Data;
using RzR.Extensions.Domain.Internal.Data;

// ReSharper disable RedundantCast

#endregion

namespace RzR.Extensions.Domain.Data.DataReader
{
    /// <summary>
    ///     DataReader to DateTimeOffset extensions.
    /// </summary>
    public static class ToDateTimeOffsetExtensions
    {
        /// <summary>
        ///     Convert to DateTimeOffset from DataReader.
        /// </summary>
        /// <remarks>
        ///     A missing column throws the data provider's exception. A typed DateTimeOffset is returned
        ///     as is. A DateTime of kind Local gets the local offset; kind Utc or Unspecified gets
        ///     +00:00, so the result does not depend on the host time zone. Strings are parsed with the
        ///     round-trip format "o" first, then with the invariant culture; a string without an offset
        ///     is read as UTC.
        /// </remarks>
        /// <exception cref="FormatException">
        ///     The value is not a date and time, or the string is not a valid date and time.
        /// </exception>
        /// <exception cref="OverflowException">
        ///     A local DateTime is outside the DateTimeOffset range in UTC.
        /// </exception>
        /// <param name="dataReader">Data reader.</param>
        /// <param name="columnName">Column name.</param>
        /// <returns>
        ///     The column value as DateTimeOffset, or <see cref="DateTimeOffset.MinValue" /> when the
        ///     value is DBNull, null, empty or whitespace.
        /// </returns>
        public static DateTimeOffset ToDateTimeOffset(this IDataReader dataReader, string columnName)
        {
            return DataValueConverter.TryRead(dataReader, columnName, out var value)
                ? DataValueConverter.ToDateTimeOffset(value, columnName)
                : DateTimeOffset.MinValue;
        }

        /// <summary>
        ///     Convert to nullable DateTimeOffset from DataReader.
        /// </summary>
        /// <remarks>
        ///     Converts like <see cref="ToDateTimeOffset" />. A missing column throws the data
        ///     provider's exception.
        /// </remarks>
        /// <exception cref="FormatException">
        ///     The value is not a date and time, or the string is not a valid date and time.
        /// </exception>
        /// <exception cref="OverflowException">
        ///     A local DateTime is outside the DateTimeOffset range in UTC.
        /// </exception>
        /// <param name="dataReader">Data reader.</param>
        /// <param name="columnName">Column name.</param>
        /// <returns>
        ///     The column value as DateTimeOffset, or null when the value is DBNull, null, empty or
        ///     whitespace.
        /// </returns>
        public static DateTimeOffset? ToNullDateTimeOffset(this IDataReader dataReader, string columnName)
        {
            return DataValueConverter.TryRead(dataReader, columnName, out var value)
                ? DataValueConverter.ToDateTimeOffset(value, columnName)
                : (DateTimeOffset?)null;
        }
    }
}