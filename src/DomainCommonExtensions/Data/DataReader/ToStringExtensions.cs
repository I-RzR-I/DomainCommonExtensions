// ***********************************************************************
//  Assembly          : RzR.Shared.Extensions.DomainCommonExtensions
//  Author            : RzR
//  Created On        : 2026-10-10 22:10
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-10-14 11:18
//  ***********************************************************************
//  <copyright file="ToStringExtensions.cs" company="RzR SOFT & TECH">
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

#endregion

namespace RzR.Extensions.Domain.Data.DataReader
{
    /// <summary>
    ///     DataReader to string extensions.
    /// </summary>
    public static class ToStringExtensions
    {
        /// <summary>
        ///     Convert to string from DataReader, or null when the value is DBNull or null.
        /// </summary>
        /// <remarks>
        ///     A missing column throws the data provider's exception. A string is returned as is,
        ///     including an empty or whitespace-only one. <see cref="DateTime" /> and <see cref="DateTimeOffset" />
        ///     values use the round-trip format "o", and <see cref="double" /> and <see cref="float" />
        ///     values the round-trip format "R"; other formattable values use the invariant culture.
        /// 
        /// </remarks>
        /// <exception cref="InvalidCastException">The value is a byte array.</exception>
        /// <param name="dataReader">Data reader.</param>
        /// <param name="columnName">Column name.</param>
        /// <returns>
        ///     The column value as string, or null when the value is DBNull or null.
        /// </returns>
        public static string ToNullString(this IDataReader dataReader, string columnName)
        {
            var value = dataReader[columnName];

            return value is null || value is DBNull
                ? null
                : DataValueConverter.ToText(value, columnName);
        }

        /// <summary>
        ///     Convert to string from DataReader, or an empty string when the value is DBNull or null.
        /// </summary>
        /// <remarks>
        ///     Converts like <see cref="ToNullString" />. A missing column throws the data provider's
        ///     exception.
        /// </remarks>
        /// <exception cref="InvalidCastException">The value is a byte array.</exception>
        /// <param name="dataReader">Data reader.</param>
        /// <param name="columnName">Column name.</param>
        /// <returns>
        ///     The column value as string, or <see cref="string.Empty" /> when the value is DBNull or
        ///     null.
        /// </returns>
        public static string ToStringOrEmpty(this IDataReader dataReader, string columnName)
        {
            return dataReader.ToNullString(columnName) ?? string.Empty;
        }
    }
}