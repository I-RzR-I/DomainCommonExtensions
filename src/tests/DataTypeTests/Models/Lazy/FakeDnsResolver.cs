// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2026-03-17 21:03
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-03-17 21:12
// ***********************************************************************
//  <copyright file="FakeDnsResolver.cs" company="RzR SOFT & TECH">
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
    internal sealed class FakeDnsResolver
    {
        private int _resolveCount;
        public int ResolveCount => _resolveCount;

        public async Task<string> ResolveAsync(string host, CancellationToken ct)
        {
            await Task.Delay(15, ct);
            var n = Interlocked.Increment(ref _resolveCount);

            return $"10.0.0.{n}"; // each resolution returns a "new" IP
        }
    }
}