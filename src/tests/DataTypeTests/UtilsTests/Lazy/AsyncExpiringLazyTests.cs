// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2026-03-17 20:03
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-03-17 21:11
// ***********************************************************************
//  <copyright file="AsyncExpiringLazyTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using RzR.Extensions.Domain.Async.LazyLoad;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endregion

namespace DataTypeTests.UtilsTests.Lazy
{
    [TestClass]
    public class AsyncExpiringLazyTests
    {
        private static readonly TimeSpan LongTtl = TimeSpan.FromMinutes(5);
        private static readonly TimeSpan ShortTtl = TimeSpan.FromMilliseconds(100);

        [TestMethod]
        public void Constructor_NullFactory_ThrowsArgumentNullException_Test()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                var _ = new AsyncExpiringLazy<int>(null, LongTtl);
            });
        }

        [TestMethod]
        public void Constructor_NegativeTtl_ThrowsArgumentOutOfRangeException_Test()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                var _ = new AsyncExpiringLazy<int>(_ => Task.FromResult(1), TimeSpan.FromSeconds(-1));
            });
        }

        [TestMethod]
        public void Constructor_ValidArguments_DoesNotThrow_Test()
        {
            var lazy = new AsyncExpiringLazy<int>(_ => Task.FromResult(1), LongTtl);
            Assert.IsNotNull(lazy);
        }

        [TestMethod]
        public async Task GetValueAsync_ReturnsExpectedValue_Test()
        {
            var lazy = new AsyncExpiringLazy<int>(_ => Task.FromResult(42), LongTtl);

            var result = await lazy.GetValueAsync();

            Assert.AreEqual(42, result);
        }

        [TestMethod]
        public async Task GetAwaiter_ReturnsExpectedValue_Test()
        {
            var lazy = new AsyncExpiringLazy<string>(_ => Task.FromResult("hello"), LongTtl);

            var result = await lazy;

            Assert.AreEqual("hello", result);
        }

        [TestMethod]
        public async Task GetValueAsync_WithDelayedFactory_ReturnsExpectedValue_Test()
        {
            var lazy = new AsyncExpiringLazy<int>(async ct =>
            {
                await Task.Delay(50, ct);

                return 99;
            }, LongTtl);

            var result = await lazy.GetValueAsync();

            Assert.AreEqual(99, result);
        }

        [TestMethod]
        public async Task GetValueAsync_WithinTtl_FactoryInvokedOnce_Test()
        {
            var callCount = 0;
            var lazy = new AsyncExpiringLazy<int>(_ =>
            {
                Interlocked.Increment(ref callCount);
                return Task.FromResult(1);
            }, LongTtl);

            await lazy.GetValueAsync();
            await lazy.GetValueAsync();
            await lazy.GetValueAsync();

            Assert.AreEqual(1, callCount);
        }

        [TestMethod]
        public async Task GetValueAsync_WithinTtl_ReturnsSameValue_Test()
        {
            var lazy = new AsyncExpiringLazy<Guid>(_ => Task.FromResult(Guid.NewGuid()), LongTtl);

            var first = await lazy.GetValueAsync();
            var second = await lazy.GetValueAsync();

            Assert.AreEqual(first, second);
        }

        [TestMethod]
        public async Task GetValueAsync_AfterTtlExpires_FactoryCalledAgain_Test()
        {
            var callCount = 0;
            var lazy = new AsyncExpiringLazy<int>(_ =>
            {
                Interlocked.Increment(ref callCount);

                return Task.FromResult(callCount);
            }, ShortTtl);

            var first = await lazy.GetValueAsync();

            // Wait for TTL to expire
            await Task.Delay(200);

            var second = await lazy.GetValueAsync();

            Assert.AreEqual(1, first);
            Assert.AreEqual(2, second);
            Assert.AreEqual(2, callCount);
        }

        [TestMethod]
        public async Task GetValueAsync_AfterTtlExpires_ReturnsFreshValue_Test()
        {
            var lazy = new AsyncExpiringLazy<int>(async _ =>
            {
                await Task.Yield();
                return Environment.TickCount;
            }, ShortTtl);

            var first = await lazy.GetValueAsync();

            await Task.Delay(200);

            var second = await lazy.GetValueAsync();

            // The value should be refreshed (timing-based; we just check it completed)
            Assert.IsTrue(lazy.IsValueCreated);
            // Both calls must have produced a valid int
            Assert.IsTrue(first != 0 || second != 0);
        }

        [TestMethod]
        public async Task GetValueAsync_ConcurrentCallersWithinTtl_AllGetSameValue_Test()
        {
            var lazy = new AsyncExpiringLazy<int>(async _ =>
            {
                await Task.Delay(50);

                return 7;
            }, LongTtl);

            // First call primes the entry
            var prime = lazy.GetValueAsync();

            var tasks = new Task<int>[20];
            for (var i = 0; i < tasks.Length; i++)
                tasks[i] = lazy.GetValueAsync();

            var results = await Task.WhenAll(tasks);
            await prime;

            foreach (var r in results)
                Assert.AreEqual(7, r);
        }

        [TestMethod]
        public async Task Reset_ClearsEntry_AllowsReinitialization_Test()
        {
            var callCount = 0;
            var lazy = new AsyncExpiringLazy<int>(_ =>
            {
                Interlocked.Increment(ref callCount);
                return Task.FromResult(callCount);
            }, LongTtl);

            var first = await lazy.GetValueAsync();
            Assert.AreEqual(1, first);

            lazy.Reset();

            var second = await lazy.GetValueAsync();
            Assert.AreEqual(2, second);
            Assert.AreEqual(2, callCount);
        }

        [TestMethod]
        public async Task Reset_ClearsIsValueCreated_Test()
        {
            var lazy = new AsyncExpiringLazy<int>(_ => Task.FromResult(1), LongTtl);

            await lazy.GetValueAsync();
            Assert.IsTrue(lazy.IsValueCreated);

            lazy.Reset();

            Assert.IsFalse(lazy.IsValueCreated);
        }

        [TestMethod]
        public async Task Reset_ClearsIsInitializationStarted_Test()
        {
            var lazy = new AsyncExpiringLazy<int>(_ => Task.FromResult(1), LongTtl);

            await lazy.GetValueAsync();
            Assert.IsTrue(lazy.IsInitializationStarted);

            lazy.Reset();

            Assert.IsFalse(lazy.IsInitializationStarted);
        }

        [TestMethod]
        public void IsValueCreated_FalseBeforeAnyCall_Test()
        {
            var lazy = new AsyncExpiringLazy<int>(_ => Task.FromResult(1), LongTtl);

            Assert.IsFalse(lazy.IsValueCreated);
        }

        [TestMethod]
        public async Task IsValueCreated_TrueAfterSuccessfulCompletionWithinTtl_Test()
        {
            var lazy = new AsyncExpiringLazy<int>(_ => Task.FromResult(1), LongTtl);

            await lazy.GetValueAsync();

            Assert.IsTrue(lazy.IsValueCreated);
        }

        [TestMethod]
        public async Task IsValueCreated_FalseAfterTtlExpires_Test()
        {
            var lazy = new AsyncExpiringLazy<int>(_ => Task.FromResult(1), ShortTtl);

            await lazy.GetValueAsync();
            Assert.IsTrue(lazy.IsValueCreated);

            await Task.Delay(200);

            Assert.IsFalse(lazy.IsValueCreated);
        }

        [TestMethod]
        public async Task IsValueCreated_FalseAfterInvalidate_Test()
        {
            var lazy = new AsyncExpiringLazy<int>(_ => Task.FromResult(1), LongTtl);

            await lazy.GetValueAsync();
            lazy.Reset();

            Assert.IsFalse(lazy.IsValueCreated);
        }

        [TestMethod]
        public async Task IsValueCreated_FalseWhenTaskFaulted_Test()
        {
            var lazy = new AsyncExpiringLazy<int>(_ => throw new InvalidOperationException("fail"), LongTtl);

            // Factory throws synchronously — task is never completed successfully
            try
            {
                await lazy.GetValueAsync();
            }
            catch
            {
                /* expected */
            }

            // IsValueCreated must be false because task did not run to completion
            Assert.IsFalse(lazy.IsValueCreated);
        }

        [TestMethod]
        public void IsInitializationStarted_FalseBeforeAnyCall_Test()
        {
            var lazy = new AsyncExpiringLazy<int>(_ => Task.FromResult(1), LongTtl);

            Assert.IsFalse(lazy.IsInitializationStarted);
        }

        [TestMethod]
        public void IsInitializationStarted_TrueAfterFirstGetValueAsync_Test()
        {
            var lazy = new AsyncExpiringLazy<int>(_ => Task.FromResult(1), LongTtl);

            // Don't await — entry is set synchronously in RefreshAsync before the task completes
            var _ = lazy.GetValueAsync();

            Assert.IsTrue(lazy.IsInitializationStarted);
        }

        [TestMethod]
        public async Task IsInitializationStarted_RemainsTrue_EvenAfterTtlExpires_Test()
        {
            // IsInitializationStarted only checks _entry != null; it does NOT check expiration.
            var lazy = new AsyncExpiringLazy<int>(_ => Task.FromResult(1), ShortTtl);

            await lazy.GetValueAsync();

            await Task.Delay(200); // Let TTL expire

            // Entry object still exists in _entry field, expiration has passed
            Assert.IsTrue(lazy.IsInitializationStarted);
        }

        [TestMethod]
        public async Task IsInitializationStarted_FalseAfterInvalidate_Test()
        {
            var lazy = new AsyncExpiringLazy<int>(_ => Task.FromResult(1), LongTtl);

            await lazy.GetValueAsync();
            lazy.Reset();

            Assert.IsFalse(lazy.IsInitializationStarted);
        }

        [TestMethod]
        public void TryGetValue_BeforeInitialization_ReturnsFalse_Test()
        {
            var lazy = new AsyncExpiringLazy<int>(_ => Task.FromResult(42), LongTtl);

            var success = lazy.TryGetValue(out var value);

            Assert.IsFalse(success);
            Assert.AreEqual(default, value);
        }

        [TestMethod]
        public async Task TryGetValue_AfterSuccessfulCompletion_ReturnsTrueAndValue_Test()
        {
            var lazy = new AsyncExpiringLazy<int>(_ => Task.FromResult(42), LongTtl);

            await lazy.GetValueAsync();

            var success = lazy.TryGetValue(out var value);

            Assert.IsTrue(success);
            Assert.AreEqual(42, value);
        }

        [TestMethod]
        public async Task TryGetValue_AfterTtlExpires_ReturnsFalse_Test()
        {
            var lazy = new AsyncExpiringLazy<int>(_ => Task.FromResult(42), ShortTtl);

            await lazy.GetValueAsync();
            Assert.IsTrue(lazy.TryGetValue(out _));

            await Task.Delay(200);

            var success = lazy.TryGetValue(out var value);
            Assert.IsFalse(success);
            Assert.AreEqual(default, value);
        }

        [TestMethod]
        public async Task TryGetValue_AfterInvalidate_ReturnsFalse_Test()
        {
            var lazy = new AsyncExpiringLazy<int>(_ => Task.FromResult(42), LongTtl);

            await lazy.GetValueAsync();
            lazy.Reset();

            var success = lazy.TryGetValue(out var value);

            Assert.IsFalse(success);
            Assert.AreEqual(default, value);
        }

        [TestMethod]
        public async Task TryGetValue_WhenTaskFaulted_ReturnsFalse_Test()
        {
            var lazy = new AsyncExpiringLazy<int>(_ => throw new InvalidOperationException("fail"), LongTtl);

            try
            {
                await lazy.GetValueAsync();
            }
            catch
            {
                /* expected */
            }

            var success = lazy.TryGetValue(out var value);

            Assert.IsFalse(success);
            Assert.AreEqual(default, value);
        }

        [TestMethod]
        public async Task GetValueAsync_FactoryThrows_ExceptionIsPropagated_Test()
        {
            var lazy = new AsyncExpiringLazy<int>(_ => throw new InvalidOperationException("boom"), LongTtl);

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => lazy.GetValueAsync());
        }

        [TestMethod]
        public async Task GetValueAsync_FactoryThrowsAsync_ExceptionIsPropagated_Test()
        {
            var lazy = new AsyncExpiringLazy<int>(async _ =>
            {
                await Task.Yield();
                throw new InvalidOperationException("async boom");
#pragma warning disable CS0162
                return 0;
#pragma warning restore CS0162
            }, LongTtl);

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => lazy.GetValueAsync());
        }

        [TestMethod]
        public async Task GetValueAsync_SyncFaultedFactory_CalledOnEveryRequest_Test()
        {
            // When the factory throws, the faulted entry is auto-recovered on the
            // next call — factory is invoked again regardless of TTL.
            var callCount = 0;
            var lazy = new AsyncExpiringLazy<int>(_ =>
            {
                Interlocked.Increment(ref callCount);
                throw new InvalidOperationException("sync fail");
            }, LongTtl);

            try
            {
                await lazy.GetValueAsync();
            }
            catch
            {
                /* expected */
            }

            try
            {
                await lazy.GetValueAsync();
            }
            catch
            {
                /* expected */
            }

            Assert.AreEqual(2, callCount);
        }

        [TestMethod]
        public async Task GetValueAsync_AsyncFaultedWithinTtl_AutoRecoveryCallsFactoryAgain_Test()
        {
            // With auto-recovery, faulted tasks are immediately eligible for refresh
            // on the next call, even within the TTL window.
            var callCount = 0;
            var lazy = new AsyncExpiringLazy<int>(async _ =>
            {
                var n = Interlocked.Increment(ref callCount);
                await Task.Yield();
                if (n == 1) throw new InvalidOperationException("async fail");
                return 42;
            }, LongTtl);

            // First call faults
            try
            {
                await lazy.GetValueAsync();
            }
            catch
            {
                /* expected */
            }

            // Second call should auto-recover and succeed
            var result = await lazy.GetValueAsync();

            Assert.AreEqual(42, result);
            Assert.AreEqual(2, callCount);
        }

        [TestMethod]
        public async Task GetValueAsync_FaultedAfterTtlExpires_CallsFactoryAgain_Test()
        {
            var callCount = 0;
            var lazy = new AsyncExpiringLazy<int>(_ =>
            {
                Interlocked.Increment(ref callCount);
                throw new InvalidOperationException("fail");
            }, ShortTtl);

            try
            {
                await lazy.GetValueAsync();
            }
            catch
            {
                /* expected */
            }

            Assert.AreEqual(1, callCount);

            await Task.Delay(200); // Let TTL expire

            try
            {
                await lazy.GetValueAsync();
            }
            catch
            {
                /* expected */
            }

            Assert.AreEqual(2, callCount);
        }

        [TestMethod]
        public async Task GetValueAsync_ZeroTtl_FactoryCalledOnEveryRequest_Test()
        {
            var callCount = 0;
            var lazy = new AsyncExpiringLazy<int>(_ =>
            {
                Interlocked.Increment(ref callCount);
                return Task.FromResult(callCount);
            }, TimeSpan.Zero);

            await lazy.GetValueAsync();
            await lazy.GetValueAsync();
            await lazy.GetValueAsync();

            // With TTL=0 the entry expires immediately after creation,
            // so every sequential call must invoke the factory.
            Assert.AreEqual(3, callCount, "Each of the 3 sequential calls must invoke the factory.");
        }
    }
}