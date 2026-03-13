// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2026-03-06 21:03
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-03-06 21:22
// ***********************************************************************
//  <copyright file="TupleResultTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using DomainCommonExtensions.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace DataTypeTests.HelperTests
{
    [TestClass]
    public class TupleResultTests
    {
        [TestMethod]
        public void Destruct_0_Test()
        {
            var tp1 = TupleResult.Create("New York City", 8000000, 468.9);
            tp1.Deconstruct(out var city, out var population, out var area);

            Assert.IsNotNull(city);
            Assert.AreEqual("New York City", city);

            Assert.IsNotNull(population);
            Assert.AreEqual(8000000, population);

            Assert.IsNotNull(area);
            Assert.AreEqual(468.9, area);
        }

        [TestMethod]
        public void Destruct_1_Test()
        {
            var tp1 = TupleResult.Create("New York City", 8000000, 468.9);
            var (city, population, area) = tp1;

            Assert.IsNotNull(city);
            Assert.AreEqual("New York City", city);

            Assert.IsNotNull(population);
            Assert.AreEqual(8000000, population);

            Assert.IsNotNull(area);
            Assert.AreEqual(468.9, area);
        }
    }
}