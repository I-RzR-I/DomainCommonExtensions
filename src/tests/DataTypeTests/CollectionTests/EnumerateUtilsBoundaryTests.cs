using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Collections;

namespace DataTypeTests.CollectionTests
{
    [TestClass]
    public class EnumerateUtilsBoundaryTests
    {
        [TestMethod]
        public void FromTo_DefaultStep_Test()
        {
            var result = EnumerateUtils.FromTo(1, 5).Take(10).ToArray();

            CollectionAssert.AreEqual(new[] { 1, 2, 3, 4, 5 }, result);
        }

        [TestMethod]
        public void FromTo_StepThree_Test()
        {
            var result = EnumerateUtils.FromTo(1, 10, 3).Take(10).ToArray();

            CollectionAssert.AreEqual(new[] { 1, 4, 7, 10 }, result);
        }

        [TestMethod]
        public void FromTo_FromGreaterThanTo_Test()
        {
            var result = EnumerateUtils.FromTo(5, 1).Take(10).ToArray();

            Assert.AreEqual(0, result.Length);
        }

        [TestMethod]
        public void FromTo_ZeroStep_Test()
        {
            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => EnumerateUtils.FromTo(1, 5, 0));

            Assert.AreEqual("stepSize", exception.ParamName);
        }

        [TestMethod]
        public void FromTo_NegativeStep_Test()
        {
            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => EnumerateUtils.FromTo(1, 5, -1));

            Assert.AreEqual("stepSize", exception.ParamName);
        }

        [TestMethod]
        public void FromTo_UpToIntMaxValue_Test()
        {
            var result = EnumerateUtils.FromTo(int.MaxValue - 2, int.MaxValue).Take(10).ToArray();

            CollectionAssert.AreEqual(new[] { int.MaxValue - 2, int.MaxValue - 1, int.MaxValue }, result);
        }

        [TestMethod]
        public void FromTo_StepOverflowsIntMaxValue_Test()
        {
            var result = EnumerateUtils.FromTo(int.MaxValue - 1, int.MaxValue, 5).Take(10).ToArray();

            CollectionAssert.AreEqual(new[] { int.MaxValue - 1 }, result);
        }

        [TestMethod]
        public void FromToDateTime_ThreeConsecutiveDays_Test()
        {
            var from = new DateTime(2024, 1, 1);
            var to = new DateTime(2024, 1, 3);

            var result = EnumerateUtils.FromTo(from, to, TimeSpan.FromDays(1)).Take(10).ToArray();

            CollectionAssert.AreEqual(new[] { from, from.AddDays(1), to }, result);
        }

        [TestMethod]
        public void FromToDateTime_ZeroStep_Test()
        {
            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => EnumerateUtils.FromTo(new DateTime(2024, 1, 1), new DateTime(2024, 1, 3), TimeSpan.Zero));

            Assert.AreEqual("stepSize", exception.ParamName);
        }

        [TestMethod]
        public void FromToDateTime_NegativeStep_Test()
        {
            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => EnumerateUtils.FromTo(new DateTime(2024, 1, 1), new DateTime(2024, 1, 3), TimeSpan.FromDays(-1)));

            Assert.AreEqual("stepSize", exception.ParamName);
        }

        [TestMethod]
        public void FromToDateTime_UpToDateTimeMaxValue_Test()
        {
            var from = DateTime.MaxValue.AddDays(-2);

            var result = EnumerateUtils.FromTo(from, DateTime.MaxValue, TimeSpan.FromDays(1)).Take(10).ToArray();

            Assert.AreEqual(3, result.Length);
            Assert.AreEqual(from, result[0]);
            Assert.AreEqual(DateTime.MaxValue, result[2]);
        }

        [TestMethod]
        public void PowersOf_TwoUpToTen_Test()
        {
            var result = EnumerateUtils.PowersOf(2, 10).Take(5000).ToArray();

            CollectionAssert.AreEqual(new[] { 1d, 2d, 4d, 8d }, result);
        }

        [TestMethod]
        public void PowersOf_LimitBelowOne_Test()
        {
            var result = EnumerateUtils.PowersOf(2, 0.5).Take(5000).ToArray();

            Assert.AreEqual(0, result.Length);
        }

        [TestMethod]
        public void PowersOf_NumberOne_Test()
        {
            var result = EnumerateUtils.PowersOf(1, 10).Take(5000).ToArray();

            CollectionAssert.AreEqual(new[] { 1d }, result);
        }

        [TestMethod]
        public void PowersOf_NumberZero_Test()
        {
            var result = EnumerateUtils.PowersOf(0, 10).Take(5000).ToArray();

            CollectionAssert.AreEqual(new[] { 1d, 0d }, result);
        }

        [TestMethod]
        public void PowersOf_NumberMinusOne_Test()
        {
            var result = EnumerateUtils.PowersOf(-1, 10).Take(5000).ToArray();

            CollectionAssert.AreEqual(new[] { 1d, -1d }, result);
        }

        [TestMethod]
        public void PowersOf_NumberMinusTwo_Test()
        {
            var result = EnumerateUtils.PowersOf(-2, 10).Take(5000).ToArray();

            CollectionAssert.AreEqual(new[] { 1d, -2d, 4d, -8d }, result);
        }

        [TestMethod]
        public void PowersOf_InfiniteLimit_Test()
        {
            var result = EnumerateUtils.PowersOf(2, double.PositiveInfinity).Take(5000).ToArray();

            Assert.AreEqual(1024, result.Length);
            Assert.AreEqual(Math.Pow(2, 1023), result[result.Length - 1]);
            Assert.IsFalse(result.Any(double.IsInfinity));
        }

        [TestMethod]
        public void PowersOf_FractionNumber_Test()
        {
            var result = EnumerateUtils.PowersOf(0.5, 10).Take(5000).ToArray();

            Assert.IsTrue(result.Length < 5000);
            Assert.IsTrue(result.Length >= 3);
            Assert.AreEqual(1d, result[0]);
            Assert.AreEqual(0.5d, result[1]);
            Assert.AreEqual(0.25d, result[2]);
        }

        [TestMethod]
        public void PowersOf_NaNNumber_Test()
        {
            var result = EnumerateUtils.PowersOf(double.NaN, 10).Take(5000).ToArray();

            CollectionAssert.AreEqual(new[] { 1d }, result);
        }
    }
}
