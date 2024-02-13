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

using DataTypeTests.Models;
using DomainCommonExtensions.CommonExtensions;
using DomainCommonExtensions.CommonExtensions.TypeParam;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace DataTypeTests
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

        [TestMethod]
        public void IfIsNulOrFunc_Null_Test()
        {
            int? i = null;

            var test = i.IfIsNullOrFuncIsTrue(0, () => i == 0);

            Assert.AreEqual(0, test);
        }

        [TestMethod]
        public void IfIsNulOrFunc_Zero_Equal_Zero_Test()
        {
            int? i = 0;

            var test = i.IfIsNullOrFuncIsTrue(-1, () => i == 0);

            Assert.AreEqual(-1, test);
        }

        [TestMethod]
        public void IfIsNulOrFunc_One_Equal_Zero_False_Test()
        {
            int? i = 1;

            var test = i.IfIsNullOrFuncIsTrue(0, () => i == 0);

            Assert.AreEqual(1, test);
        }

        [TestMethod]
        public void IfIsNulOrFunc_Null_Func_True_Test()
        {
            int? i = null;

            var test = i.IfFunc(0, () => i != 0 && i.IsNull(), true);

            Assert.AreEqual(0, test);
        }

        [TestMethod]
        public void IfIsNulOrFunc_Null_Func_False_Test()
        {
            int? i = null;

            var test = i.IfFunc(0, () => i != 0 && i.IsNull(), false);

            Assert.AreEqual(null, test);
        }

        [TestMethod]
        public void IfIsNulOrFunc_1_Func_True_Test()
        {
            int? i = 1;

            var test = i.IfFunc(0, () => i != 0 && i.IsNull(), true);

            Assert.AreEqual(1, test);
        }

        [TestMethod]
        public void IfIsNulOrFunc_1_Func_False_Test()
        {
            int? i = 1;

            var test = i.IfFunc(0, () => i != 0 && i.IsNull(), false);

            Assert.AreEqual(0, test);
        }
    }
}