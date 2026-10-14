// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2023-10-18 00:36
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-10-18 08:44
// ***********************************************************************
//  <copyright file="ToIntExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Data;
using RzR.Extensions.Domain.Internal.Data;

// ReSharper disable RedundantCast

#endregion

namespace RzR.Extensions.Domain.Data.DataReader
{
    /// <summary>
    ///     DataReader to int extensions
    /// </summary>
    public static class ToIntExtensions
    {
        /// <summary>
        ///     Convert to short from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns>The column value as short, or 0 when the value is DBNull, null, empty or whitespace.</returns>
        /// <remarks>
        ///     A missing column throws the data provider's exception. Typed numbers are converted with
        ///     overflow checks; a decimal or double must have no fractional part (it is never rounded).
        ///     Strings are parsed with the invariant culture first, then the current culture. Malformed
        ///     values throw <see cref="System.FormatException"/>, out-of-range values <see cref="System.OverflowException"/>.
        /// </remarks>
        public static short ToInt16(this IDataReader dataReader, string columnName)
        => DataValueConverter.TryRead(dataReader, columnName, out var value)
            ? DataValueConverter.ToInt16(value, columnName)
            : (short)0;

        /// <summary>
        ///     Convert to nullable short from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns>The column value as short, or null when the value is DBNull, null, empty or whitespace.</returns>
        /// <remarks>
        ///     Converts like <see cref="ToInt16"/>. A missing column throws the data provider's exception.
        /// </remarks>
        public static short? ToNullInt16(this IDataReader dataReader, string columnName)
        => DataValueConverter.TryRead(dataReader, columnName, out var value)
            ? DataValueConverter.ToInt16(value, columnName)
            : (short?)null;

        /// <summary>
        ///     Convert to ushort from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns>The column value as ushort, or 0 when the value is DBNull, null, empty or whitespace.</returns>
        /// <remarks>
        ///     Converts like <see cref="ToInt16"/>. A missing column throws the data provider's exception.
        /// </remarks>
        public static ushort ToUInt16(this IDataReader dataReader, string columnName)
        => DataValueConverter.TryRead(dataReader, columnName, out var value)

            ? DataValueConverter.ToUInt16(value, columnName) : (ushort)0;

        /// <summary>
        ///     Convert to nullable ushort from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns>The column value as ushort, or null when the value is DBNull, null, empty or whitespace.</returns>
        /// <remarks>
        ///     Converts like <see cref="ToInt16"/>. A missing column throws the data provider's exception.
        /// </remarks>
        public static ushort? ToNullUInt16(this IDataReader dataReader, string columnName)
        => DataValueConverter.TryRead(dataReader, columnName, out var value)
            ? DataValueConverter.ToUInt16(value, columnName)
            : (ushort?)null;

        /// <summary>
        ///     Convert to int from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns>The column value as int, or 0 when the value is DBNull, null, empty or whitespace.</returns>
        /// <remarks>
        ///     Converts like <see cref="ToInt16"/>. A missing column throws the data provider's exception.
        /// </remarks>
        public static int ToInt32(this IDataReader dataReader, string columnName)
        => DataValueConverter.TryRead(dataReader, columnName, out var value)
            ? DataValueConverter.ToInt32(value, columnName)
            : 0;

        /// <summary>
        ///     Convert to nullable int from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns>The column value as int, or null when the value is DBNull, null, empty or whitespace.</returns>
        /// <remarks>
        ///     Converts like <see cref="ToInt16"/>. A missing column throws the data provider's exception.
        /// </remarks>
        public static int? ToNullInt32(this IDataReader dataReader, string columnName)
        => DataValueConverter.TryRead(dataReader, columnName, out var value)
            ? DataValueConverter.ToInt32(value, columnName)
            : (int?)null;

        /// <summary>
        ///     Convert to uint from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns>The column value as uint, or 0 when the value is DBNull, null, empty or whitespace.</returns>
        /// <remarks>
        ///     Converts like <see cref="ToInt16"/>. A missing column throws the data provider's exception.
        /// </remarks>
        public static uint ToUInt32(this IDataReader dataReader, string columnName)
        => DataValueConverter.TryRead(dataReader, columnName, out var value)
            ? DataValueConverter.ToUInt32(value, columnName)
            : 0;

        /// <summary>
        ///     Convert to nullable uint from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns>The column value as uint, or null when the value is DBNull, null, empty or whitespace.</returns>
        /// <remarks>
        ///     Converts like <see cref="ToInt16"/>. A missing column throws the data provider's exception.
        /// </remarks>
        public static uint? ToNullUInt32(this IDataReader dataReader, string columnName)
        => DataValueConverter.TryRead(dataReader, columnName, out var value)
            ? DataValueConverter.ToUInt32(value, columnName)
            : (uint?)null;

        /// <summary>
        ///     Convert to long from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns>The column value as long, or 0 when the value is DBNull, null, empty or whitespace.</returns>
        /// <remarks>
        ///     Converts like <see cref="ToInt16"/>. A missing column throws the data provider's exception.
        /// </remarks>
        public static long ToInt64(this IDataReader dataReader, string columnName)
        => DataValueConverter.TryRead(dataReader, columnName, out var value)
            ? DataValueConverter.ToInt64(value, columnName)
            : 0;

        /// <summary>
        ///     Convert to nullable long from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns>The column value as long, or null when the value is DBNull, null, empty or whitespace.</returns>
        /// <remarks>
        ///     Converts like <see cref="ToInt16"/>. A missing column throws the data provider's exception.
        /// </remarks>
        public static long? ToNullInt64(this IDataReader dataReader, string columnName)
        => DataValueConverter.TryRead(dataReader, columnName, out var value)
            ? DataValueConverter.ToInt64(value, columnName)
            : (long?)null;

        /// <summary>
        ///     Convert to ulong from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns>The column value as ulong, or 0 when the value is DBNull, null, empty or whitespace.</returns>
        /// <remarks>
        ///     Converts like <see cref="ToInt16"/>. A missing column throws the data provider's exception.
        /// </remarks>
        public static ulong ToUInt64(this IDataReader dataReader, string columnName)
        => DataValueConverter.TryRead(dataReader, columnName, out var value)
            ? DataValueConverter.ToUInt64(value, columnName)
            : 0;

        /// <summary>
        ///     Convert to nullable ulong from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns>The column value as ulong, or null when the value is DBNull, null, empty or whitespace.</returns>
        /// <remarks>
        ///     Converts like <see cref="ToInt16"/>. A missing column throws the data provider's exception.
        /// </remarks>
        public static ulong? ToNullUInt64(this IDataReader dataReader, string columnName)
        => DataValueConverter.TryRead(dataReader, columnName, out var value)
            ? DataValueConverter.ToUInt64(value, columnName)
            : (ulong?)null;
    }
}
