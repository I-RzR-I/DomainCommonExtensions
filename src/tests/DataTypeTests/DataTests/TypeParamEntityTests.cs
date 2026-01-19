// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2026-01-19 23:01
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-01-19 23:15
// ***********************************************************************
//  <copyright file="TypeParamEntityTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections.Generic;
using System.Linq;
using DataTypeTests.Models;
using DomainCommonExtensions.CommonExtensions.TypeParam;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class TypeParamEntityTests
    {
        [TestMethod]
        public void GetPropertyValue_StructType_ReturnsExpectedValues()
        {
            var entity = new TestEntity
            {
                IntProp = 10,
                NullableIntProp = 20
            };

            var result = entity.GetPropertyValue<int, TestEntity>(
                    nameof(TestEntity.IntProp),
                    nameof(TestEntity.NullableIntProp))
                .ToList();

            CollectionAssert.AreEquivalent(new List<int> { 10, 20 }, result);
        }

        [TestMethod]
        public void GetPropertyValue_StructType_IgnoresNullValues()
        {
            var entity = new TestEntity
            {
                IntProp = 5,
                NullableIntProp = null
            };

            var result = entity.GetPropertyValue<int, TestEntity>(
                    nameof(TestEntity.IntProp),
                    nameof(TestEntity.NullableIntProp))
                .ToList();

            CollectionAssert.AreEquivalent(new List<int> { 5 }, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetPropertyValue_StructType_SourceIsNull_Throws()
        {
            TestEntity entity = null;

            entity.GetPropertyValue<int, TestEntity>(nameof(TestEntity.IntProp)).ToList();
        }

        [TestMethod]
        public void GetPropertyStringValue_ReturnsStringValues()
        {
            var entity = new TestEntity
            {
                IntProp = 42,
                StringProp = "Hello"
            };

            var result = entity.GetPropertyStringValue(
                    nameof(TestEntity.IntProp),
                    nameof(TestEntity.StringProp))
                .ToList();

            CollectionAssert.AreEquivalent(
                new List<string> { "42", "Hello" },
                result);
        }

        [TestMethod]
        public void GetPropertyStringValue_IgnoresNullProperties()
        {
            var entity = new TestEntity
            {
                StringProp = null
            };

            var result = entity.GetPropertyStringValue(nameof(TestEntity.StringProp)).ToList();

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetPropertyStringValue_SourceIsNull_Throws()
        {
            TestEntity entity = null;

            entity.GetPropertyStringValue(nameof(TestEntity.StringProp)).ToList();
        }

        [TestMethod]
        public void GetPropertyValue_Object_ReturnsObjects()
        {
            var entity = new TestEntity
            {
                IntProp = 7,
                StringProp = "Test"
            };

            var result = entity.GetPropertyValue(
                    nameof(TestEntity.IntProp),
                    nameof(TestEntity.StringProp))
                .ToList();

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Contains(7));
            Assert.IsTrue(result.Contains("Test"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetPropertyValue_Object_SourceIsNull_Throws()
        {
            TestEntity entity = null;

            entity.GetPropertyValue(nameof(TestEntity.IntProp)).ToList();
        }

        [TestMethod]
        public void ChangePropertyValue_UpdatesMatchingProperties()
        {
            var source = new SourceEntity
            {
                Value1 = 100,
                Value2 = "Updated"
            };

            var target = new TargetEntity
            {
                Value1 = 1,
                Value2 = "Original"
            };

            var result = target.ChangePropertyValue(
                source,
                nameof(SourceEntity.Value1),
                nameof(SourceEntity.Value2));

            Assert.AreEqual(100, result.Value1);
            Assert.AreEqual("Updated", result.Value2);
        }

        [TestMethod]
        public void ChangePropertyValue_IgnoresMissingSourceProperties()
        {
            var source = new SourceEntity
            {
                Value1 = 50
            };

            var target = new TargetEntity
            {
                Value1 = 1,
                Value2 = "Keep"
            };

            var result = target.ChangePropertyValue(
                source,
                nameof(SourceEntity.Value1),
                "NonExistingProperty");

            Assert.AreEqual(50, result.Value1);
            Assert.AreEqual("Keep", result.Value2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ChangePropertyValue_SourceResultIsNull_Throws()
        {
            TargetEntity target = null;
            var source = new SourceEntity();

            target.ChangePropertyValue(source, nameof(SourceEntity.Value1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ChangePropertyValue_SourceDataIsNull_Throws()
        {
            var target = new TargetEntity();
            SourceEntity source = null;

            target.ChangePropertyValue(source, nameof(SourceEntity.Value1));
        }
    }
}