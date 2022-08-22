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
using System.ComponentModel.DataAnnotations;
using System.Globalization;

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
        /// <param name="target"></param>
        /// <returns></returns>
        public static DateTime AsUtc(this DateTime target)
        {
            return target.Kind switch
            {
                DateTimeKind.Utc => target,
                DateTimeKind.Unspecified => DateTime.SpecifyKind(target, DateTimeKind.Utc),
                _ => target.ToUniversalTime()
            };
        }

        /// <summary>
        ///     Represents a given DateTime as an UTC DateTime
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static DateTime? AsUtc(this DateTime? target)
        {
            return target?.AsUtc();
        }

        /// <summary>
        ///     Transform datetime to string format 'yyyyMMdd'
        /// </summary>
        /// <param name="date">Input datetime</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string FormatDateToString_112(this DateTime date)
        {
            return date.ToString("yyyyMMdd");
        }

        /// <summary>
        ///     Transform datetime to specified string format
        /// </summary>
        /// <param name="date">Input datetime</param>
        /// <param name="format">Output DateTime format</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string FormatToString(this DateTime date, string format = null)
        {
            return format.IsNullOrEmpty() ? date.FormatDateToString_112() : date.ToString(format);
        }


        /// <summary>
        ///     Check if the datetime is valid culture format
        /// </summary>
        /// <param name="value">Input string date</param>
        /// <param name="cultureInfo">culture info name</param>
        /// <returns></returns>
        /// <remarks> cultureInfo = ('en-US', 'ru-RU', 'ro-RO', etc)</remarks>
        public static bool CheckCultureDateTime(this string value, [Required] string cultureInfo)
        {
            if (value.IsNullOrEmpty())
                return false;
            if (cultureInfo.IsNullOrEmpty())
                throw new ArgumentException(nameof(cultureInfo));

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
        /// <param name="datetime">Date time</param>
        /// <param name="hourStart">Start hour</param>
        /// <param name="hourStop">End hour</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool TimeBetween(this DateTime datetime, int hourStart, int hourStop)
        {
            var start = new TimeSpan(hourStart, 0, 0);
            var end = new TimeSpan(hourStop, 0, 0);
            var now = datetime.TimeOfDay;
            if (start < end)
                return start <= now && now <= end;

            return !(end < now && now < start);
        }

        /// <summary>
        ///     Calculate age from birthdate
        /// </summary>
        /// <param name="dtBirthDate">BirthDate</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int CalculateAge(this DateTime dtBirthDate)
        {
            var dtToday = DateTime.Now;

            var years = dtToday.Year - dtBirthDate.Year;
            if (dtToday.Month < dtBirthDate.Month ||
                dtToday.Month == dtBirthDate.Month && dtToday.Day < dtBirthDate.Day)
                years -= 1;

            return years;
        }

        /// <summary>
        ///     Return DateTime not null value
        /// </summary>
        /// <param name="source">Current DateTime value</param>
        /// <returns>Return DateTime.Now in case when
        ///     <para>source</para>
        ///     is null
        /// </returns>
        /// <remarks></remarks>
        public static DateTime AsNotNull(this DateTime? source)
        {
            return source ?? DateTime.Now;
        }

        /// <summary>
        ///     Return DateTime not null value
        /// </summary>
        /// <param name="source">Current DateTime value</param>
        /// <returns>Return 0001-01-01 00:00:00 in case when
        ///     <para>source</para>
        ///     is null
        /// </returns>
        /// <remarks></remarks>
        public static DateTime AsMinDateTimeOnNull(this DateTime? source)
        {
            return source ?? DateTime.MinValue;
        }

        /// <summary>
        ///     Return DateTime not null value
        /// </summary>
        /// <param name="source">Current DateTime value</param>
        /// <returns>Return 9999-12-31 29:59:59 in case when
        ///     <para>source</para>
        ///     is null
        /// </returns>
        /// <remarks></remarks>
        public static DateTime AsMaxDateTimeOnNull(this DateTime? source)
        {
            return source ?? DateTime.MaxValue;
        }

        /// <summary>
        ///     Return DateTime not null value
        /// </summary>
        /// <param name="source">Current DateTime value</param>
        /// <returns>Return 0001-01-01 in case when
        ///     <para>source</para>
        ///     is null
        /// </returns>
        /// <remarks></remarks>
        public static DateTime AsMinDateOnNull(this DateTime? source)
        {
            return source ?? DateTime.MinValue.Date;
        }

        /// <summary>
        ///     Return DateTime not null value
        /// </summary>
        /// <param name="source">Current DateTime value</param>
        /// <returns>Return 9999-12-31 in case when
        ///     <para>source</para>
        ///     is null
        /// </returns>
        /// <remarks></remarks>
        public static DateTime AsMaxDateOnNull(this DateTime? source)
        {
            return source ?? DateTime.MaxValue.Date;
        }

        /// <summary>
        ///     Get end of day
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime EndOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
        }

        /// <summary>
        ///     Start of day
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime StartOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
        }

        /// <summary>
        ///     Day of week
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int DayIndex(this DateTime date)
        {
            int index;
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    index = 7;
                    break;
                default:
                    index = (int)date.DayOfWeek;
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
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsWeekend(this DateTime value)
        {
            return value.DayOfWeek == DayOfWeek.Sunday || value.DayOfWeek == DayOfWeek.Saturday;
        }

        /// <summary>
        ///     Returns whether or not a DateTime is during a leap year.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsLeapYear(this DateTime value)
        {
            return DateTime.DaysInMonth(value.Year, 2) == 29;
        }

        /// <summary>
        ///     Date to time stamp
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static double ToTimeStamp(this DateTime date)
        {
            return (date - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }
    }
}