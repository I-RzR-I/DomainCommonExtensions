// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2026-01-06 19:01
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-01-09 20:55
// ***********************************************************************
//  <copyright file="IndexableEnumerableExtensions.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using DomainCommonExtensions.Collections;
using DomainCommonExtensions.Utilities.Ensure;
using System;

#if NETSTANDARD1_5_OR_GREATER
using System.Threading.Tasks;
#endif

#endregion

namespace DomainCommonExtensions.ArraysExtensions
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     An indexable enumerable extensions.
    /// </summary>
    /// =================================================================================================
    public static class IndexableEnumerableExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     An IIndexableEnumerable&lt;T&gt; extension method that executes the action for each
        ///     operation.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="source">The source to act on.</param>
        /// <param name="action">The action.</param>
        /// =================================================================================================
        public static void DoActionForEach<T>(
            this IIndexableEnumerable<T> source,
            Action<T> action)
        {
            DomainEnsure.IsNotNull(source, nameof(source));
            DomainEnsure.IsNotNull(action, nameof(action));

            for (int i = 0, count = source.Count; i < count; i++)
                action(source[i]);
        }


#if NETSTANDARD1_5_OR_GREATER

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     An IIndexableEnumerable&lt;T&gt; extension method that executes the action for each
        ///     asynchronous operation.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="source">The source to act on.</param>
        /// <param name="asyncAction">The asynchronous action.</param>
        /// <returns>
        ///     A Task.
        /// </returns>
        /// =================================================================================================
        public static async Task DoActionForEachAsync<T>(
            this IIndexableEnumerable<T> source,
            Func<T, Task> asyncAction)
        {
            DomainEnsure.IsNotNull(source, nameof(source));
            DomainEnsure.IsNotNull(asyncAction, nameof(asyncAction));

            for (int i = 0, count = source.Count; i < count; i++) 
                await asyncAction(source[i]).ConfigureAwait(false);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     An IIndexableEnumerable&lt;T&gt; extension method that executes the action for each
        ///     asynchronous operation.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="source">The source to act on.</param>
        /// <param name="asyncValueAction">The asynchronous action.</param>
        /// <returns>
        ///     A Task.
        /// </returns>
        /// =================================================================================================
        public static async Task DoActionForEachAsync<T>(
            this IIndexableEnumerable<T> source,
            Func<T, ValueTask> asyncValueAction)
        {
            DomainEnsure.IsNotNull(source, nameof(source));
            DomainEnsure.IsNotNull(asyncValueAction, nameof(asyncValueAction));

            for (int i = 0, count = source.Count; i < count; i++) 
                await asyncValueAction(source[i]).ConfigureAwait(false);
        }

#endif
    }
}