// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2026-10-08 10:00
//
//  Last Modified By : RzR
//  Last Modified On : 2026-10-08 10:00
// ***********************************************************************
//  <copyright file="ToBytesExtensions.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
//
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Data;
using RzR.Extensions.Domain.Internal.Data;

#endregion

namespace RzR.Extensions.Domain.Data.DataReader
{
    /// <summary>
    ///     DataReader to byte array extensions.
    /// </summary>
    public static class ToBytesExtensions
    {
        /// <summary>
        ///     Convert to byte array from DataReader.
        /// </summary>
        /// <remarks>
        ///     A missing column throws the data provider's exception. The provider's array is returned
        ///     as is, without a copy; an empty array is returned as an empty array.
        /// </remarks>
        /// <exception cref="InvalidCastException">The value is not a byte array.</exception>
        /// <param name="dataReader">Data reader.</param>
        /// <param name="columnName">Column name.</param>
        /// <returns>
        ///     The column value as byte array, or null when the value is DBNull, null, an empty or
        ///     whitespace text.
        /// </returns>
        public static byte[] ToBytes(this IDataReader dataReader, string columnName)
            => DataValueConverter.TryRead(dataReader, columnName, out var value)
                ? DataValueConverter.ToBytes(value, columnName)
                : null;
    }
}
