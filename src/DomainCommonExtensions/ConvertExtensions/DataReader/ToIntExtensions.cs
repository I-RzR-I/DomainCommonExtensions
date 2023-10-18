// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2023-10-18 00:36
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-10-18 08:44
// ***********************************************************************
//  <copyright file="ToIntExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Data;
using DomainCommonExtensions.CommonExtensions.SystemData;

#endregion

namespace DomainCommonExtensions.ConvertExtensions.DataReader
{
    /// <summary>
    ///     DataReader to int extensions
    /// </summary>
    public static class ToIntExtensions
    {
        /// <summary>
        ///     Convert to short from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static short ToInt16(this IDataReader dataReader, string columnName)
        => dataReader.IsWithValue(columnName)
            ? Convert.ToInt16(dataReader[columnName].ToString())
            : (short)0;

        /// <summary>
        ///     Convert to nullable short from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static short? ToNullInt16(this IDataReader dataReader, string columnName)
        => dataReader.IsWithValue(columnName)
            ? Convert.ToInt16(dataReader[columnName].ToString())
            : (short?)null;

        /// <summary>
        ///     Convert to ushort from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static ushort ToUInt16(this IDataReader dataReader, string columnName)
        => dataReader.IsWithValue(columnName)

            ? Convert.ToUInt16(dataReader[columnName].ToString()) : (ushort)0;

        /// <summary>
        ///     Convert to nullable ushort from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static ushort? ToNullUInt16(this IDataReader dataReader, string columnName)
        => dataReader.IsWithValue(columnName)
            ? Convert.ToUInt16(dataReader[columnName].ToString())
            : (ushort?)null;

        /// <summary>
        ///     Convert to int from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int ToInt32(this IDataReader dataReader, string columnName)
        => dataReader.IsWithValue(columnName)
            ? Convert.ToInt32(dataReader[columnName].ToString())
            : 0;

        /// <summary>
        ///     Convert to nullable int from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int? ToNullInt32(this IDataReader dataReader, string columnName)
        => dataReader.IsWithValue(columnName)
            ? Convert.ToInt32(dataReader[columnName].ToString())
            : (int?)null;

        /// <summary>
        ///     Convert to uint from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static uint ToUInt32(this IDataReader dataReader, string columnName)
        => dataReader.IsWithValue(columnName)
            ? Convert.ToUInt32(dataReader[columnName].ToString())
            : 0;

        /// <summary>
        ///     Convert to nullable uint from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static uint? ToNullUInt32(this IDataReader dataReader, string columnName)
        => dataReader.IsWithValue(columnName)
            ? Convert.ToUInt32(dataReader[columnName].ToString())
            : (uint?)null;

        /// <summary>
        ///     Convert to long from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static long ToInt64(this IDataReader dataReader, string columnName)
        => dataReader.IsWithValue(columnName)
            ? Convert.ToInt64(dataReader[columnName].ToString())
            : 0;

        /// <summary>
        ///     Convert to nullable long from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static long? ToNullInt64(this IDataReader dataReader, string columnName)
        => dataReader.IsWithValue(columnName)
            ? Convert.ToInt64(dataReader[columnName].ToString())
            : (long?)null;

        /// <summary>
        ///     Convert to ulong from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static ulong ToUInt64(this IDataReader dataReader, string columnName)
        => dataReader.IsWithValue(columnName)
            ? Convert.ToUInt64(dataReader[columnName].ToString())
            : 0;

        /// <summary>
        ///     Convert to nullable ulong from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static ulong? ToNullUInt64(this IDataReader dataReader, string columnName)
        => dataReader.IsWithValue(columnName)
            ? Convert.ToUInt64(dataReader[columnName].ToString())
            : (ulong?)null;
    }
}