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

#region U S A G E S

using System;
using System.Linq;
using RzR.Extensions.Domain.Primitives;

#endregion

namespace RzR.Extensions.Domain.Collections
{
    /// <summary>
    ///     Array extensions.
    /// </summary>
    public static class ArrayExtensions
    {
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

        /// <summary>
        ///     Append an item to source array.
        /// </summary>
        /// <typeparam name="T">Generic type parameter. Source type.</typeparam>
        /// <param name="source">Input source.</param>
        /// <param name="item">The item.</param>
        /// <returns>
        ///     Returns a new T[].
        /// </returns>
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

        /// <summary>
        ///     Append an item/s to source array.
        /// </summary>
        /// <typeparam name="T">Generic type parameter. Source type.</typeparam>
        /// <param name="source">Input source.</param>
        /// <param name="items">A variable-length parameters list containing items.</param>
        /// <returns>
        ///     Returns a new T[].
        /// </returns>
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

        /// <summary>
        ///     Overwrites the element at <paramref name="index"/> of the source array with <paramref name="item"/>.
        /// </summary>
        /// <remarks>
        ///     Despite its name, this overload does not insert or append: it replaces source[index] and
        ///     mutates the caller's array;
        ///     the array length never changes. It returns <paramref name="source"/> unchanged when <paramref name="item"/>
        ///     is null or <paramref name="index"/> is negative. Overload trap: for an int[] source, a
        ///     call such as source.AppendItem(1, 2) binds to this overload and sets source[1] = 2; to
        ///     append values, pass an array, e.g. source.AppendItem(new[] { 1, 2 }). The non-mutating
        ///     alternatives are <see cref="InsertAt{T}(T[], int, T)"/>, which inserts the item into a
        ///     new array, and <see cref="SetAt{T}(T[], int, T)"/>, which replaces the element in a copy. 
        /// </remarks>
        /// <exception cref="IndexOutOfRangeException">
        ///     Thrown when <paramref name="index"/> is greater than or equal to the length of <paramref name="source"/>.
        /// </exception>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="source">Input source; it is modified in place.</param>
        /// <param name="index">Zero-based index of the element to overwrite.</param>
        /// <param name="item">The item.</param>
        /// <returns>
        ///     The same <paramref name="source"/> instance (not a copy); a new empty array when <paramref name="source"/>
        ///     is null or empty.
        /// </returns>
        public static T[] AppendItem<T>(this T[] source, int index, T item)
        {
            if (source.IsNullOrEmptyEnumerable()) 
                return new T[] { };

            if (item.IsNull()) 
                return source;

            if (index.IsLessZero())
                return source;

            source[index] = item;

            return source;
        }

        /// <summary>
        ///     Returns a new array with <paramref name="item"/> inserted at <paramref name="index"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="source"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="index"/> is negative or greater than the length of <paramref name="source"/>.
        /// </exception>
        /// <typeparam name="T">Generic type parameter. Source type.</typeparam>
        /// <param name="source">Input source; it is never modified.</param>
        /// <param name="index">
        ///     Zero-based position of the inserted item, from 0 to the length of <paramref name="source"/>
        ///     (inclusive, which appends the item).
        /// </param>
        /// <param name="item">The item to insert; a null item is inserted as is.</param>
        /// <returns>
        ///     A new T[] one element longer than <paramref name="source"/>.
        /// </returns>
        public static T[] InsertAt<T>(this T[] source, int index, T item)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (index < 0 || index > source.Length)
                throw new ArgumentOutOfRangeException(nameof(index), index,
                    "Index must be between 0 and the length of the source array (inclusive).");

            var result = new T[source.Length + 1];
            Array.Copy(source, 0, result, 0, index);
            
            result[index] = item;
            Array.Copy(source, index, result, index + 1, source.Length - index);

            return result;
        }

        /// <summary>
        ///     Returns a copy of the source array with the element at <paramref name="index"/> replaced
        ///     by <paramref name="item"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="source"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="index"/> is negative or not less than the length 
        ///     of <paramref name="source"/>.
        /// </exception>
        /// <typeparam name="T">Generic type parameter. Source type.</typeparam>
        /// <param name="source">Input source; it is never modified.</param>
        /// <param name="index">Zero-based index of the element to replace.</param>
        /// <param name="item">The new item; a null item is set as is.</param>
        /// <returns>
        ///     A new T[] with the same length as <paramref name="source"/>.
        /// </returns>
        public static T[] SetAt<T>(this T[] source, int index, T item)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (index < 0 || index >= source.Length)
                throw new ArgumentOutOfRangeException(nameof(index), index,
                    "Index must be between 0 and the length of the source array (exclusive).");

            var result = new T[source.Length];
            Array.Copy(source, result, source.Length);
            result[index] = item;

            return result;
        }

        /// <summary>
        ///     Append an item to source array.
        /// </summary>
        /// <remarks>
        ///     The item is appended only when <paramref name="source"/> does not already contain it
        ///     (compared with Equals); otherwise <paramref name="source"/> is returned unchanged. A null
        ///     item is never appended.
        /// </remarks>
        /// <typeparam name="T">Generic type parameter. Source type.</typeparam>
        /// <param name="source">Input source.</param>
        /// <param name="itemToAppend">The item to append.</param>
        /// <returns>
        ///     Returns a new T[].
        /// </returns>
        public static T[] AppendIfNotExist<T>(this T[] source, T itemToAppend)
        {
            if (source.IsNullOrEmptyEnumerable() && itemToAppend.IsNull()) 
                return new T[] { };

            if (source.IsNullOrEmptyEnumerable()) 
                return new[] { itemToAppend };

            if (itemToAppend.IsNull())
                return source;

            if (source.IndexOf(itemToAppend) != -1)
                return source;

            return source.AppendItem(itemToAppend);
        }

        /// <summary>
        ///     Append an items to source array.
        /// </summary>
        /// <typeparam name="T">Generic type parameter. Source type.</typeparam>
        /// <param name="source">Input source.</param>
        /// <param name="itemsToAppend">The items to append.</param>
        /// <returns>
        ///     Returns a new T[].
        /// </returns>
        public static T[] AppendIfNotExists<T>(this T[] source, T[] itemsToAppend)
        {
            if (source.IsNullOrEmptyEnumerable() && itemsToAppend.IsNullOrEmptyEnumerable()) 
                return new T[] { };

            if (source.IsNullOrEmptyEnumerable()) 
                return itemsToAppend;

            if (itemsToAppend.IsNullOrEmptyEnumerable()) 
                return source;

            return source.AppendItem((from t in itemsToAppend let existIdx = source.IndexOf(t) where existIdx == -1 select t)
                .ToArray());
        }

        /// <summary>
        ///     Removes the item from source array.
        /// </summary>
        /// <typeparam name="T">Generic type parameter. Source type.</typeparam>
        /// <param name="source">Input source.</param>
        /// <param name="item">The item.</param>
        /// <returns>
        ///     Returns a new T[].
        /// </returns>
        public static T[] RemoveItem<T>(this T[] source, T item)
        {
            if (source.IsNullOrEmptyEnumerable())
                return new T[] { };

            if (item.IsNull())
                return source;

            var idx = source.IndexOf(item);

            return source.RemoveAtIdx(idx);
        }

        /// <summary>
        ///     A T[] extension method that removes at index.
        /// </summary>
        /// <typeparam name="T">Generic type parameter. Source type.</typeparam>
        /// <param name="source">Input source.</param>
        /// <param name="index">Zero-based index of the.</param>
        /// <returns>
        ///     A T[].
        /// </returns>
        public static T[] RemoveAtIdx<T>(this T[] source, int index)
        {
            if (source.IsNullOrEmptyEnumerable()) 
                return new T[] { };

            if (index.IsLessZero()) 
                return source;

            T[] dest = new T[source.Length - 1];
            if (index > 0)
                Array.Copy(source, 0, dest, 0, index);

            if (index < source.Length - 1)
                Array.Copy(source, index + 1, dest, index, source.Length - index - 1);

            return dest;
        }

        /// <summary>
        ///     Not null array
        /// </summary>
        /// <param name="array">Input array</param>
        /// <returns></returns>
        /// <typeparam name="T">Array type</typeparam>
        /// <remarks></remarks>
        public static T[] NotNull<T>(this T[] array)
        {
            return array ?? new T[] { };
        }
    }
}
