using System;
using System.Globalization;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Data.DataReader;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class DataReaderDateTimeTests
    {
        private static readonly DateTime UtcWithMilliseconds = new DateTime(2024, 1, 2, 3, 4, 5, 678, DateTimeKind.Utc);

        [TestMethod]
        public void ToDateTime_UtcMillisecondsKept_Test()
        {
            using (var reader = DataReaderTestHelper.SingleUtcDateTime(UtcWithMilliseconds))
            {
                var result = reader.ToDateTime(DataReaderTestHelper.ColumnName);

                Assert.AreEqual(UtcWithMilliseconds.Ticks, result.Ticks);
                Assert.AreEqual(DateTimeKind.Utc, result.Kind);
            }
        }

        [TestMethod]
        public void ToNullDateTime_UtcMillisecondsKept_Test()
        {
            using (var reader = DataReaderTestHelper.SingleUtcDateTime(UtcWithMilliseconds))
            {
                var result = reader.ToNullDateTime(DataReaderTestHelper.ColumnName);

                Assert.IsTrue(result.HasValue);
                Assert.AreEqual(UtcWithMilliseconds.Ticks, result.Value.Ticks);
                Assert.AreEqual(DateTimeKind.Utc, result.Value.Kind);
            }
        }

        [TestMethod]
        public void ToDateTimeParse_UtcMillisecondsKept_Test()
        {
            using (var reader = DataReaderTestHelper.SingleUtcDateTime(UtcWithMilliseconds))
            {
                var result = reader.ToDateTimeParse(DataReaderTestHelper.ColumnName);

                Assert.AreEqual(UtcWithMilliseconds.Ticks, result.Ticks);
                Assert.AreEqual(DateTimeKind.Utc, result.Kind);
            }
        }

        [TestMethod]
        public void ToNullDateTimeParse_UtcMillisecondsKept_Test()
        {
            using (var reader = DataReaderTestHelper.SingleUtcDateTime(UtcWithMilliseconds))
            {
                var result = reader.ToNullDateTimeParse(DataReaderTestHelper.ColumnName);

                Assert.IsTrue(result.HasValue);
                Assert.AreEqual(UtcWithMilliseconds.Ticks, result.Value.Ticks);
                Assert.AreEqual(DateTimeKind.Utc, result.Value.Kind);
            }
        }

        [TestMethod]
        public void ToDateTimeParseExact_TypedValueReturnedUnchanged_Test()
        {
            using (var reader = DataReaderTestHelper.SingleUtcDateTime(UtcWithMilliseconds))
            {
                var result = reader.ToDateTimeParseExact(DataReaderTestHelper.ColumnName, "yyyy-MM-dd HH:mm:ss.fff");

                Assert.AreEqual(UtcWithMilliseconds.Ticks, result.Ticks);
                Assert.AreEqual(DateTimeKind.Utc, result.Kind);
            }
        }

        [TestMethod]
        public void ToNullDateTimeParseExact_TypedValueReturnedUnchanged_Test()
        {
            using (var reader = DataReaderTestHelper.SingleUtcDateTime(UtcWithMilliseconds))
            {
                var result = reader.ToNullDateTimeParseExact(DataReaderTestHelper.ColumnName, "dd/MM/yyyy");

                Assert.IsTrue(result.HasValue);
                Assert.AreEqual(UtcWithMilliseconds.Ticks, result.Value.Ticks);
                Assert.AreEqual(DateTimeKind.Utc, result.Value.Kind);
            }
        }

        [TestMethod]
        public void ToDateTimeParseExact_StringMatchingFormatPin_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "2024-01-02 03:04:05.678"))
            {
                var result = reader.ToDateTimeParseExact(DataReaderTestHelper.ColumnName, "yyyy-MM-dd HH:mm:ss.fff");

                Assert.AreEqual(new DateTime(2024, 1, 2, 3, 4, 5, 678), result);
            }
        }

        [TestMethod]
        public void ToDateTime_DateTimeOffsetReturnsLocalDateTimeWithMilliseconds_Test()
        {
            var value = new DateTimeOffset(2024, 1, 2, 3, 4, 5, 678, TimeSpan.FromHours(2));

            using (var reader = DataReaderTestHelper.SingleValue(typeof(object), value))
            {
                var result = reader.ToDateTime(DataReaderTestHelper.ColumnName);

                Assert.AreEqual(value.LocalDateTime.Ticks, result.Ticks);
                Assert.AreEqual(DateTimeKind.Local, result.Kind);
            }
        }

        [TestMethod]
        public void ToDateTime_IsoStringUnderGermanCultureKeepsMilliseconds_Test()
        {
            using (new CultureScopeTestHelper("de-DE"))
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "2024-01-02 03:04:05.678"))
            {
                var result = reader.ToDateTime(DataReaderTestHelper.ColumnName);

                Assert.AreEqual(new DateTime(2024, 1, 2, 3, 4, 5, 678).Ticks, result.Ticks);
            }
        }

        [TestMethod]
        public void ToDateTime_DbNullPinMinValue_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(DateTime), DBNull.Value))
            {
                var result = reader.ToDateTime(DataReaderTestHelper.ColumnName);

                Assert.AreEqual(DateTime.MinValue, result);
            }
        }

        [TestMethod]
        public void ToNullDateTime_DbNullPinNull_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(DateTime), DBNull.Value))
            {
                var result = reader.ToNullDateTime(DataReaderTestHelper.ColumnName);

                Assert.IsNull(result);
            }
        }

        [TestMethod]
        public void ToDateTime_MalformedStringThrowsFormatException_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "not a date"))
            {
                Assert.ThrowsException<FormatException>(() => reader.ToDateTime(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToDateTime_ZuluStringPinLocalConversion_Test()
        {
            const string text = "2024-01-15T10:30:00Z";
            DateTime result;
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), text))
            {
                result = reader.ToDateTime(DataReaderTestHelper.ColumnName);
            }

            Assert.AreEqual(DateTime.Parse(text, CultureInfo.InvariantCulture), result);
            Assert.AreEqual(DateTimeKind.Local, result.Kind);
        }

        [TestMethod]
        public void ToDateTime_GermanDayFirstStringPin_Test()
        {
            DateTime result;
            using (new CultureScopeTestHelper("de-DE"))
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "02.01.2024"))
            {
                result = reader.ToDateTime(DataReaderTestHelper.ColumnName);
            }

            Assert.AreEqual(new DateTime(2024, 1, 2), result);
        }

        [TestMethod]
        public void ToDateTime_UsMonthFirstStringPin_Test()
        {
            DateTime result;
            using (new CultureScopeTestHelper("en-US"))
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "01/02/2024"))
            {
                result = reader.ToDateTime(DataReaderTestHelper.ColumnName);
            }

            Assert.AreEqual(new DateTime(2024, 1, 2), result);
        }

        [TestMethod]
        public void ToDateTime_IsoDateStringPin_Test()
        {
            DateTime result;
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "2024-01-02"))
            {
                result = reader.ToDateTime(DataReaderTestHelper.ColumnName);
            }

            Assert.AreEqual(new DateTime(2024, 1, 2), result);
            Assert.AreEqual(DateTimeKind.Unspecified, result.Kind);
        }
    }
}
