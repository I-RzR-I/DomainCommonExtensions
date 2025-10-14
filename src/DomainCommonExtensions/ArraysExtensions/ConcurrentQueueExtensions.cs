// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2025-10-09 14:10
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-10-09 14:27
// ***********************************************************************
//  <copyright file="ConcurrentQueueExtensions.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using DomainCommonExtensions.DataTypeExtensions;

#endregion

namespace DomainCommonExtensions.ArraysExtensions
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A concurrent queue extensions.
    /// </summary>
    /// =================================================================================================
    public static class ConcurrentQueueExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A ConcurrentQueue&lt;T&gt; extension method that purges the given queue.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="queue">The queue to act on.</param>
        /// =================================================================================================
        public static void Purge<T>(this ConcurrentQueue<T> queue)
        {
            while (queue.IsEmpty.IsFalse())
            {
                queue.TryDequeue(out _);
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A ConcurrentQueue&lt;T&gt; extension method that prunes.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="queue">The queue to act on.</param>
        /// <param name="maxSize">The maximum size of the array.</param>
        /// =================================================================================================
        public static void Prune<T>(this ConcurrentQueue<T> queue, int maxSize)
        {
            while (queue.Count > maxSize)
            {
                queue.TryDequeue(out _);
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A ConcurrentQueue&lt;T&gt; extension method that dequeue all.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="queue">The queue to act on.</param>
        /// <returns>
        ///     A T[].
        /// </returns>
        /// =================================================================================================
        public static T[] DequeueAll<T>(this ConcurrentQueue<T> queue)
        {
            var dequeued = new T[] { };
            while (queue.TryDequeue(out var value)) 
                dequeued.AppendItem(value);

            return dequeued.ToArray();
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A ConcurrentQueue&lt;T&gt; extension method that dequeue batch.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="queue">The queue to act on.</param>
        /// <param name="batchSize">Size of the batch.</param>
        /// <returns>
        ///     A T[].
        /// </returns>
        /// =================================================================================================
        public static T[] DequeueBatch<T>(this ConcurrentQueue<T> queue, int batchSize)
        {
            var dequeued = new T[] { };
            for (var i = 0; i < batchSize && queue.TryDequeue(out var value); i++) 
                dequeued.AppendItem(value);

            return dequeued.ToArray();
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A ConcurrentQueue&lt;T&gt; extension method that enqueue range.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="queue">The queue to act on.</param>
        /// <param name="items">The items.</param>
        /// =================================================================================================
        public static void EnqueueRange<T>(this ConcurrentQueue<T> queue, IEnumerable<T> items)
        {
            foreach (var item in items) 
                queue.Enqueue(item);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A ConcurrentQueue&lt;T&gt; extension method that adds an object onto the end of this
        ///     queue.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="queue">The queue to act on.</param>
        /// <param name="item">The item.</param>
        /// <param name="limit">The limit.</param>
        /// <returns>
        ///     An int.
        /// </returns>
        /// =================================================================================================
        public static int Enqueue<T>(this ConcurrentQueue<T> queue, T item, int limit)
        {
            var purgeCount = 0;
            while (queue.Count >= limit)
            {
                queue.TryDequeue(out _);
                purgeCount++;
            }

            queue.Enqueue(item);

            return purgeCount;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A ConcurrentQueue&lt;T&gt; extension method that enqueue range.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="queue">The queue to act on.</param>
        /// <param name="items">The items.</param>
        /// <param name="limit">The limit.</param>
        /// <returns>
        ///     An int.
        /// </returns>
        /// =================================================================================================
        public static int EnqueueRange<T>(this ConcurrentQueue<T> queue, IEnumerable<T> items, int limit)
        {
            var purgeCount = 0;
            foreach (var item in items) 
                purgeCount += queue.Enqueue(item, limit);

            return purgeCount;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A ConcurrentQueue&lt;T&gt; extension method that dequeue and process batches.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="queue">The queue to act on.</param>
        /// <param name="maxBatchSize">The maximum size of the batch.</param>
        /// <param name="processBatch">The process batch.</param>
        /// =================================================================================================
        public static void DequeueAndProcessBatches<T>(this ConcurrentQueue<T> queue, int maxBatchSize,
            Action<T[]> processBatch)
        {
            for (var batch = queue.DequeueBatch(maxBatchSize); batch.Any(); batch = queue.DequeueBatch(maxBatchSize))
                processBatch(batch);
        }
    }
}