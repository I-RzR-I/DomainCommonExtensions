// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2023-10-18 00:46
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-10-18 08:46
// ***********************************************************************
//  <copyright file="ToDecimalExtensions.cs" company="">
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
    ///     DataReader to decimal extensions
    /// </summary>
    public static class ToDecimalExtensions
    {
        /// <summary>
        ///     Convert to decimal from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns>The column value as decimal, or 0 when the value is DBNull, null, empty or whitespace.</returns>
        /// <remarks>
        ///     A missing column throws the data provider's exception. Typed numbers are converted directly
        ///     (NaN, infinity or a value out of the decimal range throws <see cref="System.OverflowException"/>).
        ///     Strings are parsed with the invariant culture first (exponent allowed, no thousands separator),
        ///     then with the current culture. Malformed values throw <see cref="System.FormatException"/>.
        /// </remarks>
        public static decimal ToDecimal(this IDataReader dataReader, string columnName)
            => DataValueConverter.TryRead(dataReader, columnName, out var value)
                ? DataValueConverter.ToDecimal(value, columnName)
                : 0;

        /// <summary>
        ///     Convert to nullable decimal from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns>The column value as decimal, or null when the value is DBNull, null, empty or whitespace.</returns>
        /// <remarks>
        ///     Converts like <see cref="ToDecimal"/>. A missing column throws the data provider's exception.
        /// </remarks>
        public static decimal? ToNullDecimal(this IDataReader dataReader, string columnName)
            => DataValueConverter.TryRead(dataReader, columnName, out var value)
                ? DataValueConverter.ToDecimal(value, columnName)
                : (decimal?)null;
    }
}
