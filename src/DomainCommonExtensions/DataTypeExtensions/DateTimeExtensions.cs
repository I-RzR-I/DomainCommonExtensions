// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-08-10 21:39
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-17 10:55
// ***********************************************************************
//  <copyright file="DateTimeExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;

#if NET45_OR_GREATER || NET || NETSTANDARD1_0_OR_GREATER
using System.ComponentModel.DataAnnotations;
#endif
using System.Globalization;
using DomainCommonExtensions.CommonExtensions;
using DomainCommonExtensions.Utilities.Ensure;

#endregion

namespace DomainCommonExtensions.DataTypeExtensions
{
    /// <summary>
    ///     DateTime extensions
    /// </summary>
    /// <remarks></remarks>
    public static class DateTimeExtensions
    {
        /// <summary>
        ///     Represents a given DateTime as an UTC DateTime
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static DateTime AsUtc(this DateTime input)
        {
            DomainEnsure.IsNotNull(input, nameof(input));

            return input.Kind switch
            {
                DateTimeKind.Utc => input,
                DateTimeKind.Unspecified => DateTime.SpecifyKind(input, DateTimeKind.Utc),
                _ => input.ToUniversalTime()
            };
        }

        /// <summary>
        ///     Represents a given DateTime as an UTC DateTime
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static DateTime? AsUtc(this DateTime? input)
        {
            return input?.AsUtc();
        }

        /// <summary>
        ///     Transform datetime to string format 'yyyyMMdd'
        /// </summary>
        /// <param name="input">Input datetime</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string FormatDateToString_112(this DateTime input)
        {
            DomainEnsure.IsNotNull(input, nameof(input));

            return input.ToString("yyyyMMdd");
        }

        /// <summary>
        ///     Transform datetime to specified string format
        /// </summary>
        /// <param name="input">Input datetime</param>
        /// <param name="format">Output DateTime format</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string FormatToString(this DateTime input, string format = null)
        {
            DomainEnsure.IsNotNull(input, nameof(input));

            return format.IsNullOrEmpty() ? input.FormatDateToString_112() : input.ToString(format);
        }

        /// <summary>
        ///     Check if the datetime is valid culture format
        /// </summary>
        /// <param name="value">Input string date</param>
        /// <param name="cultureInfo">culture info name</param>
        /// <returns></returns>
        /// <remarks> cultureInfo = ('en-US', 'ru-RU', 'ro-RO', etc)</remarks>
        public static bool CheckCultureDateTime(this string value, string cultureInfo)
        {
            if (value.IsNullOrEmpty())
                return false;
            DomainEnsure.IsNotNullOrEmpty(cultureInfo, nameof(cultureInfo));

            try
            {
                var d = Convert.ToDateTime(value, new CultureInfo(cultureInfo, false).DateTimeFormat);

                return d != DateTime.MinValue;
            }
            catch (FormatException format)
            {
                throw new FormatException($"Input string is not in {cultureInfo} format", format);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     Verify if time is between specified hours
        /// </summary>
        /// <param name="input">Date time</param>
        /// <param name="hourStart">Start hour</param>
        /// <param name="hourStop">End hour</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool TimeBetween(this DateTime input, int hourStart, int hourStop)
        {
            DomainEnsure.IsNotNull(input, nameof(input));

            var start = new TimeSpan(hourStart, 0, 0);
            var end = new TimeSpan(hourStop, 0, 0);
            var now = input.TimeOfDay;
            if (start < end)
                return start <= now && now <= end;

            return !(end < now && now < start);
        }

        /// <summary>
        ///     Calculate age from birthdate
        /// </summary>
        /// <param name="input">BirthDate</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int CalculateAge(this DateTime input)
        {
            DomainEnsure.IsNotNull(input, nameof(input));

            var dtToday = DateTime.Now;

            var years = dtToday.Year - input.Year;
            if (dtToday.Month < input.Month ||
                dtToday.Month == input.Month && dtToday.Day < input.Day)
                years -= 1;

            return years;
        }

        /// <summary>
        ///     Return DateTime not null value
        /// </summary>
        /// <param name="input">Current DateTime value</param>
        /// <returns>Return DateTime.Now in case when
        ///     <para>input</para>
        ///     is null
        /// </returns>
        /// <remarks></remarks>
        public static DateTime AsNotNull(this DateTime? input)
        {
            return input ?? DateTime.Now;
        }

        /// <summary>
        ///     Return DateTime not null value
        /// </summary>
        /// <param name="input">Current DateTime value</param>
        /// <param name="defaultDateTime">The default DateTime value to set in case of null <para>input</para></param>
        /// <returns>Return <para>defaultDateTime</para> value in case when
        ///     <para>input</para> is null
        /// </returns>
        /// <remarks></remarks>
        public static DateTime AsNotNull(this DateTime? input, DateTime defaultDateTime)
        {
            return input ?? defaultDateTime;
        }

        /// <summary>
        ///     Return DateTime not null value
        /// </summary>
        /// <param name="input">Current DateTime value</param>
        /// <returns>Return 0001-01-01 00:00:00 in case when
        ///     <para>source</para>
        ///     is null
        /// </returns>
        /// <remarks></remarks>
        public static DateTime AsMinDateTimeOnNull(this DateTime? input)
        {
            return input ?? DateTime.MinValue;
        }

        /// <summary>
        ///     Return DateTime not null value
        /// </summary>
        /// <param name="input">Current DateTime value</param>
        /// <returns>Return 9999-12-31 29:59:59 in case when
        ///     <para>source</para>
        ///     is null
        /// </returns>
        /// <remarks></remarks>
        public static DateTime AsMaxDateTimeOnNull(this DateTime? input)
        {
            return input ?? DateTime.MaxValue;
        }

        /// <summary>
        ///     Return DateTime not null value
        /// </summary>
        /// <param name="input">Current DateTime value</param>
        /// <returns>Return 0001-01-01 in case when
        ///     <para>source</para>
        ///     is null
        /// </returns>
        /// <remarks></remarks>
        public static DateTime AsMinDateOnNull(this DateTime? input)
        {
            return input ?? DateTime.MinValue.Date;
        }

        /// <summary>
        ///     Return DateTime not null value
        /// </summary>
        /// <param name="input">Current DateTime value</param>
        /// <returns>Return 9999-12-31 in case when
        ///     <para>source</para>
        ///     is null
        /// </returns>
        /// <remarks></remarks>
        public static DateTime AsMaxDateOnNull(this DateTime? input)
        {
            return input ?? DateTime.MaxValue.Date;
        }

        /// <summary>
        ///     Get end of day
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static DateTime EndOfDay(this DateTime input)
        {
            DomainEnsure.IsNotNull(input, nameof(input));

            return new DateTime(input.Year, input.Month, input.Day, 23, 59, 59, 999);
        }

        /// <summary>
        ///     Start of day
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static DateTime StartOfDay(this DateTime input)
        {
            DomainEnsure.IsNotNull(input, nameof(input));

            return new DateTime(input.Year, input.Month, input.Day, 0, 0, 0, 0);
        }

        /// <summary>
        ///     Day of week
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int DayIndex(this DateTime input)
        {
            DomainEnsure.IsNotNull(input, nameof(input));

            int index;
            switch (input.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    index = 7;
                    break;
                default:
                    index = (int)input.DayOfWeek;
                    break;
            }

            return index;
        }

        /// <summary>
        ///     Intersects dates
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="intersectingStartDate"></param>
        /// <param name="intersectingEndDate"></param>
        /// <returns></returns>
        public static bool Intersects(this DateTime startDate, DateTime endDate, DateTime intersectingStartDate,
            DateTime intersectingEndDate)
        {
            return intersectingEndDate >= startDate && intersectingStartDate <= endDate;
        }

        /// <summary>
        ///     Check if is weekend
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsWeekend(this DateTime input)
        {
            DomainEnsure.IsNotNull(input, nameof(input));

            return input.DayOfWeek == DayOfWeek.Sunday || input.DayOfWeek == DayOfWeek.Saturday;
        }

        /// <summary>
        ///     Returns whether or not a DateTime is during a leap year.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsLeapYear(this DateTime input)
        {
            DomainEnsure.IsNotNull(input, nameof(input));

            return DateTime.DaysInMonth(input.Year, 2) == 29;
        }

        /// <summary>
        ///     Date to time stamp
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double ToTimeStamp(this DateTime input)
        {
            DomainEnsure.IsNotNull(input, nameof(input));

            return (input - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }

        /// <summary>
        ///     Get start of week date
        /// </summary>
        /// <param name="input">Reference input date</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DateTime StartOfWeek(this DateTime input)
        {
            var dateTimeNow = input;
            if (dateTimeNow.DayOfWeek == DayOfWeek.Sunday)
                dateTimeNow = dateTimeNow.AddDays(-1);

            int delta = DayOfWeek.Monday - dateTimeNow.DayOfWeek;

            return dateTimeNow.AddDays(delta).Date;
        }

        /// <summary>
        ///     Get end of the week date
        /// </summary>
        /// <param name="input">Reference input date</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DateTime EndOfWeek(this DateTime input)
        {
            DomainEnsure.IsNotNull(input, nameof(input));

            return StartOfWeek(input).AddDays(6).Date;
        }

        /// <summary>
        ///     Get the date of the start of the month
        /// </summary>
        /// <param name="input">Reference input date</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DateTime StartOfMonth(this DateTime input)
        {
            DomainEnsure.IsNotNull(input, nameof(input));

            return new DateTime(input.Year, input.Month, 1).Date;
        }

        /// <summary>
        ///     Get the date of the end of the month
        /// </summary>
        /// <param name="input">Reference input date</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DateTime EndOfMonth(this DateTime input)
        {
            DomainEnsure.IsNotNull(input, nameof(input));

            return StartOfMonth(input).AddMonths(1).AddDays(-1).Date;
        }

        /// <summary>
        ///     Get a start of the previous month date
        /// </summary>
        /// <param name="input">Reference input date</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DateTime StartOfPreviousMonth(this DateTime input)
        {
            DomainEnsure.IsNotNull(input, nameof(input));

            input = input.AddMonths(-1);

            return new DateTime(input.Year, input.Month, 1).Date;
        }

        /// <summary>
        ///     Get the previous month date
        /// </summary>
        /// <param name="input">Reference input date</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DateTime EndOfPreviousMonth(this DateTime input)
        {
            DomainEnsure.IsNotNull(input, nameof(input));

            input = input.AddMonths(-1);

            return StartOfMonth(input).AddMonths(1).AddDays(-1).Date;
        }

        /// <summary>
        ///     Get a date for the start of the year
        /// </summary>
        /// <param name="input">Reference input date</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DateTime StartOfYear(this DateTime input)
        {
            DomainEnsure.IsNotNull(input, nameof(input));

            return new DateTime(input.Year, 1, 1).Date;
        }

        /// <summary>
        ///     Get a date for the end of the year
        /// </summary>
        /// <param name="input">Reference input date</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DateTime EndOfYear(this DateTime input)
        {
            DomainEnsure.IsNotNull(input, nameof(input));

            return new DateTime(input.Year, 12, 31).Date;
        }

        /// <summary>
        ///     Get a number of days in the month
        /// </summary>
        /// <param name="input">Reference input date</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int DaysInMonth(this DateTime input)
        {
            DomainEnsure.IsNotNull(input, nameof(input));

            return (int)(EndOfMonth(input) - StartOfMonth(input).AddDays(-1)).TotalDays;
        }

        /// <summary>
        ///     Get a number of the days in the year
        /// </summary>
        /// <param name="input">Reference input date</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int DaysInYear(this DateTime input)
        {
            DomainEnsure.IsNotNull(input, nameof(input));

            return EndOfYear(input).DayOfYear;
        }

        /// <summary>
        ///     This presumes that weeks start with Monday. Week 1 is the 1st week of the year with a Thursday in it.
        /// </summary>
        /// <param name="time">Current time</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int GetIso8601WeekOfYear(this DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            var day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        /// <summary>
        ///     Convert the given DateTime in Epoch time (Unix time).
        /// </summary>
        /// <param name="sourceTime">Source date time</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static long ToEpochTime(this DateTime sourceTime)
        {
            var t = sourceTime.ToUniversalTime() - new DateTime(1970, 1, 1);

            return (long)t.TotalSeconds;
        }

        /// <summary>
        ///     Convert the given DateTime as Excel numerical value
        /// </summary>
        /// <param name="sourceTime">Source date time</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static double ToExcelTime(this DateTime sourceTime)
        {
            var t = sourceTime - new DateTime(1899, 12, 31);

            return t.TotalDays;
        }
    }
}