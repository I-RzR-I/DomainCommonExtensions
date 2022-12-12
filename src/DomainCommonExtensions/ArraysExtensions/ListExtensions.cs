// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-12-08 22:27
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-12-08 22:30
// ***********************************************************************
//  <copyright file="ListExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections.Generic;
using System.Data;
using DomainCommonExtensions.DataTypeExtensions;

#endregion

namespace DomainCommonExtensions.ArraysExtensions
{
    /// <summary>
    ///     List extensions
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        ///     Cast list to DataSet
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        /// <typeparam name="T"></typeparam>
        /// <remarks></remarks>
        public static DataSet ToDataSet<T>(this IList<T> list)
        {
            var elementType = typeof(T);
            var ds = new DataSet();
            var t = new DataTable();
            ds.Tables.Add(t);

            //add a column to table for each public property on T
            foreach (var propInfo in elementType.GetProperties())
            {
                var colType = Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType;

                t.Columns.Add(propInfo.Name, colType);
            }

            //go through each property on T and add each value to the table
            foreach (var item in list)
            {
                var row = t.NewRow();

                foreach (var propInfo in elementType.GetProperties())
                    row[propInfo.Name] = propInfo.GetValue(item, null) ?? DBNull.Value;

                t.Rows.Add(row);
            }

            return ds;
        }

        /// <summary>
        ///     Removes the first element
        /// </summary>
        /// <param name="list">Input list</param>
        /// <returns></returns>
        /// <typeparam name="T">List type</typeparam>
        /// <remarks>Returns false if list is empty</remarks>
        public static bool RemoveFirst<T>(this IList<T> list)
        {
            var lc = list.Count;
            if (lc.IsZero()) return false;
            list.RemoveAt(0);

            return true;
        }

        /// <summary>
        ///     Removes the last element
        /// </summary>
        /// <param name="list">Input list</param>
        /// <returns></returns>
        /// <typeparam name="T">List type</typeparam>
        /// <remarks>Returns false if list is empty</remarks>
        public static bool RemoveLast<T>(this IList<T> list)
        {
            var lc = list.Count;
            if (lc.IsZero()) return false;
            list.RemoveAt(lc - 1);

            return true;
        }
    }
}