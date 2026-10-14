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
    public class TaskWithTimeoutTests
    {
        private static readonly TimeSpan ShortTimeout = TimeSpan.FromMilliseconds(50);
        private static readonly TimeSpan LongTimeout = TimeSpan.FromSeconds(30);

        [TestMethod]
        public void WithTimeout_NullTask_Test()
        {
            Task task = null;

            var exception = Assert.ThrowsException<ArgumentNullException>(() => task.WithTimeout(LongTimeout));

            Assert.AreEqual("task", exception.ParamName);
        }

        [TestMethod]
        public void WithTimeout_NullGenericTask_Test()
        {
            Task<int> task = null;

            var exception = Assert.ThrowsException<ArgumentNullException>(() => task.WithTimeout(LongTimeout));

            Assert.AreEqual("task", exception.ParamName);
        }

        [DataRow(-20000L)]
        [DataRow(-10000000L)]
        [DataRow(long.MinValue)]
        [DataRow(21474836480000L)]
        [DataRow(long.MaxValue)]
        [TestMethod]
        public void WithTimeout_TimeoutOutOfRange_Test(long timeoutTicks)
        {
            var source = new TaskCompletionSource<bool>();
            Task task = source.Task;

            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => task.WithTimeout(TimeSpan.FromTicks(timeoutTicks)));

            Assert.AreEqual("timeout", exception.ParamName);
        }

        [DataRow(-20000L)]
        [DataRow(21474836480000L)]
        [TestMethod]
        public void WithTimeout_GenericTimeoutOutOfRange_Test(long timeoutTicks)
        {
            var source = new TaskCompletionSource<int>();

            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => source.Task.WithTimeout(TimeSpan.FromTicks(timeoutTicks)));

            Assert.AreEqual("timeout", exception.ParamName);
        }

        [TestMethod]
        public void WithTimeout_CompletedTaskInvalidTimeout_Test()
        {
            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => Task.CompletedTask.WithTimeout(TimeSpan.FromMilliseconds(-2)));

            Assert.AreEqual("timeout", exception.ParamName);
        }

        [TestMethod]
        public void WithTimeout_InfiniteTimeout_Test()
        {
            var source = new TaskCompletionSource<bool>();
            Task task = source.Task;

            var result = task.WithTimeout(Timeout.InfiniteTimeSpan);

            Assert.AreSame(task, result);
        }

        [TestMethod]
        public void WithTimeout_MinusOneMillisecond_Test()
        {
            var source = new TaskCompletionSource<int>();

            var result = source.Task.WithTimeout(TimeSpan.FromMilliseconds(-1));

            Assert.AreSame(source.Task, result);
        }

        [TestMethod]
        public void WithTimeout_SubMillisecondNegativeTruncates_Test()
        {
            var source = new TaskCompletionSource<int>();

            var result = source.Task.WithTimeout(TimeSpan.FromTicks(-15000));

            Assert.AreSame(source.Task, result);
        }

        [TestMethod]
        public void WithTimeout_CompletedTask_Test()
        {
            var task = Task.CompletedTask;

            var result = task.WithTimeout(LongTimeout);

            Assert.AreSame(task, result);
        }

        [TestMethod]
        public void WithTimeout_CompletedGenericTask_Test()
        {
            var task = Task.FromResult(7);

            var result = task.WithTimeout(TimeSpan.Zero);

            Assert.AreSame(task, result);
        }

        [TestMethod]
        public void WithTimeout_FaultedTask_Test()
        {
            var task = Task.FromException(new InvalidOperationException("boom"));

            var result = task.WithTimeout(LongTimeout);

            Assert.AreSame(task, result);
        }

        [TestMethod]
        public void WithTimeout_CanceledTask_Test()
        {
            var task = Task.FromCanceled<int>(new CancellationToken(true));

            var result = task.WithTimeout(LongTimeout);

            Assert.AreSame(task, result);
        }

        [TestMethod]
        public async Task WithTimeout_PendingTaskTimesOut_Test()
        {
            var source = new TaskCompletionSource<bool>();
            Task task = source.Task;

            await Assert.ThrowsExceptionAsync<TimeoutException>(() => task.WithTimeout(ShortTimeout));

            Assert.IsFalse(source.Task.IsCompleted);
        }

        [TestMethod]
        public async Task WithTimeout_PendingGenericTaskTimesOut_Test()
        {
            var source = new TaskCompletionSource<int>();

            await Assert.ThrowsExceptionAsync<TimeoutException>(() => source.Task.WithTimeout(ShortTimeout));

            Assert.IsFalse(source.Task.IsCompleted);
        }

        [TestMethod]
        public async Task WithTimeout_ZeroTimeoutPendingTask_Test()
        {
            var source = new TaskCompletionSource<int>();

            await Assert.ThrowsExceptionAsync<TimeoutException>(() => source.Task.WithTimeout(TimeSpan.Zero));
        }

        [TestMethod]
        public async Task WithTimeout_TimedOutTaskKeepsRunning_Test()
        {
            var source = new TaskCompletionSource<int>();
            await Assert.ThrowsExceptionAsync<TimeoutException>(() => source.Task.WithTimeout(ShortTimeout));

            source.SetResult(5);

            Assert.AreEqual(5, await source.Task);
        }

        [TestMethod]
        public async Task WithTimeout_MaxTimeoutNotExceeded_Test()
        {
            var source = new TaskCompletionSource<int>();

            var result = source.Task.WithTimeout(TimeSpan.FromMilliseconds(int.MaxValue));
            Assert.IsFalse(result.IsCompleted);
            source.SetResult(11);

            Assert.AreEqual(11, await AsyncTimeoutTestHelper.WithinAsync(result, 10000));
        }

        [TestMethod]
        public async Task WithTimeout_GenericCompletesInTime_Test()
        {
            var source = new TaskCompletionSource<int>();
            var result = source.Task.WithTimeout(LongTimeout);

            source.SetResult(42);

            Assert.AreEqual(42, await AsyncTimeoutTestHelper.WithinAsync(result, 10000));
        }

        [TestMethod]
        public async Task WithTimeout_CompletesInTime_Test()
        {
            var source = new TaskCompletionSource<bool>();
            Task task = source.Task;
            var result = task.WithTimeout(LongTimeout);

            source.SetResult(true);

            Assert.IsTrue(await AsyncTimeoutTestHelper.CompletesWithinAsync(result, 10000));
            Assert.AreEqual(TaskStatus.RanToCompletion, result.Status);
        }

        [TestMethod]
        public async Task WithTimeout_FaultPropagates_Test()
        {
            var source = new TaskCompletionSource<bool>();
            Task task = source.Task;
            var expected = new InvalidOperationException("boom");
            var result = task.WithTimeout(LongTimeout);

            source.SetException(expected);

            var actual = await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => result);
            Assert.AreSame(expected, actual);
        }

        [TestMethod]
        public async Task WithTimeout_GenericFaultPropagates_Test()
        {
            var source = new TaskCompletionSource<int>();
            var expected = new FormatException("bad");
            var result = source.Task.WithTimeout(LongTimeout);

            source.SetException(expected);

            var actual = await Assert.ThrowsExceptionAsync<FormatException>(() => result);
            Assert.AreSame(expected, actual);
        }

        [TestMethod]
        public async Task WithTimeout_AllExceptionsPropagate_Test()
        {
            var first = new TaskCompletionSource<int>();
            var second = new TaskCompletionSource<int>();
            var firstError = new InvalidOperationException("first");
            var secondError = new FormatException("second");
            var result = Task.WhenAll(first.Task, second.Task).WithTimeout(LongTimeout);

            first.SetException(firstError);
            second.SetException(secondError);

            Assert.IsTrue(await AsyncTimeoutTestHelper.CompletesWithinAsync(result, 10000));
            Assert.IsTrue(result.IsFaulted);
            CollectionAssert.AreEqual(new Exception[] { firstError, secondError }, result.Exception.InnerExceptions.ToArray());
        }

        [TestMethod]
        public async Task WithTimeout_NonGenericAllExceptionsPropagate_Test()
        {
            var source = new TaskCompletionSource<bool>();
            Task task = source.Task;
            var firstError = new InvalidOperationException("first");
            var secondError = new FormatException("second");
            var result = task.WithTimeout(LongTimeout);

            source.SetException(new Exception[] { firstError, secondError });

            Assert.IsTrue(await AsyncTimeoutTestHelper.CompletesWithinAsync(result, 10000));
            Assert.IsTrue(result.IsFaulted);
            CollectionAssert.AreEqual(new Exception[] { firstError, secondError }, result.Exception.InnerExceptions.ToArray());
        }

        [TestMethod]
        public async Task WithTimeout_CancellationPropagates_Test()
        {
            var source = new TaskCompletionSource<bool>();
            Task task = source.Task;
            var result = task.WithTimeout(LongTimeout);

            source.SetCanceled();

            await Assert.ThrowsExceptionAsync<TaskCanceledException>(() => result);
            Assert.IsTrue(result.IsCanceled);
        }

        [TestMethod]
        public async Task WithTimeout_GenericCancellationPropagates_Test()
        {
            var source = new TaskCompletionSource<int>();
            var result = source.Task.WithTimeout(LongTimeout);

            source.SetCanceled();

            await Assert.ThrowsExceptionAsync<TaskCanceledException>(() => result);
            Assert.IsTrue(result.IsCanceled);
        }
    }
}
