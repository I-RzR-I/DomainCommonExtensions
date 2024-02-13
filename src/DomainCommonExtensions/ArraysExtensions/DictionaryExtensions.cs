// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-08-10 21:39
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-12 23:39
// ***********************************************************************
//  <copyright file="DictionaryExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections;
using System.Collections.Generic;
using DomainCommonExtensions.DataTypeExtensions;

#endregion

namespace DomainCommonExtensions.ArraysExtensions
{
    /// <summary>
    ///     Dictionary extensions
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        ///     Add range new props to dictionary
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="context"></param>
        /// <param name="newItems"></param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> AddRange<TKey, TValue>(this Dictionary<TKey, TValue> context,
            Dictionary<TKey, TValue> newItems)
        {
            context ??= new Dictionary<TKey, TValue>();
            foreach (var item in newItems)
                if (context.ContainsKey(item.Key).IsTrue())
                    context[item.Key] = item.Value;
                else
                    context.Add(item.Key, item.Value);

            return context;
        }

        /// <summary>
        ///     Remove keys
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="dict"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> RemoveKeys<TKey, TValue>(this Dictionary<TKey, TValue> dict,
            IEnumerable<TKey> keys)
        {
            foreach (var key in keys)
                if (dict.ContainsKey(key).IsTrue())
                    dict.Remove(key);

            return dict;
        }

        /// <summary>
        ///     Get index of item
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int IndexOf(this IDictionary dictionary, object value)
        {
            for (var i = 0; i < dictionary.Count; ++i)
                if (dictionary[i] == value)
                    return i;

            return -1;
        }

        /// <summary>
        ///     Get STRING or default value
        /// </summary>
        /// <param name="dictionary">Required. Input dictionary</param>
        /// <param name="key">Required. Search key</param>
        /// <param name="default">Optional. The default value is default.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetStringOrDefault(this Dictionary<string, string> dictionary, string key,
                    string @default = default)
        {
            if (dictionary.ContainsKey(key).IsTrue()) return dictionary[key];

            return @default;
        }

        /// <summary>
        ///     Get GUID or default value
        /// </summary>
        /// <param name="dictionary">Required. Input dictionary</param>
        /// <param name="key">Required. Search key</param>
        /// <param name="default">Optional. The default value is default.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Guid GetGuidOrDefault(this Dictionary<string, string> dictionary, string key,
            Guid @default = default)
        {
            if (dictionary.ContainsKey(key).IsTrue())
                if (Guid.TryParse(dictionary[key], out var value))
                    return value;

            return @default;
        }

        /// <summary>
        ///     Get INT or default value
        /// </summary>
        /// <param name="dictionary">Required. Input dictionary</param>
        /// <param name="key">Required. Search key</param>
        /// <param name="default">Optional. The default value is default.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int GetIntOrDefault(this Dictionary<string, string> dictionary, string key,
            int @default = default)
        {
            if (dictionary.ContainsKey(key).IsTrue())
                if (int.TryParse(dictionary[key], out var value))
                    return value;

            return @default;
        }

        /// <summary>
        ///     Get BOOL or default value
        /// </summary>
        /// <param name="dictionary">Required. Input dictionary</param>
        /// <param name="key">Required. Search key</param>
        /// <param name="default">Optional. The default value is default.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool GetBoolOrDefault(this Dictionary<string, string> dictionary, string key,
            bool @default = default)
        {
            if (dictionary.ContainsKey(key).IsTrue())
                if (bool.TryParse(dictionary[key], out var value))
                    return value;

            return @default;
        }
    }
}