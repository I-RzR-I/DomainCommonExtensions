using System;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Data.DataReader;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class DataReaderCharGuidTests
    {
        private static readonly Guid SampleGuid = new Guid("3f2504e0-4f89-11d3-9a0c-0305e82c3301");

        [TestMethod]
        public void ToChar_TypedCharPin_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(char), 'a'))
            {
                Assert.AreEqual('a', reader.ToChar(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToChar_SingleCharStringPin_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "a"))
            {
                Assert.AreEqual('a', reader.ToChar(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToChar_SpaceStringPinNullChar_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), " "))
            {
                Assert.AreEqual('\0', reader.ToChar(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToNullChar_SpaceStringPinNull_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), " "))
            {
                Assert.IsNull(reader.ToNullChar(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToChar_TwoCharStringThrowsFormatException_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "ab"))
            {
                Assert.ThrowsException<FormatException>(() => reader.ToChar(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToChar_DbNullPinNullChar_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(char), DBNull.Value))
            {
                Assert.AreEqual('\0', reader.ToChar(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToGuid_TypedGuidPin_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(Guid), SampleGuid))
            {
                Assert.AreEqual(SampleGuid, reader.ToGuid(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToNullGuid_StringGuidPin_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), SampleGuid.ToString()))
            {
                Assert.AreEqual((Guid?)SampleGuid, reader.ToNullGuid(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToGuid_DbNullPinEmpty_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(Guid), DBNull.Value))
            {
                Assert.AreEqual(Guid.Empty, reader.ToGuid(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToGuid_MalformedStringThrowsFormatException_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "not-a-guid"))
            {
                Assert.ThrowsException<FormatException>(() => reader.ToGuid(DataReaderTestHelper.ColumnName));
            }
        }
    }
}
