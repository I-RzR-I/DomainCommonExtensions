// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2026-03-17 20:03
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-03-17 20:31
// ***********************************************************************
//  <copyright file="AsyncLazyTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using RzR.Extensions.Domain.Async.LazyLoad;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DataTypeTests.Models.Lazy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endregion

namespace DataTypeTests.UtilsTests.Lazy
{
    [TestClass]
    public class AsyncLazyTests
    {
        [TestMethod]
        public void Constructor_NullFactory_ThrowsArgumentNullException_Test()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                var _ = new AsyncLazy<int>(null);
            });
        }

        [TestMethod]
        public async Task GetValueAsync_ReturnsExpectedValue_Test()
        {
            var lazy = new AsyncLazy<int>(_ => Task.FromResult(42));

            var result = await lazy.GetValueAsync();

            Assert.AreEqual(42, result);
        }

        [TestMethod]
        public async Task GetAwaiter_ReturnsExpectedValue_Test()
        {
            var lazy = new AsyncLazy<string>(_ => Task.FromResult("hello"));

            var result = await lazy;

            Assert.AreEqual("hello", result);
        }

        [TestMethod]
        public async Task GetValueAsync_WithDelayedFactory_ReturnsExpectedValue_Test()
        {
            var lazy = new AsyncLazy<int>(async ct =>
            {
                await Task.Delay(50, ct);

                return 99;
            });

            var result = await lazy.GetValueAsync();

            Assert.AreEqual(99, result);
        }

        [TestMethod]
        public async Task GetValueAsync_MultipleCalls_FactoryInvokedOnce_Test()
        {
            var callCount = 0;
            var lazy = new AsyncLazy<int>(_ =>
            {
                Interlocked.Increment(ref callCount);
                return Task.FromResult(1);
            });

            await lazy.GetValueAsync();
            await lazy.GetValueAsync();
            await lazy.GetValueAsync();

            Assert.AreEqual(1, callCount);
        }

        [TestMethod]
        public async Task GetValueAsync_MultipleCalls_ReturnSameValue_Test()
        {
            var lazy = new AsyncLazy<Guid>(_ => Task.FromResult(Guid.NewGuid()));

            var first = await lazy.GetValueAsync();
            var second = await lazy.GetValueAsync();

            Assert.AreEqual(first, second);
        }

        [TestMethod]
        public async Task GetValueAsync_ConcurrentCallers_FactoryInvokedOnce_Test()
        {
            var callCount = 0;
            var lazy = new AsyncLazy<int>(async ct =>
            {
                Interlocked.Increment(ref callCount);
                await Task.Delay(50, ct);

                return 7;
            });

            var tasks = new List<Task<int>>();
            for (var i = 0; i < 20; i++)
                tasks.Add(lazy.GetValueAsync());

            var results = await Task.WhenAll(tasks);

            Assert.AreEqual(1, callCount);
            foreach (var r in results)
                Assert.AreEqual(7, r);
        }

        [TestMethod]
        public async Task GetValueAsync_FactoryThrows_ExceptionPropagated_Test()
        {
            var lazy = new AsyncLazy<int>(_ => throw new InvalidOperationException("boom"));

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => lazy.GetValueAsync());
        }

        [TestMethod]
        public async Task GetValueAsync_FactoryThrowsAsync_ExceptionPropagated_Test()
        {
            var lazy = new AsyncLazy<int>(async _ =>
            {
                await Task.Yield();
                throw new InvalidOperationException("async boom");

#pragma warning disable CS0162 // Unreachable code detected
                return 0;
#pragma warning restore CS0162 // Unreachable code detected
            });

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => lazy.GetValueAsync());
        }

        [TestMethod]
        public async Task GetValueAsync_AfterFault_CanRetryAndSucceed_Test()
        {
            var callCount = 0;
            var lazy = new AsyncLazy<int>(_ =>
            {
                var n = Interlocked.Increment(ref callCount);
                if (n == 1) throw new InvalidOperationException("first call fails");

                return Task.FromResult(100);
            });

            // First call faults
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => lazy.GetValueAsync());

            // Give Reset() inside RunFactoryAsync a moment to clear _task
            await Task.Delay(20);

            // Second call should succeed
            var result = await lazy.GetValueAsync();

            Assert.AreEqual(100, result);
            Assert.AreEqual(2, callCount);
        }

        [TestMethod]
        public async Task GetValueAsync_CanceledToken_OperationCanceled_Test()
        {
            var lazy = new AsyncLazy<int>(async ct =>
            {
                await Task.Delay(Timeout.Infinite, ct);

                return 0;
            });

            using var cts = new CancellationTokenSource();
            var task = lazy.GetValueAsync(cts.Token);
            cts.Cancel();

            await Assert.ThrowsExceptionAsync<TaskCanceledException>(() => task);
        }

        [TestMethod]
        public async Task GetValueAsync_AfterCancellation_CanRetryAndSucceed_Test()
        {
            var lazy = new AsyncLazy<int>(async ct =>
            {
                if (ct.IsCancellationRequested) ct.ThrowIfCancellationRequested();

                await Task.Delay(50, ct);
                return 55;
            });

            using var cts = new CancellationTokenSource();
            cts.Cancel();

            await Assert.ThrowsExceptionAsync<TaskCanceledException>(() => lazy.GetValueAsync(cts.Token));

            // Give the internal Reset() time to clear _task
            await Task.Delay(20);

            var result = await lazy.GetValueAsync();
            Assert.AreEqual(55, result);
        }

        [TestMethod]
        public async Task Reset_AllowsReinitialization_Test()
        {
            var callCount = 0;
            var lazy = new AsyncLazy<int>(_ =>
            {
                Interlocked.Increment(ref callCount);

                return Task.FromResult(callCount);
            });

            var first = await lazy.GetValueAsync();
            Assert.AreEqual(1, first);
            Assert.AreEqual(1, callCount);

            lazy.Reset();

            var second = await lazy.GetValueAsync();
            Assert.AreEqual(2, second);
            Assert.AreEqual(2, callCount);
        }

        [TestMethod]
        public async Task Reset_WhileInProgress_CancelAndAllowsRetry_Test()
        {
            var tcsBlock = new TaskCompletionSource<bool>();
            var started = new TaskCompletionSource<bool>();

            var lazy = new AsyncLazy<int>(async ct =>
            {
                started.TrySetResult(true);
                var delay = Task.Delay(Timeout.Infinite, ct);
                await Task.WhenAny(tcsBlock.Task, delay);
                ct.ThrowIfCancellationRequested();

                return 1;
            });

            var pending = lazy.GetValueAsync();

            // Wait until factory has started
            await started.Task;

            lazy.Reset();

            // Unblock the factory (it may have been canceled already)
            tcsBlock.TrySetResult(true);

            // Assert the original pending task was canceled or the lazy is no longer initialized
            Assert.IsFalse(lazy.IsValueCreated);
            Assert.IsFalse(lazy.IsInitializationStarted);
        }

        [TestMethod]
        public void IsValueCreated_FalseBeforeAnyCall_Test()
        {
            var lazy = new AsyncLazy<int>(_ => Task.FromResult(1));

            Assert.IsFalse(lazy.IsValueCreated);
        }

        [TestMethod]
        public async Task IsValueCreated_TrueAfterSuccessfulCompletion_Test()
        {
            var lazy = new AsyncLazy<int>(_ => Task.FromResult(1));

            await lazy.GetValueAsync();

            Assert.IsTrue(lazy.IsValueCreated);
        }

        [TestMethod]
        public async Task IsValueCreated_FalseAfterReset_Test()
        {
            var lazy = new AsyncLazy<int>(_ => Task.FromResult(1));

            await lazy.GetValueAsync();
            Assert.IsTrue(lazy.IsValueCreated);

            lazy.Reset();

            Assert.IsFalse(lazy.IsValueCreated);
        }

        [TestMethod]
        public void IsInitializationStarted_FalseBeforeAnyCall_Test()
        {
            var lazy = new AsyncLazy<int>(_ => Task.FromResult(1));

            Assert.IsFalse(lazy.IsInitializationStarted);
        }

        [TestMethod]
        public void IsInitializationStarted_TrueAfterGetValueAsync_Test()
        {
            var lazy = new AsyncLazy<int>(_ => Task.FromResult(1));

            // Don't await yet — but task object is registered
            var _ = lazy.GetValueAsync();

            Assert.IsTrue(lazy.IsInitializationStarted);
        }

        [TestMethod]
        public async Task IsInitializationStarted_FalseAfterReset_Test()
        {
            var lazy = new AsyncLazy<int>(_ => Task.FromResult(1));

            await lazy.GetValueAsync();
            lazy.Reset();

            Assert.IsFalse(lazy.IsInitializationStarted);
        }

        [TestMethod]
        public void TryGetValue_BeforeInitialization_ReturnsFalse_Test()
        {
            var lazy = new AsyncLazy<int>(_ => Task.FromResult(42));

            var success = lazy.TryGetValue(out var value);

            Assert.IsFalse(success);
            Assert.AreEqual(default, value);
        }

        [TestMethod]
        public async Task TryGetValue_AfterSuccessfulCompletion_ReturnsTrueAndValue_Test()
        {
            var lazy = new AsyncLazy<int>(_ => Task.FromResult(42));

            await lazy.GetValueAsync();

            var success = lazy.TryGetValue(out var value);

            Assert.IsTrue(success);
            Assert.AreEqual(42, value);
        }

        [TestMethod]
        public async Task TryGetValue_AfterReset_ReturnsFalse_Test()
        {
            var lazy = new AsyncLazy<int>(_ => Task.FromResult(42));

            await lazy.GetValueAsync();
            lazy.Reset();

            var success = lazy.TryGetValue(out var value);

            Assert.IsFalse(success);
            Assert.AreEqual(default, value);
        }

        [TestMethod]
        public async Task LazyDbConnection_OpenedOnceAndReused_Test()
        {
            var openCount = 0;

            var lazyConnection = new AsyncLazy<FakeDbConnection>(async ct =>
            {
                Interlocked.Increment(ref openCount);

                return await FakeDbConnection.OpenAsync(ct);
            });

            // Simulate three concurrent service calls that each need the connection
            var t1 = Task.Run(async () => await (await lazyConnection.GetValueAsync()).QueryAsync("SELECT 1"));
            var t2 = Task.Run(async () => await (await lazyConnection.GetValueAsync()).QueryAsync("SELECT 2"));
            var t3 = Task.Run(async () => await (await lazyConnection.GetValueAsync()).QueryAsync("SELECT 3"));

            await Task.WhenAll(t1, t2, t3);

            // Connection must have been opened exactly once
            Assert.AreEqual(1, openCount);
            Assert.IsTrue(lazyConnection.IsValueCreated);

            var conn = await lazyConnection.GetValueAsync();
            Assert.IsTrue(conn.IsOpen);
        }

        [TestMethod]
        public async Task LazyOAuthToken_FetchedOnceAndCachedForAllRequests_Test()
        {
            var client = new FakeTokenClient();
            var lazyToken = new AsyncLazy<string>(ct => client.AcquireTokenAsync(ct));

            // 10 parallel HTTP calls all grab the token
            var tokens = await Task.WhenAll(
                Enumerable10(() => lazyToken.GetValueAsync()));

            // Token acquired exactly once; every caller got the same value
            Assert.AreEqual(1, client.CallCount);
            foreach (var t in tokens)
                Assert.AreEqual("token-1", t);
        }

        [TestMethod]
        public async Task LazyOAuthToken_RefetchedAfterExplicitRevocation_Test()
        {
            var client = new FakeTokenClient();
            var lazyToken = new AsyncLazy<string>(ct => client.AcquireTokenAsync(ct));

            var first = await lazyToken.GetValueAsync();
            Assert.AreEqual("token-1", first);

            // Simulate token revocation (e.g. 401 response received)
            lazyToken.Reset();

            var second = await lazyToken.GetValueAsync();
            Assert.AreEqual("token-2", second);
            Assert.AreEqual(2, client.CallCount);
        }

        private static IEnumerable<Task<string>> Enumerable10(Func<Task<string>> factory)
        {
            for (var i = 0; i < 10; i++) yield return factory();
        }

        [TestMethod]
        public async Task LazyConfig_LoadedOnceAndSharedAcrossComponents_Test()
        {
            var loadCount = 0;

            var lazyConfig = new AsyncLazy<AppConfig>(async ct =>
            {
                Interlocked.Increment(ref loadCount);
                await Task.Delay(20, ct); // simulate file / remote call

                return new AppConfig { ConnectionString = "Server=prod;", MaxRetries = 3 };
            });

            // Multiple components request config concurrently at startup
            var cfg1 = await lazyConfig.GetValueAsync();
            var cfg2 = await lazyConfig.GetValueAsync();

            Assert.AreEqual(1, loadCount);
            Assert.AreSame(cfg1, cfg2);
            Assert.AreEqual("Server=prod;", cfg1.ConnectionString);
            Assert.AreEqual(3, cfg1.MaxRetries);
        }

        [TestMethod]
        public async Task LazyConfig_CancellationDuringLoad_AllowsRetryOnNextRequest_Test()
        {
            var loadCount = 0;

            var lazyConfig = new AsyncLazy<AppConfig>(async ct =>
            {
                Interlocked.Increment(ref loadCount);
                await Task.Delay(200, ct); // slow load

                return new AppConfig { ConnectionString = "Server=prod;", MaxRetries = 3 };
            });

            // App starts shutting down before config finishes loading
            using var cts = new CancellationTokenSource();
            var pendingLoad = lazyConfig.GetValueAsync(cts.Token);
            cts.Cancel();

            await Assert.ThrowsExceptionAsync<TaskCanceledException>(() => pendingLoad);

            // Give Reset() inside RunFactoryAsync time to clear state
            await Task.Delay(20);

            // On next startup attempt the config loads successfully
            var cfg = await lazyConfig.GetValueAsync();
            Assert.IsNotNull(cfg);
            Assert.AreEqual(2, loadCount);
        }

        [TestMethod]
        public async Task ShaderCache_FastPathAfterCompilation_Test()
        {
            var lazyShaders = new AsyncLazy<ShaderCache>(async ct =>
            {
                await Task.Delay(20, ct);
                return new ShaderCache { Shaders = new[] { "vertex.glsl", "fragment.glsl" } };
            });

            // First frame: cache not ready — fall back to async path
            Assert.IsFalse(lazyShaders.TryGetValue(out _));

            await lazyShaders.GetValueAsync();

            // Subsequent frames: cache ready — zero-allocation synchronous read
            Assert.IsTrue(lazyShaders.TryGetValue(out var cache));
            Assert.IsNotNull(cache);
            CollectionAssert.AreEqual(new[] { "vertex.glsl", "fragment.glsl" }, cache.Shaders);
        }
    }
}