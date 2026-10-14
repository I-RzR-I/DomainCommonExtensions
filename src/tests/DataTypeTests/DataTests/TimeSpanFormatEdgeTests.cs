using System;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Primitives;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class TimeSpanFormatEdgeTests
    {
        [TestMethod]
        public void ToHumanReadable_ZeroDuration_Test()
        {
            using (new CultureScopeTestHelper(""))
            {
                var result = TimeSpan.Zero.ToHumanReadable();

                Assert.AreEqual("0s", result);
            }
        }

        [TestMethod]
        public void ToHumanReadable_NinetyMinutes_Test()
        {
            using (new CultureScopeTestHelper(""))
            {
                var result = TimeSpan.FromMinutes(90).ToHumanReadable();

                Assert.AreEqual("1h 30m", result);
            }
        }

        [TestMethod]
        public void ToHumanReadable_AllComponents_Test()
        {
            using (new CultureScopeTestHelper(""))
            {
                var result = new TimeSpan(1, 2, 3, 4).ToHumanReadable();

                Assert.AreEqual("1d 2h 3m 4s", result);
            }
        }

        [TestMethod]
        public void ToHumanReadable_SubSecondDuration_Test()
        {
            using (new CultureScopeTestHelper(""))
            {
                var result = TimeSpan.FromMilliseconds(500).ToHumanReadable();

                Assert.AreEqual("0s", result);
            }
        }

        [TestMethod]
        public void ToHumanReadable_NegativeNinetyMinutes_Test()
        {
            using (new CultureScopeTestHelper(""))
            {
                var result = TimeSpan.FromMinutes(-90).ToHumanReadable();

                Assert.AreEqual("-1h 30m", result);
            }
        }

        [TestMethod]
        public void ToHumanReadable_MinValue_Test()
        {
            using (new CultureScopeTestHelper(""))
            {
                var result = TimeSpan.MinValue.ToHumanReadable();

                Assert.AreEqual("-10675199d 2h 48m 5s", result);
            }
        }

        [TestMethod]
        public void ToClockFormat_NinetyMinutes_Test()
        {
            using (new CultureScopeTestHelper(""))
            {
                var result = TimeSpan.FromMinutes(90).ToClockFormat();

                Assert.AreEqual("01:30:00", result);
            }
        }

        [TestMethod]
        public void ToClockFormat_HundredHours_Test()
        {
            using (new CultureScopeTestHelper(""))
            {
                var result = TimeSpan.FromHours(100).ToClockFormat();

                Assert.AreEqual("100:00:00", result);
            }
        }

        [TestMethod]
        public void ToClockFormat_NegativeNinetyMinutes_Test()
        {
            using (new CultureScopeTestHelper(""))
            {
                var result = TimeSpan.FromMinutes(-90).ToClockFormat();

                Assert.AreEqual("-01:30:00", result);
            }
        }

        [TestMethod]
        public void ToClockFormat_NegativeThirtySeconds_Test()
        {
            using (new CultureScopeTestHelper(""))
            {
                var result = TimeSpan.FromSeconds(-30).ToClockFormat();

                Assert.AreEqual("-00:00:30", result);
            }
        }

        [TestMethod]
        public void ToClockFormat_MinValue_Test()
        {
            using (new CultureScopeTestHelper(""))
            {
                var result = TimeSpan.MinValue.ToClockFormat();

                Assert.AreEqual("-256204778:48:05", result);
            }
        }

        [TestMethod]
        public void TotalSecondsInt_NinetySeconds_Test()
        {
            var result = TimeSpan.FromSeconds(90).TotalSecondsInt();

            Assert.AreEqual(90, result);
        }

        [TestMethod]
        public void TotalSecondsInt_PositiveOverflow_Test()
        {
            var result = TimeSpan.FromDays(25000).TotalSecondsInt();

            Assert.AreEqual(int.MaxValue, result);
        }

        [TestMethod]
        public void TotalSecondsInt_NegativeOverflow_Test()
        {
            var result = TimeSpan.FromDays(-25000).TotalSecondsInt();

            Assert.AreEqual(int.MinValue, result);
        }

        [TestMethod]
        public void TotalMinutesInt_NinetyMinutes_Test()
        {
            var result = TimeSpan.FromMinutes(90).TotalMinutesInt();

            Assert.AreEqual(90, result);
        }

        [TestMethod]
        public void TotalMinutesInt_MaxValue_Test()
        {
            var result = TimeSpan.MaxValue.TotalMinutesInt();

            Assert.AreEqual(int.MaxValue, result);
        }

        [TestMethod]
        public void TotalMinutesInt_MinValue_Test()
        {
            var result = TimeSpan.MinValue.TotalMinutesInt();

            Assert.AreEqual(int.MinValue, result);
        }

        [TestMethod]
        public void ToHumanReadable_NegativeSubSecond_Test()
        {
            Assert.AreEqual("0s", TimeSpan.FromMilliseconds(-500).ToHumanReadable());
        }

        [TestMethod]
        public void ToClockFormat_NegativeSubSecond_Test()
        {
            Assert.AreEqual("00:00:00", TimeSpan.FromMilliseconds(-500).ToClockFormat());
        }
    }
}
