// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2022-08-15 19:40
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-15 19:44
// ***********************************************************************
//  <copyright file="DoubleTests.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using DomainCommonExtensions.DataTypeExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class DoubleTests
    {
        [TestMethod]
        public void MinutesToMs_1_Min_Test()
        {
            var mils = ((double)1).MinutesToMs();

            Assert.IsNotNull(mils);
            Assert.IsTrue(60000 == mils);
        }

        [TestMethod]
        public void MinutesToMs_0_5_Min_Test()
        {
            var mils = 0.5.MinutesToMs();

            Assert.IsNotNull(mils);
            Assert.IsTrue(30000 == mils);
        }

        [TestMethod]
        public void MinutesToSeconds_1_Min_Test()
        {
            var sec = ((double)1).MinutesToSeconds();

            Assert.IsNotNull(sec);
            Assert.AreEqual(60, sec);
        }

        [TestMethod]
        public void MinutesToSeconds_0_5_Min_Test()
        {
            var sec = 0.5.MinutesToSeconds();

            Assert.IsNotNull(sec);
            Assert.AreEqual(30, sec);
        }
    }
}