// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2026-03-17 21:03
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-03-17 21:15
// ***********************************************************************
//  <copyright file="FakeHealthChecker.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Threading;
using System.Threading.Tasks;

#endregion

namespace DataTypeTests.Models.Lazy
{
    internal sealed class FakeHealthChecker
    {
        private int _checkCount;
        public int CheckCount => _checkCount;

        public async Task<HealthStatus> CheckAsync(CancellationToken ct)
        {
            await Task.Delay(15, ct);
            Interlocked.Increment(ref _checkCount);

            return new HealthStatus { Healthy = true, Message = $"ok (check #{_checkCount})" };
        }
    }
}