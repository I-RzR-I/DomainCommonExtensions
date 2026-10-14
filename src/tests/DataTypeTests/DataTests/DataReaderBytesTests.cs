using System;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Data.DataReader;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class DataReaderBytesTests
    {
        private const string MissingColumn = "missing";

        [TestMethod]
        public void ToBytes_ByteArrayReturnedWithoutCopy_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(byte[]), new byte[] { 1, 2, 3 }))
            {
                var expected = reader[DataReaderTestHelper.ColumnName];

                var result = reader.ToBytes(DataReaderTestHelper.ColumnName);

                Assert.AreSame(expected, result);
                CollectionAssert.AreEqual(new byte[] { 1, 2, 3 }, result);
            }
        }

        [TestMethod]
        public void ToBytes_EmptyArrayReturnsEmptyArray_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(byte[]), new byte[0]))
            {
                var result = reader.ToBytes(DataReaderTestHelper.ColumnName);

                Assert.IsNotNull(result);
                Assert.AreEqual(0, result.Length);
            }
        }

        [TestMethod]
        public void ToBytes_DbNullReturnsNull_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(byte[]), DBNull.Value))
            {
                Assert.IsNull(reader.ToBytes(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToBytes_EmptyStringReturnsNull_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), string.Empty))
            {
                Assert.IsNull(reader.ToBytes(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToBytes_WhitespaceStringReturnsNull_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "  "))
            {
                Assert.IsNull(reader.ToBytes(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToBytes_TextValueThrowsInvalidCastException_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "ab"))
            {
                Assert.ThrowsException<InvalidCastException>(() => reader.ToBytes(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToBytes_IntegerValueThrowsInvalidCastException_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(int), 5))
            {
                Assert.ThrowsException<InvalidCastException>(() => reader.ToBytes(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToBytes_GuidValueThrowsInvalidCastException_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(Guid), new Guid("3f2504e0-4f89-11d3-9a0c-0305e82c3301")))
            {
                Assert.ThrowsException<InvalidCastException>(() => reader.ToBytes(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToBytes_MissingColumnPinArgumentException_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(byte[]), new byte[] { 1 }))
            {
                Assert.ThrowsException<ArgumentException>(() => reader.ToBytes(MissingColumn));
            }
        }
    }
}
