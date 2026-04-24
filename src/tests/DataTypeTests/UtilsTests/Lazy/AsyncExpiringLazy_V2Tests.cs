// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2026-03-17 20:03
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-03-17 21:16
// ***********************************************************************
//  <copyright file="AsyncExpiringLazy_V2Tests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using RzR.Extensions.Domain.Async.LazyLoad;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using DataTypeTests.Models.Lazy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// ReSharper disable InconsistentNaming

#endregion

namespace DataTypeTests.UtilsTests.Lazy
{
    [TestClass]
    public class AsyncExpiringLazy_V2Tests
    {
        [TestMethod]
        public async Task BearerToken_IssuedOnce_SharedAcrossAllRequests()
        {
            var issuer = new FakeTokenIssuer();
            var tokenTtl = TimeSpan.FromMinutes(5);

            var lazyToken = new AsyncExpiringLazy<string>(
                ct => issuer.IssueAsync(tokenTtl, ct), tokenTtl);

            // 10 parallel HTTP calls all need the same token
            var tasks = new Task<string>[10];
            for (var i = 0; i < tasks.Length; i++)
                tasks[i] = lazyToken.GetValueAsync();

            var tokens = await Task.WhenAll(tasks);

            Assert.AreEqual(1, issuer.IssuedCount, "Token issued exactly once.");
            var expected = tokens[0];
            foreach (var t in tokens)
                Assert.AreEqual(expected, t, "All callers must share the same token.");
        }

        [TestMethod]
        public async Task BearerToken_AutoRefreshed_AfterTtlExpires()
        {
            var issuer = new FakeTokenIssuer();
            var shortTtl = TimeSpan.FromMilliseconds(100);

            var lazyToken = new AsyncExpiringLazy<string>(
                ct => issuer.IssueAsync(shortTtl, ct), shortTtl);

            var first = await lazyToken.GetValueAsync();
            Assert.AreEqual(1, issuer.IssuedCount);
            Assert.IsTrue(first.Contains("token-1"));

            // Wait for TTL to expire
            await Task.Delay(200);

            var second = await lazyToken.GetValueAsync();
            Assert.AreEqual(2, issuer.IssuedCount, "Token must be re-issued after expiry.");
            Assert.IsTrue(second.Contains("token-2"));
            Assert.AreNotEqual(first, second);
        }

        [TestMethod]
        public async Task BearerToken_ForcedRefresh_ViaInvalidateOnAuthFailure()
        {
            // Scenario: an HTTP handler receives a 401, invalidates the token cache
            // immediately so the very next retry re-fetches without waiting for TTL.
            var issuer = new FakeTokenIssuer();
            var longTtl = TimeSpan.FromMinutes(10);

            var lazyToken = new AsyncExpiringLazy<string>(
                ct => issuer.IssueAsync(longTtl, ct), longTtl);

            var token1 = await lazyToken.GetValueAsync();
            Assert.AreEqual(1, issuer.IssuedCount);

            // Simulate receiving a 401 — reset (invalidate) immediately
            lazyToken.Reset();

            var token2 = await lazyToken.GetValueAsync();
            Assert.AreEqual(2, issuer.IssuedCount, "New token issued after forced reset.");
            Assert.AreNotEqual(token1, token2);
        }

        [TestMethod]
        public async Task DnsCache_ResolvesOnce_ReusedWithinTtl()
        {
            var resolver = new FakeDnsResolver();
            var lazyIp = new AsyncExpiringLazy<string>(
                ct => resolver.ResolveAsync("api.example.com", ct),
                TimeSpan.FromMinutes(1));

            var ip1 = await lazyIp.GetValueAsync();
            var ip2 = await lazyIp.GetValueAsync();

            Assert.AreEqual(1, resolver.ResolveCount);
            Assert.AreEqual(ip1, ip2);
            Assert.AreEqual("10.0.0.1", ip1);
        }

        [TestMethod]
        public async Task DnsCache_RefreshesAfterTtl_ReturnsNewIp()
        {
            var resolver = new FakeDnsResolver();
            var shortTtl = TimeSpan.FromMilliseconds(100);
            var lazyIp = new AsyncExpiringLazy<string>(
                ct => resolver.ResolveAsync("api.example.com", ct), shortTtl);

            var ip1 = await lazyIp.GetValueAsync();
            await Task.Delay(200); // expire TTL
            var ip2 = await lazyIp.GetValueAsync();

            Assert.AreEqual(2, resolver.ResolveCount);
            Assert.AreNotEqual(ip1, ip2, "A re-query must return the updated resolution.");
        }

        [TestMethod]
        public async Task DnsCache_TryGetValue_FastPathForHotService()
        {
            var resolver = new FakeDnsResolver();
            var lazyIp = new AsyncExpiringLazy<string>(
                ct => resolver.ResolveAsync("api.example.com", ct),
                TimeSpan.FromMinutes(1));

            // Before first resolution, synchronous fast-path returns false
            Assert.IsFalse(lazyIp.TryGetValue(out _));

            await lazyIp.GetValueAsync();

            // Hot path — no async overhead
            Assert.IsTrue(lazyIp.TryGetValue(out var cachedIp));
            Assert.AreEqual("10.0.0.1", cachedIp);
        }

        [TestMethod]
        public async Task ExchangeRate_FetchedOnce_UsedForAllOrdersWithinTtl()
        {
            var service = new FakeRateService();
            var lazyRate = new AsyncExpiringLazy<decimal>(
                ct => service.FetchRateAsync("EUR/USD", ct),
                TimeSpan.FromMinutes(1));

            var tasks = new Task<decimal>[8];
            for (var i = 0; i < tasks.Length; i++)
                tasks[i] = lazyRate.GetValueAsync();

            var rates = await Task.WhenAll(tasks);

            Assert.AreEqual(1, service.FetchCount, "Rate fetched once for all concurrent orders.");
            var expected = rates[0];
            foreach (var r in rates)
                Assert.AreEqual(expected, r);
        }

        [TestMethod]
        public async Task ExchangeRate_StaleRateRefreshed_AfterTtl()
        {
            var service = new FakeRateService();
            var shortTtl = TimeSpan.FromMilliseconds(100);
            var lazyRate = new AsyncExpiringLazy<decimal>(
                ct => service.FetchRateAsync("EUR/USD", ct), shortTtl);

            var rate1 = await lazyRate.GetValueAsync();
            await Task.Delay(200);
            var rate2 = await lazyRate.GetValueAsync();

            Assert.AreEqual(2, service.FetchCount);
            Assert.AreNotEqual(rate1, rate2, "Rate must be refreshed after TTL.");
        }

        [TestMethod]
        public async Task FeatureFlag_CachedForTtl_ChangePropagatesAfterExpiry()
        {
            var flagService = new FakeFeatureFlagService { NextValue = true };
            var shortTtl = TimeSpan.FromMilliseconds(100);

            var lazyFlag = new AsyncExpiringLazy<bool>(
                ct => flagService.FetchAsync("dark-mode", ct), shortTtl);

            var enabled1 = await lazyFlag.GetValueAsync();
            Assert.IsTrue(enabled1);
            Assert.AreEqual(1, flagService.FetchCount);

            // Remote value changes but cache is still valid
            flagService.NextValue = false;
            var enabled2 = await lazyFlag.GetValueAsync();
            Assert.IsTrue(enabled2, "Cached value must be served while TTL is active.");
            Assert.AreEqual(1, flagService.FetchCount);

            // After TTL, the new value is picked up
            await Task.Delay(200);
            var enabled3 = await lazyFlag.GetValueAsync();
            Assert.IsFalse(enabled3, "Updated value must be fetched after TTL.");
            Assert.AreEqual(2, flagService.FetchCount);
        }

        [TestMethod]
        public async Task FeatureFlag_PerFlagCacheMap_EachFlagManagedIndependently()
        {
            var flagService = new FakeFeatureFlagService { NextValue = true };
            var ttl = TimeSpan.FromMinutes(5);

            // Each flag gets its own AsyncExpiringLazy<bool>
            var flags = new ConcurrentDictionary<string, AsyncExpiringLazy<bool>>();

            AsyncExpiringLazy<bool> GetOrAddFlag(string name)
            {
                return flags.GetOrAdd(name, _ =>
                    new AsyncExpiringLazy<bool>(ct => flagService.FetchAsync(name, ct), ttl));
            }

            var darkMode = await GetOrAddFlag("dark-mode").GetValueAsync();
            var betaUi = await GetOrAddFlag("beta-ui").GetValueAsync();
            var darkMode2 = await GetOrAddFlag("dark-mode").GetValueAsync();

            Assert.IsTrue(darkMode);
            Assert.IsTrue(betaUi);
            Assert.AreEqual(darkMode, darkMode2); // same cached instance
            Assert.AreEqual(2, flagService.FetchCount, "Each distinct flag fetched once.");
        }

        [TestMethod]
        public async Task HealthCheck_RunOnce_ServedToAllProbesWithinTtl()
        {
            var checker = new FakeHealthChecker();
            var lazyHealth = new AsyncExpiringLazy<HealthStatus>(
                ct => checker.CheckAsync(ct),
                TimeSpan.FromMinutes(1));

            // 5 simultaneous health probes
            var tasks = new Task<HealthStatus>[5];
            for (var i = 0; i < tasks.Length; i++)
                tasks[i] = lazyHealth.GetValueAsync();

            var results = await Task.WhenAll(tasks);

            Assert.AreEqual(1, checker.CheckCount, "Health check executed exactly once.");
            var first = results[0];
            foreach (var r in results)
            {
                Assert.IsTrue(r.Healthy);
                Assert.AreSame(first, r, "All probes share the same cached result.");
            }
        }

        [TestMethod]
        public async Task HealthCheck_StaleResultRefreshed_AfterTtl()
        {
            var checker = new FakeHealthChecker();
            var shortTtl = TimeSpan.FromMilliseconds(100);
            var lazyHealth = new AsyncExpiringLazy<HealthStatus>(
                ct => checker.CheckAsync(ct), shortTtl);

            var status1 = await lazyHealth.GetValueAsync();
            Assert.AreEqual("ok (check #1)", status1.Message);

            await Task.Delay(200);

            var status2 = await lazyHealth.GetValueAsync();
            Assert.AreEqual(2, checker.CheckCount);
            Assert.AreEqual("ok (check #2)", status2.Message);
            Assert.AreNotSame(status1, status2);
        }

        [TestMethod]
        public async Task HealthCheck_CachedResult_AvailableSynchronously_ViaTryGetValue()
        {
            var checker = new FakeHealthChecker();
            var lazyHealth = new AsyncExpiringLazy<HealthStatus>(
                ct => checker.CheckAsync(ct),
                TimeSpan.FromMinutes(1));

            Assert.IsFalse(lazyHealth.TryGetValue(out _), "No result cached yet.");

            await lazyHealth.GetValueAsync();

            Assert.IsTrue(lazyHealth.TryGetValue(out var cached));
            Assert.IsTrue(cached.Healthy);
        }

        [TestMethod]
        public async Task ForcedInvalidation_PreviousCacheStoreDomainEvent_RefetchesImmediately()
        {
            var resolver = new FakeDnsResolver();
            var longTtl = TimeSpan.FromHours(1); // would never expire on its own

            var lazyIp = new AsyncExpiringLazy<string>(
                ct => resolver.ResolveAsync("db.internal", ct), longTtl);

            var ip1 = await lazyIp.GetValueAsync();
            Assert.AreEqual(1, resolver.ResolveCount);
            Assert.AreEqual("10.0.0.1", ip1);
            Assert.IsTrue(lazyIp.IsValueCreated);

            // A failover event is received — the DB moved to a new IP
            lazyIp.Reset();

            Assert.IsFalse(lazyIp.IsValueCreated, "Cache must be cleared after Reset.");
            Assert.IsFalse(lazyIp.IsInitializationStarted);

            var ip2 = await lazyIp.GetValueAsync();
            Assert.AreEqual(2, resolver.ResolveCount, "Fresh resolution must happen after reset.");
            Assert.AreNotEqual(ip1, ip2);
        }

        [TestMethod]
        public async Task ForcedInvalidation_MultipleConcurrentCallersAfterInvalidate_FactoryCalledOnce()
        {
            var issuer = new FakeTokenIssuer();
            var longTtl = TimeSpan.FromHours(1);

            var lazyToken = new AsyncExpiringLazy<string>(
                ct => issuer.IssueAsync(longTtl, ct), longTtl);

            // Prime the cache
            await lazyToken.GetValueAsync();
            Assert.AreEqual(1, issuer.IssuedCount);

            // Simulate receiving a 401 on multiple threads simultaneously
            lazyToken.Reset();

            var tasks = new Task<string>[10];
            for (var i = 0; i < tasks.Length; i++)
                tasks[i] = lazyToken.GetValueAsync();

            var tokens = await Task.WhenAll(tasks);

            // Despite 10 concurrent callers after Invalidate, factory runs once
            Assert.AreEqual(2, issuer.IssuedCount,
                "Token must be re-issued exactly once despite concurrent post-invalidate callers.");

            var expected = tokens[0];
            foreach (var t in tokens)
                Assert.AreEqual(expected, t);
        }
    }
}