using System;
using System.Data;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Data;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class DataRecordGetValueOrDefaultTests
    {
        [TestMethod]
        public void GetValueOrDefault_ByNameTypedValueReturnsValue_Test()
        {
            using (var reader = DataRecordTestHelper.SingleRow(("Id", typeof(int), 42)))
            {
                Assert.AreEqual(42, reader.GetValueOrDefault<int>("Id"));
            }
        }

        [TestMethod]
        public void GetValueOrDefault_ByNameDifferentCaseReturnsValue_Test()
        {
            using (var reader = DataRecordTestHelper.SingleRow(("Id", typeof(int), 42)))
            {
                Assert.AreEqual(42, reader.GetValueOrDefault<int>("ID"));
            }
        }

        [TestMethod]
        public void GetValueOrDefault_ByNameMissingColumnReturnsExplicitDefault_Test()
        {
            using (var reader = DataRecordTestHelper.SingleRow(("Id", typeof(int), 42)))
            {
                Assert.AreEqual(7, reader.GetValueOrDefault("Missing", 7));
            }
        }

        [TestMethod]
        public void GetValueOrDefault_ByNameMissingColumnReturnsImplicitDefault_Test()
        {
            using (var reader = DataRecordTestHelper.SingleRow(("Id", typeof(int), 42)))
            {
                Assert.AreEqual(0, reader.GetValueOrDefault<int>("Missing"));
                Assert.IsNull(reader.GetValueOrDefault<string>("Missing"));
            }
        }

        [TestMethod]
        public void GetValueOrDefault_ByNameNullNameReturnsDefault_Test()
        {
            using (var reader = DataRecordTestHelper.SingleRow(("Id", typeof(int), 42)))
            {
                Assert.AreEqual(5, reader.GetValueOrDefault((string)null, 5));
            }
        }

        [TestMethod]
        public void GetValueOrDefault_ByNameDbNullReturnsExplicitDefault_Test()
        {
            using (var reader = DataRecordTestHelper.SingleRow(("Id", typeof(int), DBNull.Value)))
            {
                Assert.AreEqual(-1, reader.GetValueOrDefault("Id", -1));
            }
        }

        [TestMethod]
        public void GetValueOrDefault_ByNameDbNullNullableReturnsNull_Test()
        {
            using (var reader = DataRecordTestHelper.SingleRow(("Id", typeof(int), DBNull.Value)))
            {
                Assert.IsNull(reader.GetValueOrDefault<int?>("Id"));
            }
        }

        [TestMethod]
        public void GetValueOrDefault_ByNameNullRecordThrowsArgumentNullException_Test()
        {
            var exception = Assert.ThrowsException<ArgumentNullException>(
                () => ((IDataRecord)null).GetValueOrDefault("Id", 1));

            Assert.AreEqual("record", exception.ParamName);
        }

        [TestMethod]
        public void GetValueOrDefault_ByOrdinalTypedValueReturnsValue_Test()
        {
            using (var reader = DataRecordTestHelper.SingleRow(("Id", typeof(int), 1), ("Name", typeof(string), "a")))
            {
                Assert.AreEqual("a", reader.GetValueOrDefault<string>(1));
            }
        }

        [TestMethod]
        public void GetValueOrDefault_ByOrdinalNegativeReturnsDefault_Test()
        {
            using (var reader = DataRecordTestHelper.SingleRow(("Id", typeof(int), 42)))
            {
                Assert.AreEqual(7, reader.GetValueOrDefault(-1, 7));
            }
        }

        [TestMethod]
        public void GetValueOrDefault_ByOrdinalPastEndReturnsDefault_Test()
        {
            using (var reader = DataRecordTestHelper.SingleRow(("Id", typeof(int), 42)))
            {
                Assert.AreEqual(7, reader.GetValueOrDefault(reader.FieldCount, 7));
                Assert.AreEqual(0, reader.GetValueOrDefault<int>(reader.FieldCount));
            }
        }

        [TestMethod]
        public void GetValueOrDefault_ByOrdinalDbNullReturnsImplicitDefault_Test()
        {
            using (var reader = DataRecordTestHelper.SingleRow(("Id", typeof(int), DBNull.Value)))
            {
                Assert.AreEqual(0, reader.GetValueOrDefault<int>(0));
            }
        }

        [TestMethod]
        public void GetValueOrDefault_ByOrdinalNullRecordThrowsArgumentNullException_Test()
        {
            var exception = Assert.ThrowsException<ArgumentNullException>(
                () => ((IDataRecord)null).GetValueOrDefault(0, 1));

            Assert.AreEqual("record", exception.ParamName);
        }

        [TestMethod]
        public void GetValueOrDefault_NullValueReturnsDefault_Test()
        {
            var record = new FakeDataRecordTestHelper(new[] { "Id" }, new object[] { null });

            Assert.AreEqual(3, record.GetValueOrDefault("Id", 3));
        }

        [TestMethod]
        public void GetValueOrDefault_TypedReferenceValueReturnedAsIs_Test()
        {
            using (var reader = DataRecordTestHelper.SingleRow(("Data", typeof(object), new byte[] { 1, 2 })))
            {
                var expected = reader.GetValue(0);

                Assert.AreSame(expected, reader.GetValueOrDefault<byte[]>("Data"));
            }
        }

        [TestMethod]
        public void GetValueOrDefault_TypedUtcDateTimeKeepsKind_Test()
        {
            var value = new DateTime(2024, 1, 15, 12, 30, 45, 123, DateTimeKind.Utc);
            using (var reader = DataRecordTestHelper.SingleRow(("At", typeof(object), value)))
            {
                var result = reader.GetValueOrDefault<DateTime>("At");

                Assert.AreEqual(value, result);
                Assert.AreEqual(DateTimeKind.Utc, result.Kind);
            }
        }

        [TestMethod]
        public void GetValueOrDefault_DotDecimalStringUnderGermanCultureIsInvariant_Test()
        {
            using (new CultureScopeTestHelper("de-DE"))
            using (var reader = DataRecordTestHelper.SingleRow(("Amount", typeof(string), "1.5")))
            {
                Assert.AreEqual(1.5m, reader.GetValueOrDefault<decimal>("Amount"));
            }
        }

        [TestMethod]
        public void GetValueOrDefault_DotDoubleStringUnderGermanCultureIsInvariant_Test()
        {
            using (new CultureScopeTestHelper("de-DE"))
            using (var reader = DataRecordTestHelper.SingleRow(("Amount", typeof(string), "1.5")))
            {
                Assert.AreEqual(1.5d, reader.GetValueOrDefault<double>("Amount"));
            }
        }

        [TestMethod]
        public void GetValueOrDefault_CommaDecimalStringUnderGermanCultureFallsBack_Test()
        {
            using (new CultureScopeTestHelper("de-DE"))
            using (var reader = DataRecordTestHelper.SingleRow(("Amount", typeof(string), "1,5")))
            {
                Assert.AreEqual(1.5m, reader.GetValueOrDefault<decimal>("Amount"));
            }
        }

        [TestMethod]
        public void GetValueOrDefault_DecimalToStringUnderGermanCultureIsInvariant_Test()
        {
            using (new CultureScopeTestHelper("de-DE"))
            using (var reader = DataRecordTestHelper.SingleRow(("Amount", typeof(decimal), 1.5m)))
            {
                Assert.AreEqual("1.5", reader.GetValueOrDefault<string>("Amount"));
            }
        }

        [TestMethod]
        public void GetValueOrDefault_IntegerStringToNullableIntReturnsValue_Test()
        {
            using (var reader = DataRecordTestHelper.SingleRow(("Id", typeof(string), "5")))
            {
                Assert.AreEqual((int?)5, reader.GetValueOrDefault<int?>("Id"));
            }
        }

        [TestMethod]
        public void GetValueOrDefault_LongValueToIntConverts_Test()
        {
            using (var reader = DataRecordTestHelper.SingleRow(("Id", typeof(long), 5L)))
            {
                Assert.AreEqual(5, reader.GetValueOrDefault<int>("Id"));
            }
        }

        [TestMethod]
        public void GetValueOrDefault_EmptyStringToIntReturnsDefault_Test()
        {
            using (var reader = DataRecordTestHelper.SingleRow(("Id", typeof(string), string.Empty)))
            {
                Assert.AreEqual(9, reader.GetValueOrDefault("Id", 9));
            }
        }

        [TestMethod]
        public void GetValueOrDefault_EmptyStringToNullableIntReturnsNull_Test()
        {
            using (var reader = DataRecordTestHelper.SingleRow(("Id", typeof(string), string.Empty)))
            {
                Assert.IsNull(reader.GetValueOrDefault<int?>("Id"));
            }
        }

        [TestMethod]
        public void GetValueOrDefault_EmptyStringToStringReturnsEmpty_Test()
        {
            using (var reader = DataRecordTestHelper.SingleRow(("Name", typeof(string), string.Empty)))
            {
                Assert.AreEqual(string.Empty, reader.GetValueOrDefault("Name", "fallback"));
            }
        }

        [TestMethod]
        public void GetValueOrDefault_EmptyStringToCharReturnsDefault_Test()
        {
            using (var reader = DataRecordTestHelper.SingleRow(("Code", typeof(string), string.Empty)))
            {
                Assert.AreEqual('z', reader.GetValueOrDefault("Code", 'z'));
            }
        }

        [TestMethod]
        public void GetValueOrDefault_WhitespaceStringToIntReturnsDefault_Test()
        {
            using (var reader = DataRecordTestHelper.SingleRow(("Id", typeof(string), " ")))
            {
                Assert.AreEqual(9, reader.GetValueOrDefault("Id", 9));
            }
        }

        [TestMethod]
        public void GetValueOrDefault_WhitespaceStringToStringReturnedAsIs_Test()
        {
            using (var reader = DataRecordTestHelper.SingleRow(("Name", typeof(string), " ")))
            {
                Assert.AreEqual(" ", reader.GetValueOrDefault("Name", "fallback"));
            }
        }

        [TestMethod]
        public void GetValueOrDefault_WhitespaceStringToCharReturnsSpace_Test()
        {
            using (var reader = DataRecordTestHelper.SingleRow(("Code", typeof(string), " ")))
            {
                Assert.AreEqual(' ', reader.GetValueOrDefault("Code", 'z'));
            }
        }

        [TestMethod]
        public void GetValueOrDefault_WhitespaceCharToString_Test()
        {
            using (var reader = DataRecordTestHelper.SingleRow(("Code", typeof(char), ' ')))
            {
                Assert.AreEqual(" ", reader.GetValueOrDefault("Code", "fallback"));
            }
        }

        [TestMethod]
        public void GetValueOrDefault_MalformedStringThrowsFormatException_Test()
        {
            using (var reader = DataRecordTestHelper.SingleRow(("Id", typeof(string), "abc")))
            {
                Assert.ThrowsException<FormatException>(() => reader.GetValueOrDefault("Id", 9));
            }
        }

        [TestMethod]
        public void GetValueOrDefault_ByOrdinalMalformedStringThrowsFormatException_Test()
        {
            using (var reader = DataRecordTestHelper.SingleRow(("Id", typeof(string), "abc")))
            {
                Assert.ThrowsException<FormatException>(() => reader.GetValueOrDefault(0, 9));
            }
        }

        [TestMethod]
        public void GetValueOrDefault_OutOfRangeValueThrowsOverflowException_Test()
        {
            using (var reader = DataRecordTestHelper.SingleRow(("Id", typeof(int), 300)))
            {
                Assert.ThrowsException<OverflowException>(() => reader.GetValueOrDefault<byte>("Id"));
            }
        }
    }
}
