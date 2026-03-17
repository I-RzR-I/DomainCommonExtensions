// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2026-03-17 20:03
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-03-17 20:50
// ***********************************************************************
//  <copyright file="AppConfig.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

namespace DataTypeTests.Models.Lazy
{
    internal sealed class AppConfig
    {
        public string ConnectionString { get; init; }
        public int MaxRetries { get; init; }
    }
}