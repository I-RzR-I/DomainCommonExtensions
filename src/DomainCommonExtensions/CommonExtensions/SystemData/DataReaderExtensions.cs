// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-08-10 21:39
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-12 23:39
// ***********************************************************************
//  <copyright file="DataReaderExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Data;
using System.Data.Common;
using DomainCommonExtensions.DataTypeExtensions;

#endregion

namespace DomainCommonExtensions.CommonExtensions.SystemData
{
    /// <summary>
    ///     DataReader extensions
    /// </summary>
    /// <remarks></remarks>
    public static class DataReaderExtensions
    {
        /// <summary>
        ///     Reads one row from DbDataReader, use only nullable types, generic types are not implemented yet. Returns default(T)
        ///     if reader is empty
        /// </summary>
        /// <param name="reader">Data reader</param>
        /// <param name="obj">Result data</param>
        /// <returns></returns>
        /// <typeparam name="T">Type of result</typeparam>
        /// <remarks></remarks>
        public static T ReadDbReaderOneRow<T>(this DbDataReader reader, T obj)
        {
            try
            {
                if (reader.Read())
                {
                    foreach (var item in obj.GetType().GetProperties())
                    {
                        if (!reader.HasColumn(item.Name))
                            continue;
                        item.SetValue(obj, reader[item.Name] == DBNull.Value ? null : reader[item.Name], null);
                    }

                    return obj;
                }

                return default;
            }
            catch (IndexOutOfRangeException indexOut)
            {
                throw new IndexOutOfRangeException(
                    "Possible the name of column does not equals with the model property name", indexOut);
            }
            catch (NullReferenceException nullReference)
            {
                throw new NullReferenceException("Possible reader is empty", nullReference);
            }
        }

        /// <summary>
        ///     Checks if a column value is DBNull
        /// </summary>
        /// <param name="dataReader">The data reader</param>
        /// <param name="columnName">The column name</param>
        /// <returns>A bool indicating if the column value is DBNull</returns>
        public static bool IsDbNull(this IDataReader dataReader, string columnName)
        {
            return dataReader[columnName] == DBNull.Value;
        }

        /// <summary>
        ///     Checks if a column value is not DBNull
        /// </summary>
        /// <param name="dataReader">The data reader</param>
        /// <param name="columnName">The column name</param>
        /// <returns>A bool indicating if the column value is not DBNull</returns>
        public static bool IsNotDbNull(this IDataReader dataReader, string columnName)
        {
            return dataReader[columnName] != DBNull.Value;
        }

        /// <summary>
        ///     Checks if a column value is DBNull or is Empty
        /// </summary>
        /// <param name="dataReader">The data reader</param>
        /// <param name="columnName">The column name</param>
        /// <returns>A bool indicating if the column value is DBNull</returns>
        public static bool IsDbNullOrEmpty(this IDataReader dataReader, string columnName)
        {
            return dataReader[columnName] == DBNull.Value || dataReader[columnName].ToString() == string.Empty;
        }

        /// <summary>
        ///     Checks if reader column has value
        /// </summary>
        /// <param name="dataReader">The data reader</param>
        /// <param name="columnName">The column name</param>
        /// <returns>A bool indicating if the column's value is DBNull</returns>
        public static bool IsWithValue(this IDataReader dataReader, string columnName)
        {
            return dataReader[columnName].IsNotDbNull() && dataReader[columnName].ToString().IsNotNull()
                                                          && dataReader[columnName].ToString().IsPresent();
        }

        /// <summary>
        ///     Checks if a column exists in a data reader
        /// </summary>
        /// <param name="dataReader">The data reader</param>
        /// <param name="columnName">The column name</param>
        /// <returns>A bool indicating the column exists</returns>
        public static bool ContainsColumn(this IDataReader dataReader, string columnName)
        {
            try
            {
                return dataReader.GetOrdinal(columnName).IsGreaterThanOrEqualZero();
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
        }

        /// <summary>
        ///     Checks if a column exists in a data reader and is not null or empty
        /// </summary>
        /// <param name="dataReader">The data reader</param>
        /// <param name="columnName">The column name</param>
        /// <returns>A bool indicating the column exists</returns>
        public static bool ContainsColumnAndIsNotNullOrEmpty(this IDataReader dataReader, string columnName)
        {
            try
            {
                if (dataReader.GetOrdinal(columnName).IsGreaterThanOrEqualZero())
                    return dataReader[columnName] != DBNull.Value &&
                           !string.IsNullOrEmpty(dataReader[columnName].ToString());

                return false;
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
        }
    }
}