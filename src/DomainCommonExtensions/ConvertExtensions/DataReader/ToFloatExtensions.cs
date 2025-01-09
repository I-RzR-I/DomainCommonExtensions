// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2023-10-18 00:50
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-10-18 08:47
// ***********************************************************************
//  <copyright file="ToFloatExtensions.cs" company="">
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
    ///     DataReader to float extensions
    /// </summary>
    public static class ToFloatExtensions
    {
        /// <summary>
        ///     Convert to float from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static float ToFloat(this IDataReader dataReader, string columnName)
        => dataReader.IsWithValue(columnName)
            ? float.Parse(dataReader[columnName].ToString())
            : 0;

        /// <summary>
        ///     Convert to nullable float from DataReader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static float? ToNullFloat(this IDataReader dataReader, string columnName)
        => dataReader.IsWithValue(columnName)
            ? float.Parse(dataReader[columnName].ToString())
            : (float?)null;
    }
}