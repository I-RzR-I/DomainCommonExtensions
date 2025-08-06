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

#region U S A G E S

using DomainCommonExtensions.ArraysExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace DataTypeTests.DataTests.Array
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

        [TestMethod]
        public void AppendItem_Test()
        {
            var array = new string[] { "a", "b", "c" };

            var result = array.AppendItem("d");

            Assert.IsNotNull(result);
            Assert.AreEqual("a", result[0]);
            Assert.AreEqual("d", result[3]);
        }

        [TestMethod]
        public void AppendItem_Param_Test()
        {
            var array = new string[] { "a", "b", "c" };

            var result = array.AppendItem("d", "e");

            Assert.IsNotNull(result);
            Assert.AreEqual("a", result[0]);
            Assert.AreEqual("d", result[3]);
            Assert.AreEqual("e", result[4]);
        }

        [TestMethod]
        public void AppendIfNotExists_Test()
        {
            var array = new string[] { "a", "b", "c" };

            var result = array.AppendIfNotExists(new[] { "d", "e", "a" });

            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.Length);
            Assert.AreEqual("a", result[0]);
            Assert.AreEqual("d", result[3]);
            Assert.AreEqual("e", result[4]);
        }

        [TestMethod]
        public void RemoveItem_Test()
        {
            var array = new string[] { "a", "b", "c", "d" };

            var result = array.RemoveItem("c");
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Length);
            Assert.AreEqual(-1, result.IndexOf("c"));
        }

        [TestMethod]
        public void RemoveAtIdx_Test()
        {
            var array = new string[] { "a", "b", "c", "d" };

            var result = array.RemoveAtIdx(2);
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Length);
            Assert.AreEqual(-1, result.IndexOf("c"));
        }

        [TestMethod]
        public void NotNull_Null_Test()
        {
            int[] array = null;

            var array2 = array.NotNull();

            Assert.IsNull(array);
            Assert.IsNotNull(array2);
            Assert.AreEqual(0, array2.Length);
        }

        [TestMethod]
        public void NotNull_WithData_Test()
        {
            int[] array = new[] { 1, 2, 3 };

            var array2 = array.NotNull();

            Assert.IsNotNull(array);
            Assert.IsNotNull(array2);
            Assert.AreEqual(3, array.Length);
            Assert.AreEqual(3, array2.Length);
        }
    }
}