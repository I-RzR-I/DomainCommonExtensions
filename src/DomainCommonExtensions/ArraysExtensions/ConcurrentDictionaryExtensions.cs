// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2025-10-09 18:10
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-10-09 18:05
// ***********************************************************************
//  <copyright file="ConcurrentDictionaryExtensions.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using DomainCommonExtensions.Utilities.Ensure;

#endregion

namespace DomainCommonExtensions.ArraysExtensions
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A concurrent dictionary extensions.
    /// </summary>
    /// =================================================================================================
    public static class ConcurrentDictionaryExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A ConcurrentDictionary&lt;TKey,TValue&gt; extension method that adds an or update key and value
        /// </summary>
        /// <typeparam name="TKey">Type of the key.</typeparam>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="source">The source to act on.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// =================================================================================================
        public static void AddOrUpdate<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> source, 
            TKey key, TValue value)
        {
            DomainEnsure.IsNotNull(source, nameof(source));
            DomainEnsure.IsNotNull(key, nameof(key));

            source.AddOrUpdate(key, value, (oldKey, oldValue) => value);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A ConcurrentDictionary&lt;TKey,TValue&gt; extension method that adds an or update key and value
        /// </summary>
        /// <typeparam name="TKey">Type of the key.</typeparam>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="source">The source to act on.</param>
        /// <param name="keyValuePair">The key value pair.</param>
        /// =================================================================================================
        public static void AddOrUpdate<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> source,
            KeyValuePair<TKey, TValue> keyValuePair)
        {
            DomainEnsure.IsNotNull(source, nameof(source));
            DomainEnsure.IsNotNull(keyValuePair, nameof(keyValuePair));

            source.AddOrUpdate(keyValuePair.Key, keyValuePair.Value);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A ConcurrentDictionary&lt;TKey,TValue&gt; extension method that adds an or update key and value
        /// </summary>
        /// <typeparam name="TKey">Type of the key.</typeparam>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="source">The source to act on.</param>
        /// <param name="keyValuePairs">The key value pairs.</param>
        /// =================================================================================================
        public static void AddOrUpdate<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> source, 
            IEnumerable<KeyValuePair<TKey, TValue>> keyValuePairs)
        {
            DomainEnsure.IsNotNull(source, nameof(source));
            DomainEnsure.IsNotNull(keyValuePairs, nameof(keyValuePairs));

            keyValuePairs.ForEach(source.AddOrUpdate);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A ConcurrentDictionary&lt;TKey,TValue&gt; extension method that removes this object.
        /// </summary>
        /// <typeparam name="TKey">Type of the key.</typeparam>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="dictionary">The dictionary to act on.</param>
        /// <param name="key">The key.</param>
        /// =================================================================================================
        public static void Remove<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary, TKey key)
        {
            dictionary.TryRemove(key, out _);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A ConcurrentDictionary&lt;TKey,TValue&gt; extension method that removes this object.
        /// </summary>
        /// <typeparam name="TKey">Type of the key.</typeparam>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="dictionary">The dictionary to act on.</param>
        /// <param name="keys">The keys.</param>
        /// =================================================================================================
        public static void Remove<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary,
            IEnumerable<TKey> keys)
        {
            foreach (var key in keys) 
                dictionary.Remove(key);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A ConcurrentDictionary&lt;TKey,TValue&gt; extension method that removes the where.
        /// </summary>
        /// <typeparam name="TKey">Type of the key.</typeparam>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="dictionary">The dictionary to act on.</param>
        /// <param name="predicate">The predicate.</param>
        /// =================================================================================================
        public static void RemoveWhere<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary,
            Func<TValue, bool> predicate)
        {
            dictionary.Remove(dictionary.Where(kvp => predicate(kvp.Value)).Select(kvp => kvp.Key));
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A ConcurrentDictionary&lt;TKey,TValue&gt; extension method that try remove.
        /// </summary>
        /// <typeparam name="TKey">Type of the key.</typeparam>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="dictionary">The dictionary to act on.</param>
        /// <param name="key">The key.</param>
        /// =================================================================================================
        public static void TryRemove<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary, TKey key)
        {
            dictionary.TryRemove(key, out _);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A ConcurrentDictionary&lt;TKey,TValue&gt; extension method that removes all.
        /// </summary>
        /// <typeparam name="TKey">Type of the key.</typeparam>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="dictionary">The dictionary to act on.</param>
        /// <param name="keys">The keys.</param>
        /// =================================================================================================
        public static void RemoveAll<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary,
            IEnumerable<TKey> keys)
        {
            foreach (var key in keys) 
                dictionary.TryRemove(key, out _);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A ConcurrentDictionary&lt;TKey,TValue&gt; extension method that removes all.
        /// </summary>
        /// <typeparam name="TKey">Type of the key.</typeparam>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="dictionary">The dictionary to act on.</param>
        /// <param name="predicate">The predicate.</param>
        /// =================================================================================================
        public static void RemoveAll<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary,
            Func<TValue, bool> predicate)
        {
            var keys = dictionary.Where(p => predicate(p.Value)).Select(p => p.Key).ToArray();

            RemoveAll(dictionary, keys);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A ConcurrentDictionary&lt;TKey,TValue&gt; extension method that gets value or default.
        /// </summary>
        /// <typeparam name="TKey">Type of the key.</typeparam>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="dictionary">The dictionary to act on.</param>
        /// <param name="key">The key.</param>
        /// <returns>
        ///     The value or default.
        /// </returns>
        /// =================================================================================================
        public static TValue GetValueOrDefault<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary,
            TKey key)
        {
            if (dictionary.TryGetValue(key, out var value)) 
                return value;

            return default(TValue);
        }
    }
}