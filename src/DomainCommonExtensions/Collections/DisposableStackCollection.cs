// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-12-09 00:58
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-12-09 01:16
// ***********************************************************************
//  <copyright file="DisposableStackCollection.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections.Generic;
using System.Linq;
using DomainCommonExtensions.CommonExtensions;
using DomainCommonExtensions.DataTypeExtensions;

// ReSharper disable FieldCanBeMadeReadOnly.Local

#endregion

namespace DomainCommonExtensions.Collections
{
    /// <summary>
    ///     Disposable stack collection
    /// </summary>
    public class DisposableStackCollection : IDisposable
    {
        /// <summary>
        ///     The disposable stack.
        /// </summary>
        private Stack<IDisposable> _disposable = new Stack<IDisposable>();

        /// <summary>
        ///     Gets the stack length.
        /// </summary>
        /// <value>
        ///     The length.
        /// </value>
        public int Length => _disposable.Count;

        /// <inheritdoc />
        public void Dispose()
        {
            if (_disposable.IsNotNull())
            {
                while (_disposable.Count.IsGreaterThanZero())
                    _disposable.Pop().Dispose();
            }
        }

        /// <summary>
        ///     Add to stack
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <remarks></remarks>
        public T Add<T>(T resource) where T : IDisposable
        {
            _disposable.Push(resource);

            return resource;
        }

        /// <summary>
        ///     Gets a t using the given index.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="index">Zero-based index of the collection.</param>
        /// <returns>
        ///     A T.
        /// </returns>
        public T Get<T>(int index) where T : IDisposable 
            => (T)_disposable.ElementAt(index);

        /// <summary>
        ///     Gets or default.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="index">Zero-based index of the collection.</param>
        /// <returns>
        ///     The T object or default.
        /// </returns>
        public T GetOrDefault<T>(int index) where T : IDisposable 
            => (T)_disposable.ElementAtOrDefault(index);

        /// <summary>
        ///     Clears this object to its blank/initial state.
        /// </summary>
        public void Clear() 
            => _disposable.Clear();

        /// <summary>
        ///     Removes and returns the top-of-stack object.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <returns>
        ///     The previous top-of-stack object.
        /// </returns>
        public T Pop<T>() where T : IDisposable 
            => (T)_disposable.Pop();

        /// <summary>
        ///     Returns the top-of-stack object without removing it.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <returns>
        ///     The current top-of-stack object.
        /// </returns>
        public T Peek<T>() where T : IDisposable
            => (T)_disposable.Peek();
    }
}