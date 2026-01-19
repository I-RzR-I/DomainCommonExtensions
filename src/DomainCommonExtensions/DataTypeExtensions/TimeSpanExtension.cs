// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-12-08 18:58
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-01-18 22:42
// ***********************************************************************
//  <copyright file="TimeSpanExtension.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using DomainCommonExtensions.CommonExtensions;

#endregion

namespace DomainCommonExtensions.DataTypeExtensions
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Time span extensions.
    /// </summary>
    /// =================================================================================================
    public static class TimeSpanExtension
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Returns the absolute value of the timestamp.
        /// </summary>
        /// <param name="source">Source TimeSpan.</param>
        /// <returns>
        ///     A TimeSpan.
        /// </returns>
        /// =================================================================================================
        public static TimeSpan Absolute(this TimeSpan source)
        {
            if (source.IsNull()) 
                return TimeSpan.Zero;

            return source.Ticks.IsGreaterThanOrEqualZero() 
                ? source
                : source.Negate();
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A TimeSpan is considered missing if it is zero or negative.
        /// </summary>
        /// <param name="source">Source TimeSpan.</param>
        /// <returns>
        ///     True if missing, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsMissing(this TimeSpan source)
        {
            return source.IsNotNull() && source <= TimeSpan.Zero;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A TimeSpan is considered missing if it is zero or negative.
        /// </summary>
        /// <param name="source">Source TimeSpan.</param>
        /// <returns>
        ///     True if missing, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsMissing(this TimeSpan? source)
        {
            return !source.HasValue || source.Value <= TimeSpan.Zero;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A TimeSpan? extension method that query if 'source' has valid value.
        /// </summary>
        /// <param name="source">Source TimeSpan.</param>
        /// <returns>
        ///     True if valid value, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool HasValidValue(this TimeSpan source)
        {
            return source.IsNotNull() && source > TimeSpan.Zero;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A TimeSpan? extension method that query if 'source' has valid value.
        /// </summary>
        /// <param name="source">Source TimeSpan.</param>
        /// <returns>
        ///     True if valid value, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool HasValidValue(this TimeSpan? source)
        {
            return source.HasValue && source.Value > TimeSpan.Zero;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A TimeSpan extension method that query if 'source' is zero.
        /// </summary>
        /// <param name="source">Source TimeSpan.</param>
        /// <returns>
        ///     True if zero, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsZero(this TimeSpan source)
        {
            return source.IsNotNull() && source == TimeSpan.Zero;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A TimeSpan extension method that query if 'source' is zero.
        /// </summary>
        /// <param name="source">Source TimeSpan.</param>
        /// <returns>
        ///     True if zero, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsZero(this TimeSpan? source)
        {
            return source.IsNotNull() && source == TimeSpan.Zero;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A TimeSpan extension method that query if 'source' is positive.
        /// </summary>
        /// <param name="source">Source TimeSpan.</param>
        /// <returns>
        ///     True if positive, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsPositive(this TimeSpan source)
        {
            return source.IsNotNull() && source > TimeSpan.Zero;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A TimeSpan extension method that query if 'source' is positive.
        /// </summary>
        /// <param name="source">Source TimeSpan.</param>
        /// <returns>
        ///     True if positive, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsPositive(this TimeSpan? source)
        {
            return source.IsNotNull() && source > TimeSpan.Zero;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A TimeSpan extension method that query if 'source' is negative.
        /// </summary>
        /// <param name="source">Source TimeSpan.</param>
        /// <returns>
        ///     True if negative, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsNegative(this TimeSpan source)
        {
            return source.IsNotNull() && source < TimeSpan.Zero;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A TimeSpan extension method that query if 'source' is negative.
        /// </summary>
        /// <param name="source">Source TimeSpan.</param>
        /// <returns>
        ///     True if negative, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsNegative(this TimeSpan? source)
        {
            return source.IsNotNull() && source < TimeSpan.Zero;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A TimeSpan extension method that query if 'source' is between.
        /// </summary>
        /// <param name="source">Source TimeSpan.</param>
        /// <param name="minInclusive">The minimum inclusive.</param>
        /// <param name="maxInclusive">The maximum inclusive.</param>
        /// <returns>
        ///     True if between, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsBetween(this TimeSpan source,
            TimeSpan minInclusive, TimeSpan maxInclusive)
        {
            return source >= minInclusive && source <= maxInclusive;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A TimeSpan? extension method that default if null.
        /// </summary>
        /// <param name="source">Source TimeSpan.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        ///     A TimeSpan.
        /// </returns>
        /// =================================================================================================
        public static TimeSpan DefaultIfNull(this TimeSpan? source, TimeSpan defaultValue)
        {
            return source ?? defaultValue;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A TimeSpan? extension method that zero if null.
        /// </summary>
        /// <param name="source">Source TimeSpan.</param>
        /// <returns>
        ///     A TimeSpan.
        /// </returns>
        /// =================================================================================================
        public static TimeSpan ZeroIfNull(this TimeSpan? source)
        {
            return source ?? TimeSpan.Zero;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A TimeSpan extension method that round up.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when one or more arguments are outside the required range.
        /// </exception>
        /// <param name="source">Source TimeSpan.</param>
        /// <param name="interval">The interval.</param>
        /// <returns>
        ///     A TimeSpan.
        /// </returns>
        /// =================================================================================================
        public static TimeSpan RoundUp(this TimeSpan source, TimeSpan interval)
        {
            if (interval <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(interval));

            var ticks = (source.Ticks + interval.Ticks - 1) / interval.Ticks * interval.Ticks;
            return TimeSpan.FromTicks(ticks);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A TimeSpan extension method that round down.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when one or more arguments are outside the required range.
        /// </exception>
        /// <param name="source">Source TimeSpan.</param>
        /// <param name="interval">The interval.</param>
        /// <returns>
        ///     A TimeSpan.
        /// </returns>
        /// =================================================================================================
        public static TimeSpan RoundDown(this TimeSpan source, TimeSpan interval)
        {
            if (interval <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(interval));

            var ticks = source.Ticks / interval.Ticks * interval.Ticks;
            return TimeSpan.FromTicks(ticks);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A TimeSpan extension method that clamp the given source.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Thrown when one or more arguments have unsupported or illegal values.
        /// </exception>
        /// <param name="source">Source TimeSpan.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <returns>
        ///     A TimeSpan.
        /// </returns>
        /// =================================================================================================
        public static TimeSpan Clamp(this TimeSpan source,
            TimeSpan min, TimeSpan max)
        {
            if (min > max)
                throw new ArgumentException("Min cannot be greater than max.");

            if (source < min)
                return min;

            if (source > max) 
                return max;

            return source;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A TimeSpan extension method that total seconds int.
        /// </summary>
        /// <param name="source">Source TimeSpan.</param>
        /// <returns>
        ///     The total number of seconds int.
        /// </returns>
        /// =================================================================================================
        public static int TotalSecondsInt(this TimeSpan source)
        {
            return (int)source.TotalSeconds;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A TimeSpan extension method that total minutes int.
        /// </summary>
        /// <param name="source">Source TimeSpan.</param>
        /// <returns>
        ///     The total number of minutes int.
        /// </returns>
        /// =================================================================================================
        public static int TotalMinutesInt(this TimeSpan source)
        {
            return (int)source.TotalMinutes;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A TimeSpan extension method that total milliseconds long.
        /// </summary>
        /// <param name="source">Source TimeSpan.</param>
        /// <returns>
        ///     The total number of milliseconds long.
        /// </returns>
        /// =================================================================================================
        public static long TotalMillisecondsLong(this TimeSpan source)
        {
            return (long)source.TotalMilliseconds;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Formats as HH:mm:ss (zero padded).
        /// </summary>
        /// <param name="source">Source TimeSpan.</param>
        /// <returns>
        ///     Source as a string.
        /// </returns>
        /// =================================================================================================
        public static string ToClockFormat(this TimeSpan source)
        {
            return $"{(int)source.TotalHours:00}:{source.Minutes:00}:{source.Seconds:00}";
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Formats as a human-readable duration (e.g. "2h 15m 3s").
        /// </summary>
        /// <param name="source">Source TimeSpan.</param>
        /// <returns>
        ///     Source as a string.
        /// </returns>
        /// =================================================================================================
        public static string ToHumanReadable(this TimeSpan source)
        {
            if (source == TimeSpan.Zero)
                return "0s";

            var result = "";

            if (source.Days > 0)
                result += $"{source.Days}d ";

            if (source.Hours > 0)
                result += $"{source.Hours}h ";

            if (source.Minutes > 0)
                result += $"{source.Minutes}m ";

            if (source.Seconds > 0)
                result += $"{source.Seconds}s ";

            return result.TrimEnd();
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A TimeSpan extension method that throw if missing.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when one or more arguments are outside the required range.
        /// </exception>
        /// <param name="source">Source TimeSpan.</param>
        /// <param name="paramName">(Optional) Name of the parameter.</param>
        /// =================================================================================================
        public static void ThrowIfMissing(this TimeSpan source, string paramName = null)
        {
            if (source <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(paramName, source, "TimeSpan must be greater than zero.");
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A TimeSpan extension method that throw if negative.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when one or more arguments are outside the required range.
        /// </exception>
        /// <param name="source">Source TimeSpan.</param>
        /// <param name="paramName">(Optional) Name of the parameter.</param>
        /// =================================================================================================
        public static void ThrowIfNegative(this TimeSpan source, string paramName = null)
        {
            if (source < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(
                    paramName,
                    source,
                    "TimeSpan cannot be negative.");
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A TimeSpan extension method that query if 'source' is shorter than.
        /// </summary>
        /// <param name="source">Source TimeSpan.</param>
        /// <param name="other">The other.</param>
        /// <returns>
        ///     True if shorter than, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsShorterThan(this TimeSpan source, TimeSpan other)
        {
            return source < other;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A TimeSpan extension method that query if 'source' is longer than.
        /// </summary>
        /// <param name="source">Source TimeSpan.</param>
        /// <param name="other">The other.</param>
        /// <returns>
        ///     True if longer than, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsLongerThan(this TimeSpan source, TimeSpan other)
        {
            return source > other;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A TimeSpan extension method that adds a safe to 'other'.
        /// </summary>
        /// <param name="source">Source TimeSpan.</param>
        /// <param name="other">The other.</param>
        /// <returns>
        ///     A TimeSpan.
        /// </returns>
        /// =================================================================================================
        public static TimeSpan AddSafe(this TimeSpan source, TimeSpan other)
        {
            try
            {
                return source + other;
            }
            catch (OverflowException)
            {
                return other > TimeSpan.Zero
                    ? TimeSpan.MaxValue
                    : TimeSpan.MinValue;
            }
        }
    }
}