// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2025-10-07 16:10
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-10-07 16:45
// ***********************************************************************
//  <copyright file="ListTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Collections.Generic;
using DomainCommonExtensions.ArraysExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace DataTypeTests.DataTests.Array
{
    [TestClass]
    public class ListTests
    {
        [TestMethod]
        public void AddIsNotExist_Test()
        {
            var list = new List<string> { "Id", "Code" };

            list.AddToListIfNotExist("Name");

            Assert.IsTrue(list.Count == 3);
            Assert.IsTrue(list.Contains("Name"));
        }

        [TestMethod]
        public void AddIsNotExist_WithExistKey_Test()
        {
            var list = new List<string> { "Id", "Code", "Name" };

            list.AddToListIfNotExist("Name");

            Assert.IsTrue(list.Count == 3);
            Assert.IsTrue(list.Contains("Name"));
        }
    }
}