// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2026-01-06 19:01
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-01-06 20:18
// ***********************************************************************
//  <copyright file="MutableIndexable.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using DomainCommonExtensions.Resources.Enums;
using DomainCommonExtensions.Utilities.Ensure;
using System;
using System.Collections;
using System.Collections.Generic;
using DomainCommonExtensions.DataTypeExtensions;

#endregion

namespace DomainCommonExtensions.Collections
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Mutable indexable enumerable collection.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// =================================================================================================
    public interface IMutableIndexableEnumerable<T> : IIndexableEnumerable<T>
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the capacity.
        /// </summary>
        /// <value>
        ///     The capacity.
        /// </value>
        /// =================================================================================================
        public int Capacity { get; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Attempts to add.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///     True if it succeeds, false if it fails.
        /// </returns>
        /// =================================================================================================
        bool TryAdd(T item);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Attempts to remove at.
        /// </summary>
        /// <param name="index">Zero-based index of the collection.</param>
        /// <returns>
        ///     True if it succeeds, false if it fails.
        /// </returns>
        /// =================================================================================================
        bool TryRemoveAt(int index);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Sets.
        /// </summary>
        /// <param name="index">Zero-based index of the collection.</param>
        /// <param name="value">The value.</param>
        /// =================================================================================================
        void Set(int index, T value);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Clears this object to its blank/initial state.
        /// </summary>
        /// =================================================================================================
        void Clear();
    }

    /// -------------------------------------------------------------------------------------------------
    /// <inheritdoc cref="IMutableIndexableEnumerable{T}"/>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <seealso cref="T:DomainCommonExtensions.Collections.IMutableIndexableEnumerable{T}"/>
    /// =================================================================================================
    public sealed class MutableIndexableEnumerable<T> : IMutableIndexableEnumerable<T>
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the buffer.
        /// </summary>
        /// =================================================================================================
        private readonly T[] _buffer;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     True if disposed.
        /// </summary>
        /// =================================================================================================
        private bool _disposed;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="MutableIndexableEnumerable{T}"/> class.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when one or more arguments are outside the required range.
        /// </exception>
        /// <param name="capacity">The capacity.</param>
        /// =================================================================================================
        public MutableIndexableEnumerable(int capacity)
        {
            if (capacity <= 0)
                DomainEnsure.ThrowException(nameof(capacity), ExceptionType.ArgumentOutOfRangeException);

            _buffer = new T[capacity]; // ONE allocation
            Count = 0;
            _disposed = false;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes this collection.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        /// <returns>
        ///     An IMutableIndexableEnumerable&lt;T&gt;
        /// </returns>
        /// =================================================================================================
        public static IMutableIndexableEnumerable<T> Initialize(int capacity) => new MutableIndexableEnumerable<T>(capacity);

        /// <inheritdoc />
        public int Capacity => _buffer.Length;

        /// <inheritdoc />
        public int Count { get; private set; }

        /// <inheritdoc />
        public T this[int index]
        {
            get
            {
                ThrowIfDisposed();

                if ((uint)index >= (uint)Count)
                    DomainEnsure.ThrowException(nameof(index), ExceptionType.IndexOutOfRangeException);

                return _buffer[index];
            }
        }

        /// <inheritdoc />
        public bool TryAdd(T item)
        {
            ThrowIfDisposed();

            if (Count == _buffer.Length)
                return false;

            _buffer[Count++] = item;

            return true;
        }

        /// <inheritdoc />
        public void Set(int index, T value)
        {
            ThrowIfDisposed();

            if ((uint)index >= (uint)Count)
                DomainEnsure.ThrowException(nameof(index), ExceptionType.IndexOutOfRangeException);

            _buffer[index] = value;
        }

        /// <inheritdoc />
        public bool TryRemoveAt(int index)
        {
            ThrowIfDisposed();

            if ((uint)index >= (uint)Count)
                return false;

            var moveCount = Count - index - 1;
            if (moveCount > 0)
                Array.Copy(
                    _buffer,
                    index + 1,
                    _buffer,
                    index,
                    moveCount);

            _buffer[--Count] = default!;

            return true;
        }

        /// <inheritdoc />
        public void Clear()
        {
            ThrowIfDisposed();

            Array.Clear(_buffer, 0, Count);
            Count = 0;
        }

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator()
        {
            ThrowIfDisposed();

            for (var i = 0; i < Count; i++)
                yield return _buffer[i];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <inheritdoc />
        public void DoForEach(Action<T> action)
        {
            ThrowIfDisposed();

            for (var i = 0; i < Count; i++)
                action(_buffer[i]);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (_disposed.IsTrue())
                return;

            _disposed = true;

            Array.Clear(_buffer, 0, Count);
            Count = 0;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Throw if disposed.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        ///     Thrown when a supplied object has been disposed.
        /// </exception>
        /// =================================================================================================
        private void ThrowIfDisposed()
        {
            if (_disposed.IsTrue())
                throw new ObjectDisposedException(nameof(MutableIndexableEnumerable<T>));
        }
    }
}