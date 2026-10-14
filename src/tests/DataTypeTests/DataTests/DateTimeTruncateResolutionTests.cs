using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Primitives;
using RzR.Extensions.Domain.Text;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class DateTimeTruncateResolutionTests
    {
        [TestMethod]
        public void Truncate_ToSecond_Test()
        {
            var value = new DateTime(2024, 5, 6, 12, 34, 56, 789).AddTicks(1234);

            var result = value.Truncate(TimeSpan.FromSeconds(1));

            Assert.AreEqual(new DateTime(2024, 5, 6, 12, 34, 56), result);
        }

        [TestMethod]
        public void Truncate_ToMillisecond_Test()
        {
            var value = new DateTime(2024, 5, 6, 12, 34, 56, 789).AddTicks(1234);

            var result = value.Truncate(TimeSpan.FromMilliseconds(1));

            Assert.AreEqual(new DateTime(2024, 5, 6, 12, 34, 56, 789), result);
        }

        [TestMethod]
        public void Truncate_ToMinute_Test()
        {
            var value = new DateTime(2024, 5, 6, 12, 34, 56, 789);

            var result = value.Truncate(TimeSpan.FromMinutes(1));

            Assert.AreEqual(new DateTime(2024, 5, 6, 12, 34, 0), result);
        }

        [TestMethod]
        public void Truncate_ToHour_Test()
        {
            var value = new DateTime(2024, 5, 6, 12, 34, 56, 789);

            var result = value.Truncate(TimeSpan.FromHours(1));

            Assert.AreEqual(new DateTime(2024, 5, 6, 12, 0, 0), result);
        }

        [TestMethod]
        public void Truncate_ToDay_Test()
        {
            var value = new DateTime(2024, 5, 6, 23, 59, 59, 999).AddTicks(9999);

            var result = value.Truncate(TimeSpan.FromDays(1));

            Assert.AreEqual(new DateTime(2024, 5, 6), result);
        }

        [TestMethod]
        public void Truncate_ToQuarterHour_Test()
        {
            var value = new DateTime(2024, 5, 6, 10, 44, 59, 999);

            var result = value.Truncate(TimeSpan.FromMinutes(15));

            Assert.AreEqual(new DateTime(2024, 5, 6, 10, 30, 0), result);
        }

        [TestMethod]
        public void Truncate_WeekAlignsToMinValue_Test()
        {
            var value = new DateTime(2024, 1, 3, 15, 0, 0);

            var result = value.Truncate(TimeSpan.FromDays(7));

            Assert.AreEqual(new DateTime(2024, 1, 1), result);
            Assert.AreEqual(0L, result.Ticks % TimeSpan.FromDays(7).Ticks);
        }

        [TestMethod]
        public void Truncate_TicksFormula_Test()
        {
            var value = new DateTime(2024, 5, 6, 12, 34, 56, 789).AddTicks(4321);
            var resolution = TimeSpan.FromTicks(7777);

            var result = value.Truncate(resolution);

            Assert.AreEqual(value.Ticks - value.Ticks % resolution.Ticks, result.Ticks);
        }

        [DataRow(DateTimeKind.Utc)]
        [DataRow(DateTimeKind.Local)]
        [DataRow(DateTimeKind.Unspecified)]
        [TestMethod]
        public void Truncate_PreservesKind_Test(DateTimeKind kind)
        {
            var value = new DateTime(2024, 5, 6, 12, 34, 56, 789, kind);

            var result = value.Truncate(TimeSpan.FromSeconds(1));

            Assert.AreEqual(kind, result.Kind);
            Assert.AreEqual(new DateTime(2024, 5, 6, 12, 34, 56, kind).Ticks, result.Ticks);
        }

        [DataRow(DateTimeKind.Utc)]
        [DataRow(DateTimeKind.Local)]
        [DataRow(DateTimeKind.Unspecified)]
        [TestMethod]
        public void Truncate_AlignedValueUnchanged_Test(DateTimeKind kind)
        {
            var value = new DateTime(2024, 5, 6, 12, 34, 0, kind);

            var result = value.Truncate(TimeSpan.FromMinutes(1));

            Assert.AreEqual(value.Ticks, result.Ticks);
            Assert.AreEqual(kind, result.Kind);
        }

        [TestMethod]
        public void Truncate_OneTickResolutionUnchanged_Test()
        {
            var value = new DateTime(2024, 5, 6, 12, 34, 56, 789, DateTimeKind.Utc).AddTicks(1);

            var result = value.Truncate(TimeSpan.FromTicks(1));

            Assert.AreEqual(value.Ticks, result.Ticks);
            Assert.AreEqual(DateTimeKind.Utc, result.Kind);
        }

        [TestMethod]
        public void Truncate_MaxValueToSecond_Test()
        {
            var result = DateTime.MaxValue.Truncate(TimeSpan.FromSeconds(1));

            Assert.AreEqual(new DateTime(9999, 12, 31, 23, 59, 59), result);
        }

        [TestMethod]
        public void Truncate_MaxValueToDay_Test()
        {
            var result = DateTime.MaxValue.Truncate(TimeSpan.FromDays(1));

            Assert.AreEqual(new DateTime(9999, 12, 31), result);
        }

        [TestMethod]
        public void Truncate_MaxValueOneTickUnchanged_Test()
        {
            var result = DateTime.MaxValue.Truncate(TimeSpan.FromTicks(1));

            Assert.AreEqual(DateTime.MaxValue, result);
        }

        [TestMethod]
        public void Truncate_MinValueUnchanged_Test()
        {
            var result = DateTime.MinValue.Truncate(TimeSpan.FromDays(1));

            Assert.AreEqual(DateTime.MinValue, result);
        }

        [TestMethod]
        public void Truncate_MaxResolution_Test()
        {
            var value = new DateTime(2024, 5, 6, 12, 34, 56, DateTimeKind.Utc);

            var result = value.Truncate(TimeSpan.MaxValue);

            Assert.AreEqual(DateTime.MinValue, result);
            Assert.AreEqual(DateTimeKind.Utc, result.Kind);
        }

        [TestMethod]
        public void Truncate_ZeroResolution_Test()
        {
            var value = new DateTime(2024, 5, 6);

            var ex = Assert.ThrowsException<ArgumentOutOfRangeException>(() => value.Truncate(TimeSpan.Zero));

            Assert.AreEqual("resolution", ex.ParamName);
        }

        [DataRow(-1L)]
        [DataRow(-10000000L)]
        [DataRow(long.MinValue)]
        [TestMethod]
        public void Truncate_NegativeResolution_Test(long ticks)
        {
            var value = new DateTime(2024, 5, 6);
            var resolution = TimeSpan.FromTicks(ticks);

            var ex = Assert.ThrowsException<ArgumentOutOfRangeException>(() => value.Truncate(resolution));

            Assert.AreEqual("resolution", ex.ParamName);
        }

        [TestMethod]
        public void Truncate_StringOverloadStillBinds_Test()
        {
            string result = "abcdef".Truncate(3);

            Assert.AreEqual("abc", result);
        }

        [TestMethod]
        public void Truncate_StringOverloadWithDotsStillBinds_Test()
        {
            string result = "abcdef".Truncate(5, true);

            Assert.AreEqual("ab...", result);
        }
    }
}
