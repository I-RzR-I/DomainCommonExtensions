// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2023-10-18 01:09
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-10-18 08:48
// ***********************************************************************
//  <copyright file="ToDoubleExtensions.cs" company="">
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
    ///     DataReader to double extensions
    /// </summary>
    public static class ToDoubleExtensions
    {
        /// <summary>
        ///     Convert to double from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns>The column value as double, or 0 when the value is DBNull, null, empty or whitespace.</returns>
        /// <remarks>
        ///     A missing column throws the data provider's exception. Typed numbers are converted directly,
        ///     without a text round-trip. Strings are parsed with the invariant culture first (exponent allowed,
        ///     no thousands separator), then with the current culture. Malformed values throw
        ///     <see cref="System.FormatException"/>.
        /// </remarks>
        public static double ToDouble(this IDataReader dataReader, string columnName)
            => DataValueConverter.TryRead(dataReader, columnName, out var value)
                ? DataValueConverter.ToDouble(value, columnName)
                : 0;

        /// <summary>
        ///     Convert to nullable double from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns>The column value as double, or null when the value is DBNull, null, empty or whitespace.</returns>
        /// <remarks>
        ///     Converts like <see cref="ToDouble"/>. A missing column throws the data provider's exception.
        /// </remarks>
        public static double? ToNullDouble(this IDataReader dataReader, string columnName)
            => DataValueConverter.TryRead(dataReader, columnName, out var value)
                ? DataValueConverter.ToDouble(value, columnName)
                : (double?)null;
    }
}
