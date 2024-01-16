// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2022-08-15 18:00
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-15 19:39
// ***********************************************************************
//  <copyright file="DateTimeTests.cs" company="">
//   Copyright (c) RzR. All rights reserved.
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

namespace DataTypeTests
{
    [TestClass]
    public class DateTimeTests
    {
        private readonly DateTime _initDateLocal = new DateTime(2000, 12, 31, 0, 0, 0, DateTimeKind.Local);
        private readonly DateTime _initDateUtc = new DateTime(2000, 12, 31, 0, 0, 0);

        [TestMethod]
        public void AsUtcTest()
        {
            var utc = new DateTime(2000, 12, 30, 21, 0, 0, DateTimeKind.Utc);
            var res = _initDateUtc.AsUtc();

            Assert.IsNotNull(res);
            Assert.AreNotEqual(res, utc);
        }

        [TestMethod]
        public void AsUtcWithKidTest()
        {
            var utc = new DateTime(2000, 12, 30, 22, 0, 0, DateTimeKind.Utc);
            var res = _initDateLocal.AsUtc();

            Assert.IsNotNull(res);
            Assert.AreEqual(res, utc);
        }

        [TestMethod]
        public void NullableAsUtcTest()
        {
            var res = ((DateTime?)null).AsUtc();

            Assert.IsNull(res);
        }

        [TestMethod]
        public void FormatDateToString_112Test()
        {
            const string date = "20001231";
            var res = _initDateUtc.FormatDateToString_112();

            Assert.IsNotNull(res);
            Assert.AreEqual(res, date);
        }

        [TestMethod]
        public void FormatToStringTest()
        {
            const string date = "20001231";
            var res = _initDateUtc.FormatToString();

            Assert.IsNotNull(res);
            Assert.AreEqual(res, date);
        }

        [TestMethod]
        public void FormatToStringWithFormatTest()
        {
            const string date = "2000-12-31";
            var res = _initDateUtc.FormatToString("yyyy-MM-dd");

            Assert.IsNotNull(res);
            Assert.AreEqual(res, date);
        }

        [TestMethod]
        public void CheckCultureDateTime_enUS_Test()
        {
            const string date = "2000-12-31";
            var res = date.CheckCultureDateTime("en-US");

            Assert.IsNotNull(res);
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void CheckCultureDateTime_enUS_1_Test()
        {
            const string date = "12-31-2000";
            var res = date.CheckCultureDateTime("en-US");

            Assert.IsNotNull(res);
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void CheckCultureDateTime_enUK_Test()
        {
            const string date = "2000-12-31";
            var res = date.CheckCultureDateTime("en-UK");

            Assert.IsNotNull(res);
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void CheckCultureDateTime_roRO_Test()
        {
            const string date = "31/12/2000";
            var res = date.CheckCultureDateTime("ro-RO");

            Assert.IsNotNull(res);
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void CheckCultureDateTime_roRO_1_Test()
        {
            const string date = "31.12.2000";
            var res = date.CheckCultureDateTime("ro-RO");

            Assert.IsNotNull(res);
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void CheckCultureDateTime_roRO_2_Test()
        {
            const string date = "31-12-2000";
            var res = date.CheckCultureDateTime("ro-RO");

            Assert.IsNotNull(res);
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void TimeBetweenTest()
        {
            var date = new DateTime(2022, 8, 8, 13, 0, 0);
            var res = date.TimeBetween(11, 14);

            Assert.IsNotNull(res);
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void CalculateAgeTest()
        {
            var date = new DateTime(1994, 8, 8, 13, 0, 0);
            var res = date.CalculateAge();

            Assert.IsNotNull(res);
            Assert.IsTrue(res.Equals(29));
        }

        [TestMethod]
        public void AsNotNullTest()
        {
            var res = ((DateTime?)null).AsNotNull();

            Assert.IsNotNull(res);
            Assert.IsTrue(res.ToString("ddMMyyyyHHmmss") == DateTime.Now.ToString("ddMMyyyyHHmmss"));
            Assert.IsTrue(res.Date == DateTime.Now.Date);
        }

        [TestMethod]
        public void AsMinDateTimeOnNullTest()
        {
            var res = ((DateTime?)null).AsMinDateTimeOnNull();

            Assert.IsNotNull(res);
            Assert.IsTrue(res.ToString("ddMMyyyyHHmmss") == DateTime.MinValue.ToString("ddMMyyyyHHmmss"));
            Assert.IsTrue(res.Date == DateTime.MinValue.Date);
        }

        [TestMethod]
        public void AsMaxDateTimeOnNullTest()
        {
            var res = ((DateTime?)null).AsMaxDateTimeOnNull();

            Assert.IsNotNull(res);
            Assert.IsTrue(res.ToString("ddMMyyyyHHmmss") == DateTime.MaxValue.ToString("ddMMyyyyHHmmss"));
            Assert.IsTrue(res.Date == DateTime.MaxValue.Date);
        }

        [TestMethod]
        public void AsMinDateOnNullTest()
        {
            var res = ((DateTime?)null).AsMinDateOnNull();

            Assert.IsNotNull(res);
            Assert.IsTrue(res.ToString("ddMMyyyy") == DateTime.MinValue.ToString("ddMMyyyy"));
            Assert.IsTrue(res.Date == DateTime.MinValue.Date);
        }

        [TestMethod]
        public void AsMaxDateOnNullTest()
        {
            var res = ((DateTime?)null).AsMaxDateOnNull();

            Assert.IsNotNull(res);
            Assert.IsTrue(res.ToString("ddMMyyyy") == DateTime.MaxValue.ToString("ddMMyyyy"));
            Assert.IsTrue(res.Date == DateTime.MaxValue.Date);
        }

        [TestMethod]
        public void EndOfDayTest()
        {
            var date = new DateTime();
            var end = date.EndOfDay();

            Assert.IsNotNull(end);
            Assert.AreEqual(new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999), end);
        }

        [TestMethod]
        public void StartOfDayTest()
        {
            var date = new DateTime();
            var start = date.StartOfDay();

            Assert.IsNotNull(start);
            Assert.AreEqual(new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0), start);
        }

        [TestMethod]
        public void DayIndexTest()
        {
            var date = new DateTime(2000, 1, 1);
            var day = date.DayIndex();

            Assert.IsNotNull(day);
            Assert.AreEqual(6, day);
        }

        [TestMethod]
        public void IsWeekendTest()
        {
            var date = new DateTime(2000, 1, 1);
            var day = date.IsWeekend();

            Assert.IsNotNull(day);
            Assert.AreEqual(true, day);
        }

        [TestMethod]
        public void IntersectsTest()
        {
            var startDate = new DateTime(2000, 1, 1);
            var intersect = startDate.Intersects(new DateTime(2001, 1, 1), new DateTime(2000, 1, 1), new DateTime(2000, 1, 1));

            Assert.IsTrue(intersect);
        }

        [TestMethod]
        public void IsLeapYearTest()
        {
            var date = new DateTime(2000, 1, 1);
            var result = date.IsLeapYear();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ToTimeStampTest()
        {
            var date = new DateTime(2000, 1, 1);
            var result = date.ToTimeStamp();

            Assert.AreEqual(946684800, result);
        }
    }
}