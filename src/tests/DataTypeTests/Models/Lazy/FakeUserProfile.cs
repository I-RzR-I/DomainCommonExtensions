// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2026-03-17 20:03
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-03-17 20:55
// ***********************************************************************
//  <copyright file="FakeUserProfile.cs" company="RzR SOFT & TECH">
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
    internal sealed class FakeUserProfile
    {
        public int UserId { get; set; }
        public string DisplayName { get; set; }

        public static async Task<FakeUserProfile> LoadAsync(int userId, CancellationToken ct)
        {
            await Task.Delay(20, ct);

            return new FakeUserProfile { UserId = userId, DisplayName = $"User #{userId}" };
        }
    }
}