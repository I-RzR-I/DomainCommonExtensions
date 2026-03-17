// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2026-03-17 21:03
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-03-17 21:12
// ***********************************************************************
//  <copyright file="FakeTokenIssuer.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace DataTypeTests.Models.Lazy
{
    internal sealed class FakeTokenIssuer
    {
        private int _issuedCount;
        public int IssuedCount => _issuedCount;

        /// <summary>Issues a short-lived bearer token.</summary>
        public async Task<string> IssueAsync(TimeSpan lifetime, CancellationToken ct)
        {
            await Task.Delay(20, ct);
            var n = Interlocked.Increment(ref _issuedCount);

            return $"Bearer token-{n} (valid {lifetime.TotalSeconds}s)";
        }
    }
}