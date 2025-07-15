// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2025-07-11 12:43
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-07-11 12:58
// ***********************************************************************
//  <copyright file="FuncExtensionsTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Threading.Tasks;
using DomainCommonExtensions.CommonExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace DataTypeTests.UtilsTests
{
    [TestClass]
    public class FuncExtensionsTests
    {
        [TestMethod]
        public void IsTrue_Func_ShouldBe_True_Test()
        {
            Func<bool> func = () => { return 1 == 1; };

            var funcResult = func.IsTrue();

            Assert.IsNotNull(funcResult);
            Assert.IsTrue(funcResult);
        }

        [TestMethod]
        public void IsFalse_Func_ShouldBe_True_Test()
        {
            Func<bool> func = () => { return 1 == 0; };

            var funcResult = func.IsFalse();

            Assert.IsNotNull(funcResult);
            Assert.IsTrue(funcResult);
        }

        [TestMethod]
        public void IsTrue_AsyncFunc_ShouldBe_True_Test()
        {
            Func<Task<bool>> func = async () => { return await Task.FromResult(1 == 1); };

            var funcResult = func.IsTrue();

            Assert.IsNotNull(funcResult);
            Assert.IsTrue(funcResult);
        }

        [TestMethod]
        public void IsFalse_AsyncFunc_ShouldBe_True_Test()
        {
            Func<Task<bool>> func = async () => { return await Task.FromResult(1 == 0); };

            var funcResult = func.IsFalse();

            Assert.IsNotNull(funcResult);
            Assert.IsTrue(funcResult);
        }
    }
}