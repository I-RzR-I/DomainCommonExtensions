// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2022-08-16 11:52
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-16 17:26
// ***********************************************************************
//  <copyright file="IntTests.cs" company="">
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
    public class IntTests
    {
        [TestMethod]
        public void IsNullOrZeroTest()
        {
            var checkNull = ((int?)null).IsNullOrZero();
            var checkZero = ((int?)0).IsNullOrZero();
            var checkOne = ((int?)1).IsNullOrZero();

            Assert.IsTrue(checkNull);
            Assert.IsTrue(checkZero);
            Assert.IsFalse(checkOne);
        }

        [TestMethod]
        public void IsZeroTest()
        {
            var checkZero = 0.IsZero();
            var checkOne = 1.IsZero();

            Assert.IsTrue(checkZero);
            Assert.IsFalse(checkOne);
        }

        [TestMethod]
        public void SetFlagTest()
        {
            const long flagValue = 3;
            const long initValue = 10;
            var flag1 = initValue.SetFlag(flagValue, true);
            var isSetFlag = flag1.IsFlagSet(flagValue);

            Assert.IsNotNull(flag1);
            Assert.IsTrue(isSetFlag);
            Assert.AreEqual(11, flag1);

            var flag2 = initValue.SetFlag(flagValue, false);
            var isSetFlag2 = flag1.IsFlagSet(flagValue);

            Assert.IsNotNull(flag2);
            Assert.IsTrue(isSetFlag2);
            Assert.AreEqual(8, flag2);
        }

        [TestMethod]
        public void SetFlagIntTest()
        {
            const int flagValue = 3;
            const int initValue = 10;
            var flag1 = initValue.SetFlag(flagValue, true);
            var isSetFlag = flag1.IsFlagSet(flagValue);

            Assert.IsNotNull(flag1);
            Assert.IsTrue(isSetFlag);
            Assert.AreEqual(11, flag1);

            var flag2 = initValue.SetFlag(flagValue, false);
            var isSetFlag2 = flag1.IsFlagSet(flagValue);

            Assert.IsNotNull(flag2);
            Assert.IsTrue(isSetFlag2);
            Assert.AreEqual(8, flag2);
        }
    }
}