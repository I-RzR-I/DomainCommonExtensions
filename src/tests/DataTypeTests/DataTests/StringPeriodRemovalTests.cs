using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Text;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class StringPeriodRemovalTests
    {
        [DataRow("1. ", ".", "1")]
        [DataRow("1.5. ", ".", "1.5")]
        [DataRow("10.\t", ".", "10")]
        [TestMethod]
        public void RemovePeriod_TrailingWhitespace_Test(string source, string delimiter, string expected)
        {
            var result = source.RemovePeriod(delimiter);

            Assert.AreEqual(expected, result);
        }

        [DataRow("a::", "::", "a")]
        [DataRow("10;;", ";;", "10")]
        [TestMethod]
        public void RemovePeriod_MultiCharDelimiter_Test(string source, string delimiter, string expected)
        {
            var result = source.RemovePeriod(delimiter);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void RemovePeriod_EmptyDelimiter_Test()
        {
            var result = "12".RemovePeriod("");

            Assert.AreEqual("12", result);
        }

        [TestMethod]
        public void RemovePeriod_NullDelimiter_Test()
        {
            var result = "12.".RemovePeriod(null);

            Assert.AreEqual("12.", result);
        }

        [TestMethod]
        public void RemovePeriod_IgnorableDelimiter_Test()
        {
            var result = "abc".RemovePeriod("\u00AD");

            Assert.AreEqual("abc", result);
        }

        [TestMethod]
        public void RemovePeriod_OnlyLastDelimiter_Test()
        {
            var result = "1..".RemovePeriod(".");

            Assert.AreEqual("1.", result);
        }

        [TestMethod]
        public void RemovePeriod_DefaultDelimiter_Test()
        {
            var result = "10.".RemovePeriod();

            Assert.AreEqual("10", result);
        }

        [DataRow("10.", ".", "10")]
        [DataRow("Test;", ";", "Test")]
        [DataRow("Test;Test2", ";", "Test;Test2")]
        [DataRow("1.5", ".", "1.5")]
        [TestMethod]
        public void RemovePeriod_PinnedValues_Test(string source, string delimiter, string expected)
        {
            var result = source.RemovePeriod(delimiter);

            Assert.AreEqual(expected, result);
        }

        [DataRow(null)]
        [DataRow("")]
        [TestMethod]
        public void RemovePeriod_MissingSource_Test(string source)
        {
            var result = source.RemovePeriod(".");

            Assert.AreEqual(source, result);
        }

        [TestMethod]
        public void RemovePeriodValue_TrailingDelimiterOnly_Test()
        {
            var result = "1.5.".RemovePeriodValue();

            Assert.AreEqual("1.5", result);
        }

        [DataRow("1.005.00", ".", "00", "1.005")]
        [DataRow("5.00x.00", ".", "00", "5.00x")]
        [DataRow("a;zzb;zz", ";", "zz", "a;zzb")]
        [TestMethod]
        public void RemovePeriodValue_RepeatedSuffix_Test(string source, string delimiter, string period, string expected)
        {
            var result = source.RemovePeriodValue(delimiter, period);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void RemovePeriodValue_IgnorableDelimiter_Test()
        {
            var result = "a\u00ADb".RemovePeriodValue("\u00AD", "");

            Assert.AreEqual("a\u00ADb", result);
        }

        [TestMethod]
        public void RemovePeriodValue_SuffixNotTrailing_Test()
        {
            var result = "1.00x".RemovePeriodValue(".", "00");

            Assert.AreEqual("1.00x", result);
        }

        [TestMethod]
        public void RemovePeriodValue_DefaultPeriodUnchanged_Test()
        {
            var result = "1.00".RemovePeriodValue();

            Assert.AreEqual("1.00", result);
        }

        [TestMethod]
        public void RemovePeriodValue_NullDelimiter_Test()
        {
            var result = "1.00".RemovePeriodValue(null, "");

            Assert.AreEqual("1.00", result);
        }

        [TestMethod]
        public void RemovePeriodValue_EmptyDelimiterWithPeriod_Test()
        {
            var result = "1.00".RemovePeriodValue("", "00");

            Assert.AreEqual("1.00", result);
        }

        [DataRow("10.00", ".", "00", "10")]
        [DataRow("10.25", ".", "25", "10")]
        [DataRow("Test;zas", ";", "zas", "Test")]
        [DataRow("Test. ", ".", " ", "Test")]
        [TestMethod]
        public void RemovePeriodValue_PinnedValues_Test(string source, string delimiter, string period, string expected)
        {
            var result = source.RemovePeriodValue(delimiter, period);

            Assert.AreEqual(expected, result);
        }

        [DataRow(null)]
        [DataRow("")]
        [TestMethod]
        public void RemovePeriodValue_MissingSource_Test(string source)
        {
            var result = source.RemovePeriodValue(".", "00");

            Assert.AreEqual(source, result);
        }

        [TestMethod]
        public void RemovePeriod_NonMatchingDelimiterKeepsWhitespace_Test()
        {
            var result = "1.5 ".RemovePeriod(";");

            Assert.AreEqual("1.5 ", result);
        }
    }
}
