// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-08-10 21:39
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-12 23:39
// ***********************************************************************
//  <copyright file="TaskExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Threading;
using System.Threading.Tasks;
using RzR.Extensions.Domain.Validation;

#endregion

namespace RzR.Extensions.Domain.Async
{
    /// <summary>
    ///     Task extensions
    /// </summary>
    /// <remarks></remarks>
    public static class TaskExtensions
    {
#if NET45_OR_GREATER || NET || NETSTANDARD1_0_OR_GREATER
        /// <summary>
        ///     Execute async
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        /// <returns></returns>
        public static T ExecuteAsync<T>(this Task<T> task)
        {
            return task.GetAwaiter().GetResult();
        }

        /// <summary>
        ///     Execute
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static void ExecuteAsync(this Task task)
        {
            task.GetAwaiter().GetResult();
        }

        /// <summary>
        ///     Gets a task that completes like <paramref name="task"/>, or faults with a
        ///     <see cref="TimeoutException"/> when <paramref name="task"/> does not complete within
        ///     <paramref name="timeout"/>.
        /// </summary>
        /// <remarks>
        ///     The arguments are checked before the method returns; a <see cref="TimeoutException"/> is
        ///     reported through the returned task. The timeout is converted to whole milliseconds, as
        ///     <see cref="Task.Delay(TimeSpan)"/> does.
        ///     When <paramref name="task"/> has already completed, or <paramref name="timeout"/> is
        ///     <see cref="Timeout.InfiniteTimeSpan"/>, <paramref name="task"/> itself is returned.
        ///     A timeout does not stop <paramref name="task"/>; it keeps running. If it faults later,
        ///     its exception is observed, so it is not reported through <see cref="TaskScheduler.UnobservedTaskException"/>.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="task"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="timeout"/> is negative and not <see cref="Timeout.InfiniteTimeSpan"/>
        ///     , or is greater than <see cref="int.MaxValue"/> milliseconds.
        /// </exception>
        /// <param name="task">The task to wait for.</param>
        /// <param name="timeout">
        ///     The time to wait, from <see cref="TimeSpan.Zero"/> to <see cref="int.MaxValue"/>
        ///     milliseconds, or <see cref="Timeout.InfiniteTimeSpan"/> to wait without a time limit.
        /// </param>
        /// <returns>
        ///     A task that completes, faults or is cancelled exactly as <paramref name="task"/> does,
        ///     with all of its exceptions, or faults with a <see cref="TimeoutException"/> when the time
        ///     runs out first.
        /// </returns>
        public static Task WithTimeout(this Task task, TimeSpan timeout)
        {
            if (ShouldReturnTaskAsIs(task, timeout))
                return task;

            return RaceWithTimeout(task, timeout).Unwrap();
        }

        /// <summary>
        ///     Gets a task that completes like <paramref name="task"/>, or faults with a
        ///     <see cref="TimeoutException"/> when <paramref name="task"/> does not complete within
        ///     <paramref name="timeout"/>.
        /// </summary>
        /// <remarks>
        ///     The arguments are checked before the method returns; a <see cref="TimeoutException"/> is
        ///     reported through the returned task. The timeout is converted to whole milliseconds, as
        ///     <see cref="Task.Delay(TimeSpan)"/> does.
        ///     When <paramref name="task"/> has already completed, or <paramref name="timeout"/> is
        ///     <see cref="Timeout.InfiniteTimeSpan"/>, <paramref name="task"/> itself is returned.
        ///     A timeout does not stop <paramref name="task"/>; it keeps running. If it faults later,
        ///     its exception is observed, so it is not reported through <see cref="TaskScheduler.UnobservedTaskException"/>.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="task"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="timeout"/> is negative and not <see cref="Timeout.InfiniteTimeSpan"/>
        ///     , or is greater than <see cref="int.MaxValue"/> milliseconds.
        /// </exception>
        /// <typeparam name="T">The type of the task result.</typeparam>
        /// <param name="task">The task to wait for.</param>
        /// <param name="timeout">
        ///     The time to wait, from <see cref="TimeSpan.Zero"/> to <see cref="int.MaxValue"/>
        ///     milliseconds, or <see cref="Timeout.InfiniteTimeSpan"/> to wait without a time limit.
        /// </param>
        /// <returns>
        ///     A task that completes with the result of <paramref name="task"/>, or faults or is
        ///     cancelled exactly as <paramref name="task"/> does, with all of its exceptions, or faults
        ///     with a
        ///     <see cref="TimeoutException"/> when the time runs out first.
        /// </returns>
        public static Task<T> WithTimeout<T>(this Task<T> task, TimeSpan timeout)
        {
            if (ShouldReturnTaskAsIs(task, timeout))
                return task;

            return RaceWithTimeout(task, timeout).Unwrap();
        }

        /// <summary>
        ///     Gets a task that completes like <paramref name="task"/>, faults with a
        ///     <see cref="TimeoutException"/> when <paramref name="task"/> does not complete within
        ///     <paramref name="timeout"/>, or is cancelled when <paramref name="cancellationToken"/> is
        ///     cancelled first.
        /// </summary>
        /// <remarks>
        ///     The arguments are checked before the method returns; a <see cref="TimeoutException"/> or
        ///     a cancellation is reported through the returned task. The timeout is converted to whole
        ///     milliseconds, as <see cref="Task.Delay(TimeSpan)"/> does. When <paramref name="task"/>
        ///     has already completed, <paramref name="task"/> itself is returned, even when <paramref name="cancellationToken"/>
        ///     is already cancelled. Otherwise, an already cancelled <paramref name="cancellationToken"/>
        ///     gives a cancelled task at once. With <see cref="CancellationToken.None"/> it behaves as <see cref="WithTimeout(Task, TimeSpan)"/>. 
        ///     A timeout or a cancellation does not stop <paramref name="task"/>; this method cannot
        ///     cancel it, and it keeps running. If it faults later, its exception is observed, so it is
        ///     not reported through <see cref="TaskScheduler.UnobservedTaskException"/>. Do not use it
        ///     to bound <see cref="SemaphoreSlimExtensions.LockAsync(SemaphoreSlim)"/>: when the timeout
        ///     or the cancellation wins, the pending wait still acquires the semaphore later and nothing
        ///     releases it. To bound a lock wait by cancellation use
        ///     <see cref="SemaphoreSlimExtensions.LockAsync(SemaphoreSlim, CancellationToken)"/> or
        ///     <see cref="SemaphoreSlimExtensions.LockAsync(SemaphoreSlim, TimeSpan, CancellationToken)"/>.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="task"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="timeout"/> is negative and not <see cref="Timeout.InfiniteTimeSpan"/>
        ///     , or is greater than <see cref="int.MaxValue"/> milliseconds.
        /// </exception>
        /// <param name="task">The task to wait for.</param>
        /// <param name="timeout">
        ///     The time to wait, from <see cref="TimeSpan.Zero"/> to <see cref="int.MaxValue"/>
        ///     milliseconds, or <see cref="Timeout.InfiniteTimeSpan"/> to wait without a time limit.
        /// </param>
        /// <param name="cancellationToken">
        ///     The token that cancels the wait, not <paramref name="task"/>.
        /// </param>
        /// <returns>
        ///     A task that completes, faults or is cancelled exactly as <paramref name="task"/> does,
        ///     with all of its exceptions, faults with a <see cref="TimeoutException"/> when the time
        ///     runs out first, or is cancelled when <paramref name="cancellationToken"/> is cancelled
        ///     first.
        /// </returns>
        public static Task WithTimeout(this Task task, TimeSpan timeout, CancellationToken cancellationToken)
        {
            if (!cancellationToken.CanBeCanceled)
                return WithTimeout(task, timeout);

            ShouldReturnTaskAsIs(task, timeout);
            if (task.IsCompleted)
                return task;

            return RaceWithTimeout(task, timeout, cancellationToken).Unwrap();
        }

        /// <summary>
        ///     Gets a task that completes like <paramref name="task"/>, faults with a
        ///     <see cref="TimeoutException"/> when <paramref name="task"/> does not complete within
        ///     <paramref name="timeout"/>, or is cancelled when <paramref name="cancellationToken"/> is
        ///     cancelled first.
        /// </summary>
        /// <remarks>
        ///     The arguments are checked before the method returns; a <see cref="TimeoutException"/> or
        ///     a cancellation is reported through the returned task. The timeout is converted to whole
        ///     milliseconds, as <see cref="Task.Delay(TimeSpan)"/> does. When <paramref name="task"/>
        ///     has already completed, <paramref name="task"/> itself is returned, even when <paramref name="cancellationToken"/>
        ///     is already cancelled. Otherwise, an already cancelled <paramref name="cancellationToken"/>
        ///     gives a cancelled task at once. With <see cref="CancellationToken.None"/> it behaves as <see cref="WithTimeout{T}(Task{T}, TimeSpan)"/>. A timeout or a cancellation does not stop <paramref name="task"/>; this method cannot
        ///     cancel it, and it keeps running. If it faults later, its exception is observed, so it is
        ///     not reported through <see cref="TaskScheduler.UnobservedTaskException"/>. Do not use it
        ///     to bound <see cref="SemaphoreSlimExtensions.LockAsync(SemaphoreSlim)"/>: when the timeout
        ///     or the cancellation wins, the pending wait still acquires the semaphore later and nothing
        ///     releases it. To bound a lock wait by cancellation use
        ///     <see cref="SemaphoreSlimExtensions.LockAsync(SemaphoreSlim, CancellationToken)"/> or
        ///     <see cref="SemaphoreSlimExtensions.LockAsync(SemaphoreSlim, TimeSpan, CancellationToken)"/>///     .
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="task"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="timeout"/> is negative and not <see cref="Timeout.InfiniteTimeSpan"/>
        ///     , or is greater than <see cref="int.MaxValue"/> milliseconds.
        /// </exception>
        /// <typeparam name="T">The type of the task result.</typeparam>
        /// <param name="task">The task to wait for.</param>
        /// <param name="timeout">
        ///     The time to wait, from <see cref="TimeSpan.Zero"/> to <see cref="int.MaxValue"/>
        ///     milliseconds, or <see cref="Timeout.InfiniteTimeSpan"/> to wait without a time limit.
        /// </param>
        /// <param name="cancellationToken">
        ///     The token that cancels the wait, not <paramref name="task"/>.
        /// </param>
        /// <returns>
        ///     A task that completes with the result of <paramref name="task"/>, or faults or is
        ///     cancelled exactly as <paramref name="task"/> does, with all of its exceptions, faults
        ///     with a
        ///     <see cref="TimeoutException"/> when the time runs out first, or is cancelled when
        ///     <paramref name="cancellationToken"/> is cancelled first.
        /// </returns>
        public static Task<T> WithTimeout<T>(this Task<T> task, TimeSpan timeout, CancellationToken cancellationToken)
        {
            if (!cancellationToken.CanBeCanceled)
                return WithTimeout(task, timeout);

            ShouldReturnTaskAsIs(task, timeout);
            if (task.IsCompleted)
                return task;

            return RaceWithTimeout(task, timeout, cancellationToken).Unwrap();
        }

        /// <summary>
        ///     Validates the <c>WithTimeout</c> arguments and tells whether no timer is needed.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="timeout"/> is out of range.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="task"/> is null.
        /// </exception>
        /// <param name="task">The task to wait for.</param>
        /// <param name="timeout">The time to wait.</param>
        /// <returns>
        ///     True when <paramref name="task"/> has already completed or <paramref name="timeout"/> is
        ///     infinite; otherwise false.
        /// </returns>
        private static bool ShouldReturnTaskAsIs(Task task, TimeSpan timeout)
        {
            DomainEnsure.IsNotNull(task, nameof(task));

            var timeoutMilliseconds = (long)timeout.TotalMilliseconds;
            if (timeoutMilliseconds < Timeout.Infinite || timeoutMilliseconds > int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(timeout),
                    "The timeout must be between 0 and Int32.MaxValue milliseconds, or Timeout.InfiniteTimeSpan.");

            return task.IsCompleted || timeoutMilliseconds == Timeout.Infinite;
        }

        /// <summary>
        ///     Races <paramref name="task"/> against a timer.
        /// </summary>
        /// <remarks>
        ///     Unwrapping the result yields a proxy that copies the final state of <paramref name="task"/>
        ///     , including every exception of an <see cref="AggregateException"/>, which an <c>await</c>
        ///     based implementation would reduce to the first one.
        /// </remarks>
        /// <exception cref="TimeoutException">Thrown when a Timeout error condition occurs.</exception>
        /// <typeparam name="TTask">The type of the task.</typeparam>
        /// <param name="task">The task to wait for.</param>
        /// <param name="timeout">A validated, finite timeout.</param>
        /// <returns>
        ///     A task whose result is <paramref name="task"/> when it wins the race, or that faults with
        ///     a <see cref="TimeoutException"/> when the timer wins.
        /// </returns>
        private static Task<TTask> RaceWithTimeout<TTask>(TTask task, TimeSpan timeout)
            where TTask : Task
        {
            var timerCancellation = new CancellationTokenSource();

            return Task.WhenAny(task, Task.Delay(timeout, timerCancellation.Token))
                .ContinueWith(winner =>
                {
                    timerCancellation.Cancel();
                    timerCancellation.Dispose();

                    if (winner.Result == task)
                        return task;

                    ObserveFault(task);

                    throw new TimeoutException($"The task did not complete within {timeout}.");
                }, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
        }

        /// <summary>
        ///     Races <paramref name="task"/> against a timer and <paramref name="cancellationToken"/>.
        /// </summary>
        /// <remarks>
        ///     The method is async so that an <see cref="OperationCanceledException"/> cancels the
        ///     returned task instead of faulting it; <paramref name="task"/> is still returned as is, so
        ///     unwrapping keeps all of its exceptions. Cancelling the linked source releases the timer
        ///     when <paramref name="task"/> wins, and disposing it unregisters from <paramref name="cancellationToken"/>.
        /// </remarks>
        /// <exception cref="OperationCanceledException">
        ///     Thrown when a thread cancels a running operation.
        /// </exception>
        /// <exception cref="TimeoutException">Thrown when a Timeout error condition occurs.</exception>
        /// <typeparam name="TTask">The type of the task.</typeparam>
        /// <param name="task">The task to wait for; it has not completed yet.</param>
        /// <param name="timeout">A validated timeout; it may be infinite.</param>
        /// <param name="cancellationToken">The token that cancels the wait.</param>
        /// <returns>
        ///     A task whose result is <paramref name="task"/> when it wins the race, that faults with a
        ///     <see cref="TimeoutException"/> when the timer wins, or that is cancelled when
        ///     <paramref name="cancellationToken"/> is cancelled first or already.
        /// </returns>
        private static async Task<TTask> RaceWithTimeout<TTask>(TTask task, TimeSpan timeout,
            CancellationToken cancellationToken)
            where TTask : Task
        {
            if (cancellationToken.IsCancellationRequested)
            {
                ObserveFault(task);

                throw new OperationCanceledException(cancellationToken);
            }

            using (var timerCancellation = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
            {
                var timer = Task.Delay(timeout, timerCancellation.Token);
                var winner = await Task.WhenAny(task, timer).ConfigureAwait(false);
                timerCancellation.Cancel();

                if (winner == task)
                    return task;

                ObserveFault(task);

                if (timer.IsCanceled)
                    throw new OperationCanceledException(cancellationToken);

                throw new TimeoutException($"The task did not complete within {timeout}.");
            }
        }

        /// <summary>
        ///     Marks a later fault of an abandoned task as observed.
        /// </summary>
        /// <param name="task">The abandoned task.</param>
        private static void ObserveFault(Task task)
        {
            task.ContinueWith(abandoned => abandoned.Exception, CancellationToken.None,
                TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously,
                TaskScheduler.Default);
        }
#endif
    }
}
