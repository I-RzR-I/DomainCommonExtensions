using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Primitives;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class NumbersInvariantParseTests
    {
        [TestMethod]
        public void ToIntOrDefault_NullSource_Test()
        {
            string source = null;

            Assert.AreEqual(0, source.ToIntOrDefault());
        }

        [DataRow("")]
        [DataRow("   ")]
        [DataRow("abc")]
        [DataRow("1.0")]
        [DataRow("1,000")]
        [DataRow("1e3")]
        [DataRow("0x10")]
        [DataRow("2147483648")]
        [DataRow("-2147483649")]
        [TestMethod]
        public void ToIntOrDefault_InvalidSource_Test(string source)
        {
            Assert.AreEqual(0, source.ToIntOrDefault());
        }

        [DataRow("42", 42)]
        [DataRow("-42", -42)]
        [DataRow("+42", 42)]
        [DataRow("  42  ", 42)]
        [DataRow("\t42\r\n", 42)]
        [DataRow("2147483647", int.MaxValue)]
        [DataRow("-2147483648", int.MinValue)]
        [TestMethod]
        public void ToIntOrDefault_ValidSource_Test(string source, int expected)
        {
            Assert.AreEqual(expected, source.ToIntOrDefault());
        }

        [TestMethod]
        public void ToIntOrDefault_NonInvariantCulture_Test()
        {
            using (new CultureScopeTestHelper("de-DE"))
            {
                Assert.AreEqual(0, "1.000".ToIntOrDefault());
                Assert.AreEqual(-7, "-7".ToIntOrDefault());
            }
        }

        [TestMethod]
        public void ToLongOrNull_NullSource_Test()
        {
            string source = null;

            Assert.IsNull(source.ToLongOrNull());
        }

        [DataRow("")]
        [DataRow("   ")]
        [DataRow("abc")]
        [DataRow("1.0")]
        [DataRow("1,000")]
        [DataRow("1e3")]
        [DataRow("(5)")]
        [DataRow("9223372036854775808")]
        [DataRow("-9223372036854775809")]
        [TestMethod]
        public void ToLongOrNull_InvalidSource_Test(string source)
        {
            Assert.IsNull(source.ToLongOrNull());
        }

        [DataRow("2147483648", 2147483648L)]
        [DataRow("-42", -42L)]
        [DataRow("  42  ", 42L)]
        [DataRow("9223372036854775807", long.MaxValue)]
        [DataRow("-9223372036854775808", long.MinValue)]
        [TestMethod]
        public void ToLongOrNull_ValidSource_Test(string source, long expected)
        {
            Assert.AreEqual((long?)expected, source.ToLongOrNull());
        }

        [TestMethod]
        public void ToLongOrNull_NonInvariantCulture_Test()
        {
            using (new CultureScopeTestHelper("de-DE"))
            {
                Assert.IsNull("1.000".ToLongOrNull());
                Assert.AreEqual((long?)1000L, "1000".ToLongOrNull());
            }
        }

        [TestMethod]
        public void ToLongOrDefault_NullSource_Test()
        {
            string source = null;

            Assert.AreEqual(0L, source.ToLongOrDefault());
        }

        [DataRow("")]
        [DataRow("abc")]
        [DataRow("1,000")]
        [DataRow("9223372036854775808")]
        [TestMethod]
        public void ToLongOrDefault_InvalidSource_Test(string source)
        {
            Assert.AreEqual(0L, source.ToLongOrDefault());
        }

        [TestMethod]
        public void ToLongOrDefault_ValidSource_Test()
        {
            Assert.AreEqual(2147483648L, " 2147483648 ".ToLongOrDefault());
        }

        [TestMethod]
        public void ToDecimalOrNull_NullSource_Test()
        {
            string source = null;

            Assert.IsNull(source.ToDecimalOrNull());
        }

        [DataRow("")]
        [DataRow("   ")]
        [DataRow("abc")]
        [DataRow("1,5")]
        [DataRow("1,000.5")]
        [DataRow("$5")]
        [DataRow("NaN")]
        [DataRow("Infinity")]
        [DataRow("79228162514264337593543950336")]
        [DataRow("-79228162514264337593543950336")]
        [DataRow("1e29")]
        [TestMethod]
        public void ToDecimalOrNull_InvalidSource_Test(string source)
        {
            Assert.IsNull(source.ToDecimalOrNull());
        }

        [TestMethod]
        public void ToDecimalOrNull_DecimalPoint_Test()
        {
            Assert.AreEqual((decimal?)1.5m, "1.5".ToDecimalOrNull());
        }

        [TestMethod]
        public void ToDecimalOrNull_SignAndWhiteSpace_Test()
        {
            Assert.AreEqual((decimal?)-0.25m, "  -0.25  ".ToDecimalOrNull());
            Assert.AreEqual((decimal?)3m, "+3".ToDecimalOrNull());
        }

        [TestMethod]
        public void ToDecimalOrNull_Exponent_Test()
        {
            Assert.AreEqual((decimal?)150m, "1.5E2".ToDecimalOrNull());
            Assert.AreEqual((decimal?)0.015m, "1.5e-2".ToDecimalOrNull());
        }

        [TestMethod]
        public void ToDecimalOrNull_ExponentUnderflow_Test()
        {
            Assert.AreEqual((decimal?)0m, "1e-30".ToDecimalOrNull());
        }

        [TestMethod]
        public void ToDecimalOrNull_MaxValue_Test()
        {
            Assert.AreEqual((decimal?)decimal.MaxValue, "79228162514264337593543950335".ToDecimalOrNull());
        }

        [TestMethod]
        public void ToDecimalOrNull_CommaUnderGermanCulture_Test()
        {
            using (new CultureScopeTestHelper("de-DE"))
            {
                Assert.IsNull("1,5".ToDecimalOrNull());
            }
        }

        [TestMethod]
        public void ToDecimalOrNull_PointUnderGermanCulture_Test()
        {
            using (new CultureScopeTestHelper("de-DE"))
            {
                Assert.AreEqual((decimal?)1.5m, "1.5".ToDecimalOrNull());
            }
        }

        [TestMethod]
        public void ToDecimalOrDefault_NullSource_Test()
        {
            string source = null;

            Assert.AreEqual(0m, source.ToDecimalOrDefault());
        }

        [DataRow("")]
        [DataRow("abc")]
        [DataRow("1,000.5")]
        [DataRow("79228162514264337593543950336")]
        [TestMethod]
        public void ToDecimalOrDefault_InvalidSource_Test(string source)
        {
            Assert.AreEqual(0m, source.ToDecimalOrDefault());
        }

        [TestMethod]
        public void ToDecimalOrDefault_UnderGermanCulture_Test()
        {
            using (new CultureScopeTestHelper("de-DE"))
            {
                Assert.AreEqual(0m, "1,5".ToDecimalOrDefault());
                Assert.AreEqual(1.5m, "1.5".ToDecimalOrDefault());
            }
        }

        [TestMethod]
        public void ToDoubleOrNull_NullSource_Test()
        {
            string source = null;

            Assert.IsNull(source.ToDoubleOrNull());
        }

        [DataRow("")]
        [DataRow("   ")]
        [DataRow("abc")]
        [DataRow("1,5")]
        [DataRow("1,000.5")]
        [DataRow("$5")]
        [TestMethod]
        public void ToDoubleOrNull_InvalidSource_Test(string source)
        {
            Assert.IsNull(source.ToDoubleOrNull());
        }

        [TestMethod]
        public void ToDoubleOrNull_DecimalPoint_Test()
        {
            Assert.AreEqual((double?)1.5, "1.5".ToDoubleOrNull());
        }

        [TestMethod]
        public void ToDoubleOrNull_SignAndWhiteSpace_Test()
        {
            Assert.AreEqual((double?)-0.25, "  -0.25  ".ToDoubleOrNull());
            Assert.AreEqual((double?)3.0, "+3".ToDoubleOrNull());
        }

        [TestMethod]
        public void ToDoubleOrNull_Exponent_Test()
        {
            Assert.AreEqual((double?)150.0, "1.5E2".ToDoubleOrNull());
            Assert.AreEqual((double?)1e-3, "1e-3".ToDoubleOrNull());
        }

        [TestMethod]
        public void ToDoubleOrNull_NaNSymbol_Test()
        {
            var result = "NaN".ToDoubleOrNull();

            Assert.IsNotNull(result);
            Assert.IsTrue(double.IsNaN(result.Value));
        }

        [TestMethod]
        public void ToDoubleOrNull_InfinitySymbols_Test()
        {
            Assert.AreEqual((double?)double.PositiveInfinity, "Infinity".ToDoubleOrNull());
            Assert.AreEqual((double?)double.NegativeInfinity, "-Infinity".ToDoubleOrNull());
        }

        [TestMethod]
        public void ToDoubleOrNull_CommaUnderGermanCulture_Test()
        {
            using (new CultureScopeTestHelper("de-DE"))
            {
                Assert.IsNull("1,5".ToDoubleOrNull());
            }
        }

        [TestMethod]
        public void ToDoubleOrNull_PointUnderGermanCulture_Test()
        {
            using (new CultureScopeTestHelper("de-DE"))
            {
                Assert.AreEqual((double?)1.5, "1.5".ToDoubleOrNull());
            }
        }

        [TestMethod]
        public void ToDoubleOrDefault_NullSource_Test()
        {
            string source = null;

            Assert.AreEqual(0.0, source.ToDoubleOrDefault());
        }

        [DataRow("")]
        [DataRow("abc")]
        [DataRow("1,000.5")]
        [TestMethod]
        public void ToDoubleOrDefault_InvalidSource_Test(string source)
        {
            Assert.AreEqual(0.0, source.ToDoubleOrDefault());
        }

        [TestMethod]
        public void ToDoubleOrDefault_NaNSymbol_Test()
        {
            Assert.IsTrue(double.IsNaN("NaN".ToDoubleOrDefault()));
        }

        [TestMethod]
        public void ToDoubleOrDefault_UnderGermanCulture_Test()
        {
            using (new CultureScopeTestHelper("de-DE"))
            {
                Assert.AreEqual(0.0, "1,5".ToDoubleOrDefault());
                Assert.AreEqual(1.5, "1.5".ToDoubleOrDefault());
            }
        }
    }
}
