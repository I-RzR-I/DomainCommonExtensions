// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2022-08-16 09:29
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-16 11:10
// ***********************************************************************
//  <copyright file="GuidTests.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using DomainCommonExtensions.DataTypeExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace DataTypeTests
{
    [TestClass]
    public class GuidTests
    {
        private const string NonGuid = "4270cf84-c109-4e27-8681-b002e1c152";
        private const string GuidStr = "4270cf84-c109-4e27-8681-b002e1c1524a";
        private const string GuidN = "4270cf84c1094e278681b002e1c1524a";

        [TestMethod]
        public void IsGuidTest()
        {
            var notGuid = NonGuid.IsGuid();
            var isGuid = GuidStr.IsGuid();
            var isGuidN = GuidN.IsGuid();

            Assert.IsFalse(notGuid);
            Assert.IsTrue(isGuid);
            Assert.IsTrue(isGuidN);
        }

        [TestMethod]
        public void ToGuidTest()
        {
            var g1 = NonGuid.ToGuid();
            var g2 = GuidStr.ToGuid();
            var g3 = GuidN.ToGuid();

            Assert.AreEqual(g1, Guid.Empty);
            Assert.AreNotEqual(g2, Guid.Empty);
            Assert.AreNotEqual(g3, Guid.Empty);
        }

        [TestMethod]
        public void FromDoubleQuotesWithBackSlashesToGuidTest()
        {
            var str = "\"4270cf84-c109-4e27-8681-b002e1c1524a";

            var result = str.FromDoubleQuotesWithBackSlashesToGuid();

            Assert.IsNotNull(result);
            Assert.AreNotEqual(result, Guid.Empty);
        }

        [TestMethod]
        public void TryToGuidTest()
        {
            var g1 = NonGuid.TryToGuid();
            var g2 = GuidStr.TryToGuid();
            var g3 = GuidN.TryToGuid();

            Assert.IsNull(g1);
            Assert.IsNotNull(g2);
            Assert.IsNotNull(g3);
        }

        [TestMethod]
        public void ToInt32Test()
        {
            var g1 = GuidN.ToGuid();
            var res = g1.ToInt32();
            var res1 = Guid.Empty.ToInt32();

            Assert.IsNotNull(res);
            Assert.IsNotNull(res1);
            Assert.AreEqual(1114689412, res);
            Assert.AreEqual(0, res1);
        }

        [TestMethod]
        public void ToLongTest()
        {
            var g1 = GuidN.ToGuid();
            var res = g1.ToLong();
            var res1 = Guid.Empty.ToLong();

            Assert.IsNotNull(res);
            Assert.IsNotNull(res1);
            Assert.AreEqual(5631682104563650436, res);
            Assert.AreEqual(0, res1);
        }
    }
}