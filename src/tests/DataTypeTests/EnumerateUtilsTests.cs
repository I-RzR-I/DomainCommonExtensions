// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2025-01-09 20:50
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-01-09 20:50
// ***********************************************************************
//  <copyright file="EnumerateUtilsTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using System.Linq;
using DomainCommonExtensions.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataTypeTests
{
    [TestClass]
    public class EnumerateUtilsTests
    {
        [TestMethod]
        public void Int32_FromTo_0_2_Test()
        {
            var list = EnumerateUtils.FromTo(0, 2);

            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count() == 3);
        }

        [TestMethod]
        public void Int32_FromTo_0_0_Test()
        {
            var list = EnumerateUtils.FromTo(0, 0);

            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count() == 1);
        }

        [TestMethod]
        public void Int32_FromTo_Minus5_0_Test()
        {
            var list = EnumerateUtils.FromTo(-5, 0);

            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count() == 6);
        }

        [TestMethod]
        public void Int32_FromTo_4_3_Test()
        {
            var list = EnumerateUtils.FromTo(4, 3);

            Assert.IsNotNull(list);
            Assert.IsFalse(list.Any());
        }

        [TestMethod]
        public void PowersOf_Test()
        {
            var list = EnumerateUtils.PowersOf(2, 4);

            Assert.IsNotNull(list);
            Assert.IsTrue(list.Any());
        }
    }
}