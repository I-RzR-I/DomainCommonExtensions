using System;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Data.DataReader;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class DataReaderDateTimeOffsetTests
    {
        private const string MissingColumn = "missing";

        [TestMethod]
        public void ToDateTimeOffset_TypedValueReturnedAsIs_Test()
        {
            var value = new DateTimeOffset(2024, 1, 15, 12, 30, 45, 123, TimeSpan.FromHours(2));
            using (var reader = DataReaderTestHelper.SingleValue(typeof(DateTimeOffset), value))
            {
                var result = reader.ToDateTimeOffset(DataReaderTestHelper.ColumnName);

                Assert.AreEqual(value.DateTime, result.DateTime);
                Assert.AreEqual(value.Offset, result.Offset);
            }
        }

        [TestMethod]
        public void ToNullDateTimeOffset_TypedValueReturnedAsIs_Test()
        {
            var value = new DateTimeOffset(2024, 1, 15, 12, 30, 45, 123, TimeSpan.FromHours(-5));
            using (var reader = DataReaderTestHelper.SingleValue(typeof(DateTimeOffset), value))
            {
                var result = reader.ToNullDateTimeOffset(DataReaderTestHelper.ColumnName);

                Assert.IsTrue(result.HasValue);
                Assert.AreEqual(value.DateTime, result.Value.DateTime);
                Assert.AreEqual(value.Offset, result.Value.Offset);
            }
        }

        [TestMethod]
        public void ToDateTimeOffset_UtcDateTimeGetsZeroOffset_Test()
        {
            var value = new DateTime(2024, 1, 15, 12, 30, 45, 123, DateTimeKind.Utc);
            using (var reader = DataReaderTestHelper.SingleValue(typeof(object), value))
            {
                var result = reader.ToDateTimeOffset(DataReaderTestHelper.ColumnName);

                Assert.AreEqual(new DateTime(2024, 1, 15, 12, 30, 45, 123), result.DateTime);
                Assert.AreEqual(TimeSpan.Zero, result.Offset);
            }
        }

        [TestMethod]
        public void ToDateTimeOffset_UnspecifiedDateTimeGetsZeroOffset_Test()
        {
            var value = new DateTime(2024, 1, 15, 12, 30, 45, 123, DateTimeKind.Unspecified);
            using (var reader = DataReaderTestHelper.SingleValue(typeof(object), value))
            {
                var result = reader.ToDateTimeOffset(DataReaderTestHelper.ColumnName);

                Assert.AreEqual(value, result.DateTime);
                Assert.AreEqual(TimeSpan.Zero, result.Offset);
            }
        }

        [TestMethod]
        public void ToNullDateTimeOffset_UnspecifiedDateTimeGetsZeroOffset_Test()
        {
            var value = new DateTime(2024, 7, 15, 12, 30, 45, DateTimeKind.Unspecified);
            using (var reader = DataReaderTestHelper.SingleValue(typeof(object), value))
            {
                var result = reader.ToNullDateTimeOffset(DataReaderTestHelper.ColumnName);

                Assert.IsTrue(result.HasValue);
                Assert.AreEqual(value, result.Value.DateTime);
                Assert.AreEqual(TimeSpan.Zero, result.Value.Offset);
            }
        }

        [TestMethod]
        public void ToDateTimeOffset_LocalDateTimeGetsLocalOffset_Test()
        {
            var value = new DateTime(2024, 1, 15, 12, 30, 45, DateTimeKind.Local);
            var expected = new DateTimeOffset(value);
            using (var reader = DataReaderTestHelper.SingleValue(typeof(object), value))
            {
                var result = reader.ToDateTimeOffset(DataReaderTestHelper.ColumnName);

                Assert.AreEqual(expected.DateTime, result.DateTime);
                Assert.AreEqual(expected.Offset, result.Offset);
            }
        }

        [TestMethod]
        public void ToDateTimeOffset_RoundTripStringKeepsOffset_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "2024-01-15T12:30:45.1230000+02:00"))
            {
                var result = reader.ToDateTimeOffset(DataReaderTestHelper.ColumnName);

                Assert.AreEqual(new DateTime(2024, 1, 15, 12, 30, 45, 123), result.DateTime);
                Assert.AreEqual(TimeSpan.FromHours(2), result.Offset);
            }
        }

        [TestMethod]
        public void ToDateTimeOffset_RoundTripUtcStringGetsZeroOffset_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "2024-01-15T12:30:45.0000000Z"))
            {
                var result = reader.ToDateTimeOffset(DataReaderTestHelper.ColumnName);

                Assert.AreEqual(new DateTime(2024, 1, 15, 12, 30, 45), result.DateTime);
                Assert.AreEqual(TimeSpan.Zero, result.Offset);
            }
        }

        [TestMethod]
        public void ToDateTimeOffset_StringWithoutOffsetReadAsUtc_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "2024-01-15 12:30:45"))
            {
                var result = reader.ToDateTimeOffset(DataReaderTestHelper.ColumnName);

                Assert.AreEqual(new DateTime(2024, 1, 15, 12, 30, 45), result.DateTime);
                Assert.AreEqual(TimeSpan.Zero, result.Offset);
            }
        }

        [TestMethod]
        public void ToNullDateTimeOffset_InvariantStringUnderGermanCultureParsed_Test()
        {
            using (new CultureScopeTestHelper("de-DE"))
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "01/15/2024 12:30:45"))
            {
                var result = reader.ToNullDateTimeOffset(DataReaderTestHelper.ColumnName);

                Assert.IsTrue(result.HasValue);
                Assert.AreEqual(new DateTime(2024, 1, 15, 12, 30, 45), result.Value.DateTime);
                Assert.AreEqual(TimeSpan.Zero, result.Value.Offset);
            }
        }

        [TestMethod]
        public void ToDateTimeOffset_DbNullReturnsMinValue_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(DateTimeOffset), DBNull.Value))
            {
                Assert.AreEqual(DateTimeOffset.MinValue, reader.ToDateTimeOffset(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToNullDateTimeOffset_DbNullReturnsNull_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(DateTimeOffset), DBNull.Value))
            {
                Assert.IsNull(reader.ToNullDateTimeOffset(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToDateTimeOffset_WhitespaceStringReturnsMinValue_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "  "))
            {
                Assert.AreEqual(DateTimeOffset.MinValue, reader.ToDateTimeOffset(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToNullDateTimeOffset_EmptyStringReturnsNull_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), string.Empty))
            {
                Assert.IsNull(reader.ToNullDateTimeOffset(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToDateTimeOffset_MalformedStringThrowsFormatException_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "not a date"))
            {
                Assert.ThrowsException<FormatException>(() => reader.ToDateTimeOffset(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToDateTimeOffset_IntegerValueThrowsFormatException_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(int), 5))
            {
                Assert.ThrowsException<FormatException>(() => reader.ToDateTimeOffset(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToNullDateTimeOffset_IntegerValueThrowsFormatException_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(int), 5))
            {
                Assert.ThrowsException<FormatException>(() => reader.ToNullDateTimeOffset(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToDateTimeOffset_MissingColumnPinArgumentException_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(DateTimeOffset), DateTimeOffset.MinValue))
            {
                Assert.ThrowsException<ArgumentException>(() => reader.ToDateTimeOffset(MissingColumn));
            }
        }

        [TestMethod]
        public void ToNullDateTimeOffset_MissingColumnPinArgumentException_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(DateTimeOffset), DateTimeOffset.MinValue))
            {
                Assert.ThrowsException<ArgumentException>(() => reader.ToNullDateTimeOffset(MissingColumn));
            }
        }
    }
}
