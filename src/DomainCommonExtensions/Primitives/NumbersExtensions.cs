// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-08-10 21:39
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-12 23:39
// ***********************************************************************
//  <copyright file="NumbersExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

#endregion

namespace RzR.Extensions.Domain.Primitives
{
    /// <summary>
    ///     Numbers extensions
    /// </summary>
    /// <remarks></remarks>
    public static class NumbersExtensions
    {
        /// <summary>
        ///     Are float numbers equal
        /// </summary>
        /// <param name="source"></param>
        /// <param name="toCompare"></param>
        /// <param name="epsilon"></param>
        /// <returns></returns>
        public static bool AreEqual(this float source, float toCompare, float epsilon = float.Epsilon)
        {
            return Math.Abs(source - toCompare) < epsilon;
        }

        /// <summary>
        ///     Are double numbers equal
        /// </summary>
        /// <param name="source"></param>
        /// <param name="toCompare"></param>
        /// <param name="epsilon"></param>
        /// <returns></returns>
        public static bool AreEqual(this double source, double toCompare, double epsilon = double.Epsilon)
        {
            return Math.Abs(source - toCompare) < epsilon;
        }

        /// <summary>
        ///     Get a certain percentage of the specified number.
        /// </summary>
        /// <param name="value">The number to get the percentage of.</param>
        /// <param name="percentage">The percentage of the specified number to get.</param>
        /// <returns>The actual specified percentage of the specified number.</returns>
        public static double GetPercentage(this double value, int percentage)
        {
            var percentAsDouble = (double)percentage / 100;

            return value * percentAsDouble;
        }

        /// <summary>
        ///     Get a certain percentage of the specified number.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="percentage"></param>
        /// <returns></returns>
        public static double GetPercentage(this decimal value, double percentage)
        {
            var percentAsDouble = percentage / 100;

            return (double)value * percentAsDouble;
        }

        /// <summary>
        ///     Get percent of
        /// </summary>
        /// <param name="value"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static int PercentOf(this int value, int total)
        {
            if (total.IsZero()) return 0;

            return (int)(value / (double)total * 100);
        }

        /// <summary>
        ///     Get percent of
        /// </summary>
        /// <param name="value"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static int PercentOf(this ulong value, ulong total)
        {
            if (total.IsZero()) return 0;

            return (int)(value / (double)total * 100);
        }

        /// <summary>
        ///     Percent of
        /// </summary>
        /// <param name="value"></param>
        /// <param name="total"></param>
        /// <returns>The percent; 0 when total is 0 or the result overflows decimal.</returns>
        public static double PercentOf(this decimal value, decimal total)
        {
            if (total.IsZero()) return 0;
            try
            {
                return (double)(value / total * 100);
            }
            catch (OverflowException)
            {
                return 0;
            }
        }

        /// <summary>
        ///     Get percent of
        /// </summary>
        /// <param name="value"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static int PercentOf(this long value, long total)
        {
            if (total.IsZero()) return 0;
            var v = value > 0 ? (ulong)value : 0;

            return total == 0 ? 0 : v.PercentOf((ulong)total);
        }

        /// <summary>
        ///     Get random number.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="max"/> is less than or equal to <paramref name="min"/>.
        /// </exception>
        /// <param name="random">.</param>
        /// <param name="min">.</param>
        /// <param name="max">.</param>
        /// <returns>
        ///     A long.
        /// </returns>
        public static long NextLong(this Random random, long min, long max)
        {
            if (max <= min)
                throw new ArgumentOutOfRangeException(nameof(max), "max must be > min!");
            var uRange = (ulong)(max - min);
            ulong ulongRand;
            do
            {
                var buf = new byte[8];
                random.NextBytes(buf);
                ulongRand = (ulong)BitConverter.ToInt64(buf, 0);
            } while (ulongRand > ulong.MaxValue - (ulong.MaxValue % uRange + 1) % uRange);

            return (long)(ulongRand % uRange) + min;
        }

        /// <summary>
        ///     Returns a random long from 0 (inclusive) to max (exclusive)
        /// </summary>
        /// <param name="random">The given random instance</param>
        /// <param name="max">The exclusive maximum bound.  Must be greater than 0</param>
        public static long NextLong(this Random random, long max)
        {
            return random.NextLong(0, max);
        }

        /// <summary>
        ///     Returns a random long over all possible values of long (except long.MaxValue, similar to
        ///     random.Next())
        /// </summary>
        /// <param name="random">The given random instance</param>
        public static long NextLong(this Random random)
        {
            return random.NextLong(long.MinValue, long.MaxValue);
        }

        /// <summary>
        ///     Generate unique number
        /// </summary>
        /// <param name="excludeNumbers"></param>
        /// <returns></returns>
        public static long GenerateUniqueNumberThatNoIncludesNumbers(this IEnumerable<long> excludeNumbers)
        {
            var enumerated = excludeNumbers.ToList();
            var random = new Random();
            long number = 1;

            while (enumerated.Contains(number)) number = random.NextLong(1, long.MaxValue);

            return number;
        }

        /// <summary>
        ///     Is prime number
        /// </summary>
        /// <param name="number"></param>
        /// <returns>True if the number is prime; numbers less than 2 are not prime</returns>
        public static bool IsPrime(this int number)
        {
            if (number < 2) return false;
            if ((number % 2).IsZero()) return number == 2;
            var sqrt = (int)Math.Sqrt(number);
            for (var t = 3; t <= sqrt; t += 2)
                if (number % t == 0)
                    return false;

            return number != 1;
        }

        /// <summary>
        ///     Round
        /// </summary>
        /// <param name="value"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static decimal RoundTo(this decimal value, int count)
        {
            return Math.Round(value, count);
        }

        /// <summary>
        ///     Round
        /// </summary>
        /// <param name="value"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static double RoundTo(this double value, int count)
        {
            return Math.Round(value, count);
        }

        /// <summary>
        ///     Check if the value is between the two boundaries (boundaries included)
        /// </summary>
        /// <param name="source">Source number</param>
        /// <param name="lower">Lower value</param>
        /// <param name="higher">Higher value</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsBetween(this double source, double lower, double higher)
        {
            return (source >= lower && source <= higher);
        }

        /// <summary>
        ///     Check if the value is between the two boundaries (boundaries included)
        /// </summary>
        /// <param name="source">Source number</param>
        /// <param name="lower">Lower value</param>
        /// <param name="higher">Higher value</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsBetween(this int source, int lower, int higher)
        {
            return (source >= lower && source <= higher);
        }

        /// <summary>
        ///     Get number suffix
        /// </summary>
        /// <param name="sourceNumber">Number to check; negative numbers use the suffix of their absolute value</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetNumberSuffix(this int sourceNumber)
        {
            var number = Math.Abs((long)sourceNumber);
            var lastTwoDigits = number % 100;
            if (lastTwoDigits >= 11 && lastTwoDigits <= 13)
                return "th";

            switch (number % 10)
            {
                case 1:
                    return "st";
                case 2:
                    return "nd";
                case 3:
                    return "rd";
                default:
                    return "th";
            }
        }

        /// <summary>
        ///     Parses a string as an <see cref="int"/> using the invariant culture.
        /// </summary>
        /// <param name="source">The string to parse.</param>
        /// <returns>
        ///     The parsed value, or 0 when <paramref name="source"/> is null, empty, not a valid integer
        ///     or out of range.
        /// </returns>
        public static int ToIntOrDefault(this string source)
            => int.TryParse(source, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result) ? result : 0;

        /// <summary>
        ///     Parses a string as a <see cref="long"/> using the invariant culture.
        /// </summary>
        /// <remarks>
        ///     Same rules as <see cref="ToLongOrNull(string)"/>.
        /// </remarks>
        /// <param name="source">The string to parse.</param>
        /// <returns>
        ///     The parsed value, or 0 when <paramref name="source"/> is null, empty, not a valid integer
        ///     or out of range.
        /// </returns>
        public static long ToLongOrDefault(this string source)
            => source.ToLongOrNull().GetValueOrDefault();

        /// <summary>
        ///     Parses a string as a <see cref="decimal"/> using the invariant culture.
        /// </summary>
        /// <remarks>
        ///     Same rules as <see cref="ToDecimalOrNull(string)"/>.
        /// </remarks>
        /// <param name="source">The string to parse.</param>
        /// <returns>
        ///     The parsed value, or 0 when <paramref name="source"/> is null, empty, not a valid number
        ///     or out of range.
        /// </returns>
        public static decimal ToDecimalOrDefault(this string source)
            => source.ToDecimalOrNull().GetValueOrDefault();

        /// <summary>
        ///     Parses a string as a <see cref="double"/> using the invariant culture.
        /// </summary>
        /// <remarks>
        ///     Same rules as <see cref="ToDoubleOrNull(string)"/>.
        /// </remarks>
        /// <param name="source">The string to parse.</param>
        /// <returns>
        ///     The parsed value, or 0 when <paramref name="source"/> is null, empty or not a valid
        ///     number.
        /// </returns>
        public static double ToDoubleOrDefault(this string source)
            => source.ToDoubleOrNull().GetValueOrDefault();

        /// <summary>
        ///     Parses a string as a <see cref="long"/> using the invariant culture.
        /// </summary>
        /// <param name="source">The string to parse.</param>
        /// <returns>
        ///     The parsed value, or null when <paramref name="source"/> is null, empty, not a valid
        ///     integer or out of range.
        /// </returns>
        public static long? ToLongOrNull(this string source)
            => long.TryParse(source, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result) ? result : null;

        /// <summary>
        ///     Parses a string as a <see cref="decimal"/> using the invariant culture.
        /// </summary>
        /// <param name="source">The string to parse.</param>
        /// <returns>
        ///     The parsed value, or null when <paramref name="source"/> is null, empty, not a valid
        ///     number or out of range.
        /// </returns>
        public static decimal? ToDecimalOrNull(this string source)
            => decimal.TryParse(source, NumberStyles.Float, CultureInfo.InvariantCulture, out var result) ? result : null;

        /// <summary>
        ///     Parses a string as a <see cref="double"/> using the invariant culture.
        /// </summary>
        /// <param name="source">The string to parse.</param>
        /// <returns>
        ///     The parsed value, or null when <paramref name="source"/> is null, empty or not a valid
        ///     number.
        /// </returns>
        public static double? ToDoubleOrNull(this string source)
            => double.TryParse(source, NumberStyles.Float, CultureInfo.InvariantCulture, out var result) ? result : null;
    }
}
