// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2023-10-18 15:03
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-10-18 15:04
// ***********************************************************************
//  <copyright file="ToGuidExtensions.cs" company="">
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
    ///     DataReader to Guid extensions
    /// </summary>
    public static class ToGuidExtensions
    {
        /// <summary>
        ///     Convert to Guid from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Guid ToGuid(this IDataReader dataReader, string columnName)
            => dataReader.IsWithValue(columnName)
                ? Guid.Parse(dataReader[columnName].ToString())
                : Guid.Empty;

        /// <summary>
        ///     Convert to nullable Guid from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Guid? ToNullGuid(this IDataReader dataReader, string columnName)
            => dataReader.IsWithValue(columnName)
                ? Guid.Parse(dataReader[columnName].ToString())
                : (Guid?)null;
    }
}