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
using RzR.Extensions.Domain.Primitives;

// ReSharper disable FieldCanBeMadeReadOnly.Local

#endregion

namespace RzR.Extensions.Domain.Collections.Types
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
        ///     Tracks whether this instance has already been disposed.
        /// </summary>
        private bool _disposed;

        /// <summary>
        ///     Gets the stack length.
        /// </summary>
        /// <value>
        ///     The length.
        /// </value>
        public int Length => _disposable.Count;

        /// <summary>
        ///     Disposes every resource in the stack, in last-in first-out order.
        /// </summary>
        /// <exception cref="AggregateException">
        ///     Thrown when more than one resource throws while being disposed; it holds every thrown exception.
        /// </exception>
        /// <remarks>
        ///     Null entries are skipped. A resource that throws does not stop the disposal of the remaining ones.
        ///     When exactly one resource throws, that exception is rethrown after all resources were processed
        ///     (with its original stack trace on .NET Framework 4.5+ and .NET Standard).
        ///     The instance is marked as disposed before any resource is disposed, so a second call is a no-op.
        /// </remarks>
        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            List<Exception> exceptions = null;
            if (_disposable.IsNotNull())
            {
                while (_disposable.Count.IsGreaterThanZero())
                {
                    var resource = _disposable.Pop();
                    if (resource.IsNull())
                        continue;

                    try
                    {
                        resource.Dispose();
                    }
                    catch (Exception exception)
                    {
                        (exceptions ??= new List<Exception>()).Add(exception);
                    }
                }
            }

            GC.SuppressFinalize(this);

            if (exceptions.IsNull())
                return;

            if (exceptions!.Count == 1)
                Rethrow(exceptions[0]);
            else
                throw new AggregateException(exceptions);
        }

        /// <summary>
        ///     Rethrows the given exception, preserving its original stack trace where the target framework allows it.
        /// </summary>
        /// <param name="exception">The exception to rethrow.</param>
        private static void Rethrow(Exception exception)
        {
#if NET45_OR_GREATER || NET || NETSTANDARD1_0_OR_GREATER
            System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(exception).Throw();
#else
            throw exception;
#endif
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
