// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2026-03-17 20:03
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-03-17 20:50
// ***********************************************************************
//  <copyright file="FakeTokenClient.cs" company="RzR SOFT & TECH">
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
    internal sealed class FakeTokenClient
    {
        private int _callCount;
        public int CallCount => _callCount;

        public async Task<string> AcquireTokenAsync(CancellationToken ct)
        {
            await Task.Delay(20, ct);

            return $"token-{Interlocked.Increment(ref _callCount)}";
        }
    }
}