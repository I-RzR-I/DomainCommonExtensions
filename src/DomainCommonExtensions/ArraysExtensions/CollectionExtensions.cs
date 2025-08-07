// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-08-10 21:39
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-12 23:39
// ***********************************************************************
//  <copyright file="CollectionExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using DomainCommonExtensions.CommonExtensions;

#endregion

namespace DomainCommonExtensions.ArraysExtensions
{
    /// <summary>
    ///     Collection extensions
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        ///     Add range
        /// </summary>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="context"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ICollection<TTarget> AddRange<TTarget>(this ICollection<TTarget> context,
            IEnumerable<TTarget> data)
        {
            if (context.IsNull()) return null;
            foreach (var item in data) context.Add(item);

            return context;
        }
        
        /// <summary>
        ///     Get distinct items by list propriety
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (var element in source.Where(element => seenKeys.Add(keySelector(element)))) yield return element;
        }
        
        /// <summary>
        ///     NameValueCollection to KeyValuePair
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, string>> ToKeyValuePair(this NameValueCollection collection)
        {
            return collection.AllKeys.Select(x => new KeyValuePair<string, string>(x, collection[x]));
        }

        /// <summary>
        ///     Check if element is in list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool In<T>(this T val, params T[] values) where T : struct
        {
            return values.Contains(val);
        }

        /// <summary>
        ///     Add given item to source collection
        /// </summary>
        /// <param name="source">Source collection</param>
        /// <param name="item">Item to add</param>
        /// <returns></returns>
        /// <typeparam name="T">Collection type</typeparam>
        /// <remarks></remarks>
        public static ICollection<T> With<T>(this ICollection<T> source, T item)
        {
            source.Add(item);
            
            return source;
        }

        /// <summary>
        ///     Add given items to source collection
        /// </summary>
        /// <param name="source">Source collection</param>
        /// <param name="items">Items to add</param>
        /// <returns></returns>
        /// <typeparam name="T">Collection type</typeparam>
        /// <remarks></remarks>
        public static ICollection<T> With<T>(this ICollection<T> source, T[] items)
        {
            source.AddRange(items);
            
            return source;
        }

        /// <summary>
        ///     Remove given item to source collection
        /// </summary>
        /// <param name="source">Source collection</param>
        /// <param name="item">Item to remove</param>
        /// <returns></returns>
        /// <typeparam name="T">Collection type</typeparam>
        /// <remarks></remarks>
        public static ICollection<T> Without<T>(this ICollection<T> source, T item)
        {
            source.Remove(item);
            
            return source;
        }

        /// <summary>
        ///     Adds the given items to the source and returns the source for fluent syntax
        /// </summary>
        /// <param name="source">Source collection</param>
        /// <param name="items">Items to add</param>
        /// <returns></returns>
        /// <typeparam name="T">Collection type</typeparam>
        /// <remarks></remarks>
        public static ICollection<T> WithMany<T>(this ICollection<T> source, IEnumerable<T> items)
        {
            source.AddRange(items);
            
            return source;
        }
    }
}