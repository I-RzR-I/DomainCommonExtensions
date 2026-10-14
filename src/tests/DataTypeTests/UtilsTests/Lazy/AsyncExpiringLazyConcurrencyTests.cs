using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Async.LazyLoad;

namespace DataTypeTests.UtilsTests.Lazy
{
    [TestClass]
    public class AsyncExpiringLazyConcurrencyTests
    {
        private static readonly TimeSpan LongTtl = TimeSpan.FromMinutes(5);

        [TestMethod]
        [Timeout(10000)]
        public async Task Constructor_MaxValueTtlCachesForever_Test()
        {
            var calls = 0;
            var lazy = new AsyncExpiringLazy<string>(_ =>
                Task.FromResult("value-" + Interlocked.Increment(ref calls)), TimeSpan.MaxValue);

            var first = await AsyncTimeoutTestHelper.WithinAsync(lazy.GetValueAsync());
            var second = await AsyncTimeoutTestHelper.WithinAsync(lazy.GetValueAsync());

            Assert.AreEqual("value-1", first);
            Assert.AreEqual("value-1", second);
            Assert.AreEqual(1, Volatile.Read(ref calls));
            Assert.IsTrue(lazy.IsValueCreated);
            Assert.IsTrue(lazy.TryGetValue(out var cached));
            Assert.AreEqual("value-1", cached);
        }

        [TestMethod]
        [Timeout(10000)]
        public async Task Constructor_InfiniteTimeSpanTtlAccepted_Test()
        {
            var calls = 0;
            var lazy = new AsyncExpiringLazy<string>(_ =>
                Task.FromResult("value-" + Interlocked.Increment(ref calls)), Timeout.InfiniteTimeSpan);

            var first = await AsyncTimeoutTestHelper.WithinAsync(lazy.GetValueAsync());
            var second = await AsyncTimeoutTestHelper.WithinAsync(lazy.GetValueAsync());

            Assert.AreEqual("value-1", first);
            Assert.AreEqual("value-1", second);
            Assert.AreEqual(1, Volatile.Read(ref calls));
            Assert.IsTrue(lazy.IsValueCreated);
        }

        [TestMethod]
        [Timeout(10000)]
        public void Constructor_NegativeMillisecondsTtlRejected_Test()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                new AsyncExpiringLazy<string>(_ => Task.FromResult("value"), TimeSpan.FromMilliseconds(-5)));
        }

        [TestMethod]
        [Timeout(10000)]
        public async Task GetValueAsync_ThirtyTwoGatedCallersShareOneRun_Test()
        {
            var factory = new GatedFactoryTestHelper<string>();
            var lazy = new AsyncExpiringLazy<string>(factory.InvokeAsync, LongTtl);

            var starters = Enumerable.Range(0, 32)
                .Select(_ => Task.Factory.StartNew(() => lazy.GetValueAsync(),
                    CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default))
                .ToArray();
            var pending = await AsyncTimeoutTestHelper.WithinAsync(Task.WhenAll(starters));
            await AsyncTimeoutTestHelper.WithinAsync(factory.WhenStartedAsync(1));

            Assert.AreEqual(1, factory.Calls);
            Assert.IsTrue(pending.All(t => !t.IsCompleted));

            factory.Complete(1, "value");
            var results = await AsyncTimeoutTestHelper.WithinAsync(Task.WhenAll(pending));

            Assert.AreEqual(32, results.Length);
            Assert.IsTrue(results.All(r => r == "value"));
            Assert.AreEqual(1, factory.Calls);
        }

        [TestMethod]
        [Timeout(10000)]
        public async Task Reset_StaleLoadCompletionIgnored_Test()
        {
            var factory = new GatedFactoryTestHelper<string>();
            var lazy = new AsyncExpiringLazy<string>(factory.InvokeAsync, LongTtl);

            var first = lazy.GetValueAsync();
            await AsyncTimeoutTestHelper.WithinAsync(factory.WhenStartedAsync(1));
            lazy.Reset();
            var second = lazy.GetValueAsync();
            await AsyncTimeoutTestHelper.WithinAsync(factory.WhenStartedAsync(2));

            Assert.AreEqual(2, factory.Calls);

            factory.Complete(2, "two");
            Assert.AreEqual("two", await AsyncTimeoutTestHelper.WithinAsync(second));

            factory.Complete(1, "one");
            Assert.IsTrue(await AsyncTimeoutTestHelper.CompletesWithinAsync(first));

            Assert.IsTrue(lazy.TryGetValue(out var cached));
            Assert.AreEqual("two", cached);
            Assert.AreEqual("two", await AsyncTimeoutTestHelper.WithinAsync(lazy.GetValueAsync()));
            Assert.AreEqual(2, factory.Calls);
        }

        [TestMethod]
        [Timeout(10000)]
        public async Task Reset_StaleLoadFaultKeepsNewValue_Test()
        {
            var factory = new GatedFactoryTestHelper<string>();
            var lazy = new AsyncExpiringLazy<string>(factory.InvokeAsync, LongTtl);

            var first = lazy.GetValueAsync();
            await AsyncTimeoutTestHelper.WithinAsync(factory.WhenStartedAsync(1));
            lazy.Reset();
            var second = lazy.GetValueAsync();
            await AsyncTimeoutTestHelper.WithinAsync(factory.WhenStartedAsync(2));

            factory.Complete(2, "two");
            Assert.AreEqual("two", await AsyncTimeoutTestHelper.WithinAsync(second));

            factory.Fail(1, new InvalidOperationException("stale"));
            Assert.IsTrue(await AsyncTimeoutTestHelper.CompletesWithinAsync(first));

            Assert.IsTrue(lazy.TryGetValue(out var cached));
            Assert.AreEqual("two", cached);
            Assert.IsTrue(lazy.IsValueCreated);
            Assert.AreEqual(2, factory.Calls);
            Assert.AreEqual("two", await AsyncTimeoutTestHelper.WithinAsync(lazy.GetValueAsync()));
            Assert.AreEqual(2, factory.Calls);
        }

        [TestMethod]
        [Timeout(10000)]
        public async Task GetValueAsync_FailureNotCachedRetrySucceeds_Test()
        {
            var calls = 0;
            var lazy = new AsyncExpiringLazy<string>(_ => Interlocked.Increment(ref calls) == 1
                ? Task.FromException<string>(new InvalidOperationException("boom"))
                : Task.FromResult("value"), LongTtl);

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                () => AsyncTimeoutTestHelper.WithinAsync(lazy.GetValueAsync()));
            var value = await AsyncTimeoutTestHelper.WithinAsync(lazy.GetValueAsync());

            Assert.AreEqual("value", value);
            Assert.AreEqual(2, Volatile.Read(ref calls));
        }

        [TestMethod]
        [Timeout(10000)]
        public async Task GetValueAsync_CachedValueReusedWithoutFactory_Test()
        {
            var calls = 0;
            var lazy = new AsyncExpiringLazy<string>(_ =>
                Task.FromResult("value-" + Interlocked.Increment(ref calls)), LongTtl);

            var first = await AsyncTimeoutTestHelper.WithinAsync(lazy.GetValueAsync());
            var second = await AsyncTimeoutTestHelper.WithinAsync(lazy.GetValueAsync());

            Assert.AreEqual("value-1", first);
            Assert.AreEqual("value-1", second);
            Assert.AreEqual(1, Volatile.Read(ref calls));
        }

        [TestMethod]
        [Timeout(10000)]
        public async Task IsValueCreated_FlipsAfterGatedLoadCompletes_Test()
        {
            var factory = new GatedFactoryTestHelper<string>();
            var lazy = new AsyncExpiringLazy<string>(factory.InvokeAsync, LongTtl);

            Assert.IsFalse(lazy.IsValueCreated);

            var pending = lazy.GetValueAsync();
            await AsyncTimeoutTestHelper.WithinAsync(factory.WhenStartedAsync(1));

            Assert.IsFalse(lazy.IsValueCreated);

            factory.Complete(1, "value");
            Assert.AreEqual("value", await AsyncTimeoutTestHelper.WithinAsync(pending));

            Assert.IsTrue(lazy.IsValueCreated);
        }

        [TestMethod]
        [Timeout(10000)]
        public async Task GetValueAsync_ShortTtlRefreshesAfterExpiry_Test()
        {
            var calls = 0;
            var lazy = new AsyncExpiringLazy<string>(_ =>
                Task.FromResult("value-" + Interlocked.Increment(ref calls)), TimeSpan.FromMilliseconds(50));

            var first = await AsyncTimeoutTestHelper.WithinAsync(lazy.GetValueAsync());
            await Task.Delay(150);
            var second = await AsyncTimeoutTestHelper.WithinAsync(lazy.GetValueAsync());

            Assert.AreEqual("value-1", first);
            Assert.AreEqual("value-2", second);
            Assert.AreEqual(2, Volatile.Read(ref calls));
        }

        [TestMethod]
        [Timeout(10000)]
        public async Task GetValueAsync_ZeroTtlCallersShareInFlightRun_Test()
        {
            var factory = new GatedFactoryTestHelper<string>();
            var lazy = new AsyncExpiringLazy<string>(factory.InvokeAsync, TimeSpan.Zero);

            var starters = Enumerable.Range(0, 32)
                .Select(_ => Task.Factory.StartNew(() => lazy.GetValueAsync(),
                    CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default))
                .ToArray();
            var pending = await AsyncTimeoutTestHelper.WithinAsync(Task.WhenAll(starters));
            await AsyncTimeoutTestHelper.WithinAsync(factory.WhenStartedAsync(1));

            Assert.AreEqual(1, factory.Calls);

            factory.Complete(1, "value");
            var results = await AsyncTimeoutTestHelper.WithinAsync(Task.WhenAll(pending));

            Assert.IsTrue(results.All(r => r == "value"));
            Assert.AreEqual(1, factory.Calls);
        }
    }
}
