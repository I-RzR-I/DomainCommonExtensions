// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2026-01-06 18:01
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-01-06 20:11
// ***********************************************************************
//  <copyright file="IndexableEnumerable.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections;
using System.Collections.Generic;
using DomainCommonExtensions.Utilities.Ensure;

#endregion

namespace DomainCommonExtensions.Collections
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Indexable enumerable collection.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// =================================================================================================
    public interface IIndexableEnumerable<out T> : IEnumerable<T>, IDisposable
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Indexer to get items within this collection using array index syntax.
        /// </summary>
        /// <param name="index">Zero-based index of the entry to access.</param>
        /// <returns>
        ///     The indexed item.
        /// </returns>
        /// =================================================================================================
        T this[int index] { get; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the number of records. 
        /// </summary>
        /// <value>
        ///     The count.
        /// </value>
        /// =================================================================================================
        int Count { get; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Executes for each operation.
        /// </summary>
        /// <param name="action">The action.</param>
        /// =================================================================================================
        void DoForEach(Action<T> action);
    }

    /// -------------------------------------------------------------------------------------------------
    /// <inheritdoc cref="IIndexableEnumerable{T}"/>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <seealso cref="T:DomainCommonExtensions.Collections.IIndexableEnumerable{T}"/>
    /// =================================================================================================
    public sealed class IndexableEnumerable<T> : IIndexableEnumerable<T>
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the list.
        /// </summary>
        /// =================================================================================================
        private readonly IList<T> _list;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="IndexableEnumerable{T}"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="list">(Immutable) the list.</param>
        /// =================================================================================================
        public IndexableEnumerable(IList<T> list)
        {
            DomainEnsure.IsNotNull(list, nameof(list));

            _list = list;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="IndexableEnumerable{T}"/> class.
        /// </summary>
        /// <param name="array">The array.</param>
        /// =================================================================================================
        public IndexableEnumerable(T[] array)
            : this((IList<T>)array)
        { }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="IndexableEnumerable{T}"/> class.
        /// </summary>
        /// <param name="list">(Immutable) the list.</param>
        /// =================================================================================================
        public IndexableEnumerable(List<T> list)
            : this((IList<T>)list)
        { }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes the index enumerable.
        /// </summary>
        /// <param name="list">(Immutable) the list.</param>
        /// <returns>
        ///     An IIndexableEnumerable&lt;T&gt;
        /// </returns>
        /// =================================================================================================
        public static IIndexableEnumerable<T> Initialize(IList<T> list) => new IndexableEnumerable<T>(list);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes the index enumerable.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <returns>
        ///     An IIndexableEnumerable&lt;T&gt;
        /// </returns>
        /// =================================================================================================
        public static IIndexableEnumerable<T> Initialize(T[] array) => new IndexableEnumerable<T>(array);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes the index enumerable.
        /// </summary>
        /// <param name="list">(Immutable) the list.</param>
        /// <returns>
        ///     An IIndexableEnumerable&lt;T&gt;
        /// </returns>
        /// =================================================================================================
        public static IIndexableEnumerable<T> Initialize(List<T> list) => new IndexableEnumerable<T>(list);
        
        /// <inheritdoc />
        public T this[int index] => _list[index];

        /// <inheritdoc />
        public int Count => _list.Count;

        /// <inheritdoc />
        public void DoForEach(Action<T> action)
        {
            DomainEnsure.IsNotNull(action, nameof(action));

            for (int i = 0, count = _list.Count; i < count; i++)
                action(_list[i]);
        }

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator()
            => _list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        /// <inheritdoc />
        public void Dispose() { _list.Clear(); }
    }
}