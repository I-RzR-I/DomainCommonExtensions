// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2026-03-17 21:03
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-03-17 21:14
// ***********************************************************************
//  <copyright file="FakeFeatureFlagService.cs" company="RzR SOFT & TECH">
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
    internal sealed class FakeFeatureFlagService
    {
        private int _fetchCount;
        public int FetchCount => _fetchCount;
        public bool NextValue { get; set; } = true;

        public async Task<bool> FetchAsync(string flag, CancellationToken ct)
        {
            await Task.Delay(15, ct);
            Interlocked.Increment(ref _fetchCount);

            return NextValue;
        }
    }
}