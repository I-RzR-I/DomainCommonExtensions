// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2025-10-29 09:10
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-10-29 09:39
// ***********************************************************************
//  <copyright file="TypeTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using DataTypeTests.Attributes;
using DataTypeTests.Models;
using DomainCommonExtensions.CommonExtensions.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class TypeTests
    {
        [TestMethod]
        public void HasAttribute_T_T_ShouldPass_With_False_Test()
        {
            var t = typeof(TempModel);

            var res = t.HasAttribute(typeof(TestAttribute));

            Assert.IsFalse(res);
        }

        [TestMethod]
        public void HasAttribute_OfT_ShouldPass_With_False_Test()
        {
            var t = typeof(TempModel);

            var res = t.HasAttribute<TestAttribute>();

            Assert.IsFalse(res);
        }

        [TestMethod]
        public void HasAttribute_T_T_ShouldPass_With_True_Test()
        {
            var t = typeof(IdNameActiveModel);

            var res = t.HasAttribute(typeof(TestAttribute));

            Assert.IsTrue(res);
        }

        [TestMethod]
        public void HasAttribute_OfT_ShouldPass_With_True_Test()
        {
            var t = typeof(IdNameActiveModel);

            var res = t.HasAttribute<TestAttribute>();

            Assert.IsTrue(res);
        }
    }
}