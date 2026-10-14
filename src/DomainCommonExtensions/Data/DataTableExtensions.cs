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

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using RzR.Extensions.Domain.Internal.Data;
using RzR.Extensions.Domain.Primitives;
using RzR.Extensions.Domain.Reflection;

#if NETSTANDARD2_0_OR_GREATER

using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using RzR.Extensions.Domain.Internal;

#endif

// ReSharper disable RedundantArgumentDefaultValue

#endregion

namespace RzR.Extensions.Domain.Data
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
        /// <remarks>
        ///     Each public, non-indexer property that has a setter (public or not) is filled from the column
        ///     with the same name; a hidden (new) property is filled once, through its most-derived declaration.
        ///     Properties without a setter or without a matching column are skipped. Enums are read from
        ///     numbers or names, Guids from strings, other string cells are parsed culture-invariantly first,
        ///     and typed cells convert as before. A DBNull cell sets the property's default value.
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataTable table) where T : new()
        {
            var list = new List<T>();
            var propertyColumns = GetPropertyColumns(table, typeof(T), null);

            foreach (DataRow row in table.Rows)
            {
                object obj = new T();
                SetPropertyValues(row, obj, propertyColumns);

                list.Add((T)obj);
            }

            return list;
        }

        /// <summary>
        ///     System.Data.DataTable to generic list T.
        /// </summary>
        /// <remarks>
        ///     Properties are filled and converted like <see cref="ToList{T}(DataTable)"/> does.
        /// </remarks>
        /// <param name="table">.</param>
        /// <param name="type">The type.</param>
        /// <param name="excludeProp">(Optional) The exclude property.</param>
        /// <returns>
        ///     The given data converted to a List&lt;dynamic&gt;
        /// </returns>
        public static List<dynamic> ToList(this DataTable table, Type type, string excludeProp = "Item")
        {
            var list = new List<dynamic>();
            var propertyColumns = GetPropertyColumns(table, type, excludeProp);

            foreach (DataRow row in table.Rows)
            {
                var obj = Activator.CreateInstance(type);
                SetPropertyValues(row, obj, propertyColumns);

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
        /// <remarks>
        ///     Each row is written as an object with one camel-cased property per column, in column order.
        ///     A DBNull cell is written as the default value of the column's data type. Column names that
        ///     collide after camel-casing throw <see cref="InvalidOperationException"/> when the table has rows.
        /// </remarks>
        /// <param name="table">The table to act on.</param>
        /// <returns>
        ///     DataTable as a JSON string.
        /// </returns>
        public static string ToJson(this DataTable table)
        {
            var columns = table.Columns.Cast<DataColumn>().ToArray();
            var names = new JsonEncodedText[columns.Length];
            var nullValues = new object[columns.Length];
            var usedNames = new HashSet<string>(JsonObjectSerializer.PropertyNameComparer);

            for (var i = 0; i < columns.Length; i++)
            {
                var name = JsonObjectSerializer.ConvertName(columns[i].ColumnName);
                if (!usedNames.Add(name) && table.Rows.Count > 0)
                    throw new InvalidOperationException(
                        $"The JSON property name '{name}' of column '{columns[i].ColumnName}' collides with another column.");

                names[i] = JsonObjectSerializer.EncodeName(name);
                nullValues[i] = columns[i].DataType.IsValueType ? Activator.CreateInstance(columns[i].DataType) : null;
            }

            using (var stream = new MemoryStream())
            {
                using (var writer = JsonObjectSerializer.CreateWriter(stream))
                {
                    writer.WriteStartArray();
                    foreach (DataRow row in table.Rows)
                    {
                        writer.WriteStartObject();
                        for (var i = 0; i < columns.Length; i++)
                        {
                            var value = row[columns[i]];

                            writer.WritePropertyName(names[i]);
                            JsonObjectSerializer.Serialize(writer, DBNull.Value.Equals(value) ? nullValues[i] : value,
                                columns[i].DataType);
                        }

                        writer.WriteEndObject();
                    }

                    writer.WriteEndArray();
                }

                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }
#endif

        /// <summary>
        ///     Gets the most-derived, non-indexer properties with a setter (public or not) of a type that have a
        ///     column in the table.
        /// </summary>
        /// <param name="table">The table to read the columns from.</param>
        /// <param name="type">The type to read the properties from.</param>
        /// <param name="excludeProp">The name of a property to skip, or null.</param>
        /// <returns>
        ///     The column and property pairs.
        /// </returns>
        private static List<KeyValuePair<DataColumn, PropertyInfo>> GetPropertyColumns(DataTable table, Type type,
            string excludeProp)
        {
            var propertyColumns = new List<KeyValuePair<DataColumn, PropertyInfo>>();
            var properties = ReflectionExtensions.GetMostDerivedProperties(type,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static).Values;
            foreach (var property in properties)
            {
                if (property.Name == excludeProp || !property.CanWrite)
                    continue;

                var column = table.Columns[property.Name];
                if (column != null)
                    propertyColumns.Add(new KeyValuePair<DataColumn, PropertyInfo>(column, property));
            }

            return propertyColumns;
        }

        /// <summary>
        ///     Sets the property values of an object from a row.
        /// </summary>
        /// <param name="row">The row to read.</param>
        /// <param name="target">The object to fill.</param>
        /// <param name="propertyColumns">The column and property pairs.</param>
        private static void SetPropertyValues(DataRow row, object target,
            List<KeyValuePair<DataColumn, PropertyInfo>> propertyColumns)
        {
            foreach (var propertyColumn in propertyColumns)
            {
                var value = row[propertyColumn.Key];
                var safeValue = value.IsNull() || DBNull.Value.Equals(value)
                    ? null
                    : DataValueConverter.ChangeType(value, propertyColumn.Value.PropertyType, propertyColumn.Key.ColumnName);

                propertyColumn.Value.SetValue(target, safeValue, null);
            }
        }
    }
}
