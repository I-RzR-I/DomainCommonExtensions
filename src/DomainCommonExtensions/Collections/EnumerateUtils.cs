// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-12-09 01:26
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-12-09 01:45
// ***********************************************************************
//  <copyright file="EnumerateUtils.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections.Generic;

#endregion

namespace RzR.Extensions.Domain.Collections
{
    /// <summary>
    ///     Enumerate utils
    /// </summary>
    public static class EnumerateUtils
    {
        /// <summary>
        ///     Get enumerate from specified range
        /// </summary>
        /// <param name="from">Required. Start range</param>
        /// <param name="upToIncluded">Required. End of range</param>
        /// <param name="stepSize">Optional. The default value is 1. Must be greater than zero.</param>
        /// <returns>Enumerate.FromTo(0, 3) => 0, 1, 2, 3</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="stepSize"/> is less than or equal to zero.</exception>
        /// <remarks>The step size is validated when the method is called, not when it is enumerated.</remarks>
        public static IEnumerable<int> FromTo(int from, int upToIncluded, int stepSize = 1)
        {
            if (stepSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(stepSize), stepSize, "The step size must be greater than zero.");

            return FromToIterator(from, upToIncluded, stepSize);
        }

        /// <summary>
        ///     Get enumerate from specified range
        /// </summary>
        /// <param name="from">Required. Start range</param>
        /// <param name="upToIncluded">Required. End of range</param>
        /// <param name="stepSize">Range step size. Must be greater than zero.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="stepSize"/> is less than or equal to zero.</exception>
        /// <remarks>The step size is validated when the method is called, not when it is enumerated.</remarks>
        public static IEnumerable<DateTime> FromTo(DateTime from, DateTime upToIncluded, TimeSpan stepSize)
        {
            if (stepSize <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(stepSize), stepSize, "The step size must be greater than zero.");

            return FromToIterator(from, upToIncluded, stepSize);
        }

        /// <summary>
        ///     Return power on N
        /// </summary>
        /// <param name="number"></param>
        /// <param name="upToIncluded"></param>
        /// <returns></returns>
        /// <remarks>
        ///     (n^0, n^1, n^2, n^3,..., n^X). The sequence also stops when an item is NaN or infinity,
        ///     or repeats one of the two previous items (e.g. for 0, 1 or -1).
        ///     The first item n^0 is always 1 (also for NaN or infinity) on every target framework; when 1 is
        ///     greater than <paramref name="upToIncluded"/> the sequence is empty.
        /// </remarks>
        public static IEnumerable<double> PowersOf(double number, double upToIncluded)
        {
            var i = 0.0;
            var previous = double.NaN;
            var beforePrevious = double.NaN;
            while (true)
            {
                var item = i == 0.0 ? 1.0 : Math.Pow(number, i);

                if (double.IsNaN(item) || double.IsInfinity(item) || item == previous || item == beforePrevious)
                    break;
                if (item > upToIncluded) break;

                yield return item;

                beforePrevious = previous;
                previous = item;
                i += 1.0;
            }
        }

        /// <summary>
        ///     Lazily enumerate the integer range; a long counter avoids overflow near
        ///      <see cref="int.MaxValue"/>.
        /// </summary>
        /// <param name="from">Start range.</param>
        /// <param name="upToIncluded">End of range.</param>
        /// <param name="stepSize">Validated positive step size.</param>
        /// <returns>
        ///     An enumerator that allows foreach to be used to process from to iterator in this
        ///     collection.
        /// </returns>
        private static IEnumerable<int> FromToIterator(int from, int upToIncluded, int stepSize)
        {
            for (long i = from; i <= upToIncluded; i += stepSize) 
                yield return (int)i;
        }

        /// <summary>
        ///     Lazily enumerate the date range; stops before a step that would pass <paramref name="upToIncluded"/>.
        /// </summary>
        /// <param name="from">Start range</param>
        /// <param name="upToIncluded">End of range</param>
        /// <param name="stepSize">Validated positive step size</param>
        /// <returns></returns>
        private static IEnumerable<DateTime> FromToIterator(DateTime from, DateTime upToIncluded, TimeSpan stepSize)
        {
            var i = from;
            while (i <= upToIncluded)
            {
                yield 
                    return i;

                if (upToIncluded - i < stepSize) 
                    yield break;

                i += stepSize;
            }
        }
    }
}
