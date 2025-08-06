// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-08-10 21:39
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-12 23:39
// ***********************************************************************
//  <copyright file="DataTableExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

#if NET || NETSTANDARD2_0_OR_GREATER
using DomainCommonExtensions.Helpers;
using DomainCommonExtensions.Helpers.Internal.AnonymousSelect.Factory;
#endif

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

// ReSharper disable RedundantArgumentDefaultValue

#endregion

namespace DomainCommonExtensions.CommonExtensions.SystemData
{
    /// <summary>
    ///     DataTable extensions
    /// </summary>
    /// <remarks></remarks>
    public static class DataTableExtensions
    {
        /// <summary>
        ///     System.Data.DataTable to generic list T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataTable table) where T : new()
        {
            var list = new List<T>();
            var typeProperties = typeof(T).GetProperties()
                .Select(propertyInfo => new
                {
                    PropertyInfo = propertyInfo,
                    Type = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType
                }).ToList();

            foreach (var row in table.Rows.Cast<DataRow>())
            {
                var obj = new T();
                foreach (var typeProperty in typeProperties)
                {
                    var value = row[typeProperty.PropertyInfo.Name];
                    var safeValue = value.IsNull() || DBNull.Value.Equals(value)
                        ? null
                        : Convert.ChangeType(value, typeProperty.Type);

                    typeProperty.PropertyInfo.SetValue(obj, safeValue, null);
                }

                list.Add(obj);
            }

            return list;
        }

        /// <summary>
        ///     System.Data.DataTable to generic list T.
        /// </summary>
        /// <param name="table">.</param>
        /// <param name="type">The type.</param>
        /// <param name="excludeProp">(Optional) The exclude property.</param>
        /// <returns>
        ///     The given data converted to a List&lt;dynamic&gt;
        /// </returns>
        public static List<dynamic> ToList(this DataTable table, Type type, string excludeProp = "Item")
        {
            var list = new List<dynamic>();
            var typeProperties = type.GetProperties().Where(x => x.Name != excludeProp)
                .Select(propertyInfo => new
                {
                    PropertyInfo = propertyInfo,
                    Type = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType
                }).ToList();

            foreach (var row in table.Rows.Cast<DataRow>())
            {
                var obj = Activator.CreateInstance(type);
                foreach (var typeProperty in typeProperties)
                {
                    var value = row[typeProperty.PropertyInfo.Name];
                    var safeValue = value.IsNull() || DBNull.Value.Equals(value)
                        ? null
                        : Convert.ChangeType(value, typeProperty.Type);

                    typeProperty.PropertyInfo.SetValue(obj, safeValue, null);
                }

                list.Add(obj);
            }

            return list;
        }

#if NET || NETSTANDARD2_0_OR_GREATER

        /// <summary>
        ///     A DataTable extension method that converts a table to a JSON.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="table">The table to act on.</param>
        /// <returns>
        ///     DataTable as a JSON string.
        /// </returns>
        public static string ToJson<T>(this DataTable table) where T : new()
        {
            return JsonObjectSerializer.ToString(table.ToList<T>());
        }

        /// <summary>
        ///     A DataTable extension method that converts a table to a JSON.
        /// </summary>
        /// <param name="table">The table to act on.</param>
        /// <returns>
        ///     DataTable as a JSON string.
        /// </returns>
        public static string ToJson(this DataTable table)
        {
            var columnCount = table.Columns.Count;
            var types = new Type[columnCount];
            var names = new string[columnCount];

            for (var i = 0; i < columnCount; i++)
            {
                types[i] = table.Columns[i].DataType;
                names[i] = table.Columns[i].ColumnName;
            }

            var type = AnonymousClassFactory.CreateType(types, names, true);
            var list = table.ToList(type);

            return JsonObjectSerializer.ToString(list);
        }
#endif
    }
}