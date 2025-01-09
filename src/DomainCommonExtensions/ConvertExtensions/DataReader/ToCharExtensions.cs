// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2023-10-18 09:30
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-10-18 09:33
// ***********************************************************************
//  <copyright file="ToCharExtensions.cs" company="">
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
    ///     DataReader to char extensions
    /// </summary>
    public static class ToCharExtensions
    {
        /// <summary>
        ///     Convert to char from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static char ToChar(this IDataReader dataReader, string columnName)
            => dataReader.IsWithValue(columnName)
                ? char.Parse(dataReader[columnName].ToString())
                : char.MinValue;

        /// <summary>
        ///     Convert to nullable char from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static char? ToNullChar(this IDataReader dataReader, string columnName)
            => dataReader.IsWithValue(columnName)
                ? char.Parse(dataReader[columnName].ToString())
                : (char?)null;
    }
}