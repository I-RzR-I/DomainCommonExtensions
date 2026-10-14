using System;
using System.Collections.Generic;
using System.Linq;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Reflection.TypeParam;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class TypeParamEntityConversionTests
    {
        [TestMethod]
        public void GetPropertyValue_IntAsLong_Test()
        {
            var model = new PropertyConversionTestModel();

            var result = model.GetPropertyValue<long, PropertyConversionTestModel>("A").ToList();

            CollectionAssert.AreEqual(new List<long> { 1L }, result);
        }

        [TestMethod]
        public void GetPropertyValue_IntAsDouble_Test()
        {
            var model = new PropertyConversionTestModel();

            var result = model.GetPropertyValue<double, PropertyConversionTestModel>("A", "B").ToList();

            CollectionAssert.AreEqual(new List<double> { 1.0, 2.0 }, result);
        }

        [TestMethod]
        public void GetPropertyValue_RequestedOrder_Test()
        {
            var model = new PropertyConversionTestModel();

            var result = model.GetPropertyValue<int, PropertyConversionTestModel>("B", "A").ToList();

            CollectionAssert.AreEqual(new List<int> { 2, 1 }, result);
        }

        [TestMethod]
        public void GetPropertyStringValue_RequestedOrder_Test()
        {
            var model = new PropertyConversionTestModel();

            var result = model.GetPropertyStringValue("S", "A").ToList();

            CollectionAssert.AreEqual(new List<string> { "s", "1" }, result);
        }

        [TestMethod]
        public void GetPropertyValue_ObjectRequestedOrder_Test()
        {
            var model = new PropertyConversionTestModel();

            var result = model.GetPropertyValue("B", "A").ToList();

            CollectionAssert.AreEqual(new List<object> { 2, 1 }, result);
        }

        [TestMethod]
        public void GetPropertyValue_DuplicateAndMissingNames_Test()
        {
            var model = new PropertyConversionTestModel();

            var result = model.GetPropertyValue<int, PropertyConversionTestModel>("A", "A", "Missing").ToList();

            CollectionAssert.AreEqual(new List<int> { 1 }, result);
        }

        [TestMethod]
        public void GetPropertyValue_NullValueSkipped_Test()
        {
            var model = new PropertyConversionTestModel { N = null };

            var result = model.GetPropertyValue<int, PropertyConversionTestModel>("A", "N").ToList();

            CollectionAssert.AreEqual(new List<int> { 1 }, result);
        }

        [TestMethod]
        public void GetPropertyValue_EnumAsInt_Test()
        {
            var model = new PropertyConversionTestModel();

            var result = model.GetPropertyValue<int, PropertyConversionTestModel>("D").ToList();

            CollectionAssert.AreEqual(new List<int> { 1 }, result);
        }

        [TestMethod]
        public void GetPropertyValue_IntAsEnum_Test()
        {
            var model = new PropertyConversionTestModel();

            var result = model.GetPropertyValue<DayOfWeek, PropertyConversionTestModel>("A").ToList();

            CollectionAssert.AreEqual(new List<DayOfWeek> { DayOfWeek.Monday }, result);
        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public void GetPropertyValue_NarrowingOverflow_Test()
        {
            var model = new PropertyConversionTestModel { C = long.MaxValue };

            model.GetPropertyValue<int, PropertyConversionTestModel>("C").ToList();
        }

        [TestMethod]
        public void GetPropertyValue_NullFieldNames_Test()
        {
            var model = new PropertyConversionTestModel();

            var result = model.GetPropertyValue<int, PropertyConversionTestModel>((string[])null).ToList();

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void GetPropertyStringValue_NullFieldNames_Test()
        {
            var model = new PropertyConversionTestModel();

            var result = model.GetPropertyStringValue((string[])null).ToList();

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void GetPropertyValue_ObjectNullFieldNames_Test()
        {
            var model = new PropertyConversionTestModel();

            var result = model.GetPropertyValue((string[])null).ToList();

            Assert.AreEqual(0, result.Count);
        }
    }
}
