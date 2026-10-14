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
    public class AsyncLazyConcurrencyTests
    {
        [TestMethod]
        [Timeout(10000)]
        public async Task GetValueAsync_ThirtyTwoGatedCallersShareOneRun_Test()
        {
            var factory = new GatedFactoryTestHelper<string>();
            var lazy = new AsyncLazy<string>(factory.InvokeAsync);

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
            var lazy = new AsyncLazy<string>(factory.InvokeAsync);

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
            var lazy = new AsyncLazy<string>(factory.InvokeAsync);

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
            var lazy = new AsyncLazy<string>(_ => Interlocked.Increment(ref calls) == 1
                ? Task.FromException<string>(new InvalidOperationException("boom"))
                : Task.FromResult("value"));

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
            var lazy = new AsyncLazy<string>(_ =>
                Task.FromResult("value-" + Interlocked.Increment(ref calls)));

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
            var lazy = new AsyncLazy<string>(factory.InvokeAsync);

            Assert.IsFalse(lazy.IsValueCreated);

            var pending = lazy.GetValueAsync();
            await AsyncTimeoutTestHelper.WithinAsync(factory.WhenStartedAsync(1));

            Assert.IsFalse(lazy.IsValueCreated);

            factory.Complete(1, "value");
            Assert.AreEqual("value", await AsyncTimeoutTestHelper.WithinAsync(pending));

            Assert.IsTrue(lazy.IsValueCreated);
        }
    }
}
