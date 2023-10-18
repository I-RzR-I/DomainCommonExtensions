// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2023-10-18 01:09
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-10-18 08:48
// ***********************************************************************
//  <copyright file="ToDoubleExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Data;
using DomainCommonExtensions.CommonExtensions.SystemData;

#endregion

namespace DomainCommonExtensions.ConvertExtensions.DataReader
{
    /// <summary>
    ///     DataReader to double extensions
    /// </summary>
    public static class ToDoubleExtensions
    {
        /// <summary>
        ///     Convert to double from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static double ToDouble(this IDataReader dataReader, string columnName)
            => dataReader.IsWithValue(columnName)
                ? double.Parse(dataReader[columnName].ToString())
                : 0;

        /// <summary>
        ///     Convert to nullable double from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static double? ToNullDouble(this IDataReader dataReader, string columnName)
            => dataReader.IsWithValue(columnName)
                ? double.Parse(dataReader[columnName].ToString())
                : (double?)null;
    }
}