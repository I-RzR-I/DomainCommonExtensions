// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2023-10-18 09:00
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-10-18 09:05
// ***********************************************************************
//  <copyright file="ToBoolExtensions.cs" company="">
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
    ///     DataReader to bool extensions
    /// </summary>
    public static class ToBoolExtensions
    {
        /// <summary>
        ///     Convert to bool from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns>The column value as bool, or false when the value is DBNull, null, empty or whitespace.</returns>
        /// <remarks>
        ///     A missing column throws the data provider's exception. A typed bool is returned as is; numbers
        ///     map 0 to false and any other value to true; the strings "1", "0", "true" and "false" are accepted
        ///     (case-insensitive, trimmed). Any other value throws <see cref="System.FormatException"/>.
        /// </remarks>
        public static bool ToBool(this IDataReader dataReader, string columnName)
            => DataValueConverter.TryRead(dataReader, columnName, out var value)
               && DataValueConverter.ToBoolean(value, columnName);

        /// <summary>
        ///     Convert to nullable bool from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns>The column value as bool, or null when the value is DBNull, null, empty or whitespace.</returns>
        /// <remarks>
        ///     Converts like <see cref="ToBool"/>. A missing column throws the data provider's exception.
        /// </remarks>
        public static bool? ToNullBool(this IDataReader dataReader, string columnName)
            => DataValueConverter.TryRead(dataReader, columnName, out var value)
                ? DataValueConverter.ToBoolean(value, columnName)
                : (bool?)null;
    }
}
