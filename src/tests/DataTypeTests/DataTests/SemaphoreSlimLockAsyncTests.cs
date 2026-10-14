using System;
using System.Threading;
using System.Threading.Tasks;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Async;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class SemaphoreSlimLockAsyncTests
    {
        private static readonly TimeSpan ShortTimeout = TimeSpan.FromMilliseconds(50);
        private static readonly TimeSpan LongTimeout = TimeSpan.FromSeconds(30);

        [TestMethod]
        public async Task LockAsync_AcquiresAndReleases_Test()
        {
            using var semaphore = new SemaphoreSlim(1, 1);

            var releaser = await semaphore.LockAsync();
            Assert.AreEqual(0, semaphore.CurrentCount);
            releaser.Dispose();

            Assert.AreEqual(1, semaphore.CurrentCount);
        }

        [TestMethod]
        public async Task LockAsync_TimeoutOverloadAcquiresAndReleases_Test()
        {
            using var semaphore = new SemaphoreSlim(1, 1);

            var releaser = await semaphore.LockAsync(LongTimeout);
            Assert.AreEqual(0, semaphore.CurrentCount);
            releaser.Dispose();

            Assert.AreEqual(1, semaphore.CurrentCount);
        }

        [TestMethod]
        public async Task LockAsync_UsingBlockReleases_Test()
        {
            using var semaphore = new SemaphoreSlim(1, 1);

            using (await semaphore.LockAsync())
            {
                Assert.AreEqual(0, semaphore.CurrentCount);
            }

            Assert.AreEqual(1, semaphore.CurrentCount);
        }

        [TestMethod]
        public async Task LockAsync_DisposeTwiceReleasesOnce_Test()
        {
            using var semaphore = new SemaphoreSlim(1, 10);
            var releaser = await semaphore.LockAsync();

            releaser.Dispose();
            releaser.Dispose();

            Assert.AreEqual(1, semaphore.CurrentCount);
        }

        [TestMethod]
        public async Task LockAsync_TimeoutOverloadDisposeTwiceReleasesOnce_Test()
        {
            using var semaphore = new SemaphoreSlim(1, 10);
            var releaser = await semaphore.LockAsync(LongTimeout);

            releaser.Dispose();
            releaser.Dispose();

            Assert.AreEqual(1, semaphore.CurrentCount);
        }

        [TestMethod]
        public async Task LockAsync_ConcurrentDisposeReleasesOnce_Test()
        {
            using var semaphore = new SemaphoreSlim(1, 1000);
            var releaser = await semaphore.LockAsync();

            Parallel.For(0, 200, _ => releaser.Dispose());

            Assert.AreEqual(1, semaphore.CurrentCount);
        }

        [TestMethod]
        public async Task LockAsync_HeldSemaphoreTimesOut_Test()
        {
            using var semaphore = new SemaphoreSlim(1, 1);
            var holder = await semaphore.LockAsync();

            await Assert.ThrowsExceptionAsync<TimeoutException>(() => semaphore.LockAsync(ShortTimeout));

            Assert.AreEqual(0, semaphore.CurrentCount);
            holder.Dispose();
            Assert.AreEqual(1, semaphore.CurrentCount);
        }

        [TestMethod]
        public async Task LockAsync_ZeroTimeoutOnHeldSemaphore_Test()
        {
            using var semaphore = new SemaphoreSlim(1, 1);
            var holder = await semaphore.LockAsync();

            await Assert.ThrowsExceptionAsync<TimeoutException>(() => semaphore.LockAsync(TimeSpan.Zero));

            holder.Dispose();
            Assert.AreEqual(1, semaphore.CurrentCount);
        }

        [TestMethod]
        public async Task LockAsync_ZeroTimeoutOnFreeSemaphore_Test()
        {
            using var semaphore = new SemaphoreSlim(1, 1);

            var releaser = await semaphore.LockAsync(TimeSpan.Zero);
            Assert.AreEqual(0, semaphore.CurrentCount);
            releaser.Dispose();

            Assert.AreEqual(1, semaphore.CurrentCount);
        }

        [TestMethod]
        public async Task LockAsync_InfiniteTimeout_Test()
        {
            using var semaphore = new SemaphoreSlim(1, 1);

            var releaser = await semaphore.LockAsync(Timeout.InfiniteTimeSpan);
            Assert.AreEqual(0, semaphore.CurrentCount);
            releaser.Dispose();

            Assert.AreEqual(1, semaphore.CurrentCount);
        }

        [TestMethod]
        public async Task LockAsync_SecondWaiterProceedsAfterRelease_Test()
        {
            using var semaphore = new SemaphoreSlim(1, 1);
            var first = await semaphore.LockAsync();
            var secondTask = semaphore.LockAsync();
            Assert.IsFalse(secondTask.IsCompleted);

            first.Dispose();
            var second = await AsyncTimeoutTestHelper.WithinAsync(secondTask, 10000);

            Assert.AreEqual(0, semaphore.CurrentCount);
            second.Dispose();
            Assert.AreEqual(1, semaphore.CurrentCount);
        }

        [TestMethod]
        public async Task LockAsync_TimeoutOverloadSecondWaiterProceeds_Test()
        {
            using var semaphore = new SemaphoreSlim(1, 1);
            var first = await semaphore.LockAsync(LongTimeout);
            var secondTask = semaphore.LockAsync(LongTimeout);
            Assert.IsFalse(secondTask.IsCompleted);

            first.Dispose();
            var second = await AsyncTimeoutTestHelper.WithinAsync(secondTask, 10000);

            Assert.AreEqual(0, semaphore.CurrentCount);
            second.Dispose();
            Assert.AreEqual(1, semaphore.CurrentCount);
        }

        [TestMethod]
        public async Task LockAsync_SecondDisposeDoesNotReleaseOtherHolder_Test()
        {
            using var semaphore = new SemaphoreSlim(1, 10);
            var first = await semaphore.LockAsync();
            first.Dispose();
            var second = await semaphore.LockAsync();

            first.Dispose();

            Assert.AreEqual(0, semaphore.CurrentCount);
            second.Dispose();
            Assert.AreEqual(1, semaphore.CurrentCount);
        }

        [TestMethod]
        public void LockAsync_NullSemaphore_Test()
        {
            SemaphoreSlim semaphore = null;

            var exception = Assert.ThrowsException<ArgumentNullException>(() => semaphore.LockAsync());

            Assert.AreEqual("semaphore", exception.ParamName);
        }

        [TestMethod]
        public void LockAsync_TimeoutOverloadNullSemaphore_Test()
        {
            SemaphoreSlim semaphore = null;

            var exception = Assert.ThrowsException<ArgumentNullException>(() => semaphore.LockAsync(LongTimeout));

            Assert.AreEqual("semaphore", exception.ParamName);
        }

        [DataRow(-20000L)]
        [DataRow(-10000000L)]
        [DataRow(21474836480000L)]
        [DataRow(long.MaxValue)]
        [TestMethod]
        public void LockAsync_TimeoutOutOfRange_Test(long timeoutTicks)
        {
            using var semaphore = new SemaphoreSlim(1, 1);

            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => semaphore.LockAsync(TimeSpan.FromTicks(timeoutTicks)));

            Assert.AreEqual("timeout", exception.ParamName);
            Assert.AreEqual(1, semaphore.CurrentCount);
        }

        [TestMethod]
        public async Task LockAsync_DisposedSemaphore_Test()
        {
            var semaphore = new SemaphoreSlim(1, 1);
            semaphore.Dispose();

            await Assert.ThrowsExceptionAsync<ObjectDisposedException>(() => semaphore.LockAsync());
        }

        [TestMethod]
        public async Task LockAsync_TimeoutOverloadDisposedSemaphore_Test()
        {
            var semaphore = new SemaphoreSlim(1, 1);
            semaphore.Dispose();

            await Assert.ThrowsExceptionAsync<ObjectDisposedException>(() => semaphore.LockAsync(LongTimeout));
        }
    }
}
