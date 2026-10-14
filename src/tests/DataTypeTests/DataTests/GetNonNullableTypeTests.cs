using System;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Reflection;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class GetNonNullableTypeTests
    {
        [TestMethod]
        public void GetNonNullableType_NullableInt_Test()
        {
            var result = typeof(int?).GetNonNullableType();

            Assert.AreEqual(typeof(int), result);
        }

        [TestMethod]
        public void GetNonNullableType_NullableDateTime_Test()
        {
            var result = typeof(DateTime?).GetNonNullableType();

            Assert.AreEqual(typeof(DateTime), result);
        }

        [TestMethod]
        public void GetNonNullableType_StringType_Test()
        {
            var result = typeof(string).GetNonNullableType();

            Assert.AreEqual(typeof(string), result);
        }

        [TestMethod]
        public void GetNonNullableType_IntType_Test()
        {
            var result = typeof(int).GetNonNullableType();

            Assert.AreEqual(typeof(int), result);
        }

        [TestMethod]
        public void GetNonNullableType_NullableEnum_Test()
        {
            var result = typeof(DayOfWeek?).GetNonNullableType();

            Assert.AreEqual(typeof(DayOfWeek), result);
        }

        [TestMethod]
        public void GetNonNullableType_NullableUserStruct_Test()
        {
            var result = typeof(TestPoint?).GetNonNullableType();

            Assert.AreEqual(typeof(TestPoint), result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetNonNullableType_NullType_Test()
        {
            Type type = null;

            type.GetNonNullableType();
        }

        [TestMethod]
        public void GetNonNullableType_OpenNullable_Test()
        {
            var result = typeof(Nullable<>).GetNonNullableType();

            Assert.AreEqual(typeof(Nullable<>), result);
        }
    }
}
