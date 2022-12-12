// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2022-12-08 09:25
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-12-08 09:26
// ***********************************************************************
//  <copyright file="ArrayTests.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using DomainCommonExtensions.ArraysExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataTypeTests
{
    [TestClass]
    public class ArrayTests
    {
        [TestMethod]
        public void IndexOfEmptyOrNullInputTest()
        {
            var input = new string[1];
            var index = input.IndexOf("test");

            Assert.IsNotNull(index);
            Assert.AreEqual(-1, index);
        }

        [TestMethod]
        public void IndexOfTest()
        {
            var input = new string[] { "home", "phone", "pc", "", null, "", "" };
            
            Assert.AreEqual(-1, input.IndexOf("test"));
            Assert.AreEqual(2, input.IndexOf("pc"));
            Assert.AreEqual(3, input.IndexOf(""));
            Assert.AreEqual(4, input.IndexOf(null));
        }
    }
}