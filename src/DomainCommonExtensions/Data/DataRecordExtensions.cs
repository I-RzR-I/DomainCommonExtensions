// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-08-10 21:39
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-12 23:39
// ***********************************************************************
//  <copyright file="DataRecordExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections.Generic;
using System.Data;
using RzR.Extensions.Domain.Internal.Data;
using RzR.Extensions.Domain.Validation;

#endregion

namespace RzR.Extensions.Domain.Data
{
    /// <summary>
    ///     DataRecord extensions
    /// </summary>
    /// <remarks></remarks>
    public static class DataRecordExtensions
    {
        /// <summary>
        ///     Checks if reader contains column with the name of model property
        /// </summary>
        /// <param name="dr">Data record</param>
        /// <param name="columnName">Column name</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool HasColumn(this IDataRecord dr, string columnName)
        {
            for (var i = 0; i < dr.FieldCount; i++)
                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;

            return false;
        }

        /// <summary>
        ///     Tries to find the ordinal of a column by name, without throwing when the column is
        ///     missing.
        /// </summary>
        /// <remarks>
        ///     An exact (ordinal) match wins; otherwise the first column that matches with
        ///     <see cref="StringComparison.OrdinalIgnoreCase"/> is used, similar to SqlDataReader. On
        ///     duplicate
        ///     names the first match wins. Unlike <see cref="HasColumn"/>, which uses
        ///     <see cref="StringComparison.InvariantCultureIgnoreCase"/>, the comparison is culture-free.
        /// 
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="record"/> is null.</exception>
        /// <param name="record">The data record.</param>
        /// <param name="name">The column name.</param>
        /// <param name="ordinal">
        ///     [out] The zero-based column ordinal, or -1 when no column matches.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> when a column matches <paramref name="name"/>; otherwise
        ///     <see langword="false"/>, also for a null name.
        /// </returns>
        public static bool TryGetOrdinal(this IDataRecord record, string name, out int ordinal)
        {
            DomainEnsure.IsNotNull(record, nameof(record));

            ordinal = -1;
            if (name == null)
                return false;

            var fieldCount = record.FieldCount;
            for (var i = 0; i < fieldCount; i++)
            {
                var columnName = record.GetName(i);
                if (string.Equals(columnName, name, StringComparison.Ordinal))
                {
                    ordinal = i;

                    return true;
                }

                if (ordinal < 0 && string.Equals(columnName, name, StringComparison.OrdinalIgnoreCase))
                    ordinal = i;
            }

            return ordinal >= 0;
        }

        /// <summary>
        ///     Gets a column value by name converted to <typeparamref name="T"/>, or a default value
        ///     when the column is missing or holds no value.
        /// </summary>
        /// <remarks>
        ///     Converts like <see cref="GetValueOrDefault{T}(IDataRecord, int, T)"/>. Each call scans
        ///     the column names; when reading many rows, resolve the ordinal once with <see cref="TryGetOrdinal"/>
        ///     and use the ordinal overload.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="record"/> is null.</exception>
        /// <exception cref="FormatException">
        ///     A present value has an invalid format for the target type.
        /// </exception>
        /// <exception cref="OverflowException">
        ///     A present value is outside the range of the target type.
        /// </exception>
        /// <exception cref="InvalidCastException">
        ///     A present value cannot be converted to the target type.
        /// </exception>
        /// <typeparam name="T">
        ///     The target type; a nullable type converts to its underlying type.
        /// </typeparam>
        /// <param name="record">The data record.</param>
        /// <param name="name">The column name, matched like <see cref="TryGetOrdinal"/>.</param>
        /// <param name="defaultValue">
        ///     (Optional) The value returned when the column is missing or holds no value.
        /// </param>
        /// <returns>
        ///     The converted column value, or <paramref name="defaultValue"/>.
        /// </returns>
        public static T GetValueOrDefault<T>(this IDataRecord record, string name, T defaultValue = default)
            => record.TryGetOrdinal(name, out var ordinal)
                ? record.GetValueOrDefault(ordinal, defaultValue)
                : defaultValue;

        /// <summary>
        ///     Gets a column value by ordinal converted to <typeparamref name="T"/>, or a default value
        ///     when the ordinal is out of range or the column holds no value.
        /// </summary>
        /// <remarks>
        ///     DBNull and null return <paramref name="defaultValue"/>. An empty string returns it for
        ///     every target type that cannot hold the string itself, and a whitespace-only string does
        ///     the same except for <see cref="char"/>. A typed value of the target type is returned as
        ///     is; other values are converted with the invariant culture first (strings fall back to the
        ///     current culture, like the DataReader <c>To*</c> extensions). A malformed present value
        ///     throws instead of returning the default, so corrupt data is not hidden.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="record"/> is null.</exception>
        /// <exception cref="FormatException">
        ///     A present value has an invalid format for the target type.
        /// </exception>
        /// <exception cref="OverflowException">
        ///     A present value is outside the range of the target type.
        /// </exception>
        /// <exception cref="InvalidCastException">
        ///     A present value cannot be converted to the target type.
        /// </exception>
        /// <typeparam name="T">
        ///     The target type; a nullable type converts to its underlying type.
        /// </typeparam>
        /// <param name="record">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <param name="defaultValue">
        ///     (Optional) The value returned when the column is missing or holds no value.
        /// </param>
        /// <returns>
        ///     The converted column value, or <paramref name="defaultValue"/>.
        /// </returns>
        public static T GetValueOrDefault<T>(this IDataRecord record, int ordinal, T defaultValue = default)
        {
            DomainEnsure.IsNotNull(record, nameof(record));

            if (ordinal < 0 || ordinal >= record.FieldCount)
                return defaultValue;

            var value = record.GetValue(ordinal);
            if (DataValueConverter.IsAbsent(value, typeof(T)))
                return defaultValue;

            return value is T typed
                ? typed
                : (T)DataValueConverter.ConvertTo(value, typeof(T), record.GetName(ordinal));
        }

        /// <summary>
        ///     Copies the current row of a data record into a dictionary keyed by column name.
        /// </summary>
        /// <remarks>
        ///     On duplicate column names (compared case-insensitively) the first column wins and the
        ///     others are skipped; nothing is thrown. Unnamed columns (an empty or null name, for
        ///     example the columns of SQL <c>SELECT COUNT(*), MAX(x)</c>) share the key "" and only the
        ///     first one is kept.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="record"/> is null.</exception>
        /// <param name="record">The data record.</param>
        /// <returns>
        ///     A dictionary with a case-insensitive (<see cref="StringComparer.OrdinalIgnoreCase"/>) key
        ///     comparer; DBNull values are stored as null.
        /// </returns>
        public static Dictionary<string, object> ToRowDictionary(this IDataRecord record)
        {
            DomainEnsure.IsNotNull(record, nameof(record));

            var fieldCount = record.FieldCount;
            var row = new Dictionary<string, object>(fieldCount, StringComparer.OrdinalIgnoreCase);
            for (var i = 0; i < fieldCount; i++)
            {
                var name = record.GetName(i) ?? string.Empty;
                if (row.ContainsKey(name))
                    continue;

                var value = record.GetValue(i);
                row.Add(name, value is DBNull ? null : value);
            }

            return row;
        }
    }
}
