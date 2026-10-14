// ***********************************************************************
//  Assembly          : RzR.Shared.Extensions.DomainCommonExtensions
//  Author            : RzR
//  Created On        : 2026-10-10 22:10
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-10-14 09:11
//  ***********************************************************************
//  <copyright file="SemaphoreSlimExtensions.cs" company="RzR SOFT & TECH">
//      Copyright (c) RzR. All rights reserved.
//  </copyright>
//  <contact>
//      https://iamrzr.dev/contact
//  </contact>
//  <summary></summary>
//  ***********************************************************************

#region U S I N G

using System;
using System.Threading;
using System.Threading.Tasks;
using RzR.Extensions.Domain.Internal;
using RzR.Extensions.Domain.Validation;

#endregion

namespace RzR.Extensions.Domain.Async
{
#if NET45_OR_GREATER || NET || NETSTANDARD1_0_OR_GREATER

    /// <summary>
    ///     SemaphoreSlim extensions.
    /// </summary>
    public static class SemaphoreSlimExtensions
    {
        /// <summary>
        ///     Waits asynchronously to enter <paramref name="semaphore" /> and returns an object that
        ///     releases it when disposed.
        /// </summary>
        /// <remarks>
        ///     Use it with <c>using (await semaphore.LockAsync()) { ... }</c>. Further
        ///     <see cref="IDisposable.Dispose" /> calls on the result do nothing, so the semaphore is
        ///     never released more often than it was entered.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="semaphore" /> is null.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        ///     Thrown when <paramref name="semaphore" /> has been disposed.
        /// </exception>
        /// <param name="semaphore">The semaphore to enter.</param>
        /// <returns>
        ///     A task whose result releases <paramref name="semaphore" /> once, on its first
        ///     <see cref="IDisposable.Dispose" /> call.
        /// </returns>
        public static Task<IDisposable> LockAsync(this SemaphoreSlim semaphore)
        {
            return LockAsync(semaphore, Timeout.InfiniteTimeSpan);
        }

        /// <summary>
        ///     Waits asynchronously, up to <paramref name="timeout" />, to enter <paramref name="semaphore" />
        ///     and returns an object that releases it when disposed.
        /// </summary>
        /// <remarks>
        ///     The argument exceptions are thrown before the method returns; the
        ///     <see cref="TimeoutException" /> is reported through the returned task. On a timeout the
        ///     semaphore has not been entered, so there is nothing to release. Further <see cref="IDisposable.Dispose" />
        ///     calls on the result do nothing. To bound the wait use <see cref="LockAsync(SemaphoreSlim, TimeSpan)" />,
        ///     not <see cref="TaskExtensions.WithTimeout(Task, TimeSpan)" />: when the timeout wins, the
        ///     pending wait still acquires the semaphore later and nothing releases it.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="semaphore" /> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="timeout" /> is negative and not <see cref="Timeout.InfiniteTimeSpan" />
        ///     , or is greater than <see cref="int.MaxValue" /> milliseconds.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        ///     Thrown when <paramref name="semaphore" /> has been disposed.
        /// </exception>
        /// <param name="semaphore">The semaphore to enter.</param>
        /// <param name="timeout">
        ///     The time to wait, from <see cref="TimeSpan.Zero" /> to <see cref="int.MaxValue" />
        ///     milliseconds, or <see cref="Timeout.InfiniteTimeSpan" /> to wait without a time limit.
        /// </param>
        /// <returns>
        ///     A task whose result releases <paramref name="semaphore" /> once, on its first
        ///     <see cref="IDisposable.Dispose" /> call. The task faults with a <see cref="TimeoutException" />
        ///     when the semaphore could not be entered within <paramref name="timeout" />.
        /// </returns>
        public static Task<IDisposable> LockAsync(this SemaphoreSlim semaphore, TimeSpan timeout)
        {
            DomainEnsure.IsNotNull(semaphore, nameof(semaphore));

            return CreateReleaserAsync(semaphore, semaphore.WaitAsync(timeout), timeout);
        }

        /// <summary>
        ///     Waits asynchronously, until <paramref name="cancellationToken" /> is cancelled, to enter
        ///     <paramref name="semaphore" /> and returns an object that releases it when disposed. 
        /// </summary>
        /// <remarks>
        ///     The argument exceptions are thrown before the method returns; the cancellation is
        ///     reported through the returned task, also when <paramref name="cancellationToken" /> is
        ///     already cancelled. A cancelled wait has not entered the semaphore, so there is nothing to
        ///     release. Further <see cref="IDisposable.Dispose" /> calls on the result do nothing. 
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="semaphore" /> is null.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        ///     Thrown when <paramref name="semaphore" /> has been disposed.
        /// </exception>
        /// <param name="semaphore">The semaphore to enter.</param>
        /// <param name="cancellationToken">The token that cancels the wait.</param>
        /// <returns>
        ///     A task whose result releases <paramref name="semaphore" /> once, on its first
        ///     <see cref="IDisposable.Dispose" /> call. The task is cancelled when
        ///     <paramref name="cancellationToken" /> is cancelled before the semaphore is entered. 
        /// </returns>
        public static Task<IDisposable> LockAsync(this SemaphoreSlim semaphore, CancellationToken cancellationToken)
        {
            return LockAsync(semaphore, Timeout.InfiniteTimeSpan, cancellationToken);
        }

        /// <summary>
        ///     Waits asynchronously, up to <paramref name="timeout" /> and until
        ///     <paramref name="cancellationToken" /> is cancelled, to enter <paramref name="semaphore" />
        ///     and returns an object that releases it when disposed.
        /// </summary>
        /// <remarks>
        ///     The argument exceptions are thrown before the method returns; the
        ///     <see cref="TimeoutException" /> and the cancellation are reported through the returned
        ///     task,
        ///     also when <paramref name="cancellationToken" /> is already cancelled. On a timeout or a
        ///     cancellation the semaphore has not been entered, so there is nothing to release. Further <see cref="IDisposable.Dispose" />
        ///     calls on the result do nothing. With <see cref="CancellationToken.None" /> it behaves as <see cref="LockAsync(SemaphoreSlim, TimeSpan)" />
        ///     .
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="semaphore" /> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="timeout" /> is negative and not <see cref="Timeout.InfiniteTimeSpan" />
        ///     , or is greater than <see cref="int.MaxValue" /> milliseconds.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        ///     Thrown when <paramref name="semaphore" /> has been disposed.
        /// </exception>
        /// <param name="semaphore">The semaphore to enter.</param>
        /// <param name="timeout">
        ///     The time to wait, from <see cref="TimeSpan.Zero" /> to <see cref="int.MaxValue" />
        ///     milliseconds, or <see cref="Timeout.InfiniteTimeSpan" /> to wait without a time limit.
        /// </param>
        /// <param name="cancellationToken">The token that cancels the wait.</param>
        /// <returns>
        ///     A task whose result releases <paramref name="semaphore" /> once, on its first
        ///     <see cref="IDisposable.Dispose" /> call. The task faults with a <see cref="TimeoutException" />
        ///     when the semaphore could not be entered within <paramref name="timeout" />, and is
        ///     cancelled when <paramref name="cancellationToken" /> is cancelled before the semaphore is
        ///     entered.
        /// </returns>
        public static Task<IDisposable> LockAsync(this SemaphoreSlim semaphore, TimeSpan timeout,
            CancellationToken cancellationToken)
        {
            DomainEnsure.IsNotNull(semaphore, nameof(semaphore));

            return CreateReleaserAsync(semaphore, semaphore.WaitAsync(timeout, cancellationToken), timeout);
        }

        /// <summary>
        ///     Awaits a pending semaphore wait and wraps the entered semaphore in a releaser.
        /// </summary>
        /// <exception cref="TimeoutException">Thrown when the wait timed out.</exception>
        /// <param name="semaphore">The semaphore being entered.</param>
        /// <param name="wait">The pending <see cref="SemaphoreSlim.WaitAsync(TimeSpan)" /> task.</param>
        /// <param name="timeout">The timeout passed to the wait, used in the error message.</param>
        /// <returns>
        ///     The releaser for the entered semaphore.
        /// </returns>
        private static async Task<IDisposable> CreateReleaserAsync(SemaphoreSlim semaphore, Task<bool> wait,
            TimeSpan timeout)
        {
            if (!await wait.ConfigureAwait(false))
                throw new TimeoutException($"The semaphore could not be entered within {timeout}.");

            return new SemaphoreReleaser(semaphore);
        }
    }
#endif
}