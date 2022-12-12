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
using System.Linq;
using DomainCommonExtensions.DataTypeExtensions;

#endregion

namespace DomainCommonExtensions.CommonExtensions
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
        /// <returns></returns>
        public static double PercentOf(this decimal value, decimal total)
        {
            if (total.IsZero()) return 0;
            try
            {
                return (double)(value / total * 100);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return 0;
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
        ///     Get random number
        /// </summary>
        /// <param name="random"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
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
        /// <returns></returns>
        public static bool IsPrime(this int number)
        {
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
        /// <param name="sourceNumber">Number to check</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetNumberSuffix(this int sourceNumber)
        {
            var text = sourceNumber.ToString();
            string result;
            if (text.Length > 2)
            {
                var num = int.Parse(text.Substring(text.Length - 2, 2));
                if (num >= 11 && num <= 13)
                {
                    result = "th";
                    return result;
                }
            }
            if (sourceNumber > 20)
            {
                switch (int.Parse(text.Substring(text.Length - 1, 1)))
                {
                    case 1:
                        result = "st";
                        return result;
                    case 2:
                        result = "nd";
                        return result;
                    case 3:
                        result = "rd";
                        return result;
                }
            }
            else
            {
                switch (sourceNumber)
                {
                    case 1:
                        result = "st";
                        return result;
                    case 2:
                        result = "nd";
                        return result;
                    case 3:
                        result = "rd";
                        return result;
                }
            }
            result = "th";

            return result;
        }
    }
}