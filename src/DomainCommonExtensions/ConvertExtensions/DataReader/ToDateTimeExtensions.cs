// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2023-10-18 15:06
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-10-18 15:06
// ***********************************************************************
//  <copyright file="ToDateTimeExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Data;
using System.Globalization;
using DomainCommonExtensions.CommonExtensions.SystemData;

// ReSharper disable RedundantCast

#endregion

namespace DomainCommonExtensions.ConvertExtensions.DataReader
{
    /// <summary>
    ///     DataReader to DateTime extensions
    /// </summary>
    public static class ToDateTimeExtensions
    {
        /// <summary>
        ///     Convert to DateTime from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns>Return converted DateTime value or DateTime.MinValue if the reader does not have value.</returns>
        /// <remarks></remarks>
        public static DateTime ToDateTime(this IDataReader dataReader, string columnName)
            => dataReader.IsWithValue(columnName)
                ? Convert.ToDateTime(dataReader[columnName].ToString())
                : DateTime.MinValue;

        /// <summary>
        ///     Convert to nullable Guid from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns>Return converted DateTime value or NULL if the reader does not have value.</returns>
        /// <remarks></remarks>
        public static DateTime? ToNullDateTime(this IDataReader dataReader, string columnName)
            => dataReader.IsWithValue(columnName)
                ? Convert.ToDateTime(dataReader[columnName].ToString())
                : (DateTime?)null;

        /// <summary>
        ///     DateTime.ParseExact from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <param name="format">DateTime format</param>
        /// <returns>Return parsed DateTime value or DateTime.MinValue if the reader does not have value.</returns>
        /// <remarks></remarks>
        public static DateTime ToDateTimeParseExact(this IDataReader dataReader, string columnName, string format)
            => dataReader.IsWithValue(columnName)
                ? DateTime.ParseExact(dataReader[columnName].ToString(), format, CultureInfo.InvariantCulture)
                : DateTime.MinValue;

        /// <summary>
        ///     Nullable DateTime.ParseExact from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <param name="format">DateTime format</param>
        /// <returns>Return parsed DateTime value or NULL if the reader does not have value.</returns>
        /// <remarks></remarks>
        public static DateTime? ToNullDateTimeParseExact(this IDataReader dataReader, string columnName, string format)
            => dataReader.IsWithValue(columnName)
                ? DateTime.ParseExact(dataReader[columnName].ToString(), format, CultureInfo.InvariantCulture)
                : (DateTime?)null;

        /// <summary>
        ///     DateTime.Parse from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns>Return parsed DateTime value or DateTime.MinValue if the reader does not have value.</returns>
        /// <remarks></remarks>
        public static DateTime ToDateTimeParse(this IDataReader dataReader, string columnName)
            => dataReader.IsWithValue(columnName)
                ? DateTime.Parse(dataReader[columnName].ToString())
                : DateTime.MinValue;

        /// <summary>
        ///     Nullable DateTime.Parse from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns>Return parsed DateTime value or NULL MinValue if the reader does not have value.</returns>
        /// <remarks></remarks>
        public static DateTime? ToNullDateTimeParse(this IDataReader dataReader, string columnName)
            => dataReader.IsWithValue(columnName)
                ? DateTime.Parse(dataReader[columnName].ToString())
                : (DateTime?)null;
    }
}