// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2023-10-18 08:52
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-10-18 08:58
// ***********************************************************************
//  <copyright file="ToByteExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Data;
using DomainCommonExtensions.CommonExtensions.SystemData;

// ReSharper disable RedundantCast

#endregion

namespace DomainCommonExtensions.ConvertExtensions.DataReader
{
    /// <summary>
    ///     DataReader to byte/sbyte extensions
    /// </summary>
    public static class ToByteExtensions
    {
        /// <summary>
        ///     Convert to byte from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static byte ToByte(this IDataReader dataReader, string columnName)
            => dataReader.IsWithValue(columnName)
                ? byte.Parse(dataReader[columnName].ToString())
                : byte.MinValue;

        /// <summary>
        ///     Convert to nullable byte from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static byte? ToNullByte(this IDataReader dataReader, string columnName)
            => dataReader.IsWithValue(columnName)
                ? byte.Parse(dataReader[columnName].ToString())
                : (byte?)null;

        /// <summary>
        ///     Convert to sbyte from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static sbyte ToSByte(this IDataReader dataReader, string columnName)
            => dataReader.IsWithValue(columnName)
                ? sbyte.Parse(dataReader[columnName].ToString())
                : sbyte.MinValue;

        /// <summary>
        ///     Convert to nullable sbyte from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static sbyte? ToNullSByte(this IDataReader dataReader, string columnName)
            => dataReader.IsWithValue(columnName)
                ? sbyte.Parse(dataReader[columnName].ToString())
                : (sbyte?)null;
    }
}