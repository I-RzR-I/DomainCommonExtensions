// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2026-01-18 22:01
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-01-18 22:41
// ***********************************************************************
//  <copyright file="TimeSpanTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using DomainCommonExtensions.DataTypeExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class TimeSpanTests
    {
        [TestMethod]
        public void IsMissing_TimeSpan_ReturnsTrue_For_Zero()
        {
            Assert.IsTrue(TimeSpan.Zero.IsMissing());
        }

        [TestMethod]
        public void IsMissing_TimeSpan_ReturnsTrue_For_Negative()
        {
            Assert.IsTrue(TimeSpan.FromSeconds(-1).IsMissing());
        }

        [TestMethod]
        public void IsMissing_TimeSpan_ReturnsFalse_For_Positive()
        {
            Assert.IsFalse(TimeSpan.FromSeconds(1).IsMissing());
        }

        [TestMethod]
        public void IsMissing_NullableTimeSpan_ReturnsTrue_For_Null()
        {
            TimeSpan? value = null;

            Assert.IsTrue(value.IsMissing());
        }

        [TestMethod]
        public void IsMissing_NullableTimeSpan_ReturnsTrue_For_Zero()
        {
            TimeSpan? value = TimeSpan.Zero;

            Assert.IsTrue(value.IsMissing());
        }

        [TestMethod]
        public void HasValidValue_TimeSpan_ReturnsTrue_For_Positive()
        {
            Assert.IsTrue(TimeSpan.FromMilliseconds(1).HasValidValue());
        }

        [TestMethod]
        public void HasValidValue_TimeSpan_ReturnsFalse_For_Zero()
        {
            Assert.IsFalse(TimeSpan.Zero.HasValidValue());
        }

        [TestMethod]
        public void HasValidValue_NullableTimeSpan_ReturnsFalse_For_Null()
        {
            TimeSpan? value = null;

            Assert.IsFalse(value.HasValidValue());
        }

        [TestMethod]
        public void IsZero_Works()
        {
            Assert.IsTrue(TimeSpan.Zero.IsZero());
            Assert.IsFalse(TimeSpan.FromTicks(1).IsZero());
        }

        [TestMethod]
        public void IsPositive_Works()
        {
            Assert.IsTrue(TimeSpan.FromSeconds(1).IsPositive());
            Assert.IsFalse(TimeSpan.Zero.IsPositive());
        }

        [TestMethod]
        public void IsNegative_Works()
        {
            Assert.IsTrue(TimeSpan.FromSeconds(-1).IsNegative());
            Assert.IsFalse(TimeSpan.Zero.IsNegative());
        }

        [TestMethod]
        public void IsBetween_Works()
        {
            var value = TimeSpan.FromSeconds(5);

            Assert.IsTrue(value.IsBetween(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(10)));
            Assert.IsFalse(value.IsBetween(TimeSpan.FromSeconds(6), TimeSpan.FromSeconds(10)));
        }

        [TestMethod]
        public void DefaultIfNull_Works()
        {
            TimeSpan? value = null;
            var defaultValue = TimeSpan.FromSeconds(10);

            Assert.AreEqual(defaultValue, value.DefaultIfNull(defaultValue));
        }

        [TestMethod]
        public void ZeroIfNull_Works()
        {
            TimeSpan? value = null;

            Assert.AreEqual(TimeSpan.Zero, value.ZeroIfNull());
        }

        [TestMethod]
        public void RoundUp_Works()
        {
            var value = TimeSpan.FromSeconds(61);
            var interval = TimeSpan.FromSeconds(30);

            var result = value.RoundUp(interval);

            Assert.AreEqual(TimeSpan.FromSeconds(90), result);
        }

        [TestMethod]
        public void RoundDown_Works()
        {
            var value = TimeSpan.FromSeconds(61);
            var interval = TimeSpan.FromSeconds(30);

            var result = value.RoundDown(interval);

            Assert.AreEqual(TimeSpan.FromSeconds(60), result);
        }

        [TestMethod]
        public void RoundUp_Throws_For_Invalid_Interval()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => TimeSpan.FromSeconds(1).RoundUp(TimeSpan.Zero));
        }

        [TestMethod]
        public void RoundDown_Throws_For_Invalid_Interval()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => TimeSpan.FromSeconds(1).RoundDown(TimeSpan.Zero));
        }

        [TestMethod]
        public void Clamp_Returns_Min_When_Below()
        {
            var result = TimeSpan.FromSeconds(1)
                .Clamp(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10));

            Assert.AreEqual(TimeSpan.FromSeconds(5), result);
        }

        [TestMethod]
        public void Clamp_Returns_Max_When_Above()
        {
            var result = TimeSpan.FromSeconds(20)
                .Clamp(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10));

            Assert.AreEqual(TimeSpan.FromSeconds(10), result);
        }

        [TestMethod]
        public void Clamp_Returns_Source_When_Inside()
        {
            var result = TimeSpan.FromSeconds(7)
                .Clamp(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10));

            Assert.AreEqual(TimeSpan.FromSeconds(7), result);
        }

        [TestMethod]
        public void Clamp_Throws_When_Min_Greater_Than_Max()
        {
            Assert.ThrowsException<ArgumentException>(() => TimeSpan.FromSeconds(5)
                .Clamp(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(1)));
        }

        [TestMethod]
        public void TotalSecondsInt_Works()
        {
            Assert.AreEqual(61, TimeSpan.FromSeconds(61.9).TotalSecondsInt());
        }

        [TestMethod]
        public void TotalMinutesInt_Works()
        {
            Assert.AreEqual(2, TimeSpan.FromMinutes(2.5).TotalMinutesInt());
        }

        [TestMethod]
        public void TotalMillisecondsLong_Works()
        {
            Assert.AreEqual(1500L, TimeSpan.FromMilliseconds(1500.8).TotalMillisecondsLong());
        }

        [TestMethod]
        public void ToClockFormat_Works()
        {
            var value = new TimeSpan(1, 2, 3);
            Assert.AreEqual("01:02:03", value.ToClockFormat());
        }

        [TestMethod]
        public void ToHumanReadable_Works()
        {
            var value = new TimeSpan(1, 2, 3, 4);

            Assert.AreEqual("1d 2h 3m 4s", value.ToHumanReadable());
        }

        [TestMethod]
        public void ToHumanReadable_Returns_Zero_For_Zero()
        {
            Assert.AreEqual("0s", TimeSpan.Zero.ToHumanReadable());
        }

        [TestMethod]
        public void ThrowIfMissing_Throws_For_Zero()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => TimeSpan.Zero.ThrowIfMissing());
        }

        [TestMethod]
        public void ThrowIfNegative_Throws_For_Negative()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => TimeSpan.FromSeconds(-1).ThrowIfNegative());
        }

        [TestMethod]
        public void IsShorterThan_Works()
        {
            Assert.IsTrue(TimeSpan.FromSeconds(1).IsShorterThan(TimeSpan.FromSeconds(2)));
        }

        [TestMethod]
        public void IsLongerThan_Works()
        {
            Assert.IsTrue(TimeSpan.FromSeconds(3).IsLongerThan(TimeSpan.FromSeconds(2)));
        }

        [TestMethod]
        public void AddSafe_Returns_MaxValue_On_Overflow()
        {
            var result = TimeSpan.MaxValue.AddSafe(TimeSpan.FromSeconds(1));

            Assert.AreEqual(TimeSpan.MaxValue, result);
        }
    }
}