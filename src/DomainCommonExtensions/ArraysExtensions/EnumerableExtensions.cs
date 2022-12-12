﻿// ***********************************************************************
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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using DomainCommonExtensions.CommonExtensions;

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
            return source == null ? string.Empty : string.Join(separator, source);
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
            if (!list.Any())
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
            if (!list.Any())
                return false;
            var first = list.FirstOrDefault();

            return item.Equals(first);
        }

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

            if (!enumeratedSource.Any() || !enumeratedTarget.Any()) return false;
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
            if (!values.Any())
                return values;
            if (!values.First().Any())
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
            Contract.Requires(source != null);
            Contract.Requires(select >= 0);

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

            if (!list.IsNull() && list.Any())
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
            // ReSharper restore PossibleMultipleEnumeration
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
    }
}