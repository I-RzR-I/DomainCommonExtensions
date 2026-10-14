// ***********************************************************************
//  Assembly          : RzR.Shared.Extensions.DomainCommonExtensions
//  Author            : RzR
//  Created On        : 2026-10-10 22:10
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-10-14 11:17
//  ***********************************************************************
//  <copyright file="ToEnumExtensions.cs" company="RzR SOFT & TECH">
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

// ReSharper disable RedundantCast

#endregion

namespace RzR.Extensions.Domain.Data.DataReader
{
    /// <summary>
    ///     DataReader to enum extensions.
    /// </summary>
    public static class ToEnumExtensions
    {
        /// <summary>
        ///     Convert to an enum value from DataReader.
        /// </summary>
        /// <remarks>
        ///     A missing column throws the data provider's exception. A typed integral value is
        ///     converted with
        ///     <see cref="Enum.ToObject(Type, object)" />; a decimal, double or float value only when it
        ///     has no
        ///     fractional part. Strings are parsed by member name (case-insensitive, trimmed) or as a
        ///     number. Undefined values are returned as is, like a cast.
        /// </remarks>
        /// <exception cref="FormatException">
        ///     The value is not a number, a fractional number, or a string that is neither a member name
        ///     nor a number.
        /// </exception>
        /// <exception cref="OverflowException">
        ///     The number is outside the range of the enum's underlying type.
        /// </exception>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="dataReader">Data reader.</param>
        /// <param name="columnName">Column name.</param>
        /// <returns>
        ///     The column value as <typeparamref name="TEnum" />, or default(<typeparamref name="TEnum" />
        ///     ) when the value is DBNull, null, empty or whitespace.
        /// </returns>
        public static TEnum ToEnum<TEnum>(this IDataReader dataReader, string columnName)
            where TEnum : struct, Enum
        {
            return DataValueConverter.TryRead(dataReader, columnName, out var value)
                ? (TEnum)DataValueConverter.ToEnumValue(value, typeof(TEnum), columnName)
                : default;
        }

        /// <summary>
        ///     Convert to a nullable enum value from DataReader.
        /// </summary>
        /// <remarks>
        ///     Converts like <see cref="ToEnum{TEnum}" />. A missing column throws the data provider's
        ///     exception.
        /// </remarks>
        /// <exception cref="FormatException">
        ///     The value is not a number, a fractional number, or a string that is neither a member name
        ///     nor a number.
        /// </exception>
        /// <exception cref="OverflowException">
        ///     The number is outside the range of the enum's underlying type.
        /// </exception>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="dataReader">Data reader.</param>
        /// <param name="columnName">Column name.</param>
        /// <returns>
        ///     The column value as <typeparamref name="TEnum" />, or null when the value is DBNull, null,
        ///     empty or whitespace.
        /// </returns>
        public static TEnum? ToNullEnum<TEnum>(this IDataReader dataReader, string columnName)
            where TEnum : struct, Enum
        {
            return DataValueConverter.TryRead(dataReader, columnName, out var value)
                ? (TEnum)DataValueConverter.ToEnumValue(value, typeof(TEnum), columnName)
                : (TEnum?)null;
        }
    }
}