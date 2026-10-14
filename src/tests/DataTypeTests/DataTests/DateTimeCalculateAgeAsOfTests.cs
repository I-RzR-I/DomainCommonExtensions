using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Primitives;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class DateTimeCalculateAgeAsOfTests
    {
        [TestMethod]
        public void CalculateAge_DayBeforeBirthday_Test()
        {
            var birthDate = new DateTime(1990, 6, 15);

            var result = birthDate.CalculateAge(new DateTime(2024, 6, 14));

            Assert.AreEqual(33, result);
        }

        [TestMethod]
        public void CalculateAge_OnBirthday_Test()
        {
            var birthDate = new DateTime(1990, 6, 15);

            var result = birthDate.CalculateAge(new DateTime(2024, 6, 15));

            Assert.AreEqual(34, result);
        }

        [TestMethod]
        public void CalculateAge_DayAfterBirthday_Test()
        {
            var birthDate = new DateTime(1990, 6, 15);

            var result = birthDate.CalculateAge(new DateTime(2024, 6, 16));

            Assert.AreEqual(34, result);
        }

        [TestMethod]
        public void CalculateAge_EarlierMonthInAsOfYear_Test()
        {
            var birthDate = new DateTime(1990, 6, 15);

            var result = birthDate.CalculateAge(new DateTime(2024, 5, 31));

            Assert.AreEqual(33, result);
        }

        [TestMethod]
        public void CalculateAge_LaterMonthInAsOfYear_Test()
        {
            var birthDate = new DateTime(1990, 6, 15);

            var result = birthDate.CalculateAge(new DateTime(2024, 7, 1));

            Assert.AreEqual(34, result);
        }

        [TestMethod]
        public void CalculateAge_SameDate_Test()
        {
            var birthDate = new DateTime(2024, 3, 10);

            var result = birthDate.CalculateAge(new DateTime(2024, 3, 10));

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void CalculateAge_SameDayEarlierTime_Test()
        {
            var birthDate = new DateTime(2024, 3, 10, 18, 0, 0);
            var asOf = new DateTime(2024, 3, 10, 8, 0, 0);

            var result = birthDate.CalculateAge(asOf);

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void CalculateAge_BirthdayEarlierTimeOfDay_Test()
        {
            var birthDate = new DateTime(2000, 5, 20, 23, 59, 59);
            var asOf = new DateTime(2010, 5, 20, 0, 0, 0);

            var result = birthDate.CalculateAge(asOf);

            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void CalculateAge_YearEndBoundary_Test()
        {
            var birthDate = new DateTime(2000, 12, 31);

            Assert.AreEqual(0, birthDate.CalculateAge(new DateTime(2001, 12, 30)));
            Assert.AreEqual(1, birthDate.CalculateAge(new DateTime(2001, 12, 31)));
            Assert.AreEqual(1, birthDate.CalculateAge(new DateTime(2002, 1, 1)));
        }

        [TestMethod]
        public void CalculateAge_LeapBirthdayNonLeapFeb28_Test()
        {
            var birthDate = new DateTime(2000, 2, 29);

            var result = birthDate.CalculateAge(new DateTime(2023, 2, 28));

            Assert.AreEqual(22, result);
        }

        [TestMethod]
        public void CalculateAge_LeapBirthdayNonLeapMarch1_Test()
        {
            var birthDate = new DateTime(2000, 2, 29);

            var result = birthDate.CalculateAge(new DateTime(2023, 3, 1));

            Assert.AreEqual(23, result);
        }

        [TestMethod]
        public void CalculateAge_LeapBirthdayLeapFeb28_Test()
        {
            var birthDate = new DateTime(2000, 2, 29);

            var result = birthDate.CalculateAge(new DateTime(2024, 2, 28));

            Assert.AreEqual(23, result);
        }

        [TestMethod]
        public void CalculateAge_LeapBirthdayLeapFeb29_Test()
        {
            var birthDate = new DateTime(2000, 2, 29);

            var result = birthDate.CalculateAge(new DateTime(2024, 2, 29));

            Assert.AreEqual(24, result);
        }

        [TestMethod]
        public void CalculateAge_LeapBirthdayFirstNonLeapYear_Test()
        {
            var birthDate = new DateTime(2000, 2, 29);

            Assert.AreEqual(0, birthDate.CalculateAge(new DateTime(2001, 2, 28)));
            Assert.AreEqual(1, birthDate.CalculateAge(new DateTime(2001, 3, 1)));
        }

        [DataRow(DateTimeKind.Utc, DateTimeKind.Local)]
        [DataRow(DateTimeKind.Local, DateTimeKind.Utc)]
        [DataRow(DateTimeKind.Unspecified, DateTimeKind.Utc)]
        [DataRow(DateTimeKind.Utc, DateTimeKind.Unspecified)]
        [TestMethod]
        public void CalculateAge_MixedKindsIgnored_Test(DateTimeKind birthKind, DateTimeKind asOfKind)
        {
            var birthDate = new DateTime(2000, 1, 1, 23, 0, 0, birthKind);
            var asOf = new DateTime(2010, 1, 1, 0, 30, 0, asOfKind);

            var result = birthDate.CalculateAge(asOf);

            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void CalculateAge_MinValueToMaxValue_Test()
        {
            var result = DateTime.MinValue.CalculateAge(DateTime.MaxValue);

            Assert.AreEqual(9998, result);
        }

        [TestMethod]
        public void CalculateAge_AsOfDayBeforeBirth_Test()
        {
            var birthDate = new DateTime(2000, 6, 15);
            var asOf = new DateTime(2000, 6, 14, 23, 59, 59);

            var ex = Assert.ThrowsException<ArgumentOutOfRangeException>(() => birthDate.CalculateAge(asOf));

            Assert.AreEqual("asOf", ex.ParamName);
        }

        [TestMethod]
        public void CalculateAge_AsOfYearsBeforeBirth_Test()
        {
            var birthDate = new DateTime(2000, 6, 15);
            var asOf = new DateTime(1990, 1, 1);

            var ex = Assert.ThrowsException<ArgumentOutOfRangeException>(() => birthDate.CalculateAge(asOf));

            Assert.AreEqual("asOf", ex.ParamName);
        }

        [TestMethod]
        public void CalculateAge_MaxValueBirthMinValueAsOf_Test()
        {
            var ex = Assert.ThrowsException<ArgumentOutOfRangeException>(() => DateTime.MaxValue.CalculateAge(DateTime.MinValue));

            Assert.AreEqual("asOf", ex.ParamName);
        }

        [TestMethod]
        public void CalculateAge_ParameterlessOverloadBinds_Test()
        {
            Func<DateTime, int> legacy = DateTimeExtensions.CalculateAge;
            Func<DateTime, DateTime, int> asOf = DateTimeExtensions.CalculateAge;

            Assert.AreEqual(1, legacy.Method.GetParameters().Length);
            Assert.AreEqual(2, asOf.Method.GetParameters().Length);
            Assert.AreEqual("asOf", asOf.Method.GetParameters()[1].Name);
        }
    }
}
