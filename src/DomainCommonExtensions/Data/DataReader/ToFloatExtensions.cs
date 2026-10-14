// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2023-10-18 00:50
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-10-18 08:47
// ***********************************************************************
//  <copyright file="ToFloatExtensions.cs" company="">
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
    ///     DataReader to float extensions
    /// </summary>
    public static class ToFloatExtensions
    {
        /// <summary>
        ///     Convert to float from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns>The column value as float, or 0 when the value is DBNull, null, empty or whitespace.</returns>
        /// <remarks>
        ///     A missing column throws the data provider's exception. Typed numbers are converted directly; a
        ///     value out of the float range becomes infinity. Strings are parsed with the invariant culture first
        ///     (exponent allowed, no thousands separator), then with the current culture. Malformed values throw
        ///     <see cref="System.FormatException"/>.
        /// </remarks>
        public static float ToFloat(this IDataReader dataReader, string columnName)
        => DataValueConverter.TryRead(dataReader, columnName, out var value)
            ? DataValueConverter.ToSingle(value, columnName)
            : 0;

        /// <summary>
        ///     Convert to nullable float from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns>The column value as float, or null when the value is DBNull, null, empty or whitespace.</returns>
        /// <remarks>
        ///     Converts like <see cref="ToFloat"/>. A missing column throws the data provider's exception.
        /// </remarks>
        public static float? ToNullFloat(this IDataReader dataReader, string columnName)
        => DataValueConverter.TryRead(dataReader, columnName, out var value)
            ? DataValueConverter.ToSingle(value, columnName)
            : (float?)null;
    }
}
