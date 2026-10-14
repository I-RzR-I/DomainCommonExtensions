// ***********************************************************************
//  Assembly          : RzR.Shared.Extensions.DomainCommonExtensions
//  Author            : RzR
//  Created On        : 2026-10-10 22:10
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-10-14 11:19
//  ***********************************************************************
//  <copyright file="ToTimeSpanExtensions.cs" company="RzR SOFT & TECH">
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
    ///     DataReader to TimeSpan extensions.
    /// </summary>
    public static class ToTimeSpanExtensions
    {
        /// <summary>
        ///     Convert to TimeSpan from DataReader.
        /// </summary>
        /// <remarks>
        ///     A missing column throws the data provider's exception. A typed TimeSpan (for example SQL
        ///     Server time or MySQL TIME) is returned as is. Strings are parsed with the constant format
        ///     "c" and the invariant culture first, then with the invariant culture. Numbers and
        ///     DateTime values are not accepted, because ticks, seconds and a time of day cannot be told
        ///     apart.
        /// </remarks>
        /// <exception cref="InvalidCastException">
        ///     The value is neither a TimeSpan nor a string.
        /// </exception>
        /// <exception cref="FormatException">The string is not a valid time interval.</exception>
        /// <exception cref="OverflowException">The string is outside the TimeSpan range.</exception>
        /// <param name="dataReader">Data reader.</param>
        /// <param name="columnName">Column name.</param>
        /// <returns>
        ///     The column value as TimeSpan, or <see cref="TimeSpan.Zero" /> when the value is DBNull,
        ///     null, empty or whitespace.
        /// </returns>
        public static TimeSpan ToTimeSpan(this IDataReader dataReader, string columnName)
        {
            return DataValueConverter.TryRead(dataReader, columnName, out var value)
                ? DataValueConverter.ToTimeSpan(value, columnName)
                : TimeSpan.Zero;
        }

        /// <summary>
        ///     Convert to nullable TimeSpan from DataReader.
        /// </summary>
        /// <remarks>
        ///     Converts like <see cref="ToTimeSpan" />. A missing column throws the data provider's
        ///     exception.
        /// </remarks>
        /// <exception cref="InvalidCastException">
        ///     The value is neither a TimeSpan nor a string.
        /// </exception>
        /// <exception cref="FormatException">The string is not a valid time interval.</exception>
        /// <exception cref="OverflowException">The string is outside the TimeSpan range.</exception>
        /// <param name="dataReader">Data reader.</param>
        /// <param name="columnName">Column name.</param>
        /// <returns>
        ///     The column value as TimeSpan, or null when the value is DBNull, null, empty or whitespace.
        /// </returns>
        public static TimeSpan? ToNullTimeSpan(this IDataReader dataReader, string columnName)
        {
            return DataValueConverter.TryRead(dataReader, columnName, out var value)
                ? DataValueConverter.ToTimeSpan(value, columnName)
                : (TimeSpan?)null;
        }
    }
}