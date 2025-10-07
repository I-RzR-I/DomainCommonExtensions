// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2025-10-07 16:10
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-10-07 16:35
// ***********************************************************************
//  <copyright file="DictionaryTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Collections.Generic;
using DomainCommonExtensions.ArraysExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace DataTypeTests.DataTests.Array
{
    [TestClass]
    public class DictionaryTests
    {
        [TestMethod]
        public void AddIsNotExist_Test()
        {
            var dict = new Dictionary<string, object>()
            {
                { "Id", 1 },
                {"Code", "Temp_CDC"}
            };

            dict.AddIfNotExist("Name", "Test1");

            var kvResult = dict["Name"];
            
            Assert.IsNotNull(kvResult);
            Assert.AreEqual("Test1", kvResult.ToString());
        }

        [TestMethod]
        public void AddIsNotExist_WithExistKey_Test()
        {
            var dict = new Dictionary<string, object>()
            {
                { "Id", 1 },
                {"Name", "TestName"},
                {"Code", "Temp_CDC"}
            };

            dict.AddIfNotExist("Name", "Test1");

            var kvResult = dict["Name"];
            
            Assert.IsNotNull(kvResult);
            Assert.AreEqual("TestName", kvResult.ToString());
        }

        [TestMethod]
        public void AddIsNotExist_KV_Test()
        {
            var dict = new Dictionary<string, object>()
            {
                { "Id", 1 },
                {"Code", "Temp_CDC"}
            };

            dict.AddIfNotExist(new KeyValuePair<string, object>("Name", "Test1"));

            var kvResult = dict["Name"];
            
            Assert.IsNotNull(kvResult);
            Assert.AreEqual("Test1", kvResult.ToString());
        }

        [TestMethod]
        public void AddIsNotExist_KV_WithExistKey_Test()
        {
            var dict = new Dictionary<string, object>()
            {
                { "Id", 1 },
                {"Name", "TestName"},
                {"Code", "Temp_CDC"}
            };

            dict.AddIfNotExist(new KeyValuePair<string, object>("Name", "Test1"));

            var kvResult = dict["Name"];
            
            Assert.IsNotNull(kvResult);
            Assert.AreEqual("TestName", kvResult.ToString());
        }

        [TestMethod]
        public void IsNullOrEmpty_Null_Test()
        {
            Dictionary<string, object> dict = null;

            Assert.IsNull(dict);
            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.IsTrue(dict.IsNullOrEmpty());
            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.IsTrue(dict.IsNullOrEmptyEnumerable());
        }

        [TestMethod]
        public void IsNullOrEmpty_Empty_Test()
        {
            var dict = new Dictionary<string, object>();

            Assert.IsNotNull(dict);
            Assert.IsTrue(dict.IsNullOrEmpty());
            Assert.IsTrue(dict.IsNullOrEmptyEnumerable());
        }
    }
}