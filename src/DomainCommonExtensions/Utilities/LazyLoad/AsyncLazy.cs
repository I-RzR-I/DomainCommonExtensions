#if NET45_OR_GREATER ||NET || NETSTANDARD1_0_OR_GREATER

// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2026-03-13 20:03
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-03-17 23:45
// ***********************************************************************
//  <copyright file="AsyncLazy.cs" company="RzR SOFT & TECH">
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
using DomainCommonExtensions.CommonExtensions;

#endregion

namespace DomainCommonExtensions.Utilities.LazyLoad
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A thread-safe asynchronous lazy initializer. The factory delegate is invoked exactly
    ///     once, and it's result is cached for all subsequent callers. If the factory faults or
    ///     is cancelled, the cached state is automatically cleared, allowing the next call to
    ///     <see cref="GetValueAsync"/> to retry. This class cannot be inherited.
    /// </summary>
    /// <typeparam name="T">The type of value produced by the factory.</typeparam>
    /// <remarks>
    ///     <b>Thread safety:</b> all public members are safe to call concurrently.
    ///     Only one factory invocation is ever in flight; concurrent callers share the same
    ///     in-progress <see cref="Task{T}"/>. Supports <c>await lazy</c> syntax directly
    ///     via <see cref="GetAwaiter"/>.
    /// </remarks>
    /// =================================================================================================
    public sealed class AsyncLazy<T>
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the factory.
        /// </summary>
        /// =================================================================================================
        private readonly Func<CancellationToken, Task<T>> _factory;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     The current lazy state (task + CTS), or null if uninitialized / reset.
        /// </summary>
        /// =================================================================================================
        private LazyState _state;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of <see cref="AsyncLazy{T}"/> with the specified factory.
        /// </summary>
        /// <param name="factory">
        ///     An asynchronous delegate that produces the value. Invoked on first demand and
        ///     automatically retried after a fault or cancellation. Receives a
        ///     <see cref="CancellationToken"/> that is cancelled when <see cref="Reset"/> is
        ///     called while the factory is executing.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="factory"/> is <see langword="null"/>.
        /// </exception>
        /// =================================================================================================
        public AsyncLazy(Func<CancellationToken, Task<T>> factory)
        {
            if (factory.IsNull())
                throw new ArgumentNullException(nameof(factory));

            _factory = factory;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets a value indicating whether the lazy value has been successfully computed
        ///     and is currently cached.
        /// </summary>
        /// <value>
        ///     <see langword="true"/> if the factory completed without error and the result has
        ///     not been cleared by <see cref="Reset"/>; <see langword="false"/> if initialization
        ///     has not started, is still in progress, faulted, cancelled, or was reset.
        /// </value>
        /// =================================================================================================
        public bool IsValueCreated
        {
            get
            {
                var state = Volatile.Read(ref _state);

                return state.IsNotNull() 
                       && state.Task.Status == TaskStatus.RanToCompletion;
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets a value indicating whether a factory invocation has been started, regardless
        ///     of whether initialization has completed.
        /// </summary>
        /// <value>
        ///     <see langword="true"/> once <see cref="GetValueAsync"/> has been called at least
        ///     once and the state has not been cleared by <see cref="Reset"/>; otherwise
        ///     <see langword="false"/>. Returns <see langword="true"/> while the factory is still
        ///     running or even if it has faulted (until <see cref="Reset"/> is called).
        /// </value>
        /// =================================================================================================
        public bool IsInitializationStarted => Volatile.Read(ref _state).IsNotNull();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Returns the lazily-initialized value, invoking the factory on the first call.
        ///     Subsequent calls return the same cached <see cref="Task{T}"/> without
        ///     re-invoking the factory.
        /// </summary>
        /// <param name="cancellationToken">
        ///     A <see cref="CancellationToken"/> that is linked to the token passed to the factory.
        ///     Only affects the current in-flight factory invocation; other concurrently waiting
        ///     callers are not independently cancelled.
        /// </param>
        /// <returns>
        ///     A <see cref="Task{T}"/> that completes with the initialized value. All concurrent
        ///     and subsequent callers share the same task instance.
        /// </returns>
        /// <remarks>
        ///     If the factory faults or is cancelled the internal state is automatically cleared
        ///     so that the next call retries initialization from scratch.
        /// </remarks>
        /// =================================================================================================
        public Task<T> GetValueAsync(CancellationToken cancellationToken = default)
        {
            while (true)
            {
                var current = Volatile.Read(ref _state);

                if (current.IsNotNull() && !current.Task.IsFaulted && !current.Task.IsCanceled)
                    return current.Task;

                var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                var tcs = new TaskCompletionSource<T>();
                var newState = new LazyState(tcs.Task, cts);

                if (Interlocked.CompareExchange(ref _state, newState, current) != current)
                {
                    cts.Dispose();
                    continue; // Retry instead of returning a potentially-null value
                }

                _ = RunFactoryAsync(tcs, cts);

                return tcs.Task;
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Invokes the user-supplied factory, forwards its result or exception to
        ///     <paramref name="taskCompletionSource"/>, and disposes <paramref name="cancelTokenSource"/>
        ///     on the success path. Calls <see cref="Reset"/> on fault or cancellation so that
        ///     the next caller can retry.
        /// </summary>
        /// <param name="taskCompletionSource">
        ///     The <see cref="TaskCompletionSource{T}"/> whose task is already exposed to all callers.
        /// </param>
        /// <param name="cancelTokenSource">
        ///     The linked <see cref="CancellationTokenSource"/> created for this factory run;
        ///     disposed on the success path to release the external token registration.
        /// </param>
        /// <returns>
        ///     A <see cref="Task"/> that completes once the factory has resolved and the TCS
        ///     has been signalled.
        /// </returns>
        /// =================================================================================================
        private async Task RunFactoryAsync(TaskCompletionSource<T> taskCompletionSource,
            CancellationTokenSource cancelTokenSource)
        {
            try
            {
                var task = _factory(cancelTokenSource.Token);

                if (!task.IsCompleted)
                {
                    var result = await task.ConfigureAwait(false);
                    taskCompletionSource.TrySetResult(result);
                }
                else
                {
                    // Avoid running continuations inline on the caller's thread
                    // when the factory returns an already-completed task.
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            var result = await task.ConfigureAwait(false);
                            taskCompletionSource.TrySetResult(result);
                            cancelTokenSource.Dispose();
                        }
                        catch (OperationCanceledException)
                        {
                            taskCompletionSource.TrySetCanceled();
                            Reset();
                        }
                        catch (Exception ex)
                        {
                            taskCompletionSource.TrySetException(ex);
                            Reset();
                        }
                    });

                    return;
                }
            }
            catch (OperationCanceledException)
            {
                taskCompletionSource.TrySetCanceled();
                Reset();

                return;
            }
            catch (Exception ex)
            {
                taskCompletionSource.TrySetException(ex);
                Reset();

                return;
            }

            // Success path: dispose the linked CTS to release the token registration.
            cancelTokenSource.Dispose();
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Clears the cached state and cancels any in-progress factory invocation, returning
        ///     the instance to its uninitialized state.
        /// </summary>
        /// <remarks>
        ///     After this call <see cref="IsInitializationStarted"/> and <see cref="IsValueCreated"/>
        ///     both return <see langword="false"/>, and the next call to <see cref="GetValueAsync"/>
        ///     starts a fresh factory invocation. Safe to call concurrently or multiple times.
        /// </remarks>
        /// =================================================================================================
        public void Reset()
        {
            var old = Interlocked.Exchange(ref _state, null);

            if (old?.CancelTokenSource == null)
                return;

            try
            {
                old.CancelTokenSource.Cancel();
            }
            catch (ObjectDisposedException) { }

            old.CancelTokenSource.Dispose();
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Attempts to retrieve the cached value synchronously without triggering initialization.
        /// </summary>
        /// <param name="value">
        ///     When this method returns <see langword="true"/>, contains the cached value;
        ///     otherwise contains <see langword="default"/>.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the factory has completed successfully and the result is
        ///     still cached; <see langword="false"/> if the value has not been computed yet, the
        ///     factory is still running, faulted, or the state was cleared by <see cref="Reset"/>.
        /// </returns>
        /// =================================================================================================
        public bool TryGetValue(out T value)
        {
            var state = Volatile.Read(ref _state);

            if (state.IsNotNull() 
                && state.Task.Status == TaskStatus.RanToCompletion)
            {
                value = state.Task.Result;

                return true;
            }

            value = default!;

            return false;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the awaiter for this instance, enabling <c>await myAsyncLazy</c> syntax as a
        ///     shorthand for <c>await myAsyncLazy.GetValueAsync()</c>.
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
        ///     Holds the in-flight or completed factory task together with its associated
        ///     <see cref="CancellationTokenSource"/> as a single atomically-swappable reference.
        ///     Bundling both into one object eliminates the race condition between
        ///     <see cref="GetValueAsync"/> and <see cref="Reset"/>.
        /// </summary>
        /// =================================================================================================
        private sealed class LazyState
        {
            /// -------------------------------------------------------------------------------------------------
            /// <summary>
            ///     The linked <see cref="CancellationTokenSource"/> whose token is forwarded to the
            ///     factory. Cancelled by <see cref="Reset"/> if a factory run is in progress;
            ///     disposed on the success path by <see cref="RunFactoryAsync"/>.
            /// </summary>
            /// =================================================================================================
            public readonly CancellationTokenSource CancelTokenSource;

            /// -------------------------------------------------------------------------------------------------
            /// <summary>
            ///     The task exposed to all callers awaiting the lazy value. Backed by a
            ///     <see cref="TaskCompletionSource{T}"/> until the factory completes.
            /// </summary>
            /// =================================================================================================
            public readonly Task<T> Task;

            /// -------------------------------------------------------------------------------------------------
            /// <summary>
            ///     Initializes a new instance of <see cref="LazyState"/>.
            /// </summary>
            /// <param name="task">The task that will carry the factory result.</param>
            /// <param name="cancelTokenSource">
            ///     The linked <see cref="CancellationTokenSource"/> for this factory run.
            /// </param>
            /// =================================================================================================
            public LazyState(Task<T> task, CancellationTokenSource cancelTokenSource)
            {
                Task = task;
                CancelTokenSource = cancelTokenSource;
            }
        }
    }
}

#endif