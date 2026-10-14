using System;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Data.DataReader;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class DataReaderStringTests
    {
        private const string MissingColumn = "missing";

        [TestMethod]
        public void ToNullString_DbNullReturnsNull_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), DBNull.Value))
            {
                Assert.IsNull(reader.ToNullString(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToStringOrEmpty_DbNullReturnsEmpty_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), DBNull.Value))
            {
                Assert.AreEqual(string.Empty, reader.ToStringOrEmpty(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToNullString_TypedStringReturnedAsIs_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "abc"))
            {
                Assert.AreEqual("abc", reader.ToNullString(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToNullString_EmptyStringReturnsEmpty_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), string.Empty))
            {
                Assert.AreEqual(string.Empty, reader.ToNullString(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToStringOrEmpty_EmptyStringReturnsEmpty_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), string.Empty))
            {
                Assert.AreEqual(string.Empty, reader.ToStringOrEmpty(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToNullString_WhitespaceStringReturnedAsIs_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "  "))
            {
                Assert.AreEqual("  ", reader.ToNullString(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToStringOrEmpty_WhitespaceStringReturnedAsIs_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "  "))
            {
                Assert.AreEqual("  ", reader.ToStringOrEmpty(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToStringOrEmpty_TypedStringReturnedAsIs_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "abc"))
            {
                Assert.AreEqual("abc", reader.ToStringOrEmpty(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToNullString_NegativeIntegerUnderSwedishCultureUsesInvariant_Test()
        {
            using (new CultureScopeTestHelper("sv-SE"))
            using (var reader = DataReaderTestHelper.SingleValue(typeof(int), -1234))
            {
                Assert.AreEqual("-1234", reader.ToNullString(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToNullString_DecimalUnderGermanCultureUsesInvariant_Test()
        {
            using (new CultureScopeTestHelper("de-DE"))
            using (var reader = DataReaderTestHelper.SingleValue(typeof(decimal), 1.5m))
            {
                Assert.AreEqual("1.5", reader.ToNullString(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToStringOrEmpty_DoubleUnderGermanCultureUsesInvariant_Test()
        {
            using (new CultureScopeTestHelper("de-DE"))
            using (var reader = DataReaderTestHelper.SingleValue(typeof(double), 1.5d))
            {
                Assert.AreEqual("1.5", reader.ToStringOrEmpty(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToNullString_GuidUsesInvariantDefaultFormat_Test()
        {
            var value = new Guid("3f2504e0-4f89-11d3-9a0c-0305e82c3301");
            using (var reader = DataReaderTestHelper.SingleValue(typeof(Guid), value))
            {
                Assert.AreEqual("3f2504e0-4f89-11d3-9a0c-0305e82c3301", reader.ToNullString(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToNullString_UtcDateTimeUsesRoundTripFormat_Test()
        {
            var value = new DateTime(2024, 1, 15, 12, 30, 45, 123, DateTimeKind.Utc);
            using (var reader = DataReaderTestHelper.SingleValue(typeof(object), value))
            {
                Assert.AreEqual("2024-01-15T12:30:45.1230000Z", reader.ToNullString(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToNullString_UnspecifiedDateTimeUnderGermanCultureUsesRoundTripFormat_Test()
        {
            var value = new DateTime(2024, 1, 15, 12, 30, 45, DateTimeKind.Unspecified);
            using (new CultureScopeTestHelper("de-DE"))
            using (var reader = DataReaderTestHelper.SingleValue(typeof(object), value))
            {
                Assert.AreEqual("2024-01-15T12:30:45.0000000", reader.ToNullString(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToNullString_DateTimeOffsetUsesRoundTripFormat_Test()
        {
            var value = new DateTimeOffset(2024, 1, 15, 12, 30, 0, TimeSpan.FromHours(2));
            using (var reader = DataReaderTestHelper.SingleValue(typeof(object), value))
            {
                Assert.AreEqual("2024-01-15T12:30:00.0000000+02:00", reader.ToNullString(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToNullString_ByteArrayThrowsInvalidCastException_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(byte[]), new byte[] { 1, 2 }))
            {
                Assert.ThrowsException<InvalidCastException>(() => reader.ToNullString(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToStringOrEmpty_ByteArrayThrowsInvalidCastException_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(byte[]), new byte[] { 1, 2 }))
            {
                Assert.ThrowsException<InvalidCastException>(() => reader.ToStringOrEmpty(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToNullString_MissingColumnPinArgumentException_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "abc"))
            {
                Assert.ThrowsException<ArgumentException>(() => reader.ToNullString(MissingColumn));
            }
        }

        [TestMethod]
        public void ToStringOrEmpty_MissingColumnPinArgumentException_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "abc"))
            {
                Assert.ThrowsException<ArgumentException>(() => reader.ToStringOrEmpty(MissingColumn));
            }
        }
    }
}
