// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2026-03-17 21:03
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-03-17 21:13
// ***********************************************************************
//  <copyright file="FakeRateService.cs" company="RzR SOFT & TECH">
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
    internal sealed class FakeRateService
    {
        private int _fetchCount;
        public int FetchCount => _fetchCount;

        public async Task<decimal> FetchRateAsync(string pair, CancellationToken ct)
        {
            await Task.Delay(15, ct);
            Interlocked.Increment(ref _fetchCount);

            return 1.08m + _fetchCount * 0.001m; // slightly different rate each call
        }
    }
}