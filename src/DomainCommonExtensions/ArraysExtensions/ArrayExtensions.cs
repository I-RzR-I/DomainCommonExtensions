// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-12-08 09:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-12-08 09:49
// ***********************************************************************
//  <copyright file="ArrayExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DomainCommonExtensions.CommonExtensions;
using DomainCommonExtensions.DataTypeExtensions;

namespace DomainCommonExtensions.ArraysExtensions
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Array extensions.
    /// </summary>
    /// =================================================================================================
    public static class ArrayExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Returns the first index of the given value.
        /// </summary>
        /// <remarks>
        ///     In case when no found any value result is -1.
        /// </remarks>
        /// <typeparam name="T">Type of input.</typeparam>
        /// <param name="source">Input source.</param>
        /// <param name="value">Value to search.</param>
        /// <returns>
        ///     An int.
        /// </returns>
        /// =================================================================================================
        public static int IndexOf<T>(this T[] source, T value)
        {
            const int index = -1;
            try
            {
                if (source.IsNullOrEmptyEnumerable()) return index;

                for (var i = 0; i < source.Length; i++)
                    if (Equals(source[i], value))
                        return i;
            }
            catch
            {
                return index;
            }

            return index;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Append an item to source array.
        /// </summary>
        /// <typeparam name="T">Generic type parameter. Source type.</typeparam>
        /// <param name="source">Input source.</param>
        /// <param name="item">The item.</param>
        /// <returns>
        ///     Returns a new T[].
        /// </returns>
        /// =================================================================================================
        public static T[] AppendItem<T>(this T[] source, T item)
        {
            if (source.IsNullOrEmptyEnumerable() && item.IsNull()) return new T[] { };
            if (item.IsNull()) return source;
            if (source.IsNullOrEmptyEnumerable()) return new[] { item };

            var result = new T[source.Length + 1];
            source.CopyTo(result, 0);
            result[source.Length] = item;

            return result;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Append an item/s to source array.
        /// </summary>
        /// <typeparam name="T">Generic type parameter. Source type.</typeparam>
        /// <param name="source">Input source.</param>
        /// <param name="items">A variable-length parameters list containing items.</param>
        /// <returns>
        ///     Returns a new T[].
        /// </returns>
        /// =================================================================================================
        public static T[] AppendItem<T>(this T[] source, params T[] items)
        {
            if (source.IsNullOrEmptyEnumerable() && items.IsNullOrEmptyEnumerable()) return new T[] { };
            if (items.IsNullOrEmptyEnumerable()) return source;
            if (source.IsNullOrEmptyEnumerable()) return items;

            var result = new T[source.Length + items.Length];
            source.CopyTo(result, 0);
            items.CopyTo(result, source.Length);

            return result;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Append an item to source array.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="source">Input source.</param>
        /// <param name="index">Zero-based index of the.</param>
        /// <param name="item">The item.</param>
        /// <returns>
        ///     Returns a new T[].
        /// </returns>
        /// =================================================================================================
        public static T[] AppendItem<T>(this T[] source, int index, T item)
        {
            if (source.IsNullOrEmptyEnumerable()) return new T[] { };
            if (item.IsNull()) return source;
            if (index.IsLessZero()) return source;

            source[index] = item;

            return source;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Append an items to source array.
        /// </summary>
        /// <typeparam name="T">Generic type parameter. Source type.</typeparam>
        /// <param name="source">Input source.</param>
        /// <param name="itemsToAppend">The items to append.</param>
        /// <returns>
        ///     Returns a new T[].
        /// </returns>
        /// =================================================================================================
        public static T[] AppendIfNotExists<T>(this T[] source, T[] itemsToAppend)
        {
            if (source.IsNullOrEmptyEnumerable() && itemsToAppend.IsNullOrEmptyEnumerable()) return new T[] { };
            if (source.IsNullOrEmptyEnumerable()) return itemsToAppend;
            if (itemsToAppend.IsNullOrEmptyEnumerable()) return source;

            return source.AppendItem((from t in itemsToAppend let existIdx = source.IndexOf(t) where existIdx == -1 select t).ToArray());
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Removes the item from source array.
        /// </summary>
        /// <typeparam name="T">Generic type parameter. Source type.</typeparam>
        /// <param name="source">Input source.</param>
        /// <param name="item">The item.</param>
        /// <returns>
        ///     Returns a new T[].
        /// </returns>
        /// =================================================================================================
        public static T[] RemoveItem<T>(this T[] source, T item)
        {
            if (source.IsNullOrEmptyEnumerable()) return new T[] { }; 
            if (item.IsNull()) return source;

            var idx = source.IndexOf(item);

            return source.RemoveAtIdx(idx);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A T[] extension method that removes at index.
        /// </summary>
        /// <typeparam name="T">Generic type parameter. Source type.</typeparam>
        /// <param name="source">Input source.</param>
        /// <param name="index">Zero-based index of the.</param>
        /// <returns>
        ///     A T[].
        /// </returns>
        /// =================================================================================================
        public static T[] RemoveAtIdx<T>(this T[] source, int index)
        {
            if (source.IsNullOrEmptyEnumerable()) return new T[] { }; 
            if (index.IsLessZero()) return source;

            T[] dest = new T[source.Length - 1];
            if (index > 0)
                Array.Copy(source, 0, dest, 0, index);

            if (index < source.Length - 1)
                Array.Copy(source, index + 1, dest, index, source.Length - index - 1);

            return dest;
        }
    }
}