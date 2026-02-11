// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2026-01-28 22:01
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-01-28 22:52
// ***********************************************************************
//  <copyright file="ObservableEnumerator.cs" company="RzR SOFT & TECH">
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
using DomainCommonExtensions.CommonExtensions;

// ReSharper disable PossibleMultipleEnumeration

#endregion

namespace DomainCommonExtensions.Collections
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     An observable enumerator. This class cannot be inherited.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <seealso cref="T:System.Collections.Generic.IEnumerator{T}"/>
    /// =================================================================================================
    public sealed class ObservableEnumerator<T> : IEnumerator<T>
    {
        private readonly IEnumerable<T> _source;

        private T _current = default;
        private T _peek = default;

        private bool _hasCurrent, _hasPeek;

        private IEnumerator<T> _inner;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="ObservableEnumerator{T}"/> class.
        /// </summary>
        /// <param name="source">The source data.</param>
        /// =================================================================================================
        internal ObservableEnumerator(IEnumerable<T> source)
        {
            _source = source;
            _inner = source.GetEnumerator();
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="ObservableEnumerator{T}"/> class.
        /// </summary>
        /// <param name="inner">The inner.</param>
        /// =================================================================================================
        internal ObservableEnumerator(IEnumerator<T> inner)
        {
            _inner = inner;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets a value indicating whether this object is last.
        /// </summary>
        /// <value>
        ///     True if this object is last, false if not.
        /// </value>
        /// =================================================================================================
        public bool IsLast
        {
            get
            {
                EnsureStarted();
                EnsurePeeked();

                return !_hasPeek;
            }
        }

        /// <inheritdoc />
        public T Current
        {
            get
            {
                EnsureStarted();

                return _current;
            }
        }

        /// <inheritdoc />
        object IEnumerator.Current => Current;

        /// <inheritdoc />
        public bool MoveNext()
        {
            if (_hasPeek)
            {
                _current = _peek;
                _hasPeek = false;
                _hasCurrent = true;

                return true;
            }

            if (!_inner.MoveNext())
            {
                _hasCurrent = false;

                return false;
            }

            _current = _inner.Current;
            _hasCurrent = true;

            return true;
        }

        /// <inheritdoc />
        public void Reset()
        {
            if (_source.IsNull())
                throw new NotSupportedException("Reset is not supported.");

            _inner.Dispose();
            _inner = _source.GetEnumerator();

            _hasCurrent = false;
            _hasPeek = false;
        }

        /// <inheritdoc />
        public void Dispose() => _inner.Dispose();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Attempts to peek.
        /// </summary>
        /// <param name="value">[out] The value.</param>
        /// <returns>
        ///     True if it succeeds, false if it fails.
        /// </returns>
        /// =================================================================================================
        public bool TryPeek(out T value)
        {
            EnsureStarted();
            EnsurePeeked();

            if (_hasPeek)
            {
                value = _peek;

                return true;
            }

            value = default;

            return false;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Ensures that peeked.
        /// </summary>
        /// =================================================================================================
        private void EnsurePeeked()
        {
            if (_hasPeek)
                return;

            if (_inner.MoveNext())
            {
                _peek = _inner.Current;
                _hasPeek = true;
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Ensures that enumeration started.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when the requested operation is invalid.
        /// </exception>
        /// =================================================================================================
        private void EnsureStarted()
        {
            if (!_hasCurrent)
                throw new InvalidOperationException("Enumeration has not started.");
        }
    }
}