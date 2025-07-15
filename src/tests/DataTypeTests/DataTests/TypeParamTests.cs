// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2024-02-13 17:01
// 
//  Last Modified By : RzR
//  Last Modified On : 2024-02-13 19:41
// ***********************************************************************
//  <copyright file="TypeParamTests.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using DataTypeTests.Models;
using DomainCommonExtensions.CommonExtensions.TypeParam;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DomainCommonExtensions.CommonExtensions;
using DomainCommonExtensions.DataTypeExtensions;

#endregion

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class TypeParamTests
    {
        [DataRow(null, 0, 0)]
        [DataRow(null, null, null)]
        [DataRow(1, 0, 1)]
        [DataRow(0, 0, 0)]
        [TestMethod]
        public void IfIsNull_NullableInt_Test(int? sourceValue, int? defaultValue, int? exceptedResult)
        {
            var test = sourceValue.IfIsNull(defaultValue);

            Assert.AreEqual(exceptedResult, test);
        }

        [DataRow(null, 0, 0)]
        [DataRow(null, null, null)]
        [DataRow(1, 0, 1)]
        [DataRow(0, 0, 0)]
        [TestMethod]
        public void IfIsNull_NullableDecimal_Test(int? sourceValue, int? defaultValue, int? exceptedResult)
        {
            var test = ((decimal?)sourceValue).IfIsNull(defaultValue);

            Assert.AreEqual(exceptedResult, test);
        }

        [DataRow(null, 0D, 0D)]
        [DataRow(1D, 0D, 1D)]
        [DataRow(0D, 0D, 0D)]
        [DataRow(null, null, null)]
        [TestMethod]
        public void IfIsNull_NullableDouble_Test(double? sourceValue, double? defaultValue, double? exceptedResult)
        {
            var test = sourceValue.IfIsNull(defaultValue);

            Assert.AreEqual(exceptedResult, test);
        }

        [DataRow(null, 0D, 0D)]
        [DataRow(1D, 0D, 1D)]
        [DataRow(0D, 0D, 0D)]
        [TestMethod]
        public void IfIsNull_Double_Test(double? sourceValue, double defaultValue, double exceptedResult)
        {
            var test = sourceValue.IfIsNull(defaultValue);

            Assert.AreEqual(exceptedResult, test);
        }

        [TestMethod]
        public void IfIsNull_ClassModel_Test()
        {
            var testModel = new TempModel();
            var test = testModel.IfIsNull(null);
            Assert.AreEqual(testModel, test);

            var test1 = ((TempModel)null).IfIsNull(null);
            Assert.AreEqual(null, test1);

            var testModel2 = new TempModel();
            var test2 = ((TempModel)null).IfIsNull(testModel2);
            Assert.AreEqual(testModel2, test2);
        }

        [DataRow("a", "a", "a")]
        [DataRow("a", "g", "a")]
        [DataRow("a", null, "a")]
        [DataRow(null, "r", null)]
        [DataRow(null, null, null)]
        [TestMethod]
        public void IfEquals_string_Test(string sourceValue, string checkValue, string exceptedResult)
        {
            var r = sourceValue.IfEquals(checkValue, exceptedResult);

            if (sourceValue.IsNull())
                Assert.IsNull(r);
            else
                Assert.IsNotNull(r);
            Assert.AreEqual(exceptedResult, r);
        }

        [DataRow(1, 1, 1)]
        [DataRow(2, 1, 2)]
        [DataRow(3, null, 3)]
        [DataRow(null, 2, null)]
        [DataRow(null, null, null)]
        [TestMethod]
        public void IfEquals_int_Test(int sourceValue, int checkValue, int exceptedResult)
        {
            var r = sourceValue.IfEquals(checkValue, exceptedResult);

            if (sourceValue.IsNull())
                Assert.IsNull(r);
            else
                Assert.IsNotNull(r);
            Assert.AreEqual(exceptedResult, r);
        }

        [DataRow(1, 1, 1)]
        [DataRow(2, 1, 2)]
        [DataRow(3, null, 3)]
        [DataRow(null, 2, null)]
        [DataRow(null, null, null)]
        [TestMethod]
        public void IfEquals_long_Test(long sourceValue, long checkValue, long exceptedResult)
        {
            var r = sourceValue.IfEquals(checkValue, exceptedResult);

            if (sourceValue.IsNull())
                Assert.IsNull(r);
            else
                Assert.IsNotNull(r);
            Assert.AreEqual(exceptedResult, r);
        }

        [DataRow(1.2, 1.21, 1.2)]
        [DataRow(2, 1, 2)]
        [DataRow(3, null, 3)]
        [DataRow(null, 2, null)]
        [DataRow(null, null, null)]
        [TestMethod]
        public void IfEquals_double_Test(double sourceValue, double checkValue, double exceptedResult)
        {
            var r = sourceValue.IfEquals(checkValue, exceptedResult);

            if (sourceValue.IsNull())
                Assert.IsNull(r);
            else
                Assert.IsNotNull(r);
            Assert.AreEqual(exceptedResult, r);
        }

        [DataRow("5e9d1a52-d737-4dd0-9075-e9b0f46c19d2", "5e9d1a52-d737-4dd0-9075-e9b0f46c19d2", "00000000-0000-0000-0000-000000000000")]
        [DataRow("5e9d1a52-d737-4dd0-9075-e9b0f46c19d2", "00000000-0000-0000-0000-000000000000", "5e9d1a52-d737-4dd0-9075-e9b0f46c19d2")]
        [DataRow("5e9d1a52-d737-4dd0-9075-e9b0f46c19d2", null, "5e9d1a52-d737-4dd0-9075-e9b0f46c19d2")]
        [DataRow(null, "00000000-0000-0000-0000-000000000000", null)]
        [DataRow(null, null, null)]
        [TestMethod]
        public void IfEquals_Guid_Test(string sourceValue, string checkValue, string exceptedResult)
        {
            var sourceGuid = (Guid?)sourceValue.ToGuid();
            var checkValueGuid = (Guid?)checkValue.ToGuid();
            var exceptedResultGuid = (Guid?)exceptedResult.ToGuid();

            var r = sourceGuid.IfEquals(checkValueGuid, exceptedResultGuid);

            if (sourceGuid.IsNull())
                Assert.IsNull(r);
            else
                Assert.IsNotNull(r);
            Assert.AreEqual(exceptedResultGuid, r);
        }

        [DataRow("a", "a", "a")]
        [DataRow("a", "g", "a")]
        [DataRow("a", null, "a")]
        [DataRow(null, "r", null)]
        [DataRow(null, null, null)]
        [TestMethod]
        public void IfNotEquals_string_Test(string sourceValue, string checkValue, string exceptedResult)
        {
            var r = sourceValue.IfNotEquals(checkValue, exceptedResult);

            if (sourceValue.IsNull())
                Assert.IsNull(r);
            else
                Assert.IsNotNull(r);
            Assert.AreEqual(exceptedResult, r);
        }

        [DataRow(1, 1, 1)]
        [DataRow(2, 1, 2)]
        [DataRow(3, null, 3)]
        [DataRow(null, 2, null)]
        [DataRow(null, null, null)]
        [TestMethod]
        public void IfNotEquals_int_Test(int sourceValue, int checkValue, int exceptedResult)
        {
            var r = sourceValue.IfNotEquals(checkValue, exceptedResult);

            if (sourceValue.IsNull())
                Assert.IsNull(r);
            else
                Assert.IsNotNull(r);
            Assert.AreEqual(exceptedResult, r);
        }

        [DataRow(1, 1, 1)]
        [DataRow(2, 1, 2)]
        [DataRow(3, null, 3)]
        [DataRow(null, 2, null)]
        [DataRow(null, null, null)]
        [TestMethod]
        public void IfNotEquals_long_Test(long sourceValue, long checkValue, long exceptedResult)
        {
            var r = sourceValue.IfNotEquals(checkValue, exceptedResult);

            if (sourceValue.IsNull())
                Assert.IsNull(r);
            else
                Assert.IsNotNull(r);
            Assert.AreEqual(exceptedResult, r);
        }
    }
}