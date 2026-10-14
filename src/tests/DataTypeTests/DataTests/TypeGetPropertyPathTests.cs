using System;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Reflection;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class TypeGetPropertyPathTests
    {
        [TestMethod]
        public void GetPropertyPath_NullType_Test()
        {
            Type type = null;

            var exception = Assert.ThrowsException<ArgumentNullException>(() => type.GetPropertyPath("Child.Name"));

            Assert.AreEqual("type", exception.ParamName);
        }

        [TestMethod]
        public void GetPropertyPath_NullPath_Test()
        {
            var type = typeof(PropertyPathRootTestHelper);

            var exception = Assert.ThrowsException<ArgumentNullException>(() => type.GetPropertyPath(null));

            Assert.AreEqual("propertyPath", exception.ParamName);
        }

        [TestMethod]
        public void GetPropertyPath_MissingNestedSegment_Test()
        {
            var type = typeof(PropertyPathRootTestHelper);

            var exception = Assert.ThrowsException<ArgumentException>(() => type.GetPropertyPath("Child.Missing"));

            StringAssert.Contains(exception.Message, "Missing");
        }

        [TestMethod]
        public void GetPropertyPath_MissingRootSegment_Test()
        {
            var type = typeof(PropertyPathRootTestHelper);

            var exception = Assert.ThrowsException<ArgumentException>(() => type.GetPropertyPath("Missing.Name"));

            StringAssert.Contains(exception.Message, "Missing");
        }

        [TestMethod]
        public void GetPropertyPath_ValidNestedPath_Test()
        {
            var type = typeof(PropertyPathRootTestHelper);

            var result = type.GetPropertyPath("Child.Name");

            Assert.AreEqual(2, result.Length);
            Assert.AreEqual("Child", result[0].Name);
            Assert.AreEqual(typeof(PropertyPathChildTestHelper), result[0].PropertyType);
            Assert.AreEqual("Name", result[1].Name);
            Assert.AreEqual(typeof(string), result[1].PropertyType);
        }

        [TestMethod]
        public void GetPropertyPath_SingleSegment_Test()
        {
            var type = typeof(PropertyPathRootTestHelper);

            var result = type.GetPropertyPath("Id");

            Assert.AreEqual(1, result.Length);
            Assert.AreEqual("Id", result[0].Name);
            Assert.AreEqual(typeof(int), result[0].PropertyType);
        }
    }
}
