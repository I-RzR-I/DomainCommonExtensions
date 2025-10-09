// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-12-08 22:46
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-12-08 22:46
// ***********************************************************************
//  <copyright file="EnumerableExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using DomainCommonExtensions.CommonExtensions;
using DomainCommonExtensions.CommonExtensions.Reflection;
using DomainCommonExtensions.CommonExtensions.TypeParam;
using DomainCommonExtensions.DataTypeExtensions;
using DomainCommonExtensions.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using DomainCommonExtensions.Utilities.Ensure;

// ReSharper disable UnusedParameter.Local
// ReSharper restore PossibleMultipleEnumeration

#endregion

namespace DomainCommonExtensions.ArraysExtensions
{
    /// <summary>
    ///     Enumerable extensions
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        ///     Replace item in source
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IEnumerable<T> Replace<T>(this IEnumerable<T> enumerable, int index, T value)
        {
            var current = 0;
            foreach (var item in enumerable)
            {
                yield return current == index ? value : item;
                current++;
            }
        }

        /// <summary>
        ///     Join as string
        /// </summary>
        /// <param name="source"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string Join(this IEnumerable<string> source, string separator)
        {
            return source.IsNull() ? string.Empty : string.Join(separator, source);
        }

        /// <summary>
        ///     Is last
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool IsLast<T>(this IEnumerable<T> items, T item)
        {
            var list = items?.ToList() ?? new List<T>();
            if (list.IsNullOrEmptyEnumerable())
                return false;
            var last = list.ElementAt(list.Count - 1);

            return item.Equals(last);
        }

        /// <summary>
        ///     Is first
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool IsFirst<T>(this IEnumerable<T> items, T item)
        {
            var list = items?.ToList() ?? new List<T>();
            if (list.IsNullOrEmptyEnumerable())
                return false;
            var first = list.FirstOrDefault();

            return item.Equals(first);
        }

#if NET45_OR_GREATER || NET || NETSTANDARD1_0_OR_GREATER
        /// <summary>
        ///     Get differences from 2 list
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static (IList<TItem>, IList<TItem>) GetDifferences<TItem>(this IEnumerable<TItem> source,
            IEnumerable<TItem> target)
        {
            var aData = source?.ToList() ?? new List<TItem>();
            var bData = target?.ToList() ?? new List<TItem>();

            var toExclude = aData.Where(left => !bData.Contains(left)).ToList();
            var toAdd = bData.Where(right => !aData.Contains(right)).ToList();

            return (toAdd, toExclude);
        }
#endif

        /// <summary>
        ///     Contains any
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool ContainsAny<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource> target)
        {
            if (source.IsNull() || target.IsNull()) return false;

            var enumeratedSource = source.ToList();
            var enumeratedTarget = target.ToList();

            if (enumeratedSource.IsNullOrEmptyEnumerable() || enumeratedTarget.IsNullOrEmptyEnumerable()) return false;
            var common = enumeratedSource.Intersect(enumeratedTarget);

            return common.Any();
        }

        /// <summary>
        ///     Any start with
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool AnyStartWith(this IEnumerable<string> source, IEnumerable<string> target)
        {
            return source.Any(x => target.Any(y => x.StartsWith(y, StringComparison.InvariantCultureIgnoreCase)));
        }

        /// <summary>
        ///     Any start with
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool AnyStartWith(this IEnumerable<string> source, string target)
        {
            return source.Any(x => x.StartsWith(target, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        ///     To observable collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="coll"></param>
        /// <returns></returns>
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> coll)
        {
            var c = new ObservableCollection<T>();
            foreach (var e in coll)
                c.Add(e);

            return c;
        }

        /// <summary>
        ///     Randomize collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <returns></returns>
        public static IEnumerable<T> Randomize<T>(this IEnumerable<T> target)
        {
            var r = new Random();

            return target.OrderBy(x => r.Next());
        }

        /// <summary>
        ///     Transpose
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> Transpose<T>(this IEnumerable<IEnumerable<T>> collection)
        {
            var values = collection?.ToList() ?? new List<IEnumerable<T>>();
            if (values.IsNullOrEmptyEnumerable())
                return values;
            if (values.First().IsNullOrEmptyEnumerable())
                return Transpose(values.Skip(1));

            var x = values.First().First();
            var xs = values.First().Skip(1);
            var xss = values.Skip(1).ToList();

            return
                new[]
                    {
                        new[] {x}
                            .Concat(xss.Select(ht => ht.First()))
                    }
                    .Concat(new[] { xs }
                        .Concat(xss.Select(ht => ht.Skip(1)))
                        .Transpose());
        }

        /// <summary>
        ///     To collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static Collection<T> ToCollection<T>(this IEnumerable<T> enumerable)
        {
            var collection = new Collection<T>();
            foreach (var i in enumerable)
                collection.Add(i);

            return collection;
        }

        /// <summary>
        ///     Returns all combinations of a chosen amount of selected elements in the sequence.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the input sequence.</typeparam>
        /// <param name="source">The source for this extension method.</param>
        /// <param name="select">The amount of elements to select for every combination.</param>
        /// <param name="repetition">True when repetition of elements is allowed.</param>
        /// <returns>All combinations of a chosen amount of selected elements in the sequence.</returns>
        public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> source, int select,
            bool repetition = false)
        {
            Contract.Requires(source.IsNotNull());
            Contract.Requires(select.IsGreaterThanOrEqualZero());

            var enumerable = source.ToList();

            return select == 0
                ? new[] { new T[0] }
                : enumerable.SelectMany((element, index) =>
                    enumerable
                        .Skip(repetition ? index : index + 1)
                        .Combinations(select - 1, repetition)
                        .Select(c => new[] { element }.Concat(c)));
        }

        /// <summary>
        ///     List to System.Data.DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> data)
        {
            var props =
                TypeDescriptor.GetProperties(typeof(T));
            var table = new DataTable();
            for (var i = 0; i < props.Count; i++)
            {
                var prop = props[i];
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            var values = new object[props.Count];
            foreach (var item in data)
            {
                for (var i = 0; i < values.Length; i++) values[i] = props[i].GetValue(item) ?? DBNull.Value;
                table.Rows.Add(values);
            }

            return table;
        }

        /// <summary>
        ///     Dynamic list to System.Data.DataTable
        /// </summary>
        /// <param name="data">List of data</param>
        /// <returns></returns>
        /// <typeparam name="T">Anonymous type (dynamic) List<list type="dynamic"></list></typeparam>
        /// <remarks></remarks>
        public static DataTable ToDataTableDynamic<T>(this IEnumerable<T> data)
        {
            var table = new DataTable();
            var inputData = data.ToList();
            if (inputData.Any())
            {
                var props = inputData.ToArray()[0]
                    .GetType()
                    .GetProperties()
                    .Where(x => x.PropertyType.IsSimpleType())
                    .ToList();

                foreach (var prop in props) table.Columns.Add(prop.Name, typeof(string));

                var values = new object[props.Count];
                foreach (var item in inputData)
                {
                    for (var i = 0; i < values.Length; i++) values[i] = item.GetStringPropertyValue(props[i].Name);
                    table.Rows.Add(values);
                }
            }

            return table;
        }

        /// <summary>
        ///     Check is enumerable is null or empty
        /// </summary>
        /// <param name="source">Source data</param>
        /// <returns></returns>
        /// <typeparam name="T">Enumerable type</typeparam>
        /// <remarks></remarks>
        public static bool IsNullOrEmptyEnumerable<T>(this IEnumerable<T> source)
        {
            return source.IsNull() || !source.Any();
        }

#if NET45_OR_GREATER || NET || NETSTANDARD1_0_OR_GREATER
        /// <summary>
        ///     Format list to list with item index
        /// </summary>
        /// <param name="self">Input list</param>
        /// <returns></returns>
        /// <typeparam name="T">List type</typeparam>
        /// <remarks></remarks>
        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self)
        {
            return self?.Select((item, index) => (item, index)) ?? new List<(T, int)>();
        }
#endif

        /// <summary>
        ///     Format list to list with item index
        /// </summary>
        /// <param name="self">Input list</param>
        /// <returns></returns>
        /// <typeparam name="T">List type</typeparam>
        /// <remarks></remarks>
        public static IEnumerable<WithIndexModel<T>> WithIndexModel<T>(this IEnumerable<T> self)
        {
            return self?.Select(
                (item, index) => new WithIndexModel<T>()
                {
                    Index = index,
                    Item = item
                }) ?? new List<WithIndexModel<T>>();
        }

        /// <summary>
        ///     Generates a string from a list of values with the given delimiter
        /// </summary>
        /// <typeparam name="T">List element type</typeparam>
        /// <param name="list">List</param>
        /// <param name="delimiter">Separator</param>
        /// <returns>Result string</returns>
        public static string ListToString<T>(this IEnumerable<T> list, string delimiter)
        {
            // ReSharper disable PossibleMultipleEnumeration
            var result = new StringBuilder();

            if (list.IsNotNull() && list.Any())
            {
                var notFirst = false;
                foreach (var item in list)
                {
                    if (notFirst)
                        result.Append(delimiter);
                    result.Append(item);
                    notFirst = true;
                }
            }

            return result.ToString();
        }

        /// <summary>
        ///     Cloning a collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<T> CloneCollection<T>(this IEnumerable<T> source)
        {
            var result = new List<T>();
            // ReSharper disable once GenericEnumeratorNotDisposed
            var enm = source.GetEnumerator();
            while (enm.MoveNext())
                result.Add(enm.Current.Clone());

            return result.AsEnumerable();
        }

        /// <summary>
        ///     Not null list
        /// </summary>
        /// <param name="list">Input list</param>
        /// <returns></returns>
        /// <typeparam name="T">List type</typeparam>
        /// <remarks></remarks>
        public static IEnumerable<T> NotNull<T>(this IEnumerable<T> list)
        {
            return list ?? Enumerable.Empty<T>();
        }

        /// <summary>
        ///     Execute action for every item from list
        /// </summary>
        /// <param name="source">Source list</param>
        /// <param name="action">Action to execute</param>
        /// <typeparam name="T">Source type</typeparam>
        /// <remarks></remarks>
        public static void ActionForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }

        /// <summary>
        ///     Create string separated by space
        /// </summary>
        /// <param name="list">Source list</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToSpaceSeparatedString(this IEnumerable<string> list)
        {
            if (list.IsNull())
                return string.Empty;

            var sb = new StringBuilder();
            foreach (var element in list)
            {
                sb.Append(element + " ");
            }

            return sb.ToString().Trim();
        }

        /// <summary>
        ///     Check if list has duplicates
        /// </summary>
        /// <param name="list">source list</param>
        /// <param name="selector">Duplicate selector</param>
        /// <returns></returns>
        /// <typeparam name="T">Type of list</typeparam>
        /// <typeparam name="TProp"></typeparam>
        /// <remarks></remarks>
        public static bool HasDuplicates<T, TProp>(this IEnumerable<T> list, Func<T, TProp> selector)
        {
            var d = new HashSet<TProp>();

            return list.Any(t => !d.Add(selector(t)));
        }

        /// <summary>
        ///     Get list duplicates
        /// </summary>
        /// <param name="list">Source list</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static IEnumerable<string> GetDuplicates(this IEnumerable<string> list)
        {
            var duplicates = list
                .GroupBy(x => x)
                .Where(g => g.Count() > 1)
                .Select(y => y.Key)
                .ToArray();

            return duplicates.ToArray();
        }

        /// <summary>
        ///     Get list duplicates
        /// </summary>
        /// <param name="list">Source list</param>
        /// <typeparam name="T">Type of list</typeparam>
        /// <returns></returns>
        /// <remarks></remarks>
        public static IEnumerable<T> GetDuplicates<T>(this IEnumerable<T> list)
        {
            var duplicates = list
                .GroupBy(x => x)
                .Where(g => g.Count() > 1)
                .Select(y => y.Key)
                .ToArray();

            return duplicates.ToArray();
        }

        /// <summary>
        ///     Execute action for every item
        /// </summary>
        /// <param name="list">Source list</param>
        /// <param name="action">Action to be executed</param>
        /// <typeparam name="T">Type of list</typeparam>
        /// <returns></returns>
        /// <remarks></remarks>
        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (T item in list)
            {
                action(item);
            }
        }

        /// <summary>
        ///     Execute action for every item and return new data
        /// </summary>
        /// <param name="list">Source list</param>
        /// <param name="action">Action to be executed</param>
        /// <typeparam name="T">Type of list</typeparam>
        /// <returns></returns>
        /// <remarks></remarks>
        public static IEnumerable<T> ForEachAndReturn<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (T item in list)
            {
                action(item);
            }

            return list;
        }

        /// <summary>
        ///     Chunk source array of data by size.
        /// </summary>
        /// <param name="source">Source list/array.</param>
        /// <param name="chunkSize">The chunk size. Default value is 100.</param>
        /// <typeparam name="T">Type of list</typeparam>
        /// <returns></returns>
        /// <remarks></remarks>
        public static IEnumerable<IEnumerable<T>> Chunked<T>(this IEnumerable<T> source, int chunkSize = 100)
        {
            if (source.IsNullOrEmptyEnumerable())
                return new List<IEnumerable<T>>();
            if (chunkSize.IsNull() || chunkSize.IsLessOrEqualZero())
                return new List<IEnumerable<T>>(1) { source };

            var chunkArray = source.ToArray();
            var chunksCount = (int)Math.Ceiling((double)source.Count() / chunkSize);
            var chunks = new List<IEnumerable<T>>(chunksCount);

            for (var chunkIndex = 0; chunkIndex < chunksCount; chunkIndex++)
            {
                var newChunkSize = chunkIndex == chunksCount - 1 ? chunkArray.Length - (chunkSize * chunkIndex) : chunkSize;
                var newChunkSourceIndex = chunkIndex.IsZero() ? chunkIndex : (chunkSize * chunkIndex) - 1;

                var chunk = new T[newChunkSize];
                Array.Copy(chunkArray, newChunkSourceIndex, chunk, 0, newChunkSize);

                chunks.Add(chunk);
            }

            return chunks;
        }

        /// <summary>
        ///     Enumerates add if not exist in this collection.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="source">Source Enumerate.</param>
        /// <param name="item">Item to add.</param>
        /// <returns>
        ///     An enumerator that allows foreach to be used to process add if not exist in this
        ///     collection.
        /// </returns>
        public static IEnumerable<T> AddIfNotExist<T>(this IEnumerable<T> source, T item)
        {
            if (source.Any(x => x.Equals(item)).IsFalse())
                source = source.Concat(new[] { item });

            return source;
        }

        /// <summary>
        ///     An IEnumerable&lt;TSource&gt; extension method that converts from source type to destination.
        /// </summary>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <typeparam name="TDest">Type of the destination.</typeparam>
        /// <param name="source">Source array of data.</param>
        /// <param name="converter">The converter (Collection converter).</param>
        /// <returns>
        ///     A TDest[].
        /// </returns>
        public static TDest[] Convert<TSource, TDest>(this IEnumerable<TSource> source, Func<TSource, TDest> converter)
        {
            if (source.IsNullOrEmptyEnumerable().IsFalse())
            {
                DomainEnsure.IsNotNull(converter, nameof(converter));

                return source.Select(converter).ToArray();
            }
            else
                return new TDest[0];
        }

        /// <summary>
        ///     An IEnumerable extension method that converts this object to a querystring.
        /// </summary>
        /// <param name="collection">Collection of parameters value.</param>
        /// <param name="label">The label (name of the query parameter).</param>
        /// <returns>
        ///     The given data converted to a querystring.
        /// </returns>
        public static string ConvertToQuerystring(this IEnumerable collection, string label)
        {
            if (collection.IsNull())
                return null;

            DomainEnsure.IsNotNull(label, nameof(label));

            var nvc = new NameValueCollection();
            foreach (var value in collection)
            {
                nvc.Add(label, value.ToString());
            }

            var result = string.Join("&",
                nvc.AllKeys.Where(key => nvc[key].IsPresent())
                    .Select(key => string.Join("&", nvc.GetValues(key).NotNull().Select(val => ($"{key}={val}")))));

            return result;
        }
    }
}