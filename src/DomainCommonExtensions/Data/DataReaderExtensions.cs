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
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using RzR.Extensions.Domain.Internal.Data;
using RzR.Extensions.Domain.Primitives;
using RzR.Extensions.Domain.Validation;

#endregion

namespace RzR.Extensions.Domain.Data
{
    /// <summary>
    ///     DataReader extensions
    /// </summary>
    /// <remarks></remarks>
    public static class DataReaderExtensions
    {
        /// <summary>
        ///     Reads one row from DbDataReader, use only nullable types, generic types are not implemented yet. 
        ///     Returns default(T) if reader is empty
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
                        if (reader.HasColumn(item.Name).IsFalse())
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
        ///     Reads all remaining rows of a data reader and maps each one to an item.
        /// </summary>
        /// <remarks>
        ///     Only the current result set is read. The reader is not closed or disposed.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="reader"/> or <paramref name="map"/> is null.
        /// </exception>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="reader">The data reader, positioned before the first row to read.</param>
        /// <param name="map">
        ///     Maps the current row to an item. The record passed in is the reader itself, so the map
        ///     must copy what it needs and must not keep the record.
        /// </param>
        /// <returns>
        ///     The mapped items, in reader order; empty when there are no rows.
        /// </returns>
        public static List<T> ReadAll<T>(this IDataReader reader, Func<IDataRecord, T> map)
        {
            DomainEnsure.IsNotNull(reader, nameof(reader));
            DomainEnsure.IsNotNull(map, nameof(map));

            var items = new List<T>();
            while (reader.Read())
                items.Add(map(reader));

            return items;
        }

#if NET45_OR_GREATER || NET || NETSTANDARD1_0_OR_GREATER

        /// <summary>
        ///     Asynchronously reads all remaining rows of a data reader and maps each one to an item.
        /// </summary>
        /// <remarks>
        ///     The arguments are checked before the task starts. Only the current result set is read.
        ///     The reader is not closed or disposed. Not available on .NET Framework 4.0.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="reader"/> or <paramref name="map"/> is null.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        ///     The returned task is canceled when <paramref name="cancellationToken"/> is canceled.
        /// </exception>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="reader">The data reader, positioned before the first row to read.</param>
        /// <param name="map">
        ///     Maps the current row to an item. The record passed in is the reader itself, so the map
        ///     must copy what it needs and must not keep the record.
        /// </param>
        /// <param name="cancellationToken">(Optional) The token to cancel the read.</param>
        /// <returns>
        ///     A task whose result is the mapped items, in reader order; empty when there are no rows.
        /// </returns>
        public static Task<List<T>> ReadAllAsync<T>(this DbDataReader reader, Func<IDataRecord, T> map,
            CancellationToken cancellationToken = default)
        {
            DomainEnsure.IsNotNull(reader, nameof(reader));
            DomainEnsure.IsNotNull(map, nameof(map));

            return ReadAllCoreAsync(reader, map, cancellationToken);
        }

        /// <summary>
        ///     Reads and maps the rows for <see cref="ReadAllAsync{T}"/> once the arguments are checked.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="reader">The data reader.</param>
        /// <param name="map">Maps the current row to an item.</param>
        /// <param name="cancellationToken">The token to cancel the read.</param>
        /// <returns>
        ///     A task whose result is the mapped items.
        /// </returns>
        private static async Task<List<T>> ReadAllCoreAsync<T>(DbDataReader reader, Func<IDataRecord, T> map,
            CancellationToken cancellationToken)
        {
            var items = new List<T>();
            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            {
                cancellationToken.ThrowIfCancellationRequested();
                items.Add(map(reader));
            }

            return items;
        }
#endif

        /// <summary>
        ///     Checks if a column value is DBNull
        /// </summary>
        /// <param name="dataReader">The data reader</param>
        /// <param name="columnName">The column name</param>
        /// <returns>A bool indicating if the column value is DBNull</returns>
        /// <remarks>
        ///     Only DBNull counts; empty or whitespace strings are values. A missing column throws the data
        ///     provider's exception.
        /// </remarks>
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
        /// <remarks>
        ///     DBNull and an empty text count as empty; a whitespace-only text (for example " ") counts as a
        ///     value, unlike <see cref="IsWithValue"/>. A missing column throws the data provider's exception.
        /// </remarks>
        public static bool IsDbNullOrEmpty(this IDataReader dataReader, string columnName)
        {
            return dataReader[columnName] == DBNull.Value || dataReader[columnName].ToString() == string.Empty;
        }

        /// <summary>
        ///     Checks if reader column has value
        /// </summary>
        /// <param name="dataReader">The data reader</param>
        /// <param name="columnName">The column name</param>
        /// <returns>A bool indicating if the column has a value</returns>
        /// <remarks>
        ///     DBNull, null, an empty or whitespace-only text and a whitespace char mean "no value"; this is the
        ///     rule the DataReader <c>To*</c> extensions use. A missing column throws the data provider's exception.
        /// </remarks>
        public static bool IsWithValue(this IDataReader dataReader, string columnName)
        {
            return DataValueConverter.TryRead(dataReader, columnName, out _);
        }

        /// <summary>
        ///     Checks if a column exists in a data reader
        /// </summary>
        /// <param name="dataReader">The data reader</param>
        /// <param name="columnName">The column name</param>
        /// <returns>A bool indicating the column exists</returns>
        /// <remarks>
        ///     Uses the provider's own name matching. A missing column returns false whether the provider throws
        ///     <see cref="IndexOutOfRangeException"/> (SqlClient, Npgsql, MySqlConnector) or
        ///     <see cref="ArgumentException"/> / <see cref="ArgumentOutOfRangeException"/> (DataTableReader,
        ///     Microsoft.Data.Sqlite). A null column name still throws <see cref="ArgumentNullException"/>.
        /// </remarks>
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
            catch (ArgumentException exception) when (!(exception is ArgumentNullException))
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
        /// <remarks>
        ///     A missing column returns false, as in <see cref="ContainsColumn"/>. DBNull and an empty text return
        ///     false; a whitespace-only text counts as a value.
        /// </remarks>
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
            catch (ArgumentException exception) when (!(exception is ArgumentNullException))
            {
                return false;
            }
        }
    }
}
