// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2026-03-17 20:03
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-03-17 20:50
// ***********************************************************************
//  <copyright file="FakeDbConnection.cs" company="RzR SOFT & TECH">
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
    internal sealed class FakeDbConnection
    {
        public bool IsOpen { get; private set; }

        public static async Task<FakeDbConnection> OpenAsync(CancellationToken ct)
        {
            await Task.Delay(30, ct); // simulate network round-trip

            return new FakeDbConnection { IsOpen = true };
        }

        public Task<string[]> QueryAsync(string sql)
        {
            return Task.FromResult(new[] { $"row1 for [{sql}]", $"row2 for [{sql}]" });
        }
    }
}