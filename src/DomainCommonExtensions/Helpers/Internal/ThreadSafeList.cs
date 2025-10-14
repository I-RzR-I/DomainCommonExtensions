// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2025-10-13 14:10
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-10-13 15:22
// ***********************************************************************
//  <copyright file="ThreadSafeList.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Collections.Generic;

#endregion

namespace DomainCommonExtensions.Helpers.Internal
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     List of thread safes. This class cannot be inherited.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <seealso cref="T:System.Collections.Generic.IEnumerable{T}"/>
    /// =================================================================================================
    internal sealed class ThreadSafeList<T> : IEnumerable<T>
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the lock.
        /// </summary>
        /// =================================================================================================
        private static readonly object _lock = new object();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) list of internals.
        /// </summary>
        /// =================================================================================================
        private readonly List<T> _internalList = new List<T>();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Other Elements of IList implementation.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate
        ///     through the collection.
        /// </returns>
        /// =================================================================================================
        public IEnumerator<T> GetEnumerator()
        {
            return Clone().GetEnumerator();
        }

        /// <inheritdoc/>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Clone().GetEnumerator();
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Makes a deep copy of this object.
        /// </summary>
        /// <returns>
        ///     A copy of this object.
        /// </returns>
        /// =================================================================================================
        public List<T> Clone()
        {
            var newList = new List<T>();

            lock (_lock)
            {
                newList.AddRange(_internalList);
            }

            return newList;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Adds an item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// =================================================================================================
        public void AddItem(T item)
        {
            lock (_lock)
            {
                _internalList.Add(item);
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Removes the item described by item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// =================================================================================================
        public void RemoveItem(T item)
        {
            lock (_lock)
            {
                _internalList.Remove(item);
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Index of the given item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///     An int.
        /// </returns>
        /// =================================================================================================
        public int IndexOf(T item)
        {
            var index = -1;
            lock (_lock)
            {
                index = _internalList.IndexOf(item);
            }

            return index;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Indexer to get or set items within this collection using array index syntax.
        /// </summary>
        /// <param name="index">Zero-based index of the entry to access.</param>
        /// <returns>
        ///     The indexed item.
        /// </returns>
        /// =================================================================================================
        public T this[int index]
        {

            get
            {
                lock (_lock)
                {
                    return _internalList[index];
                }
            }
            set
            {
                lock (_lock)
                {
                    _internalList[index] = value;
                }
            }
        }
    }
}