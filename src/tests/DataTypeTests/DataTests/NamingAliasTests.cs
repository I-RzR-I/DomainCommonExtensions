using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Primitives;
using RzR.Extensions.Domain.Text;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class NamingAliasTests
    {
        [DataRow("my.gzip.gz")]
        [DataRow("archive.tar.gz")]
        [DataRow("a.b.c.b")]
        [DataRow("README")]
        [DataRow("a.txt")]
        [DataRow(".gitignore")]
        [DataRow("file.")]
        [TestMethod]
        public void GetFileNameAndExtension_MatchesOldMember_Test(string fullName)
        {
            var expected = fullName.GetFileNameAndExtenstion();

            var result = fullName.GetFileNameAndExtension();

            Assert.IsNotNull(result);
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void GetFileNameAndExtension_MultiDotName_Test()
        {
            var result = "my.gzip.gz".GetFileNameAndExtension();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual("my.gzip", result[0]);
            Assert.AreEqual("gz", result[1]);
        }

        [TestMethod]
        public void GetFileNameAndExtension_NoExtension_Test()
        {
            var result = "README".GetFileNameAndExtension();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual("README", result[0]);
            Assert.AreEqual("README", result[1]);
        }

        [DataRow(null)]
        [DataRow("")]
        [DataRow("   ")]
        [TestMethod]
        public void GetFileNameAndExtension_MissingValue_Test(string fullName)
        {
            var result = fullName.GetFileNameAndExtension();

            Assert.IsNull(result);
            Assert.IsNull(fullName.GetFileNameAndExtenstion());
        }

        [DataRow("AQID")]
        [DataRow("SGVsbG8=")]
        [DataRow("AA==")]
        [DataRow("")]
        [TestMethod]
        public void DecodeBytesFromBase64_MatchesOldMember_Test(string encoded)
        {
            var expected = encoded.DeCodeBytesFromBase64();

            var result = encoded.DecodeBytesFromBase64();

            Assert.IsNotNull(result);
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DecodeBytesFromBase64_ValidInput_Test()
        {
            var result = "AQID".DecodeBytesFromBase64();

            CollectionAssert.AreEqual(new byte[] { 1, 2, 3 }, result);
        }

        [TestMethod]
        public void DecodeBytesFromBase64_EmptyInput_Test()
        {
            var result = string.Empty.DecodeBytesFromBase64();

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Length);
        }

        [TestMethod]
        public void DecodeBytesFromBase64_NullInput_Test()
        {
            string encoded = null;

            var ex = Assert.ThrowsException<ArgumentNullException>(() => encoded.DecodeBytesFromBase64());

            Assert.AreEqual("encoded", ex.ParamName);
        }

        [TestMethod]
        public void DecodeBytesFromBase64_NullMatchesOldMember_Test()
        {
            string encoded = null;

            var oldEx = Assert.ThrowsException<ArgumentNullException>(() => encoded.DeCodeBytesFromBase64());
            var newEx = Assert.ThrowsException<ArgumentNullException>(() => encoded.DecodeBytesFromBase64());

            Assert.AreEqual(oldEx.ParamName, newEx.ParamName);
        }

        [DataRow("%%%")]
        [DataRow("AQI")]
        [DataRow("not base64!")]
        [TestMethod]
        public void DecodeBytesFromBase64_InvalidFormat_Test(string encoded)
        {
            Assert.ThrowsException<FormatException>(() => encoded.DecodeBytesFromBase64());
        }

        [DataRow("%%%")]
        [DataRow("AQI")]
        [TestMethod]
        public void DecodeBytesFromBase64_InvalidFormatMatchesOldMember_Test(string encoded)
        {
            Assert.ThrowsException<FormatException>(() => encoded.DeCodeBytesFromBase64());
            Assert.ThrowsException<FormatException>(() => encoded.DecodeBytesFromBase64());
        }

        [DataRow(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc)]
        [DataRow(2024, 5, 6, 7, 8, 9, 123, DateTimeKind.Utc)]
        [DataRow(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)]
        [DataRow(1969, 12, 31, 23, 59, 59, 500, DateTimeKind.Utc)]
        [DataRow(2024, 5, 6, 7, 8, 9, 123, DateTimeKind.Local)]
        [DataRow(2000, 2, 29, 12, 0, 0, 999, DateTimeKind.Local)]
        [DataRow(2024, 5, 6, 7, 8, 9, 123, DateTimeKind.Unspecified)]
        [DataRow(1999, 12, 31, 23, 59, 59, 1, DateTimeKind.Unspecified)]
        [TestMethod]
        public void ToUnixTimeSeconds_MatchesOldMember_Test(int year, int month, int day, int hour, int minute, int second, int millisecond, DateTimeKind kind)
        {
            var source = new DateTime(year, month, day, hour, minute, second, millisecond, kind);

            var result = source.ToUnixTimeSeconds();

            Assert.AreEqual(source.ToEpochTime(), result);
        }

        [DataRow(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc)]
        [DataRow(2024, 5, 6, 7, 8, 9, 123, DateTimeKind.Utc)]
        [DataRow(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)]
        [DataRow(1969, 12, 31, 23, 59, 59, 500, DateTimeKind.Utc)]
        [DataRow(2024, 5, 6, 7, 8, 9, 123, DateTimeKind.Local)]
        [DataRow(2000, 2, 29, 12, 0, 0, 999, DateTimeKind.Local)]
        [DataRow(2024, 5, 6, 7, 8, 9, 123, DateTimeKind.Unspecified)]
        [DataRow(1999, 12, 31, 23, 59, 59, 1, DateTimeKind.Unspecified)]
        [TestMethod]
        public void ToUnixTimeMilliseconds_MatchesOldMember_Test(int year, int month, int day, int hour, int minute, int second, int millisecond, DateTimeKind kind)
        {
            var source = new DateTime(year, month, day, hour, minute, second, millisecond, kind);

            var result = source.ToUnixTimeMilliseconds();

            Assert.AreEqual(source.ToEpoch(), result);
        }

        [TestMethod]
        public void ToUnixTimeSeconds_UnixEpochUtc_Test()
        {
            var source = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var result = source.ToUnixTimeSeconds();

            Assert.AreEqual(0L, result);
        }

        [TestMethod]
        public void ToUnixTimeMilliseconds_UnixEpochUtc_Test()
        {
            var source = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var result = source.ToUnixTimeMilliseconds();

            Assert.AreEqual(0L, result);
        }

        [TestMethod]
        public void ToUnixTimeSeconds_KnownUtcDate_Test()
        {
            var source = new DateTime(2024, 1, 15, 0, 0, 0, DateTimeKind.Utc);

            var result = source.ToUnixTimeSeconds();

            Assert.AreEqual(1705276800L, result);
        }

        [TestMethod]
        public void ToUnixTimeMilliseconds_KnownUtcDate_Test()
        {
            var source = new DateTime(2024, 1, 15, 0, 0, 0, DateTimeKind.Utc);

            var result = source.ToUnixTimeMilliseconds();

            Assert.AreEqual(1705276800000L, result);
        }

        [TestMethod]
        public void ToUnixTimeSeconds_PreEpochTruncatesTowardZero_Test()
        {
            var source = new DateTime(1969, 12, 31, 23, 59, 59, 500, DateTimeKind.Utc);

            var result = source.ToUnixTimeSeconds();

            Assert.AreEqual(0L, result);
        }

        [TestMethod]
        public void ToUnixTimeMilliseconds_PreEpochHalfSecond_Test()
        {
            var source = new DateTime(1969, 12, 31, 23, 59, 59, 500, DateTimeKind.Utc);

            var result = source.ToUnixTimeMilliseconds();

            Assert.AreEqual(-500L, result);
        }

        [TestMethod]
        public void ToUnixTimeSeconds_UtcMatchesDateTimeOffset_Test()
        {
            var source = new DateTime(2024, 5, 6, 7, 8, 9, 123, DateTimeKind.Utc);

            var result = source.ToUnixTimeSeconds();

            Assert.AreEqual(new DateTimeOffset(source).ToUnixTimeSeconds(), result);
        }

        [TestMethod]
        public void ToUnixTimeMilliseconds_UtcMatchesDateTimeOffset_Test()
        {
            var source = new DateTime(2024, 5, 6, 7, 8, 9, 123, DateTimeKind.Utc);

            var result = source.ToUnixTimeMilliseconds();

            Assert.AreEqual(new DateTimeOffset(source).ToUnixTimeMilliseconds(), result);
        }

        [TestMethod]
        public void ToUnixTimeMilliseconds_NotSwappedWithSeconds_Test()
        {
            var source = new DateTime(2024, 5, 6, 7, 8, 9, 123, DateTimeKind.Utc);

            var seconds = source.ToUnixTimeSeconds();
            var milliseconds = source.ToUnixTimeMilliseconds();

            Assert.AreEqual(seconds * 1000L + 123L, milliseconds);
        }

        [TestMethod]
        public void ToUnixTimeSeconds_DateTimeOffsetBindsBcl_Test()
        {
            var source = new DateTimeOffset(1969, 12, 31, 23, 59, 59, 500, TimeSpan.Zero);

            var result = source.ToUnixTimeSeconds();

            Assert.AreEqual(-1L, result);
        }

        [TestMethod]
        public void ToUnixTimeMilliseconds_DateTimeOffsetBindsBcl_Test()
        {
            var source = new DateTimeOffset(1969, 12, 31, 23, 59, 59, 500, TimeSpan.Zero);

            var result = source.ToUnixTimeMilliseconds();

            Assert.AreEqual(-500L, result);
        }
    }
}
