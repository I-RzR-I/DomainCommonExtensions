// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2023-10-18 00:46
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-10-18 08:46
// ***********************************************************************
//  <copyright file="ToDecimalExtensions.cs" company="">
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

// ReSharper disable RedundantCast

#endregion

namespace DomainCommonExtensions.ConvertExtensions.DataReader
{
    /// <summary>
    ///     DataReader to decimal extensions
    /// </summary>
    public static class ToDecimalExtensions
    {
        /// <summary>
        ///     Convert to decimal from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static decimal ToDecimal(this IDataReader dataReader, string columnName)
            => dataReader.IsWithValue(columnName)
                ? Convert.ToDecimal(dataReader[columnName].ToString())
                : 0;

        /// <summary>
        ///     Convert to nullable decimal from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static decimal? ToNullDecimal(this IDataReader dataReader, string columnName)
            => dataReader.IsWithValue(columnName)
                ? Convert.ToDecimal(dataReader[columnName]?.ToString())
                : (decimal?)null;
    }
}