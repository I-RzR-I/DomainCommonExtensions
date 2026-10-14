using System;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Data.DataReader;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class DataReaderBoolTests
    {
        [TestMethod]
        public void ToBool_IntOneIsTrue_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(object), 1))
            {
                Assert.IsTrue(reader.ToBool(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToBool_IntZeroIsFalse_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(object), 0))
            {
                Assert.IsFalse(reader.ToBool(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToBool_Int64OneIsTrue_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(object), 1L))
            {
                Assert.IsTrue(reader.ToBool(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToNullBool_Int64ZeroIsFalse_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(object), 0L))
            {
                Assert.AreEqual((bool?)false, reader.ToNullBool(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToBool_DecimalNonZeroIsTrue_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(object), 2m))
            {
                Assert.IsTrue(reader.ToBool(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToBool_StringOneIsTrue_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "1"))
            {
                Assert.IsTrue(reader.ToBool(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToBool_StringZeroIsFalse_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "0"))
            {
                Assert.IsFalse(reader.ToBool(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToNullBool_PaddedStringOneIsTrue_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), " 1 "))
            {
                Assert.AreEqual((bool?)true, reader.ToNullBool(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToBool_StringTruePin_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "true"))
            {
                Assert.IsTrue(reader.ToBool(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToBool_PaddedUpperCaseTruePin_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), " TRUE "))
            {
                Assert.IsTrue(reader.ToBool(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToBool_TypedBoolPin_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(bool), true))
            {
                Assert.IsTrue(reader.ToBool(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToBool_MalformedStringThrowsFormatException_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "abc"))
            {
                Assert.ThrowsException<FormatException>(() => reader.ToBool(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToBool_DbNullPinFalse_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(bool), DBNull.Value))
            {
                Assert.IsFalse(reader.ToBool(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToNullBool_WhitespacePinNull_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "  "))
            {
                Assert.IsNull(reader.ToNullBool(DataReaderTestHelper.ColumnName));
            }
        }
    }
}
