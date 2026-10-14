using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Primitives;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class LongFromUnixSecondsTests
    {
        [TestMethod]
        public void FromUnixSeconds_Zero_Test()
        {
            var result = 0L.FromUnixSeconds();

            Assert.AreEqual(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc), result);
            Assert.AreEqual(DateTimeKind.Utc, result.Kind);
        }

        [TestMethod]
        public void FromUnixSeconds_PositiveValue_Test()
        {
            var result = 1700000000L.FromUnixSeconds();

            Assert.AreEqual(new DateTime(2023, 11, 14, 22, 13, 20, DateTimeKind.Utc), result);
            Assert.AreEqual(DateTimeKind.Utc, result.Kind);
        }

        [TestMethod]
        public void FromUnixSeconds_NegativeOne_Test()
        {
            long seconds = -1;

            var result = seconds.FromUnixSeconds();

            Assert.AreEqual(new DateTime(1969, 12, 31, 23, 59, 59, DateTimeKind.Utc), result);
            Assert.AreEqual(DateTimeKind.Utc, result.Kind);
        }

        [TestMethod]
        public void FromUnixSeconds_NegativeValue_Test()
        {
            long seconds = -2208988800;

            var result = seconds.FromUnixSeconds();

            Assert.AreEqual(new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc), result);
            Assert.AreEqual(DateTimeKind.Utc, result.Kind);
        }

        [TestMethod]
        public void FromUnixSeconds_MinBound_Test()
        {
            long seconds = -62135596800;

            var result = seconds.FromUnixSeconds();

            Assert.AreEqual(DateTime.MinValue.Ticks, result.Ticks);
            Assert.AreEqual(DateTimeKind.Utc, result.Kind);
        }

        [TestMethod]
        public void FromUnixSeconds_MaxBound_Test()
        {
            var result = 253402300799L.FromUnixSeconds();

            Assert.AreEqual(new DateTime(9999, 12, 31, 23, 59, 59, DateTimeKind.Utc), result);
            Assert.AreEqual(DateTime.MaxValue.Ticks - (TimeSpan.TicksPerSecond - 1), result.Ticks);
            Assert.AreEqual(DateTimeKind.Utc, result.Kind);
        }

        [DataRow(-62135596801L)]
        [DataRow(253402300800L)]
        [DataRow(long.MaxValue)]
        [DataRow(long.MinValue)]
        [TestMethod]
        public void FromUnixSeconds_OutOfRange_Test(long seconds)
        {
            var ex = Assert.ThrowsException<ArgumentOutOfRangeException>(() => seconds.FromUnixSeconds());

            Assert.AreEqual("seconds", ex.ParamName);
        }

        [DataRow(0L)]
        [DataRow(1L)]
        [DataRow(-1L)]
        [DataRow(86399L)]
        [DataRow(951782400L)]
        [DataRow(1700000000L)]
        [DataRow(2147483647L)]
        [DataRow(2147483648L)]
        [DataRow(-2147483648L)]
        [DataRow(-2208988800L)]
        [DataRow(-62135596800L)]
        [DataRow(253402300799L)]
        [TestMethod]
        public void FromUnixSeconds_MatchesDateTimeOffset_Test(long seconds)
        {
            var expected = DateTimeOffset.FromUnixTimeSeconds(seconds).UtcDateTime;

            var result = seconds.FromUnixSeconds();

            Assert.AreEqual(expected.Ticks, result.Ticks);
            Assert.AreEqual(expected.Kind, result.Kind);
        }

        [DataRow(0L)]
        [DataRow(-1L)]
        [DataRow(1700000000L)]
        [DataRow(-62135596800L)]
        [DataRow(253402300799L)]
        [TestMethod]
        public void TryFromUnixSeconds_InRange_Test(long seconds)
        {
            var success = seconds.TryFromUnixSeconds(out var result);

            Assert.IsTrue(success);
            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(seconds).UtcDateTime.Ticks, result.Ticks);
            Assert.AreEqual(DateTimeKind.Utc, result.Kind);
        }

        [DataRow(-62135596801L)]
        [DataRow(253402300800L)]
        [DataRow(long.MaxValue)]
        [DataRow(long.MinValue)]
        [TestMethod]
        public void TryFromUnixSeconds_OutOfRange_Test(long seconds)
        {
            var success = seconds.TryFromUnixSeconds(out var result);

            Assert.IsFalse(success);
            Assert.AreEqual(0L, result.Ticks);
            Assert.AreEqual(DateTimeKind.Unspecified, result.Kind);
        }

        [DataRow(0L)]
        [DataRow(-1L)]
        [DataRow(951782400L)]
        [DataRow(-62135596800L)]
        [DataRow(253402300799L)]
        [TestMethod]
        public void TryFromUnixSeconds_MatchesFromUnixSeconds_Test(long seconds)
        {
            Assert.IsTrue(seconds.TryFromUnixSeconds(out var tryResult));

            var result = seconds.FromUnixSeconds();

            Assert.AreEqual(result.Ticks, tryResult.Ticks);
            Assert.AreEqual(result.Kind, tryResult.Kind);
        }

        [TestMethod]
        public void TryFromUnixSeconds_LeapDay_Test()
        {
            var success = 951782400L.TryFromUnixSeconds(out var result);

            Assert.IsTrue(success);
            Assert.AreEqual(new DateTime(2000, 2, 29, 0, 0, 0, DateTimeKind.Utc), result);
        }
    }
}
