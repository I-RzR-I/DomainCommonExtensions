// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2026-03-17 20:03
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-03-17 20:59
// ***********************************************************************
//  <copyright file="AsyncLazy_V2Tests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using DataTypeTests.Models.Lazy;
using DomainCommonExtensions.Utilities.LazyLoad;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable InconsistentNaming

#endregion

namespace DataTypeTests.UtilsTests.Lazy
{
    [TestClass]
    public class AsyncLazy_V2Tests
    {
        [TestMethod]
        public async Task LazyDbConnection_OpenedOnce_ReusedAcrossOperations_Test()
        {
            var openCount = 0;
            var lazyDb = new AsyncLazy<FakeDbConnection>(async ct =>
            {
                Interlocked.Increment(ref openCount);
                return await FakeDbConnection.OpenAsync(ct);
            });

            // Multiple "repository methods" all await the same lazy connection
            var conn1 = await lazyDb.GetValueAsync();
            var conn2 = await lazyDb.GetValueAsync();
            var conn3 = await lazyDb.GetValueAsync();

            Assert.AreEqual(1, openCount, "Connection should be opened exactly once.");
            Assert.IsTrue(conn1.IsOpen);
            Assert.AreSame(conn1, conn2, "All callers must share the same connection object.");
            Assert.AreSame(conn2, conn3);
        }

        [TestMethod]
        public async Task LazyDbConnection_ConcurrentRepositoryMethods_SingleOpenCall_Test()
        {
            var openCount = 0;
            var lazyDb = new AsyncLazy<FakeDbConnection>(async ct =>
            {
                Interlocked.Increment(ref openCount);
                return await FakeDbConnection.OpenAsync(ct);
            });

            // Simulate 10 repository methods firing simultaneously at startup
            var tasks = new Task<FakeDbConnection>[10];
            for (var i = 0; i < tasks.Length; i++)
                tasks[i] = lazyDb.GetValueAsync();

            var connections = await Task.WhenAll(tasks);

            Assert.AreEqual(1, openCount);
            var first = connections[0];
            foreach (var c in connections)
                Assert.AreSame(first, c, "All concurrent callers must receive the same instance.");
        }

        [TestMethod]
        public async Task LazyRemoteConfig_FetchedOnce_AvailableEverywhere_Test()
        {
            var fetchCount = 0;
            var lazyConfig = new AsyncLazy<FakeRemoteConfig>(async ct =>
            {
                Interlocked.Increment(ref fetchCount);
                return await FakeRemoteConfig.FetchAsync(ct);
            });

            var cfg1 = await lazyConfig.GetValueAsync();
            var cfg2 = await lazyConfig.GetValueAsync();

            Assert.AreEqual(1, fetchCount);
            Assert.AreEqual("production", cfg1.Environment);
            Assert.AreEqual(100, cfg1.MaxConnections);
            Assert.AreSame(cfg1, cfg2);
        }

        [TestMethod]
        public async Task LazyRemoteConfig_TryGetValue_ReturnsCachedConfigWithoutNetworkCall_Test()
        {
            var lazyConfig = new AsyncLazy<FakeRemoteConfig>(async ct =>
            {
                return await FakeRemoteConfig.FetchAsync(ct);
            });

            // Before first fetch, TryGetValue must return false
            Assert.IsFalse(lazyConfig.TryGetValue(out _));

            await lazyConfig.GetValueAsync();

            // After fetch, read config synchronously with zero I/O
            Assert.IsTrue(lazyConfig.TryGetValue(out var cfg));
            Assert.AreEqual("production", cfg.Environment);
        }

        [TestMethod]
        public async Task PerUserLazyProfile_EachUserLoadedIndependently_Test()
        {
            var profileCache = new ConcurrentDictionary<int, AsyncLazy<FakeUserProfile>>();

            AsyncLazy<FakeUserProfile> GetOrCreate(int userId)
            {
                return profileCache.GetOrAdd(userId, id =>
                    new AsyncLazy<FakeUserProfile>(ct => FakeUserProfile.LoadAsync(id, ct)));
            }

            var p1 = await GetOrCreate(1).GetValueAsync();
            var p2 = await GetOrCreate(2).GetValueAsync();
            var p1Again = await GetOrCreate(1).GetValueAsync();

            Assert.AreEqual(1, p1.UserId);
            Assert.AreEqual(2, p2.UserId);
            Assert.AreSame(p1, p1Again, "Same user profile returned from cache.");
        }

        [TestMethod]
        public async Task PerUserLazyProfile_LogoutResets_NextAccessReloads_Test()
        {
            var loadCount = 0;
            var profileCache = new ConcurrentDictionary<int, AsyncLazy<FakeUserProfile>>();

            AsyncLazy<FakeUserProfile> GetOrCreate(int userId)
            {
                return profileCache.GetOrAdd(userId, id =>
                    new AsyncLazy<FakeUserProfile>(async ct =>
                    {
                        Interlocked.Increment(ref loadCount);
                        return await FakeUserProfile.LoadAsync(id, ct);
                    }));
            }

            // First load
            var p1 = await GetOrCreate(42).GetValueAsync();
            Assert.AreEqual(1, loadCount);
            Assert.AreEqual("User #42", p1.DisplayName);

            // Simulate logout: reset the lazy for user 42
            GetOrCreate(42).Reset();

            // Give Reset a moment to clear state
            await Task.Delay(10);

            // Next access reloads the profile
            var p1Reloaded = await GetOrCreate(42).GetValueAsync();
            Assert.AreEqual(2, loadCount);
            Assert.AreEqual("User #42", p1Reloaded.DisplayName);
        }

        [TestMethod]
        public async Task TransientFault_AutoReset_SecondCallSucceeds_Test()
        {
            var attempt = 0;
            var lazyService = new AsyncLazy<FakeDbConnection>(async ct =>
            {
                var n = Interlocked.Increment(ref attempt);
                if (n == 1)
                {
                    await Task.Yield();
                    throw new TimeoutException("Transient: service temporarily unavailable.");
                }

                return await FakeDbConnection.OpenAsync(ct);
            });

            // First call — transient failure
            await Assert.ThrowsExceptionAsync<TimeoutException>(() => lazyService.GetValueAsync());

            // Allow internal Reset() from the fault handler to clear _state
            await Task.Delay(20);

            // Second call — auto-recovered, succeeds
            var conn = await lazyService.GetValueAsync();

            Assert.IsTrue(conn.IsOpen);
            Assert.AreEqual(2, attempt);
        }

        [TestMethod]
        public async Task TransientFault_ThirdRetrySucceeds_CorrectValueReturned_Test()
        {
            var attempt = 0;
            var lazyService = new AsyncLazy<FakeDbConnection>(async ct =>
            {
                var n = Interlocked.Increment(ref attempt);
                await Task.Yield();
                if (n < 3)
                    throw new InvalidOperationException($"Attempt {n} failed.");
                return await FakeDbConnection.OpenAsync(ct);
            });

            for (var retry = 0; retry < 2; retry++)
            {
                await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => lazyService.GetValueAsync());
                await Task.Delay(20);
            }

            var conn = await lazyService.GetValueAsync();
            Assert.IsTrue(conn.IsOpen);
            Assert.AreEqual(3, attempt);
        }

        [TestMethod]
        public async Task AppShutdown_CancelsInProgressFactory_LazyIsResetAfterwards_Test()
        {
            var factoryStarted = new TaskCompletionSource<bool>();

            var lazyService = new AsyncLazy<FakeDbConnection>(async ct =>
            {
                factoryStarted.TrySetResult(true);
                await Task.Delay(Timeout.Infinite, ct); // blocks until cancelled
                return null;
            });

            using var shutdownCts = new CancellationTokenSource();
            var pendingTask = lazyService.GetValueAsync(shutdownCts.Token);

            // Wait until the factory has actually started
            await factoryStarted.Task;

            // Signal shutdown
            shutdownCts.Cancel();

            await Assert.ThrowsExceptionAsync<TaskCanceledException>(() => pendingTask);

            // Allow internal Reset() from the cancellation handler to clear _state
            await Task.Delay(20);

            // Lazy is clean — IsInitializationStarted is false
            Assert.IsFalse(lazyService.IsInitializationStarted);
        }

        [TestMethod]
        public async Task DependentServices_ServiceBRequiresServiceA_BothInitializedOnce_Test()
        {
            var aInitCount = 0;
            var bInitCount = 0;

            var lazyA = new AsyncLazy<FakeDbConnection>(async ct =>
            {
                Interlocked.Increment(ref aInitCount);
                return await FakeDbConnection.OpenAsync(ct);
            });

            // ServiceB depends on ServiceA's connection
            var lazyB = new AsyncLazy<string>(async ct =>
            {
                Interlocked.Increment(ref bInitCount);
                var connA = await lazyA.GetValueAsync(ct); // depends on A
                await Task.Delay(10, ct);
                return $"ServiceB using connection(open={connA.IsOpen})";
            });

            // Three concurrent consumers of ServiceB
            var t1 = lazyB.GetValueAsync();
            var t2 = lazyB.GetValueAsync();
            var t3 = lazyB.GetValueAsync();

            var results = await Task.WhenAll(t1, t2, t3);

            Assert.AreEqual(1, aInitCount, "ServiceA initialized once.");
            Assert.AreEqual(1, bInitCount, "ServiceB initialized once.");
            foreach (var r in results)
                Assert.AreEqual("ServiceB using connection(open=True)", r);
        }
    }
}