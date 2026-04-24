#if NET45_OR_GREATER || NET || NETSTANDARD1_0_OR_GREATER

// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2026-03-13 20:03
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-03-17 23:11
// ***********************************************************************
//  <copyright file="AsyncExpiringLazy.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using RzR.Extensions.Domain.CommonExtensions;

#endregion

namespace RzR.Extensions.Domain.Utilities.LazyLoad
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A thread-safe asynchronous lazy initializer with a configurable time-to-live (TTL).
    ///     The value is computed by the supplied factory and cached until the TTL elapses or
    ///     <see cref="Reset"/> is called. After expiry the next caller transparently triggers
    ///     a fresh factory invocation. This class cannot be inherited.
    /// </summary>
    /// <typeparam name="T">The type of value produced by the factory.</typeparam>
    /// <remarks>
    ///     <para>
    ///         <b>Thread safety:</b> all public members are safe to call concurrently.
    ///         A thundering-herd guard ensures that only one factory invocation is in flight
    ///         at a time; concurrent callers share the same in-progress <see cref="Task{T}"/>.
    ///     </para>
    ///     <para>
    ///         <b>Fault recovery:</b> if the factory faults or is cancelled, the entry is treated
    ///         as immediately invalid on the next call, allowing automatic retry without waiting
    ///         for the TTL window to close. Supports <c>await lazy</c> syntax directly via
    ///         <see cref="GetAwaiter"/>.
    ///     </para>
    /// </remarks>
    /// =================================================================================================
    public sealed class AsyncExpiringLazy<T>
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the factory.
        /// </summary>
        /// =================================================================================================
        private readonly Func<CancellationToken, Task<T>> _factory;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the TTL.
        /// </summary>
        /// =================================================================================================
        private readonly TimeSpan _ttl;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     The entry. Accessed via <see cref="Volatile.Read{T}"/> and
        ///     <see cref="Interlocked.CompareExchange{T}"/> — do not mark volatile.
        /// </summary>
        /// =================================================================================================
        private Entry _entry;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of <see cref="AsyncExpiringLazy{T}"/> with the
        ///     specified factory and time-to-live.
        /// </summary>
        /// <param name="factory">
        ///     An asynchronous delegate that produces the value on demand (first access and after
        ///     each expiry, fault, or reset). Receives a <see cref="CancellationToken"/> forwarded
        ///     from the caller of <see cref="GetValueAsync"/>.
        /// </param>
        /// <param name="ttl">
        ///     How long a successfully computed value remains valid. After this duration the next
        ///     <see cref="GetValueAsync"/> call triggers a fresh factory invocation. Pass
        ///     <see cref="TimeSpan.Zero"/> to disable caching (factory called on every request).
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="factory"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="ttl"/> is negative.
        /// </exception>
        /// =================================================================================================
        public AsyncExpiringLazy(Func<CancellationToken, Task<T>> factory, TimeSpan ttl)
        {
            if (factory.IsNull())
                throw new ArgumentNullException(nameof(factory));

            _factory = factory;

            if (ttl < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(ttl), "TTL must not be negative.");

            _ttl = ttl;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets a value indicating whether the lazy value has been successfully computed
        ///     and its TTL has not yet elapsed.
        /// </summary>
        /// <value>
        ///     <see langword="true"/> if the factory completed without error and the cached result
        ///     is still within its TTL window; <see langword="false"/> if no value has been computed
        ///     yet, the factory faulted, the TTL has expired, or the entry was cleared by
        ///     <see cref="Reset"/>.
        /// </value>
        /// =================================================================================================
        public bool IsValueCreated
        {
            get
            {
                var entry = Volatile.Read(ref _entry);

                return entry.IsNotNull()
                       && entry.Task.Status == TaskStatus.RanToCompletion
                       && DateTimeOffset.UtcNow < entry.Expiration;
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets a value indicating whether a factory invocation has been started and the entry
        ///     has not been cleared by <see cref="Reset"/>.
        /// </summary>
        /// <value>
        ///     <see langword="true"/> once the first <see cref="GetValueAsync"/> call has been made;
        ///     otherwise <see langword="false"/>. Note that this returns <see langword="true"/> even
        ///     after the TTL expires — the entry object still exists until <see cref="Reset"/> clears
        ///     it or a new factory run replaces it.
        /// </value>
        /// =================================================================================================
        public bool IsInitializationStarted => Volatile.Read(ref _entry).IsNotNull();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Returns the cached value if it is still within its TTL and healthy; otherwise
        ///     triggers a fresh factory invocation and awaits its result.
        /// </summary>
        /// <param name="cancellationToken">
        ///     A <see cref="CancellationToken"/> forwarded to the factory when a new invocation is
        ///     needed. Has no effect when a valid cached value is returned directly.
        /// </param>
        /// <returns>
        ///     A <see cref="Task{T}"/> that completes with the (possibly freshly computed) value.
        ///     Concurrent callers that race during a single factory run share the same in-progress task.
        /// </returns>
        /// <remarks>
        ///     If the cached task is faulted or cancelled it is treated as immediately expired and a
        ///     new factory invocation is started, regardless of the configured TTL.
        /// </remarks>
        /// =================================================================================================
        public Task<T> GetValueAsync(CancellationToken cancellationToken = default)
        {
            var entry = Volatile.Read(ref _entry);

            if (entry.IsNotNull()
                && DateTimeOffset.UtcNow < entry.Expiration
                && !entry.Task.IsFaulted
                && !entry.Task.IsCanceled)
                return entry.Task;

            return RefreshAsync(entry, cancellationToken);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Performs a lock-free cache refresh using a compare-and-swap (CAS) loop.
        ///     A <see cref="TaskCompletionSource{T}"/> placeholder is stored atomically before the
        ///     factory is invoked so that losing threads can await the winner's task rather than
        ///     starting duplicate factory calls (thundering-herd prevention).
        /// </summary>
        /// <param name="current">
        ///     The entry snapshot that the caller observed; used as the expected value in the CAS.
        /// </param>
        /// <param name="cancellationToken">
        ///     Forwarded to the factory when this call wins the CAS race.
        /// </param>
        /// <returns>
        ///     The task that will hold the refreshed value; shared by all callers that join during
        ///     the same factory run.
        /// </returns>
        /// =================================================================================================
        private Task<T> RefreshAsync(Entry current, CancellationToken cancellationToken = default)
        {
            var tcs = new TaskCompletionSource<T>();
            var newEntry = new Entry(tcs.Task, DateTimeOffset.UtcNow + _ttl);

            if (Interlocked.CompareExchange(ref _entry, newEntry, current) != current)
            {
                // Another thread won the race. Re-read and decide.
                var readValue = Volatile.Read(ref _entry);

                if (readValue.IsNotNull()
                    && DateTimeOffset.UtcNow < readValue.Expiration
                    && !readValue.Task.IsFaulted
                    && !readValue.Task.IsCanceled)
                    return readValue.Task;

                // readValue is null (Reset), expired, or faulted — retry.
                return RefreshAsync(readValue, cancellationToken);
            }

            // We won the CAS — invoke the factory.
            RunFactory(tcs, cancellationToken);

            return tcs.Task;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Invokes the user-supplied factory and forwards its result, fault, or cancellation to
        ///     <paramref name="taskCompletionSource"/>. Declared <c>async void</c> so that
        ///     <see cref="RefreshAsync"/> can return the TCS task immediately; exceptions are
        ///     captured via the TCS rather than propagated to the caller.
        /// </summary>
        /// <param name="taskCompletionSource">
        ///     The <see cref="TaskCompletionSource{T}"/> whose task is already being awaited by
        ///     all callers that joined this factory run.
        /// </param>
        /// <param name="cancellationToken">
        ///     The token forwarded from the originating <see cref="GetValueAsync"/> call.
        /// </param>
        /// =================================================================================================
        private async void RunFactory(TaskCompletionSource<T> taskCompletionSource, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _factory(cancellationToken).ConfigureAwait(false);
                taskCompletionSource.TrySetResult(result);
            }
            catch (OperationCanceledException)
            {
                taskCompletionSource.TrySetCanceled();
            }
            catch (Exception ex)
            {
                taskCompletionSource.TrySetException(ex);
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Clears the cached entry immediately, regardless of the TTL, returning the instance
        ///     to its uninitialized state.
        /// </summary>
        /// <remarks>
        ///     After this call <see cref="IsInitializationStarted"/> and <see cref="IsValueCreated"/>
        ///     both return <see langword="false"/>, and the next <see cref="GetValueAsync"/> call
        ///     triggers a fresh factory invocation. Any in-progress factory run is not cancelled;
        ///     it will still complete its TCS, but the result won't be visible to new callers.
        ///     Safe to call concurrently or multiple times.
        /// </remarks>
        /// =================================================================================================
        public void Reset()
        {
            Interlocked.Exchange(ref _entry, null);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Attempts to retrieve the cached value synchronously without triggering a factory
        ///     invocation or refreshing an expired entry.
        /// </summary>
        /// <param name="value">
        ///     When this method returns <see langword="true"/>, contains the cached value;
        ///     otherwise contains <see langword="default"/>.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if a successfully computed value is currently cached and its
        ///     TTL has not expired; <see langword="false"/> if no value is available, the entry has
        ///     expired, the factory faulted, or the entry was cleared by <see cref="Reset"/>.
        /// </returns>
        /// =================================================================================================
        public bool TryGetValue(out T value)
        {
            var entry = Volatile.Read(ref _entry);

            if (entry.IsNotNull() &&
                entry.Task.Status == TaskStatus.RanToCompletion &&
                DateTimeOffset.UtcNow < entry.Expiration)
            {
                value = entry.Task.Result;

                return true;
            }

            value = default!;

            return false;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the awaiter for this instance, enabling <c>await myExpiringLazy</c> syntax as
        ///     a shorthand for <c>await myExpiringLazy.GetValueAsync()</c>.
        /// </summary>
        /// <returns>
        ///     A <see cref="TaskAwaiter{T}"/> for the lazy value.
        /// </returns>
        /// =================================================================================================
        public TaskAwaiter<T> GetAwaiter()
        {
            return GetValueAsync().GetAwaiter();
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     An immutable snapshot holding a factory's result task alongside the
        ///     <see cref="DateTimeOffset"/> at which the cached value expires. Stored as a single
        ///     atomically-swappable reference to enable lock-free updates.
        /// </summary>
        /// =================================================================================================
        private sealed class Entry
        {
            /// -------------------------------------------------------------------------------------------------
            /// <summary>
            ///     The UTC instant at which this entry expires. Once <see cref="DateTimeOffset.UtcNow"/>
            ///     exceeds this value the entry is no longer served as valid by
            ///     <see cref="GetValueAsync"/> or <see cref="TryGetValue"/>.
            /// </summary>
            /// =================================================================================================
            public readonly DateTimeOffset Expiration;

            /// -------------------------------------------------------------------------------------------------
            /// <summary>
            ///     The task that holds (or will hold) the factory's computed value.
            /// </summary>
            /// =================================================================================================
            public readonly Task<T> Task;

            /// -------------------------------------------------------------------------------------------------
            /// <summary>
            ///     Initializes a new instance of <see cref="Entry"/>.
            /// </summary>
            /// <param name="task">The task that carries the factory result.</param>
            /// <param name="expiration">The UTC instant at which this entry expires.</param>
            /// =================================================================================================
            public Entry(Task<T> task, DateTimeOffset expiration)
            {
                Task = task;
                Expiration = expiration;
            }
        }
    }
}

#endif