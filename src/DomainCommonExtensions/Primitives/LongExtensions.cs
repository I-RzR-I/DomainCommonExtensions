// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-12-08 21:47
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-12-08 21:47
// ***********************************************************************
//  <copyright file="LongExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Globalization;

#endregion

namespace RzR.Extensions.Domain.Primitives
{
    /// <summary>
    ///     Long - Int64 extensions
    /// </summary>
    /// <remarks></remarks>
    public static class LongExtensions
    {
        /// <summary>
        ///     Check if value is NULL or 0
        /// </summary>
        /// <param name="value">Input value to check</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsNullOrZero(this long? value)
        {
            return value.IsNull() || value.IsZero();
        }

        /// <summary>
        ///     Check if the value is == 0
        /// </summary>
        /// <param name="value">Input value to check</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsZero(this long value)
        {
            return value == 0;
        }

        /// <summary>
        ///     Check if the value is == 0
        /// </summary>
        /// <param name="value">Input value to check</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsZero(this long? value)
        {
            return value == 0;
        }

        /// <summary>
        ///     Check if the value is less or equal with 0
        /// </summary>
        /// <param name="value">Input value to check</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsLessOrEqualZero(this long value)
        {
            return value <= 0;
        }

        /// <summary>
        ///     Check if the value is greater than or equal with 0
        /// </summary>
        /// <param name="value">Input value to check</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsGreaterThanOrEqualZero(this long value)
        {
            return value >= 0;
        }

        /// <summary>
        ///     Check if the value is greater than 0
        /// </summary>
        /// <param name="value">Input value to check</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsGreaterThanZero(this long value)
        {
            return value > 0;
        }

        /// <summary>
        ///     Sets a flag
        /// </summary>
        /// <param name="value">Where to install</param>
        /// <param name="flag">Flag</param>
        /// <param name="set">True - set, false - reset</param>
        /// <returns></returns>
        public static long SetFlag(this long value, long flag, bool set)
        {
            return (value & ~flag) | (set ? flag : 0);
        }

        /// <summary>
        ///     Checks whether any bit of the flag is set
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="flag">Flag mask</param>
        /// <returns>true if ANY bit of flag is set in value; otherwise false (also false when flag is 0).</returns>
        public static bool IsFlagSet(this long value, long flag)
        {
            return (value & flag) != 0;
        }

        /// <summary>
        ///     Checks whether all bits of <paramref name="flags"/> are set in <paramref name="value"/>.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <param name="flags">Flags mask.</param>
        /// <returns>
        ///     True if every bit of <paramref name="flags"/> is set in <paramref name="value"/>;
        ///     otherwise false. True when <paramref name="flags"/> is 0, like Enum.HasFlag.
        /// </returns>
        public static bool HasAllFlags(this long value, long flags)
        {
            return (value & flags) == flags;
        }

        /// <summary>
        ///     Checks whether any bit of <paramref name="flags"/> is set in <paramref name="value"/>.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <param name="flags">Flags mask.</param>
        /// <returns>
        ///     True if at least one bit of <paramref name="flags"/> is set in <paramref name="value"/>;
        ///     otherwise false. False when <paramref name="flags"/> is 0.
        /// </returns>
        public static bool HasAnyFlag(this long value, long flags)
        {
            return (value & flags) != 0;
        }

        /// <summary>
        ///     Check current value is less than 0
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsLessZero(this long value)
        {
            return value < 0;
        }

        /// <summary>
        ///     Check current value is less than 0
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsLessZero(this long? value)
        {
            return (value ?? 0) < 0;
        }

        /// <summary>
        ///     A long extension method that converts a size to a readable file size.
        /// </summary>
        /// <remarks>
        ///     See also <see cref="AsReadableFileSize(long, IFormatProvider)"/> for a culture-specific
        ///     format.
        /// </remarks>
        /// <param name="size">The size to act on.</param>
        /// <returns>
        ///     A string.
        /// </returns>
        public static string AsReadableFileSize(this long size)
        {
            return size.AsReadableFileSize(CultureInfo.CurrentCulture);
        }

        /// <summary>
        ///     Converts a size in bytes to a readable file size, formatting the number with the
        ///     specified format provider.
        /// </summary>
        /// <remarks>
        ///     Uses the same algorithm and output as <see cref="AsReadableFileSize(long)"/>, which is
        ///     equivalent to calling this overload with <see cref="CultureInfo.CurrentCulture"/>. The
        ///     unit is chosen before the value is rounded, so a size just below a unit boundary is shown
        ///     as 1024.0 of the smaller unit (e.g. 1048575 gives "1024.0 KB", not "1.0 MB"). From MB
        ///     upward the value is truncated to whole units of the next smaller unit before rounding.
        ///     Sizes below 1024, including negative sizes, are shown in bytes.
        /// </remarks>
        /// <param name="size">The size, in bytes, to act on.</param>
        /// <param name="provider">
        ///     The format provider used for the number, e.g. <see cref="CultureInfo.InvariantCulture"/>.
        ///     When null, the current culture is used.
        /// </param>
        /// <returns>
        ///     The size with a unit suffix: "bytes" (no decimals), "KB", "MB", "GB", "TB", "PB" (one
        ///     decimal)
        ///     or "EB" (no decimals), e.g. "1.5 GB" with the invariant culture.
        /// </returns>
        public static string AsReadableFileSize(this long size, IFormatProvider provider)
        {
            if (size < 1024)
            {
                return size.ToString("F0", provider) + " bytes";
            }

            if ((size >> 10) < 1024)
            {
                return (size / (float)1024).ToString("F1", provider) + " KB";
            }

            if ((size >> 20) < 1024)
            {
                return ((size >> 10) / (float)1024).ToString("F1", provider) + " MB";
            }

            if ((size >> 30) < 1024)
            {
                return ((size >> 20) / (float)1024).ToString("F1", provider) + " GB";
            }

            if ((size >> 40) < 1024)
            {
                return ((size >> 30) / (float)1024).ToString("F1", provider) + " TB";
            }

            if ((size >> 50) < 1024)
            {
                return ((size >> 40) / (float)1024).ToString("F1", provider) + " PB";
            }

            return ((size >> 50) / (float)1024).ToString("F0", provider) + " EB";
        }

        /// <summary>
        ///     Converts a Unix time, in seconds since 1970-01-01T00:00:00Z, to a UTC DateTime.
        /// </summary>
        /// <remarks>
        ///     For milliseconds use <see cref="DateTimeExtensions.FromEpoch(long)"/>.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="seconds"/> is less than -62135596800 (0001-01-01T00:00:00Z) or greater
        ///     than
        ///     253402300799 (9999-12-31T23:59:59Z).
        /// </exception>
        /// <param name="seconds">
        ///     The number of seconds since the Unix epoch; negative values are before it.
        /// </param>
        /// <returns>
        ///     A DateTime with <see cref="DateTimeKind.Utc"/> Kind.
        /// </returns>
        public static DateTime FromUnixSeconds(this long seconds)
        {
            if (!seconds.TryFromUnixSeconds(out var result))
                throw new ArgumentOutOfRangeException(nameof(seconds), seconds,
                    "The value must be between -62135596800 and 253402300799 Unix seconds.");

            return result;
        }

        /// <summary>
        ///     Tries to convert a Unix time, in seconds since 1970-01-01T00:00:00Z, to a UTC DateTime.
        /// </summary>
        /// <param name="seconds">
        ///     The number of seconds since the Unix epoch; negative values are before it.
        /// </param>
        /// <param name="result">
        ///     [out] When this method returns true, the UTC DateTime; otherwise <c>default(DateTime)</c>.
        /// </param>
        /// <returns>
        ///     True if <paramref name="seconds"/> is between -62135596800 (0001-01-01T00:00:00Z) and
        ///     253402300799 (9999-12-31T23:59:59Z) inclusive; otherwise false.
        /// </returns>
        public static bool TryFromUnixSeconds(this long seconds, out DateTime result)
        {
            const long unixEpochTicks = 621355968000000000L;
            const long minUnixSeconds = -62135596800L;
            const long maxUnixSeconds = 253402300799L;

            if (seconds < minUnixSeconds || seconds > maxUnixSeconds)
            {
                result = default;

                return false;
            }

            result = new DateTime(unixEpochTicks + seconds * TimeSpan.TicksPerSecond, DateTimeKind.Utc);

            return true;
        }
    }
}
