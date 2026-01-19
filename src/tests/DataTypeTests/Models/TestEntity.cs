// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2026-01-19 23:01
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-01-19 23:14
// ***********************************************************************
//  <copyright file="TestEntity.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using System;

namespace DataTypeTests.Models
{
    public class TestEntity
    {
        public int IntProp { get; set; }
        public int? NullableIntProp { get; set; }
        public string StringProp { get; set; }
        public DateTime DateProp { get; set; }
    }
}