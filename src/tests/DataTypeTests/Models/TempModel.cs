// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2024-02-13 18:20
// 
//  Last Modified By : RzR
//  Last Modified On : 2024-02-13 18:20
// ***********************************************************************
//  <copyright file="TempModel.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using System;

namespace DataTypeTests.Models
{
    public class TempModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public decimal Version { get; set; }

        public decimal? Price { get; set; }

        public RecordType RecordType { get; set; }

        public bool IsActive { get; set; }

        public double DblValue1 { get; set; }

        public double? DblValue2 { get; set; }
    }
}