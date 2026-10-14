// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2023-10-18 08:52
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-10-18 08:58
// ***********************************************************************
//  <copyright file="ToByteExtensions.cs" company="">
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
    ///     DataReader to byte/sbyte extensions
    /// </summary>
    public static class ToByteExtensions
    {
        /// <summary>
        ///     Convert to byte from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns>The column value as byte, or 0 when the value is DBNull, null, empty or whitespace.</returns>
        /// <remarks>
        ///     A missing column throws the data provider's exception. Typed numbers are converted with
        ///     overflow checks; a decimal or double must have no fractional part (it is never rounded).
        ///     Strings are parsed with the invariant culture first, then the current culture. Malformed
        ///     values throw <see cref="System.FormatException"/>, out-of-range values
        ///     <see cref="System.OverflowException"/>.
        /// </remarks>
        public static byte ToByte(this IDataReader dataReader, string columnName)
            => DataValueConverter.TryRead(dataReader, columnName, out var value)
                ? DataValueConverter.ToByte(value, columnName)
                : byte.MinValue;

        /// <summary>
        ///     Convert to nullable byte from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns>The column value as byte, or null when the value is DBNull, null, empty or whitespace.</returns>
        /// <remarks>
        ///     Converts like <see cref="ToByte"/>. A missing column throws the data provider's exception.
        /// </remarks>
        public static byte? ToNullByte(this IDataReader dataReader, string columnName)
            => DataValueConverter.TryRead(dataReader, columnName, out var value)
                ? DataValueConverter.ToByte(value, columnName)
                : (byte?)null;

        /// <summary>
        ///     Convert to sbyte from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns>
        ///     The column value as sbyte, or <see cref="sbyte.MinValue"/> (-128, not 0) when the value is DBNull,
        ///     null, empty or whitespace.
        /// </returns>
        /// <remarks>
        ///     Converts like <see cref="ToByte"/>. A missing column throws the data provider's exception.
        /// </remarks>
        public static sbyte ToSByte(this IDataReader dataReader, string columnName)
            => DataValueConverter.TryRead(dataReader, columnName, out var value)
                ? DataValueConverter.ToSByte(value, columnName)
                : sbyte.MinValue;

        /// <summary>
        ///     Convert to nullable sbyte from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns>The column value as sbyte, or null when the value is DBNull, null, empty or whitespace.</returns>
        /// <remarks>
        ///     Converts like <see cref="ToByte"/>. A missing column throws the data provider's exception.
        /// </remarks>
        public static sbyte? ToNullSByte(this IDataReader dataReader, string columnName)
            => DataValueConverter.TryRead(dataReader, columnName, out var value)
                ? DataValueConverter.ToSByte(value, columnName)
                : (sbyte?)null;
    }
}
