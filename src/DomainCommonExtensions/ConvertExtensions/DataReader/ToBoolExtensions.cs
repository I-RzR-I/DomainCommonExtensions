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
using DomainCommonExtensions.CommonExtensions.SystemData;

// ReSharper disable RedundantCast

#endregion

namespace DomainCommonExtensions.ConvertExtensions.DataReader
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
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool ToBool(this IDataReader dataReader, string columnName)
            => dataReader.IsWithValue(columnName)
               && bool.Parse(dataReader[columnName].ToString());

        /// <summary>
        ///     Convert to nullable bool from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool? ToNullBool(this IDataReader dataReader, string columnName)
            => dataReader.IsWithValue(columnName)
                ? bool.Parse(dataReader[columnName].ToString())
                : (bool?)null;
    }
}