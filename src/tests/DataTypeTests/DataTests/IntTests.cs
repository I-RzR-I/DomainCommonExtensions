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

using DomainCommonExtensions.CommonExtensions;
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

        [DataRow(0, "0 bytes")]
        [DataRow(1024, "1.0 KB")]
        [DataRow(1902021, "1.8 MB")]
        [DataRow(99999999, "95.4 MB")]
        [DataRow(1073741824, "1.0 GB")]
        [DataRow(1610612736, "1.5 GB")]
        [DataRow(1099511627776, "1.0 TB")]
        [TestMethod]
        public void AsReadableFileSize_Test(long size, string excepted)
        {
            var result = size.AsReadableFileSize();

            Assert.IsNotNull(result);
            Assert.AreEqual(excepted, result);
        }

        [DataRow(1, 0, 2)]
        [DataRow(10, 5, 11)]
        [DataRow(10, 9, 10)]
        [DataRow(10, 10, 10)]
        [DataRow(10, 10, 11)]
        [DataRow(11, 10, 11)]
        [TestMethod]
        public void IsBetween_Test(int sourceValue, int min, int max)
        {
            var result = sourceValue.IsBetween(min, max);

            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }
    }
}