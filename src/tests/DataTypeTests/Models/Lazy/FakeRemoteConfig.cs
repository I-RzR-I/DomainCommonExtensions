// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2026-03-17 20:03
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-03-17 20:54
// ***********************************************************************
//  <copyright file="FakeRemoteConfig.cs" company="RzR SOFT & TECH">
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
    internal sealed class FakeRemoteConfig
    {
        public string Environment { get; set; }
        public int MaxConnections { get; set; }

        public static async Task<FakeRemoteConfig> FetchAsync(CancellationToken ct)
        {
            await Task.Delay(20, ct);

            return new FakeRemoteConfig { Environment = "production", MaxConnections = 100 };
        }
    }
}