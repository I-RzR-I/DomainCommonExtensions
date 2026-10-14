using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Primitives;

namespace DataTypeTests.HelperTests
{
    [TestClass]
    public class RandomHelperLongRangeTests
    {
        [TestMethod]
        public void Long_FullRangeNoThrow_Test()
        {
            var rnd = new RandomHelper(new Random(42));

            var values = new List<long>();
            for (var i = 0; i < 20; i++)
                values.Add(rnd.Long(long.MinValue, long.MaxValue));

            Assert.AreEqual(20, values.Count);
        }

        [TestMethod]
        public void Long_FullRangeNotConstant_Test()
        {
            var rnd = new RandomHelper(new Random(42));

            var values = new List<long>();
            for (var i = 0; i < 20; i++)
                values.Add(rnd.Long(long.MinValue, long.MaxValue));

            Assert.IsTrue(values.Distinct().Count() > 1);
        }

        [TestMethod]
        public void Long_FullRangeSeededDeterministic_Test()
        {
            var first = new RandomHelper(new Random(42));
            var second = new RandomHelper(new Random(42));

            var firstValues = new List<long>();
            var secondValues = new List<long>();
            for (var i = 0; i < 10; i++)
            {
                firstValues.Add(first.Long(long.MinValue, long.MaxValue));
                secondValues.Add(second.Long(long.MinValue, long.MaxValue));
            }

            CollectionAssert.AreEqual(firstValues, secondValues);
        }

        [DataRow(long.MinValue, long.MaxValue - 1)]
        [DataRow(long.MinValue + 1, long.MaxValue)]
        [TestMethod]
        public void Long_RangeOneBelowFull_Test(long min, long max)
        {
            var rnd = new RandomHelper(new Random(42));

            var values = new List<long>();
            for (var i = 0; i < 20; i++)
                values.Add(rnd.Long(min, max));

            Assert.IsTrue(values.All(x => x >= min && x <= max));
            Assert.IsTrue(values.Distinct().Count() > 1);
        }

        [TestMethod]
        public void Long_RangeAboveHalf_Test()
        {
            var rnd = new RandomHelper(new Random(42));
            var min = long.MinValue;
            var max = long.MaxValue / 2;

            var values = new List<long>();
            for (var i = 0; i < 50; i++)
                values.Add(rnd.Long(min, max));

            Assert.IsTrue(values.All(x => x >= min && x <= max));
            Assert.IsTrue(values.Any(x => x >= long.MinValue / 2));
        }

        [DataRow(100L, 200L)]
        [DataRow(-1000L, 1000000000000L)]
        [DataRow(0L, long.MaxValue - 1)]
        [DataRow(0L, long.MaxValue)]
        [DataRow(-5L, -5L)]
        [TestMethod]
        public void Long_InRangeSeededKnownAnswer_Test(long min, long max)
        {
            var oracle = new Random(42);
            var rnd = new RandomHelper(new Random(42));

            for (var i = 0; i < 5; i++)
            {
                var buffer = new byte[8];
                oracle.NextBytes(buffer);
                var expected = Math.Abs(BitConverter.ToInt64(buffer, 0) % (max - min + 1)) + min;

                var actual = rnd.Long(min, max);

                Assert.AreEqual(expected, actual, $"Call: {i}");
            }
        }

        [TestMethod]
        public void Long_SingleValueRange_Test()
        {
            var rnd = new RandomHelper(new Random(42));

            for (var i = 0; i < 10; i++)
                Assert.AreEqual(long.MaxValue, rnd.Long(long.MaxValue, long.MaxValue));
        }

        [DataRow(long.MaxValue, long.MinValue)]
        [DataRow(0L, -1L)]
        [TestMethod]
        public void Long_MaxBelowMin_Test(long min, long max)
        {
            var rnd = new RandomHelper(new Random(42));

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => rnd.Long(min, max));
        }

        [TestMethod]
        public void TimeSpan_FullRange_Test()
        {
            var rnd = new RandomHelper(new Random(42));

            var values = new List<TimeSpan>();
            for (var i = 0; i < 20; i++)
                values.Add(rnd.TimeSpan(TimeSpan.MinValue, TimeSpan.MaxValue));

            Assert.IsTrue(values.Distinct().Count() > 1);
        }
    }
}
