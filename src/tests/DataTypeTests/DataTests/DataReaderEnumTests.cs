using System;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Data.DataReader;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class DataReaderEnumTests
    {
        private const string MissingColumn = "missing";

        [TestMethod]
        public void ToEnum_IntegerValueReturnsMember_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(int), 1))
            {
                Assert.AreEqual(OrdinalTestEnum.Second, reader.ToEnum<OrdinalTestEnum>(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToNullEnum_IntegerValueReturnsMember_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(int), 2))
            {
                Assert.AreEqual((OrdinalTestEnum?)OrdinalTestEnum.Third, reader.ToNullEnum<OrdinalTestEnum>(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToEnum_ByteValueReturnsByteEnumMember_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(byte), (byte)7))
            {
                Assert.AreEqual(ByteBasedTestEnum.Seven, reader.ToEnum<ByteBasedTestEnum>(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToEnum_LongValueReturnsLongEnumMember_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(long), (long)int.MaxValue + 1))
            {
                Assert.AreEqual(LongBasedTestEnum.Large, reader.ToEnum<LongBasedTestEnum>(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToEnum_ShortValueReturnsIntEnumMember_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(short), (short)-5))
            {
                Assert.AreEqual(NegativeTestEnum.Neg, reader.ToEnum<NegativeTestEnum>(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToEnum_UndefinedIntegerReturnedAsIs_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(int), 99))
            {
                Assert.AreEqual((OrdinalTestEnum)99, reader.ToEnum<OrdinalTestEnum>(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToNullEnum_UndefinedNumericStringReturnedAsIs_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "99"))
            {
                Assert.AreEqual((OrdinalTestEnum?)(OrdinalTestEnum)99, reader.ToNullEnum<OrdinalTestEnum>(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToEnum_MemberNameIsCaseInsensitive_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "second"))
            {
                Assert.AreEqual(OrdinalTestEnum.Second, reader.ToEnum<OrdinalTestEnum>(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToNullEnum_MemberNameIsTrimmed_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), " Third "))
            {
                Assert.AreEqual((OrdinalTestEnum?)OrdinalTestEnum.Third, reader.ToNullEnum<OrdinalTestEnum>(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToEnum_NumericStringReturnsMember_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "2"))
            {
                Assert.AreEqual(OrdinalTestEnum.Third, reader.ToEnum<OrdinalTestEnum>(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToEnum_NegativeNumericStringReturnsMember_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "-5"))
            {
                Assert.AreEqual(NegativeTestEnum.Neg, reader.ToEnum<NegativeTestEnum>(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToEnum_UnknownNameThrowsFormatException_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "Unknown"))
            {
                Assert.ThrowsException<FormatException>(() => reader.ToEnum<OrdinalTestEnum>(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToNullEnum_UnknownNameThrowsFormatException_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "Unknown"))
            {
                Assert.ThrowsException<FormatException>(() => reader.ToNullEnum<OrdinalTestEnum>(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToEnum_IntegralDecimalReturnsMember_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(decimal), 2.0m))
            {
                Assert.AreEqual(OrdinalTestEnum.Third, reader.ToEnum<OrdinalTestEnum>(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToEnum_IntegralDoubleReturnsMember_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(double), 1.0d))
            {
                Assert.AreEqual(OrdinalTestEnum.Second, reader.ToEnum<OrdinalTestEnum>(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToEnum_FractionalDecimalThrowsFormatException_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(decimal), 1.5m))
            {
                Assert.ThrowsException<FormatException>(() => reader.ToEnum<OrdinalTestEnum>(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToNullEnum_FractionalDoubleThrowsFormatException_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(double), 1.5d))
            {
                Assert.ThrowsException<FormatException>(() => reader.ToNullEnum<OrdinalTestEnum>(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToEnum_FractionalFloatThrowsFormatException_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(float), 1.5f))
            {
                Assert.ThrowsException<FormatException>(() => reader.ToEnum<OrdinalTestEnum>(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToEnum_FractionalValueMessageNamesEnum_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(decimal), 1.5m))
            {
                var exception = Assert.ThrowsException<FormatException>(() => reader.ToEnum<OrdinalTestEnum>(DataReaderTestHelper.ColumnName));

                StringAssert.Contains(exception.Message, nameof(OrdinalTestEnum));
                Assert.IsNotNull(exception.InnerException);
            }
        }

        [TestMethod]
        public void ToNullEnum_OutOfRangeValueMessageNamesEnum_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(int), 300))
            {
                var exception = Assert.ThrowsException<OverflowException>(() => reader.ToNullEnum<ByteBasedTestEnum>(DataReaderTestHelper.ColumnName));

                StringAssert.Contains(exception.Message, nameof(ByteBasedTestEnum));
                Assert.IsNotNull(exception.InnerException);
            }
        }

        [TestMethod]
        public void ToEnum_IntegerOutOfByteRangeThrowsOverflowException_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(int), 300))
            {
                Assert.ThrowsException<OverflowException>(() => reader.ToEnum<ByteBasedTestEnum>(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToNullEnum_NegativeIntegerToUnsignedEnumThrowsOverflowException_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(int), -1))
            {
                Assert.ThrowsException<OverflowException>(() => reader.ToNullEnum<UIntBasedTestEnum>(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToEnum_NumericStringOutOfRangeThrowsOverflowException_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "300"))
            {
                Assert.ThrowsException<OverflowException>(() => reader.ToEnum<ByteBasedTestEnum>(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToEnum_GuidValueThrowsFormatException_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(Guid), new Guid("3f2504e0-4f89-11d3-9a0c-0305e82c3301")))
            {
                Assert.ThrowsException<FormatException>(() => reader.ToEnum<OrdinalTestEnum>(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToEnum_DbNullReturnsDefault_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(int), DBNull.Value))
            {
                Assert.AreEqual(default(OrdinalTestEnum), reader.ToEnum<OrdinalTestEnum>(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToNullEnum_DbNullReturnsNull_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(int), DBNull.Value))
            {
                Assert.IsNull(reader.ToNullEnum<OrdinalTestEnum>(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToEnum_WhitespaceStringReturnsDefault_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "  "))
            {
                Assert.AreEqual(default(NegativeTestEnum), reader.ToEnum<NegativeTestEnum>(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToNullEnum_EmptyStringReturnsNull_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), string.Empty))
            {
                Assert.IsNull(reader.ToNullEnum<OrdinalTestEnum>(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToEnum_MissingColumnPinArgumentException_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(int), 1))
            {
                Assert.ThrowsException<ArgumentException>(() => reader.ToEnum<OrdinalTestEnum>(MissingColumn));
            }
        }

        [TestMethod]
        public void ToNullEnum_MissingColumnPinArgumentException_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(int), 1))
            {
                Assert.ThrowsException<ArgumentException>(() => reader.ToNullEnum<OrdinalTestEnum>(MissingColumn));
            }
        }
    }
}
