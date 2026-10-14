using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Async;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class AsyncCancellationOverloadTests
    {
        private static readonly TimeSpan ShortTimeout = TimeSpan.FromMilliseconds(50);
        private static readonly TimeSpan LongTimeout = TimeSpan.FromSeconds(30);

        [TestMethod]
        public async Task LockAsync_TokenAcquiresAndReleases_Test()
        {
            using var semaphore = new SemaphoreSlim(1, 1);
            using var cancellation = new CancellationTokenSource();

            var releaser = await semaphore.LockAsync(cancellation.Token);
            Assert.AreEqual(0, semaphore.CurrentCount);
            releaser.Dispose();

            Assert.AreEqual(1, semaphore.CurrentCount);
        }

        [TestMethod]
        public async Task LockAsync_TimeoutTokenAcquiresAndReleases_Test()
        {
            using var semaphore = new SemaphoreSlim(1, 1);
            using var cancellation = new CancellationTokenSource();

            var releaser = await semaphore.LockAsync(LongTimeout, cancellation.Token);
            Assert.AreEqual(0, semaphore.CurrentCount);
            releaser.Dispose();

            Assert.AreEqual(1, semaphore.CurrentCount);
        }

        [TestMethod]
        public async Task LockAsync_TokenUsingBlockReleases_Test()
        {
            using var semaphore = new SemaphoreSlim(1, 1);
            using var cancellation = new CancellationTokenSource();

            using (await semaphore.LockAsync(cancellation.Token))
            {
                Assert.AreEqual(0, semaphore.CurrentCount);
            }

            Assert.AreEqual(1, semaphore.CurrentCount);
        }

        [TestMethod]
        public async Task LockAsync_CancelledTokenOnHeldSemaphore_Test()
        {
            using var semaphore = new SemaphoreSlim(1, 1);
            using var cancellation = new CancellationTokenSource();
            cancellation.Cancel();
            var holder = await semaphore.LockAsync();

            var result = semaphore.LockAsync(cancellation.Token);

            Assert.IsTrue(await AsyncTimeoutTestHelper.CompletesWithinAsync(result, 10000));
            Assert.IsTrue(result.IsCanceled);
            Assert.AreEqual(0, semaphore.CurrentCount);
            holder.Dispose();
            Assert.AreEqual(1, semaphore.CurrentCount);
        }

        [TestMethod]
        public async Task LockAsync_TimeoutCancelledTokenOnHeldSemaphore_Test()
        {
            using var semaphore = new SemaphoreSlim(1, 1);
            using var cancellation = new CancellationTokenSource();
            cancellation.Cancel();
            var holder = await semaphore.LockAsync();

            var result = semaphore.LockAsync(LongTimeout, cancellation.Token);

            Assert.IsTrue(await AsyncTimeoutTestHelper.CompletesWithinAsync(result, 10000));
            Assert.IsTrue(result.IsCanceled);
            Assert.AreEqual(0, semaphore.CurrentCount);
            holder.Dispose();
            Assert.AreEqual(1, semaphore.CurrentCount);
        }

        [TestMethod]
        public async Task LockAsync_CancelledTokenOnFreeSemaphore_Test()
        {
            using var semaphore = new SemaphoreSlim(1, 1);
            using var cancellation = new CancellationTokenSource();
            cancellation.Cancel();

            var result = semaphore.LockAsync(cancellation.Token);

            Assert.IsTrue(await AsyncTimeoutTestHelper.CompletesWithinAsync(result, 10000));
            Assert.IsTrue(result.IsCanceled);
            Assert.AreEqual(1, semaphore.CurrentCount);
        }

        [TestMethod]
        public async Task LockAsync_CancelWhileWaitingDoesNotHold_Test()
        {
            using var semaphore = new SemaphoreSlim(1, 1);
            using var cancellation = new CancellationTokenSource();
            var holder = await semaphore.LockAsync();
            var waiter = semaphore.LockAsync(cancellation.Token);
            Assert.IsFalse(waiter.IsCompleted);

            cancellation.Cancel();

            Assert.IsTrue(await AsyncTimeoutTestHelper.CompletesWithinAsync(waiter, 10000));
            Assert.IsTrue(waiter.IsCanceled);
            Assert.AreEqual(0, semaphore.CurrentCount);
            holder.Dispose();
            Assert.AreEqual(1, semaphore.CurrentCount);
        }

        [TestMethod]
        public async Task LockAsync_TimeoutCancelWhileWaitingDoesNotHold_Test()
        {
            using var semaphore = new SemaphoreSlim(1, 1);
            using var cancellation = new CancellationTokenSource();
            var holder = await semaphore.LockAsync();
            var waiter = semaphore.LockAsync(LongTimeout, cancellation.Token);
            Assert.IsFalse(waiter.IsCompleted);

            cancellation.Cancel();

            Assert.IsTrue(await AsyncTimeoutTestHelper.CompletesWithinAsync(waiter, 10000));
            Assert.IsTrue(waiter.IsCanceled);
            Assert.AreEqual(0, semaphore.CurrentCount);
            holder.Dispose();
            Assert.AreEqual(1, semaphore.CurrentCount);
        }

        [TestMethod]
        public async Task LockAsync_CancelledWhileWaitingCarriesToken_Test()
        {
            using var semaphore = new SemaphoreSlim(1, 1);
            using var cancellation = new CancellationTokenSource();
            var holder = await semaphore.LockAsync();
            var waiter = semaphore.LockAsync(cancellation.Token);

            cancellation.Cancel();

            Assert.IsTrue(await AsyncTimeoutTestHelper.CompletesWithinAsync(waiter, 10000));
            try
            {
                await waiter;
                Assert.Fail("The returned task was not cancelled.");
            }
            catch (OperationCanceledException ex)
            {
                Assert.AreEqual(cancellation.Token, ex.CancellationToken);
            }
            finally
            {
                holder.Dispose();
            }
        }

        [TestMethod]
        public async Task LockAsync_LaterWaiterProceedsAfterCancel_Test()
        {
            using var semaphore = new SemaphoreSlim(1, 1);
            using var cancellation = new CancellationTokenSource();
            using var laterCancellation = new CancellationTokenSource();
            var holder = await semaphore.LockAsync();
            var cancelledWaiter = semaphore.LockAsync(cancellation.Token);
            var laterWaiter = semaphore.LockAsync(laterCancellation.Token);

            cancellation.Cancel();
            Assert.IsTrue(await AsyncTimeoutTestHelper.CompletesWithinAsync(cancelledWaiter, 10000));
            Assert.IsTrue(cancelledWaiter.IsCanceled);
            Assert.IsFalse(laterWaiter.IsCompleted);
            holder.Dispose();
            var later = await AsyncTimeoutTestHelper.WithinAsync(laterWaiter, 10000);

            Assert.AreEqual(0, semaphore.CurrentCount);
            later.Dispose();
            Assert.AreEqual(1, semaphore.CurrentCount);
        }

        [TestMethod]
        public async Task LockAsync_TimeoutLaterWaiterProceedsAfterCancel_Test()
        {
            using var semaphore = new SemaphoreSlim(1, 1);
            using var cancellation = new CancellationTokenSource();
            using var laterCancellation = new CancellationTokenSource();
            var holder = await semaphore.LockAsync();
            var cancelledWaiter = semaphore.LockAsync(LongTimeout, cancellation.Token);
            var laterWaiter = semaphore.LockAsync(LongTimeout, laterCancellation.Token);

            cancellation.Cancel();
            Assert.IsTrue(await AsyncTimeoutTestHelper.CompletesWithinAsync(cancelledWaiter, 10000));
            Assert.IsTrue(cancelledWaiter.IsCanceled);
            Assert.IsFalse(laterWaiter.IsCompleted);
            holder.Dispose();
            var later = await AsyncTimeoutTestHelper.WithinAsync(laterWaiter, 10000);

            Assert.AreEqual(0, semaphore.CurrentCount);
            later.Dispose();
            Assert.AreEqual(1, semaphore.CurrentCount);
        }

        [TestMethod]
        public async Task LockAsync_TimeoutWithLiveToken_Test()
        {
            using var semaphore = new SemaphoreSlim(1, 1);
            using var cancellation = new CancellationTokenSource();
            var holder = await semaphore.LockAsync();

            await Assert.ThrowsExceptionAsync<TimeoutException>(
                () => semaphore.LockAsync(ShortTimeout, cancellation.Token));

            Assert.IsFalse(cancellation.IsCancellationRequested);
            Assert.AreEqual(0, semaphore.CurrentCount);
            holder.Dispose();
            Assert.AreEqual(1, semaphore.CurrentCount);
        }

        [TestMethod]
        public async Task LockAsync_TimeoutWithNoneToken_Test()
        {
            using var semaphore = new SemaphoreSlim(1, 1);
            var holder = await semaphore.LockAsync();

            await Assert.ThrowsExceptionAsync<TimeoutException>(
                () => semaphore.LockAsync(ShortTimeout, CancellationToken.None));

            Assert.AreEqual(0, semaphore.CurrentCount);
            holder.Dispose();
            Assert.AreEqual(1, semaphore.CurrentCount);
        }

        [TestMethod]
        public void LockAsync_TokenOverloadNullSemaphore_Test()
        {
            SemaphoreSlim semaphore = null;
            using var cancellation = new CancellationTokenSource();
            cancellation.Cancel();

            var exception = Assert.ThrowsException<ArgumentNullException>(
                () => semaphore.LockAsync(cancellation.Token));

            Assert.AreEqual("semaphore", exception.ParamName);
        }

        [TestMethod]
        public void LockAsync_TimeoutTokenOverloadNullSemaphore_Test()
        {
            SemaphoreSlim semaphore = null;
            using var cancellation = new CancellationTokenSource();
            cancellation.Cancel();

            var exception = Assert.ThrowsException<ArgumentNullException>(
                () => semaphore.LockAsync(LongTimeout, cancellation.Token));

            Assert.AreEqual("semaphore", exception.ParamName);
        }

        [DataRow(-20000L, false)]
        [DataRow(-20000L, true)]
        [DataRow(-10000000L, false)]
        [DataRow(21474836480000L, false)]
        [DataRow(21474836480000L, true)]
        [DataRow(long.MaxValue, true)]
        [TestMethod]
        public void LockAsync_TokenTimeoutOutOfRange_Test(long timeoutTicks, bool cancelled)
        {
            using var semaphore = new SemaphoreSlim(1, 1);
            using var cancellation = new CancellationTokenSource();
            if (cancelled)
                cancellation.Cancel();

            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => semaphore.LockAsync(TimeSpan.FromTicks(timeoutTicks), cancellation.Token));

            Assert.AreEqual("timeout", exception.ParamName);
            Assert.AreEqual(1, semaphore.CurrentCount);
        }

        [TestMethod]
        public async Task LockAsync_TokenOverloadDisposedSemaphore_Test()
        {
            var semaphore = new SemaphoreSlim(1, 1);
            semaphore.Dispose();
            using var cancellation = new CancellationTokenSource();

            await Assert.ThrowsExceptionAsync<ObjectDisposedException>(
                () => semaphore.LockAsync(cancellation.Token));
        }

        [TestMethod]
        public async Task LockAsync_TimeoutTokenDisposedSemaphore_Test()
        {
            var semaphore = new SemaphoreSlim(1, 1);
            semaphore.Dispose();
            using var cancellation = new CancellationTokenSource();

            await Assert.ThrowsExceptionAsync<ObjectDisposedException>(
                () => semaphore.LockAsync(LongTimeout, cancellation.Token));
        }

        [TestMethod]
        public async Task LockAsync_TokenDisposeTwiceReleasesOnce_Test()
        {
            using var semaphore = new SemaphoreSlim(1, 10);
            using var cancellation = new CancellationTokenSource();
            var releaser = await semaphore.LockAsync(cancellation.Token);

            releaser.Dispose();
            releaser.Dispose();

            Assert.AreEqual(1, semaphore.CurrentCount);
        }

        [TestMethod]
        public async Task LockAsync_TimeoutTokenDisposeTwiceReleasesOnce_Test()
        {
            using var semaphore = new SemaphoreSlim(1, 10);
            using var cancellation = new CancellationTokenSource();
            var releaser = await semaphore.LockAsync(LongTimeout, cancellation.Token);

            releaser.Dispose();
            releaser.Dispose();

            Assert.AreEqual(1, semaphore.CurrentCount);
        }

        [TestMethod]
        public void WithTimeout_TokenNullTask_Test()
        {
            Task task = null;
            using var cancellation = new CancellationTokenSource();
            cancellation.Cancel();

            var exception = Assert.ThrowsException<ArgumentNullException>(
                () => task.WithTimeout(LongTimeout, cancellation.Token));

            Assert.AreEqual("task", exception.ParamName);
        }

        [TestMethod]
        public void WithTimeout_TokenNullGenericTask_Test()
        {
            Task<int> task = null;
            using var cancellation = new CancellationTokenSource();
            cancellation.Cancel();

            var exception = Assert.ThrowsException<ArgumentNullException>(
                () => task.WithTimeout(LongTimeout, cancellation.Token));

            Assert.AreEqual("task", exception.ParamName);
        }

        [TestMethod]
        public void WithTimeout_NoneTokenNullTask_Test()
        {
            Task task = null;

            var exception = Assert.ThrowsException<ArgumentNullException>(
                () => task.WithTimeout(LongTimeout, CancellationToken.None));

            Assert.AreEqual("task", exception.ParamName);
        }

        [DataRow(-20000L)]
        [DataRow(-10000000L)]
        [DataRow(long.MinValue)]
        [DataRow(21474836480000L)]
        [DataRow(long.MaxValue)]
        [TestMethod]
        public void WithTimeout_CancelledTokenTimeoutOutOfRange_Test(long timeoutTicks)
        {
            var source = new TaskCompletionSource<bool>();
            Task task = source.Task;
            using var cancellation = new CancellationTokenSource();
            cancellation.Cancel();

            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => task.WithTimeout(TimeSpan.FromTicks(timeoutTicks), cancellation.Token));

            Assert.AreEqual("timeout", exception.ParamName);
        }

        [DataRow(-20000L)]
        [DataRow(21474836480000L)]
        [TestMethod]
        public void WithTimeout_GenericCancelledTokenTimeoutOutOfRange_Test(long timeoutTicks)
        {
            var source = new TaskCompletionSource<int>();
            using var cancellation = new CancellationTokenSource();
            cancellation.Cancel();

            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => source.Task.WithTimeout(TimeSpan.FromTicks(timeoutTicks), cancellation.Token));

            Assert.AreEqual("timeout", exception.ParamName);
        }

        [TestMethod]
        public void WithTimeout_CompletedTaskCancelledTokenInvalidTimeout_Test()
        {
            using var cancellation = new CancellationTokenSource();
            cancellation.Cancel();

            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => Task.CompletedTask.WithTimeout(TimeSpan.FromMilliseconds(-2), cancellation.Token));

            Assert.AreEqual("timeout", exception.ParamName);
        }

        [TestMethod]
        public void WithTimeout_CompletedGenericCancelledTokenInvalidTimeout_Test()
        {
            using var cancellation = new CancellationTokenSource();
            cancellation.Cancel();

            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => Task.FromResult(7).WithTimeout(TimeSpan.FromMilliseconds(-2), cancellation.Token));

            Assert.AreEqual("timeout", exception.ParamName);
        }

        [TestMethod]
        public void WithTimeout_LiveTokenTimeoutOutOfRange_Test()
        {
            var source = new TaskCompletionSource<int>();
            using var cancellation = new CancellationTokenSource();

            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => source.Task.WithTimeout(TimeSpan.FromMilliseconds(-2), cancellation.Token));

            Assert.AreEqual("timeout", exception.ParamName);
        }

        [DataRow(30000L)]
        [DataRow(0L)]
        [DataRow(-1L)]
        [TestMethod]
        public void WithTimeout_CompletedTaskCancelledToken_Test(long timeoutMilliseconds)
        {
            var task = Task.CompletedTask;
            using var cancellation = new CancellationTokenSource();
            cancellation.Cancel();

            var result = task.WithTimeout(TimeSpan.FromMilliseconds(timeoutMilliseconds), cancellation.Token);

            Assert.AreSame(task, result);
        }

        [DataRow(30000L)]
        [DataRow(0L)]
        [DataRow(-1L)]
        [TestMethod]
        public void WithTimeout_CompletedGenericTaskCancelledToken_Test(long timeoutMilliseconds)
        {
            var task = Task.FromResult(7);
            using var cancellation = new CancellationTokenSource();
            cancellation.Cancel();

            var result = task.WithTimeout(TimeSpan.FromMilliseconds(timeoutMilliseconds), cancellation.Token);

            Assert.AreSame(task, result);
            Assert.AreEqual(7, result.Result);
        }

        [TestMethod]
        public void WithTimeout_FaultedTaskCancelledToken_Test()
        {
            var task = Task.FromException(new InvalidOperationException("boom"));
            using var cancellation = new CancellationTokenSource();
            cancellation.Cancel();

            var result = task.WithTimeout(LongTimeout, cancellation.Token);

            Assert.AreSame(task, result);
        }

        [TestMethod]
        public void WithTimeout_CanceledTaskCancelledToken_Test()
        {
            var task = Task.FromCanceled<int>(new CancellationToken(true));
            using var cancellation = new CancellationTokenSource();
            cancellation.Cancel();

            var result = task.WithTimeout(LongTimeout, cancellation.Token);

            Assert.AreSame(task, result);
        }

        [TestMethod]
        public void WithTimeout_CompletedTaskLiveToken_Test()
        {
            var task = Task.FromResult(9);
            using var cancellation = new CancellationTokenSource();

            var result = task.WithTimeout(LongTimeout, cancellation.Token);

            Assert.AreSame(task, result);
        }

        [DataRow(30000L)]
        [DataRow(-1L)]
        [TestMethod]
        public async Task WithTimeout_PendingTaskCancelledToken_Test(long timeoutMilliseconds)
        {
            var source = new TaskCompletionSource<bool>();
            Task task = source.Task;
            using var cancellation = new CancellationTokenSource();
            cancellation.Cancel();

            var result = task.WithTimeout(TimeSpan.FromMilliseconds(timeoutMilliseconds), cancellation.Token);

            Assert.AreNotSame(task, result);
            Assert.IsTrue(await AsyncTimeoutTestHelper.CompletesWithinAsync(result, 10000));
            Assert.IsTrue(result.IsCanceled);
            Assert.IsFalse(source.Task.IsCompleted);
        }

        [DataRow(30000L)]
        [DataRow(-1L)]
        [TestMethod]
        public async Task WithTimeout_PendingGenericTaskCancelledToken_Test(long timeoutMilliseconds)
        {
            var source = new TaskCompletionSource<int>();
            using var cancellation = new CancellationTokenSource();
            cancellation.Cancel();

            var result = source.Task.WithTimeout(TimeSpan.FromMilliseconds(timeoutMilliseconds), cancellation.Token);

            Assert.AreNotSame(source.Task, result);
            Assert.IsTrue(await AsyncTimeoutTestHelper.CompletesWithinAsync(result, 10000));
            Assert.IsTrue(result.IsCanceled);
            Assert.IsFalse(source.Task.IsCompleted);
        }

        [DataRow(30000L)]
        [DataRow(-1L)]
        [TestMethod]
        public async Task WithTimeout_CancelWhilePending_Test(long timeoutMilliseconds)
        {
            var source = new TaskCompletionSource<bool>();
            Task task = source.Task;
            using var cancellation = new CancellationTokenSource();
            var result = task.WithTimeout(TimeSpan.FromMilliseconds(timeoutMilliseconds), cancellation.Token);
            Assert.IsFalse(result.IsCompleted);

            cancellation.Cancel();

            Assert.IsTrue(await AsyncTimeoutTestHelper.CompletesWithinAsync(result, 10000));
            Assert.IsTrue(result.IsCanceled);
            Assert.IsFalse(source.Task.IsCompleted);
        }

        [DataRow(30000L)]
        [DataRow(-1L)]
        [TestMethod]
        public async Task WithTimeout_GenericCancelWhilePending_Test(long timeoutMilliseconds)
        {
            var source = new TaskCompletionSource<int>();
            using var cancellation = new CancellationTokenSource();
            var result = source.Task.WithTimeout(TimeSpan.FromMilliseconds(timeoutMilliseconds), cancellation.Token);
            Assert.IsFalse(result.IsCompleted);

            cancellation.Cancel();

            Assert.IsTrue(await AsyncTimeoutTestHelper.CompletesWithinAsync(result, 10000));
            Assert.IsTrue(result.IsCanceled);
            Assert.IsFalse(source.Task.IsCompleted);
        }

        [TestMethod]
        public async Task WithTimeout_CancelledWhilePendingCarriesToken_Test()
        {
            var source = new TaskCompletionSource<int>();
            using var cancellation = new CancellationTokenSource();
            var result = source.Task.WithTimeout(LongTimeout, cancellation.Token);

            cancellation.Cancel();

            Assert.IsTrue(await AsyncTimeoutTestHelper.CompletesWithinAsync(result, 10000));
            try
            {
                await result;
                Assert.Fail("The returned task was not cancelled.");
            }
            catch (OperationCanceledException ex)
            {
                Assert.AreEqual(cancellation.Token, ex.CancellationToken);
            }
        }

        [TestMethod]
        public async Task WithTimeout_CancelledWaitKeepsTaskRunning_Test()
        {
            var source = new TaskCompletionSource<int>();
            using var cancellation = new CancellationTokenSource();
            var result = source.Task.WithTimeout(LongTimeout, cancellation.Token);
            cancellation.Cancel();
            Assert.IsTrue(await AsyncTimeoutTestHelper.CompletesWithinAsync(result, 10000));

            source.SetResult(5);

            Assert.AreEqual(5, await source.Task);
            Assert.IsTrue(result.IsCanceled);
        }

        [TestMethod]
        public async Task WithTimeout_TokenGenericCompletesInTime_Test()
        {
            var source = new TaskCompletionSource<int>();
            using var cancellation = new CancellationTokenSource();
            var result = source.Task.WithTimeout(LongTimeout, cancellation.Token);

            source.SetResult(42);

            Assert.AreEqual(42, await AsyncTimeoutTestHelper.WithinAsync(result, 10000));
        }

        [TestMethod]
        public async Task WithTimeout_TokenCompletesInTime_Test()
        {
            var source = new TaskCompletionSource<bool>();
            Task task = source.Task;
            using var cancellation = new CancellationTokenSource();
            var result = task.WithTimeout(LongTimeout, cancellation.Token);

            source.SetResult(true);

            Assert.IsTrue(await AsyncTimeoutTestHelper.CompletesWithinAsync(result, 10000));
            Assert.AreEqual(TaskStatus.RanToCompletion, result.Status);
        }

        [TestMethod]
        public async Task WithTimeout_TokenInfiniteTimeoutCompletes_Test()
        {
            var source = new TaskCompletionSource<int>();
            using var cancellation = new CancellationTokenSource();
            var result = source.Task.WithTimeout(Timeout.InfiniteTimeSpan, cancellation.Token);

            source.SetResult(13);

            Assert.AreEqual(13, await AsyncTimeoutTestHelper.WithinAsync(result, 10000));
        }

        [TestMethod]
        public async Task WithTimeout_CancelAfterCompletionKeepsResult_Test()
        {
            var source = new TaskCompletionSource<int>();
            using var cancellation = new CancellationTokenSource();
            var result = source.Task.WithTimeout(LongTimeout, cancellation.Token);
            source.SetResult(42);
            Assert.AreEqual(42, await AsyncTimeoutTestHelper.WithinAsync(result, 10000));

            cancellation.Cancel();

            Assert.AreEqual(TaskStatus.RanToCompletion, result.Status);
            Assert.AreEqual(42, result.Result);
        }

        [TestMethod]
        public async Task WithTimeout_TokenFaultPropagates_Test()
        {
            var source = new TaskCompletionSource<bool>();
            Task task = source.Task;
            using var cancellation = new CancellationTokenSource();
            var expected = new InvalidOperationException("boom");
            var result = task.WithTimeout(LongTimeout, cancellation.Token);

            source.SetException(expected);

            var actual = await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => result);
            Assert.AreSame(expected, actual);
        }

        [TestMethod]
        public async Task WithTimeout_TokenGenericFaultPropagates_Test()
        {
            var source = new TaskCompletionSource<int>();
            using var cancellation = new CancellationTokenSource();
            var expected = new FormatException("bad");
            var result = source.Task.WithTimeout(LongTimeout, cancellation.Token);

            source.SetException(expected);

            var actual = await Assert.ThrowsExceptionAsync<FormatException>(() => result);
            Assert.AreSame(expected, actual);
        }

        [TestMethod]
        public async Task WithTimeout_TokenAllExceptionsPropagate_Test()
        {
            var first = new TaskCompletionSource<int>();
            var second = new TaskCompletionSource<int>();
            using var cancellation = new CancellationTokenSource();
            var firstError = new InvalidOperationException("first");
            var secondError = new FormatException("second");
            var result = Task.WhenAll(first.Task, second.Task).WithTimeout(LongTimeout, cancellation.Token);

            first.SetException(firstError);
            second.SetException(secondError);

            Assert.IsTrue(await AsyncTimeoutTestHelper.CompletesWithinAsync(result, 10000));
            Assert.IsTrue(result.IsFaulted);
            CollectionAssert.AreEqual(new Exception[] { firstError, secondError }, result.Exception.InnerExceptions.ToArray());
        }

        [TestMethod]
        public async Task WithTimeout_TokenNonGenericAllExceptionsPropagate_Test()
        {
            var first = new TaskCompletionSource<bool>();
            var second = new TaskCompletionSource<bool>();
            using var cancellation = new CancellationTokenSource();
            var firstError = new InvalidOperationException("first");
            var secondError = new FormatException("second");
            Task task = Task.WhenAll((Task)first.Task, second.Task);
            var result = task.WithTimeout(LongTimeout, cancellation.Token);

            first.SetException(firstError);
            second.SetException(secondError);

            Assert.IsTrue(await AsyncTimeoutTestHelper.CompletesWithinAsync(result, 10000));
            Assert.IsTrue(result.IsFaulted);
            CollectionAssert.AreEqual(new Exception[] { firstError, secondError }, result.Exception.InnerExceptions.ToArray());
        }

        [TestMethod]
        public async Task WithTimeout_TokenTaskCancellationPropagates_Test()
        {
            var source = new TaskCompletionSource<int>();
            using var cancellation = new CancellationTokenSource();
            var result = source.Task.WithTimeout(LongTimeout, cancellation.Token);

            source.SetCanceled();

            Assert.IsTrue(await AsyncTimeoutTestHelper.CompletesWithinAsync(result, 10000));
            Assert.IsTrue(result.IsCanceled);
            Assert.IsFalse(cancellation.IsCancellationRequested);
        }

        [TestMethod]
        public async Task WithTimeout_TokenPendingTaskTimesOut_Test()
        {
            var source = new TaskCompletionSource<bool>();
            Task task = source.Task;
            using var cancellation = new CancellationTokenSource();

            await Assert.ThrowsExceptionAsync<TimeoutException>(() => task.WithTimeout(ShortTimeout, cancellation.Token));

            Assert.IsFalse(source.Task.IsCompleted);
            Assert.IsFalse(cancellation.IsCancellationRequested);
        }

        [TestMethod]
        public async Task WithTimeout_TokenPendingGenericTaskTimesOut_Test()
        {
            var source = new TaskCompletionSource<int>();
            using var cancellation = new CancellationTokenSource();

            await Assert.ThrowsExceptionAsync<TimeoutException>(() => source.Task.WithTimeout(ShortTimeout, cancellation.Token));

            Assert.IsFalse(source.Task.IsCompleted);
            Assert.IsFalse(cancellation.IsCancellationRequested);
        }

        [TestMethod]
        public async Task WithTimeout_TokenZeroTimeoutPendingTask_Test()
        {
            var source = new TaskCompletionSource<int>();
            using var cancellation = new CancellationTokenSource();

            await Assert.ThrowsExceptionAsync<TimeoutException>(() => source.Task.WithTimeout(TimeSpan.Zero, cancellation.Token));
        }

        [TestMethod]
        public async Task WithTimeout_TokenTimedOutTaskKeepsRunning_Test()
        {
            var source = new TaskCompletionSource<int>();
            using var cancellation = new CancellationTokenSource();
            await Assert.ThrowsExceptionAsync<TimeoutException>(() => source.Task.WithTimeout(ShortTimeout, cancellation.Token));

            source.SetResult(5);

            Assert.AreEqual(5, await source.Task);
        }

        [DataRow(false)]
        [DataRow(true)]
        [TestMethod]
        public void WithTimeout_NoneTokenCompletedTask_Test(bool useDefault)
        {
            var token = useDefault ? default : CancellationToken.None;
            var task = Task.CompletedTask;

            var result = task.WithTimeout(LongTimeout, token);

            Assert.AreSame(task, result);
            Assert.AreSame(task.WithTimeout(LongTimeout), result);
        }

        [DataRow(false)]
        [DataRow(true)]
        [TestMethod]
        public void WithTimeout_NoneTokenCompletedGenericTask_Test(bool useDefault)
        {
            var token = useDefault ? default : CancellationToken.None;
            var task = Task.FromResult(7);

            var result = task.WithTimeout(TimeSpan.Zero, token);

            Assert.AreSame(task, result);
            Assert.AreSame(task.WithTimeout(TimeSpan.Zero), result);
        }

        [DataRow(false)]
        [DataRow(true)]
        [TestMethod]
        public void WithTimeout_NoneTokenInfiniteTimeout_Test(bool useDefault)
        {
            var token = useDefault ? default : CancellationToken.None;
            var source = new TaskCompletionSource<int>();

            var result = source.Task.WithTimeout(Timeout.InfiniteTimeSpan, token);

            Assert.AreSame(source.Task.WithTimeout(Timeout.InfiniteTimeSpan), result);
        }

        [DataRow(false)]
        [DataRow(true)]
        [TestMethod]
        public async Task WithTimeout_NoneTokenTimesOut_Test(bool useDefault)
        {
            var token = useDefault ? default : CancellationToken.None;
            var source = new TaskCompletionSource<int>();

            await Assert.ThrowsExceptionAsync<TimeoutException>(() => source.Task.WithTimeout(ShortTimeout));
            await Assert.ThrowsExceptionAsync<TimeoutException>(() => source.Task.WithTimeout(ShortTimeout, token));

            Assert.IsFalse(source.Task.IsCompleted);
        }

        [DataRow(false)]
        [DataRow(true)]
        [TestMethod]
        public async Task WithTimeout_NoneTokenCompletesInTime_Test(bool useDefault)
        {
            var token = useDefault ? default : CancellationToken.None;
            var source = new TaskCompletionSource<int>();
            var expected = source.Task.WithTimeout(LongTimeout);
            var result = source.Task.WithTimeout(LongTimeout, token);

            source.SetResult(42);

            Assert.AreEqual(await AsyncTimeoutTestHelper.WithinAsync(expected, 10000),
                await AsyncTimeoutTestHelper.WithinAsync(result, 10000));
            Assert.AreEqual(42, result.Result);
        }

        [DataRow(false)]
        [DataRow(true)]
        [TestMethod]
        public async Task WithTimeout_NoneTokenAllExceptionsPropagate_Test(bool useDefault)
        {
            var token = useDefault ? default : CancellationToken.None;
            var source = new TaskCompletionSource<bool>();
            Task task = source.Task;
            var firstError = new InvalidOperationException("first");
            var secondError = new FormatException("second");
            var expected = task.WithTimeout(LongTimeout);
            var result = task.WithTimeout(LongTimeout, token);

            source.SetException(new Exception[] { firstError, secondError });

            Assert.IsTrue(await AsyncTimeoutTestHelper.CompletesWithinAsync(expected, 10000));
            Assert.IsTrue(await AsyncTimeoutTestHelper.CompletesWithinAsync(result, 10000));
            Assert.IsTrue(result.IsFaulted);
            CollectionAssert.AreEqual(expected.Exception.InnerExceptions.ToArray(), result.Exception.InnerExceptions.ToArray());
            CollectionAssert.AreEqual(new Exception[] { firstError, secondError }, result.Exception.InnerExceptions.ToArray());
        }

        [DataRow(false)]
        [DataRow(true)]
        [TestMethod]
        public async Task WithTimeout_NoneTokenCancellationPropagates_Test(bool useDefault)
        {
            var token = useDefault ? default : CancellationToken.None;
            var source = new TaskCompletionSource<int>();
            var result = source.Task.WithTimeout(LongTimeout, token);

            source.SetCanceled();

            Assert.IsTrue(await AsyncTimeoutTestHelper.CompletesWithinAsync(result, 10000));
            Assert.IsTrue(result.IsCanceled);
        }
    }
}
