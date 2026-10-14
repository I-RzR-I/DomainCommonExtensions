using System;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Data.DataReader;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class DataReaderTimeSpanTests
    {
        private const string MissingColumn = "missing";

        [TestMethod]
        public void ToTimeSpan_TypedTimeSpanReturnedAsIs_Test()
        {
            var value = new TimeSpan(1, 2, 3, 4, 5);
            using (var reader = DataReaderTestHelper.SingleValue(typeof(TimeSpan), value))
            {
                Assert.AreEqual(value, reader.ToTimeSpan(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToNullTimeSpan_TypedTimeSpanReturnedAsIs_Test()
        {
            var value = new TimeSpan(1, 2, 3, 4, 5);
            using (var reader = DataReaderTestHelper.SingleValue(typeof(TimeSpan), value))
            {
                Assert.AreEqual((TimeSpan?)value, reader.ToNullTimeSpan(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToTimeSpan_ConstantFormatStringParsed_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "1.02:03:04.5000000"))
            {
                Assert.AreEqual(new TimeSpan(1, 2, 3, 4, 500), reader.ToTimeSpan(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToNullTimeSpan_NegativeConstantFormatStringParsed_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "-01:30:00"))
            {
                Assert.AreEqual((TimeSpan?)TimeSpan.FromMinutes(-90), reader.ToNullTimeSpan(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToTimeSpan_GeneralFormatStringParsed_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "1:02:03:04"))
            {
                Assert.AreEqual(new TimeSpan(1, 2, 3, 4), reader.ToTimeSpan(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToTimeSpan_GeneralFormatUnderGermanCultureIsInvariant_Test()
        {
            using (new CultureScopeTestHelper("de-DE"))
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "1:02:03:04.5"))
            {
                Assert.AreEqual(new TimeSpan(1, 2, 3, 4, 500), reader.ToTimeSpan(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToTimeSpan_DbNullReturnsZero_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(TimeSpan), DBNull.Value))
            {
                Assert.AreEqual(TimeSpan.Zero, reader.ToTimeSpan(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToNullTimeSpan_DbNullReturnsNull_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(TimeSpan), DBNull.Value))
            {
                Assert.IsNull(reader.ToNullTimeSpan(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToTimeSpan_EmptyStringReturnsZero_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), string.Empty))
            {
                Assert.AreEqual(TimeSpan.Zero, reader.ToTimeSpan(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToTimeSpan_WhitespaceStringReturnsZero_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "  "))
            {
                Assert.AreEqual(TimeSpan.Zero, reader.ToTimeSpan(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToNullTimeSpan_WhitespaceStringReturnsNull_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "  "))
            {
                Assert.IsNull(reader.ToNullTimeSpan(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToTimeSpan_MalformedStringThrowsFormatException_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "abc"))
            {
                Assert.ThrowsException<FormatException>(() => reader.ToTimeSpan(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToNullTimeSpan_MalformedStringThrowsFormatException_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "abc"))
            {
                Assert.ThrowsException<FormatException>(() => reader.ToNullTimeSpan(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToTimeSpan_OutOfRangeStringThrowsOverflowException_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "99999999.00:00:00"))
            {
                Assert.ThrowsException<OverflowException>(() => reader.ToTimeSpan(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToTimeSpan_IntegerValueThrowsInvalidCastException_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(int), 3600))
            {
                Assert.ThrowsException<InvalidCastException>(() => reader.ToTimeSpan(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToNullTimeSpan_TicksValueThrowsInvalidCastException_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(long), TimeSpan.TicksPerSecond))
            {
                Assert.ThrowsException<InvalidCastException>(() => reader.ToNullTimeSpan(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToTimeSpan_DateTimeValueThrowsInvalidCastException_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(DateTime), new DateTime(2024, 1, 15, 12, 30, 0)))
            {
                Assert.ThrowsException<InvalidCastException>(() => reader.ToTimeSpan(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToTimeSpan_MissingColumnPinArgumentException_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(TimeSpan), TimeSpan.Zero))
            {
                Assert.ThrowsException<ArgumentException>(() => reader.ToTimeSpan(MissingColumn));
            }
        }

        [TestMethod]
        public void ToNullTimeSpan_MissingColumnPinArgumentException_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(TimeSpan), TimeSpan.Zero))
            {
                Assert.ThrowsException<ArgumentException>(() => reader.ToNullTimeSpan(MissingColumn));
            }
        }
    }
}
