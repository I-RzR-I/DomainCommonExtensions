using System;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Text;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class StringIfHelpersTests
    {
        [TestMethod]
        public void IfContains_NullSource_Test()
        {
            string source = null;

            var result = source.IfContains("a", "X");

            Assert.IsNull(result);
        }

        [TestMethod]
        public void IfNotContains_NullSource_Test()
        {
            string source = null;

            var result = source.IfNotContains("a", "X");

            Assert.IsNull(result);
        }

        [TestMethod]
        public void IfStartsWith_NullSource_Test()
        {
            string source = null;

            var result = source.IfStartsWith("a", "X");

            Assert.IsNull(result);
        }

        [TestMethod]
        public void IfNotStartsWith_NullSource_Test()
        {
            string source = null;

            var result = source.IfNotStartsWith("a", "X");

            Assert.IsNull(result);
        }

        [TestMethod]
        public void IfContains_NullSearchValue_Test()
        {
            Assert.ThrowsException<ArgumentNullException>(() => "abc".IfContains(null, "X"));
        }

        [TestMethod]
        public void IfNotContains_NullSearchValue_Test()
        {
            Assert.ThrowsException<ArgumentNullException>(() => "abc".IfNotContains(null, "X"));
        }

        [TestMethod]
        public void IfStartsWith_NullSearchValue_Test()
        {
            Assert.ThrowsException<ArgumentNullException>(() => "abc".IfStartsWith(null, "X"));
        }

        [TestMethod]
        public void IfNotStartsWith_NullSearchValue_Test()
        {
            Assert.ThrowsException<ArgumentNullException>(() => "abc".IfNotStartsWith(null, "X"));
        }

        [TestMethod]
        public void IfStartsWith_SoftHyphenPrefix_Test()
        {
            var result = "abc".IfStartsWith("\u00AD", "X");

            Assert.AreEqual("abc", result);
        }

        [TestMethod]
        public void IfNotStartsWith_SoftHyphenPrefix_Test()
        {
            var result = "abc".IfNotStartsWith("\u00AD", "X");

            Assert.AreEqual("X", result);
        }

        [TestMethod]
        public void IfStartsWith_NullCharPrefix_Test()
        {
            var result = "abc".IfStartsWith("\u0000", "X");

            Assert.AreEqual("abc", result);
        }

        [TestMethod]
        public void IfNotStartsWith_NullCharPrefix_Test()
        {
            var result = "abc".IfNotStartsWith("\u0000", "X");

            Assert.AreEqual("X", result);
        }

        [DataRow("en-US")]
        [DataRow("tr-TR")]
        [DataRow("")]
        [TestMethod]
        public void IfStartsWith_ComposedPrefixCulture_Test(string cultureName)
        {
            using (new CultureScopeTestHelper(cultureName))
            {
                var result = "\u00C5bc".IfStartsWith("A\u030A", "X");

                Assert.AreEqual("\u00C5bc", result);
            }
        }

        [DataRow("en-US")]
        [DataRow("tr-TR")]
        [DataRow("")]
        [TestMethod]
        public void IfNotStartsWith_ComposedPrefixCulture_Test(string cultureName)
        {
            using (new CultureScopeTestHelper(cultureName))
            {
                var result = "\u00C5bc".IfNotStartsWith("A\u030A", "X");

                Assert.AreEqual("X", result);
            }
        }

        [TestMethod]
        public void IfStartsWith_TurkishCultureCaseSensitive_Test()
        {
            using (new CultureScopeTestHelper("tr-TR"))
            {
                Assert.AreEqual("X", "Istanbul".IfStartsWith("I", "X"));
                Assert.AreEqual("istanbul", "istanbul".IfStartsWith("I", "X"));
                Assert.AreEqual("\u0131stanbul", "\u0131stanbul".IfStartsWith("i", "X"));
            }
        }

        [DataRow("$FFFFFF", "$", "X")]
        [DataRow("FFFFFF", "$", "FFFFFF")]
        [DataRow("abc", "", "X")]
        [TestMethod]
        public void IfStartsWith_OrdinalPrefix_Test(string source, string searchValue, string expected)
        {
            var result = source.IfStartsWith(searchValue, "X");

            Assert.AreEqual(expected, result);
        }

        [DataRow("$FFFFFF", "$", "$FFFFFF")]
        [DataRow("FFFFFF", "$", "X")]
        [DataRow("abc", "", "abc")]
        [TestMethod]
        public void IfNotStartsWith_OrdinalPrefix_Test(string source, string searchValue, string expected)
        {
            var result = source.IfNotStartsWith(searchValue, "X");

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void IfContains_IgnorableValue_Test()
        {
            var result = "abc".IfContains("\u00AD", "X");

            Assert.AreEqual("abc", result);
        }

        [TestMethod]
        public void IfNotContains_IgnorableValue_Test()
        {
            var result = "abc".IfNotContains("\u00AD", "X");

            Assert.AreEqual("X", result);
        }
    }
}
