// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2023-10-18 15:03
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-10-18 15:04
// ***********************************************************************
//  <copyright file="ToGuidExtensions.cs" company="">
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
    ///     DataReader to Guid extensions
    /// </summary>
    public static class ToGuidExtensions
    {
        /// <summary>
        ///     Convert to Guid from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns>The column value as Guid, or Guid.Empty when the value is DBNull, null, empty or whitespace.</returns>
        /// <remarks>
        ///     A missing column throws the data provider's exception. A typed Guid is returned as is; other
        ///     values are parsed from their text. Malformed values (including byte arrays) throw
        ///     <see cref="FormatException"/>.
        /// </remarks>
        public static Guid ToGuid(this IDataReader dataReader, string columnName)
            => DataValueConverter.TryRead(dataReader, columnName, out var value)
                ? DataValueConverter.ToGuid(value, columnName)
                : Guid.Empty;

        /// <summary>
        ///     Convert to nullable Guid from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns>The column value as Guid, or null when the value is DBNull, null, empty or whitespace.</returns>
        /// <remarks>
        ///     Converts like <see cref="ToGuid"/>. A missing column throws the data provider's exception.
        /// </remarks>
        public static Guid? ToNullGuid(this IDataReader dataReader, string columnName)
            => DataValueConverter.TryRead(dataReader, columnName, out var value)
                ? DataValueConverter.ToGuid(value, columnName)
                : (Guid?)null;
    }
}
