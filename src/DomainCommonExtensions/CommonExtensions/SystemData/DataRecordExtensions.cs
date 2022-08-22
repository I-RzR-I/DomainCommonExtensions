// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-08-10 21:39
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-12 23:39
// ***********************************************************************
//  <copyright file="DataRecordExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Data;

#endregion

namespace DomainCommonExtensions.CommonExtensions.SystemData
{
    /// <summary>
    ///     DataRecord extensions
    /// </summary>
    /// <remarks></remarks>
    public static class DataRecordExtensions
    {
        /// <summary>
        ///     Checks if reader contains column with the name of model property
        /// </summary>
        /// <param name="dr">Data record</param>
        /// <param name="columnName">Column name</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool HasColumn(this IDataRecord dr, string columnName)
        {
            for (var i = 0; i < dr.FieldCount; i++)
                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;

            return false;
        }
    }
}