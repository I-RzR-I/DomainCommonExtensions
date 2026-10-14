using System.Globalization;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Primitives;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class LongAsReadableFileSizeProviderTests
    {
        [DataRow(0L, "0 bytes")]
        [DataRow(1L, "1 bytes")]
        [DataRow(1023L, "1023 bytes")]
        [DataRow(1024L, "1.0 KB")]
        [DataRow(1536L, "1.5 KB")]
        [DataRow(1048575L, "1024.0 KB")]
        [DataRow(1048576L, "1.0 MB")]
        [DataRow(1572864L, "1.5 MB")]
        [DataRow(1101823L, "1.0 MB")]
        [DataRow(99999999L, "95.4 MB")]
        [DataRow(1073741823L, "1024.0 MB")]
        [DataRow(1073741824L, "1.0 GB")]
        [DataRow(1610612736L, "1.5 GB")]
        [DataRow(1099511627776L, "1.0 TB")]
        [DataRow(1649267441664L, "1.5 TB")]
        [DataRow(1125899906842624L, "1.0 PB")]
        [DataRow(1688849860263936L, "1.5 PB")]
        [DataRow(1152921504606846975L, "1024.0 PB")]
        [DataRow(1152921504606846976L, "1 EB")]
        [DataRow(2305843009213693952L, "2 EB")]
        [DataRow(long.MaxValue, "8 EB")]
        [TestMethod]
        public void AsReadableFileSize_InvariantProvider_Test(long size, string expected)
        {
            var result = size.AsReadableFileSize(CultureInfo.InvariantCulture);

            Assert.AreEqual(expected, result);
        }

        [DataRow(-1L, "-1 bytes")]
        [DataRow(-1024L, "-1024 bytes")]
        [DataRow(-1048576L, "-1048576 bytes")]
        [DataRow(long.MinValue, "-9223372036854775808 bytes")]
        [TestMethod]
        public void AsReadableFileSize_NegativeSize_Test(long size, string expected)
        {
            var result = size.AsReadableFileSize(CultureInfo.InvariantCulture);

            Assert.AreEqual(expected, result);
        }

        [DataRow(1023L, "1023 bytes")]
        [DataRow(1536L, "1,5 KB")]
        [DataRow(1048575L, "1024,0 KB")]
        [DataRow(1610612736L, "1,5 GB")]
        [DataRow(long.MaxValue, "8 EB")]
        [TestMethod]
        public void AsReadableFileSize_GermanProvider_Test(long size, string expected)
        {
            var provider = new CultureInfo("de-DE", false);

            var result = size.AsReadableFileSize(provider);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void AsReadableFileSize_ProviderOverridesCurrentCulture_Test()
        {
            using (new CultureScopeTestHelper("de-DE"))
            {
                var result = 1536L.AsReadableFileSize(CultureInfo.InvariantCulture);

                Assert.AreEqual("1.5 KB", result);
            }
        }

        [DataRow(1536L, "1#5 KB")]
        [DataRow(1610612736L, "1#5 GB")]
        [DataRow(-5L, "~5 bytes")]
        [TestMethod]
        public void AsReadableFileSize_CustomNumberFormat_Test(long size, string expected)
        {
            var provider = (NumberFormatInfo)NumberFormatInfo.InvariantInfo.Clone();
            provider.NumberDecimalSeparator = "#";
            provider.NegativeSign = "~";

            var result = size.AsReadableFileSize(provider);

            Assert.AreEqual(expected, result);
        }

        [DataRow(1023L, "1023 bytes")]
        [DataRow(1536L, "1,5 KB")]
        [DataRow(1610612736L, "1,5 GB")]
        [TestMethod]
        public void AsReadableFileSize_NullProvider_Test(long size, string expected)
        {
            using (new CultureScopeTestHelper("de-DE"))
            {
                var result = size.AsReadableFileSize(null);

                Assert.AreEqual(expected, result);
            }
        }

    }
}
