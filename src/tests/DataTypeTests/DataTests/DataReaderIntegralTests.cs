using System;
using System.Globalization;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Data.DataReader;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class DataReaderIntegralTests
    {
        [TestMethod]
        public void ToInt32_IntegralDecimalReturnsValue_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(decimal), 5.00m))
            {
                Assert.AreEqual(5, reader.ToInt32(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToNullInt64_IntegralDecimalReturnsValue_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(object), 5.00m))
            {
                Assert.AreEqual((long?)5, reader.ToNullInt64(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToByte_IntegralDecimalReturnsValue_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(object), 5.00m))
            {
                Assert.AreEqual((byte)5, reader.ToByte(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToInt32_FractionalDecimalThrowsFormatException_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(decimal), 5.5m))
            {
                Assert.ThrowsException<FormatException>(() => reader.ToInt32(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToInt32_FractionalDoubleThrowsFormatException_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(double), 2.5d))
            {
                Assert.ThrowsException<FormatException>(() => reader.ToInt32(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToInt32_Int64AboveRangeThrowsOverflowException_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(long), (long)int.MaxValue + 1))
            {
                Assert.ThrowsException<OverflowException>(() => reader.ToInt32(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToUInt32_NegativeIntThrowsOverflowException_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(int), -1))
            {
                Assert.ThrowsException<OverflowException>(() => reader.ToUInt32(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToByte_IntAboveRangeThrowsOverflowException_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(int), 256))
            {
                Assert.ThrowsException<OverflowException>(() => reader.ToByte(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToInt32_TypedBoolThrowsFormatException_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(bool), true))
            {
                Assert.ThrowsException<FormatException>(() => reader.ToInt32(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToInt64_TypedMaxValuePin_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(long), long.MaxValue))
            {
                Assert.AreEqual(long.MaxValue, reader.ToInt64(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToUInt64_TypedMaxValuePin_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(ulong), ulong.MaxValue))
            {
                Assert.AreEqual(ulong.MaxValue, reader.ToUInt64(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToInt16_InvariantStringPin_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "-123"))
            {
                Assert.AreEqual((short)-123, reader.ToInt16(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToInt32_DbNullPinZero_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(int), DBNull.Value))
            {
                Assert.AreEqual(0, reader.ToInt32(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToNullInt32_DbNullPinNull_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(int), DBNull.Value))
            {
                Assert.IsNull(reader.ToNullInt32(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToInt32_EmptyStringPinZero_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), ""))
            {
                Assert.AreEqual(0, reader.ToInt32(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToInt32_WhitespaceStringPinZero_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "  "))
            {
                Assert.AreEqual(0, reader.ToInt32(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToNullInt32_WhitespaceStringPinNull_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "  "))
            {
                Assert.IsNull(reader.ToNullInt32(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToSByte_DbNullPinMinValue_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(sbyte), DBNull.Value))
            {
                Assert.AreEqual((sbyte)-128, reader.ToSByte(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToNullSByte_DbNullPinNull_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(sbyte), DBNull.Value))
            {
                Assert.IsNull(reader.ToNullSByte(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToSByte_TypedNegativePin_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(sbyte), (sbyte)-5))
            {
                Assert.AreEqual((sbyte)-5, reader.ToSByte(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToInt32_InfinityDoubleThrowsFormatPin_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(double), double.PositiveInfinity))
            {
                Assert.ThrowsException<FormatException>(() => reader.ToInt32(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToBool_NaNDoubleThrowsFormatPin_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(double), double.NaN))
            {
                Assert.ThrowsException<FormatException>(() => reader.ToBool(DataReaderTestHelper.ColumnName));
            }
        }
    }
}
