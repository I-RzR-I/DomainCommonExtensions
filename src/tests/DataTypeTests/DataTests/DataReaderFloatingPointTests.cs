using System;
using System.Globalization;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Data.DataReader;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class DataReaderFloatingPointTests
    {
        [TestMethod]
        public void ToDecimal_SmallDoubleReturnsExactDecimal_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(double), 1e-5d))
            {
                Assert.AreEqual(0.00001m, reader.ToDecimal(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToNullDecimal_SmallDoubleUnderGermanCultureReturnsExactDecimal_Test()
        {
            using (new CultureScopeTestHelper("de-DE"))
            using (var reader = DataReaderTestHelper.SingleValue(typeof(object), 1e-5d))
            {
                Assert.AreEqual((decimal?)0.00001m, reader.ToNullDecimal(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToDecimal_ExponentStringReturnsValue_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "1E3"))
            {
                Assert.AreEqual(1000m, reader.ToDecimal(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToDecimal_DotStringUnderGermanCultureIsInvariant_Test()
        {
            using (new CultureScopeTestHelper("de-DE"))
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "1.5"))
            {
                Assert.AreEqual(1.5m, reader.ToDecimal(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToDecimal_CommaStringUnderGermanCulturePin_Test()
        {
            using (new CultureScopeTestHelper("de-DE"))
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "1,5"))
            {
                Assert.AreEqual(1.5m, reader.ToDecimal(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToDecimal_TypedDecimalUnderGermanCulturePin_Test()
        {
            using (new CultureScopeTestHelper("de-DE"))
            using (var reader = DataReaderTestHelper.SingleValue(typeof(decimal), 123.456m))
            {
                Assert.AreEqual(123.456m, reader.ToDecimal(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToDecimal_MalformedStringThrowsFormatException_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "abc"))
            {
                Assert.ThrowsException<FormatException>(() => reader.ToDecimal(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToNullDecimal_DbNullPinNull_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(decimal), DBNull.Value))
            {
                Assert.IsNull(reader.ToNullDecimal(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToDouble_DotStringUnderGermanCultureIsInvariant_Test()
        {
            using (new CultureScopeTestHelper("de-DE"))
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "1.5"))
            {
                Assert.AreEqual(1.5d, reader.ToDouble(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToNullDouble_DotStringUnderGermanCultureIsInvariant_Test()
        {
            using (new CultureScopeTestHelper("de-DE"))
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "1.5"))
            {
                Assert.AreEqual((double?)1.5d, reader.ToNullDouble(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToDouble_CommaStringUnderGermanCulturePin_Test()
        {
            using (new CultureScopeTestHelper("de-DE"))
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "1,5"))
            {
                Assert.AreEqual(1.5d, reader.ToDouble(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToDouble_TypedValueRoundTripsExactly_Test()
        {
            var value = 0.1d + 0.2d;

            using (var reader = DataReaderTestHelper.SingleValue(typeof(double), value))
            {
                Assert.AreEqual(value, reader.ToDouble(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToDouble_TypedValueUnderGermanCultureRoundTripsExactly_Test()
        {
            var value = 0.1d + 0.2d;

            using (new CultureScopeTestHelper("de-DE"))
            using (var reader = DataReaderTestHelper.SingleValue(typeof(double), value))
            {
                Assert.AreEqual(value, reader.ToDouble(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToDouble_MalformedStringThrowsFormatException_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "abc"))
            {
                Assert.ThrowsException<FormatException>(() => reader.ToDouble(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToDouble_DbNullPinZero_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(double), DBNull.Value))
            {
                Assert.AreEqual(0d, reader.ToDouble(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToFloat_DotStringUnderGermanCultureIsInvariant_Test()
        {
            using (new CultureScopeTestHelper("de-DE"))
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), "1.5"))
            {
                Assert.AreEqual(1.5f, reader.ToFloat(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToNullFloat_TypedValuePin_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(float), 2.25f))
            {
                Assert.AreEqual((float?)2.25f, reader.ToNullFloat(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToDecimal_TypedDoubleKeepsDigitsPin_Test()
        {
            decimal result;
            using (var reader = DataReaderTestHelper.SingleValue(typeof(double), 0.1d + 0.2d))
            {
                result = reader.ToDecimal(DataReaderTestHelper.ColumnName);
            }

            Assert.AreEqual(decimal.Parse((0.1d + 0.2d).ToString(CultureInfo.InvariantCulture), NumberStyles.Float, CultureInfo.InvariantCulture), result);
        }
    }
}
