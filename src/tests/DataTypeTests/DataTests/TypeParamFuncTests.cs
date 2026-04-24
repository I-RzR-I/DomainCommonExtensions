// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2025-07-15 12:18
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-07-15 12:19
// ***********************************************************************
//  <copyright file="TypeParamFuncTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Async;
using RzR.Extensions.Domain.Cryptography;
using RzR.Extensions.Domain.Data;
using RzR.Extensions.Domain.Diagnostics;
using RzR.Extensions.Domain.IO;
using RzR.Extensions.Domain.Linq;
using RzR.Extensions.Domain.Primitives;
using RzR.Extensions.Domain.Reflection.TypeParam;
#endregion

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class TypeParamFuncTests
    {
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