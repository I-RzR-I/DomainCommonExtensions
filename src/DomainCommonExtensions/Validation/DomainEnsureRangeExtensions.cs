// ***********************************************************************
//  Assembly          : RzR.Shared.Extensions.DomainCommonExtensions
//  Author            : RzR
//  Created On        : 2026-10-14 22:10
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-10-14 10:57
//  ***********************************************************************
//  <copyright file="DomainEnsureRangeExtensions.cs" company="RzR SOFT & TECH">
//      Copyright (c) RzR. All rights reserved.
//  </copyright>
//  <contact>
//      https://iamrzr.dev/contact
//  </contact>
//  <summary></summary>
//  ***********************************************************************

#region U S I N G

using System;
using System.Globalization;
using System.Runtime.CompilerServices;

#endregion

namespace RzR.Extensions.Domain.Validation
{
    /// <summary>
    ///     Numeric range guard extensions for <see cref="int" />, <see cref="long" />, <see cref="double" />
    ///     and <see cref="decimal" /> values.
    /// </summary>
    public static class DomainEnsureRangeExtensions
    {
        private const string NegativeMessage = "Value cannot be negative.";
        private const string ZeroMessage = "Value cannot be zero.";
        private const string NegativeOrZeroMessage = "Value must be greater than zero.";
        private const string InvalidBoundsMessage = "The lower bound cannot be greater than the upper bound.";

        /// <summary>
        ///     An int extension method that throws if the value is negative.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="value" /> is less than zero.
        /// </exception>
        /// <param name="value">The value to check.</param>
        /// <param name="paramName">
        ///     (Optional) Name of the parameter; defaults to the caller's argument expression.
        /// </param>
        public static void ThrowIfNegative(this int value, [CallerArgumentExpression("value")] string paramName = null)
        {
            if (value < 0)
                ThrowArgumentOutOfRange(paramName, value, NegativeMessage);
        }

        /// <summary>
        ///     An int extension method that throws if the value is zero.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="value" /> equals zero.
        /// </exception>
        /// <param name="value">The value to check.</param>
        /// <param name="paramName">
        ///     (Optional) Name of the parameter; defaults to the caller's argument expression.
        /// </param>
        public static void ThrowIfZero(this int value, [CallerArgumentExpression("value")] string paramName = null)
        {
            if (value == 0)
                ThrowArgumentOutOfRange(paramName, value, ZeroMessage);
        }

        /// <summary>
        ///     An int extension method that throws if the value is negative or zero.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="value" /> is less than or equal to zero.
        /// </exception>
        /// <param name="value">The value to check.</param>
        /// <param name="paramName">
        ///     (Optional) Name of the parameter; defaults to the caller's argument expression.
        /// </param>
        public static void ThrowIfNegativeOrZero(this int value,
            [CallerArgumentExpression("value")] string paramName = null)
        {
            if (value <= 0)
                ThrowArgumentOutOfRange(paramName, value, NegativeOrZeroMessage);
        }

        /// <summary>
        ///     An int extension method that throws if the value is outside an inclusive range.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Thrown when <paramref name="min" /> is greater than <paramref name="max" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="value" /> is outside the inclusive range [<paramref name="min" />
        ///     ,
        ///     <paramref name="max" />].
        /// </exception>
        /// <param name="value">The value to check.</param>
        /// <param name="min">The inclusive lower bound.</param>
        /// <param name="max">The inclusive upper bound.</param>
        /// <param name="paramName">
        ///     (Optional) Name of the parameter; defaults to the caller's argument expression.
        /// </param>
        public static void ThrowIfOutOfRange(this int value, int min, int max,
            [CallerArgumentExpression("value")] string paramName = null)
        {
            if (min > max)
                throw new ArgumentException(InvalidBoundsMessage, nameof(min));
            if (value < min || value > max)
                ThrowArgumentOutOfRange(paramName, value, min, max);
        }

        /// <summary>
        ///     A long extension method that throws if the value is negative.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="value" /> is less than zero.
        /// </exception>
        /// <param name="value">The value to check.</param>
        /// <param name="paramName">
        ///     (Optional) Name of the parameter; defaults to the caller's argument expression.
        /// </param>
        public static void ThrowIfNegative(this long value, [CallerArgumentExpression("value")] string paramName = null)
        {
            if (value < 0)
                ThrowArgumentOutOfRange(paramName, value, NegativeMessage);
        }

        /// <summary>
        ///     A long extension method that throws if the value is zero.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="value" /> equals zero.
        /// </exception>
        /// <param name="value">The value to check.</param>
        /// <param name="paramName">
        ///     (Optional) Name of the parameter; defaults to the caller's argument expression.
        /// </param>
        public static void ThrowIfZero(this long value, [CallerArgumentExpression("value")] string paramName = null)
        {
            if (value == 0)
                ThrowArgumentOutOfRange(paramName, value, ZeroMessage);
        }

        /// <summary>
        ///     A long extension method that throws if the value is negative or zero.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="value" /> is less than or equal to zero.
        /// </exception>
        /// <param name="value">The value to check.</param>
        /// <param name="paramName">
        ///     (Optional) Name of the parameter; defaults to the caller's argument expression.
        /// </param>
        public static void ThrowIfNegativeOrZero(this long value,
            [CallerArgumentExpression("value")] string paramName = null)
        {
            if (value <= 0)
                ThrowArgumentOutOfRange(paramName, value, NegativeOrZeroMessage);
        }

        /// <summary>
        ///     A long extension method that throws if the value is outside an inclusive range.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Thrown when <paramref name="min" /> is greater than <paramref name="max" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="value" /> is outside the inclusive range [<paramref name="min" />
        ///     ,
        ///     <paramref name="max" />].
        /// </exception>
        /// <param name="value">The value to check.</param>
        /// <param name="min">The inclusive lower bound.</param>
        /// <param name="max">The inclusive upper bound.</param>
        /// <param name="paramName">
        ///     (Optional) Name of the parameter; defaults to the caller's argument expression.
        /// </param>
        public static void ThrowIfOutOfRange(this long value, long min, long max,
            [CallerArgumentExpression("value")] string paramName = null)
        {
            if (min > max)
                throw new ArgumentException(InvalidBoundsMessage, nameof(min));
            if (value < min || value > max)
                ThrowArgumentOutOfRange(paramName, value, min, max);
        }

        /// <summary>
        ///     A double extension method that throws if the value is negative.
        /// </summary>
        /// <remarks>
        ///     <see cref="double.NaN" /> and negative zero do not throw: neither compares less than zero.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="value" /> is less than zero.
        /// </exception>
        /// <param name="value">The value to check.</param>
        /// <param name="paramName">
        ///     (Optional) Name of the parameter; defaults to the caller's argument expression.
        /// </param>
        public static void ThrowIfNegative(this double value,
            [CallerArgumentExpression("value")] string paramName = null)
        {
            if (value < 0d)
                ThrowArgumentOutOfRange(paramName, value, NegativeMessage);
        }

        /// <summary>
        ///     A double extension method that throws if the value is zero.
        /// </summary>
        /// <remarks>
        ///     Negative zero throws because it equals zero; <see cref="double.NaN" /> does not throw.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="value" /> equals zero.
        /// </exception>
        /// <param name="value">The value to check.</param>
        /// <param name="paramName">
        ///     (Optional) Name of the parameter; defaults to the caller's argument expression.
        /// </param>
        public static void ThrowIfZero(this double value, [CallerArgumentExpression("value")] string paramName = null)
        {
            if (value == 0d)
                ThrowArgumentOutOfRange(paramName, value, ZeroMessage);
        }

        /// <summary>
        ///     A double extension method that throws if the value is negative or zero.
        /// </summary>
        /// <remarks>
        ///     Negative zero throws because it equals zero; <see cref="double.NaN" /> does not throw
        ///     because it does not compare less than or equal to zero.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="value" /> is less than or equal to zero.
        /// </exception>
        /// <param name="value">The value to check.</param>
        /// <param name="paramName">
        ///     (Optional) Name of the parameter; defaults to the caller's argument expression.
        /// </param>
        public static void ThrowIfNegativeOrZero(this double value,
            [CallerArgumentExpression("value")] string paramName = null)
        {
            if (value <= 0d)
                ThrowArgumentOutOfRange(paramName, value, NegativeOrZeroMessage);
        }

        /// <summary>
        ///     A double extension method that throws if the value is outside an inclusive range.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Thrown when <paramref name="min" /> or <paramref name="max" /> is <see cref="double.NaN" />
        ///     , or <paramref name="min" /> is greater than <paramref name="max" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="value" /> is <see cref="double.NaN" /> or outside the
        ///     inclusive range [<paramref name="min" />, <paramref name="max" />].
        /// </exception>
        /// <param name="value">The value to check.</param>
        /// <param name="min">The inclusive lower bound.</param>
        /// <param name="max">The inclusive upper bound.</param>
        /// <param name="paramName">
        ///     (Optional) Name of the parameter; defaults to the caller's argument expression.
        /// </param>
        public static void ThrowIfOutOfRange(this double value, double min, double max,
            [CallerArgumentExpression("value")] string paramName = null)
        {
            if (double.IsNaN(min))
                throw new ArgumentException("The lower bound cannot be NaN.", nameof(min));

            if (double.IsNaN(max))
                throw new ArgumentException("The upper bound cannot be NaN.", nameof(max));

            if (min > max)
                throw new ArgumentException(InvalidBoundsMessage, nameof(min));

            if (double.IsNaN(value) || value < min || value > max)
                ThrowArgumentOutOfRange(paramName, value, min, max);
        }

        /// <summary>
        ///     A decimal extension method that throws if the value is negative.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="value" /> is less than zero.
        /// </exception>
        /// <param name="value">The value to check.</param>
        /// <param name="paramName">
        ///     (Optional) Name of the parameter; defaults to the caller's argument expression.
        /// </param>
        public static void ThrowIfNegative(this decimal value,
            [CallerArgumentExpression("value")] string paramName = null)
        {
            if (value < 0m)
                ThrowArgumentOutOfRange(paramName, value, NegativeMessage);
        }

        /// <summary>
        ///     A decimal extension method that throws if the value is zero.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="value" /> equals zero.
        /// </exception>
        /// <param name="value">The value to check.</param>
        /// <param name="paramName">
        ///     (Optional) Name of the parameter; defaults to the caller's argument expression.
        /// </param>
        public static void ThrowIfZero(this decimal value, [CallerArgumentExpression("value")] string paramName = null)
        {
            if (value == 0m)
                ThrowArgumentOutOfRange(paramName, value, ZeroMessage);
        }

        /// <summary>
        ///     A decimal extension method that throws if the value is negative or zero.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="value" /> is less than or equal to zero.
        /// </exception>
        /// <param name="value">The value to check.</param>
        /// <param name="paramName">
        ///     (Optional) Name of the parameter; defaults to the caller's argument expression.
        /// </param>
        public static void ThrowIfNegativeOrZero(this decimal value,
            [CallerArgumentExpression("value")] string paramName = null)
        {
            if (value <= 0m)
                ThrowArgumentOutOfRange(paramName, value, NegativeOrZeroMessage);
        }

        /// <summary>
        ///     A decimal extension method that throws if the value is outside an inclusive range.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Thrown when <paramref name="min" /> is greater than <paramref name="max" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="value" /> is outside the inclusive range 
        ///     [<paramref name="min" />, <paramref name="max" />].
        /// </exception>
        /// <param name="value">The value to check.</param>
        /// <param name="min">The inclusive lower bound.</param>
        /// <param name="max">The inclusive upper bound.</param>
        /// <param name="paramName">
        ///     (Optional) Name of the parameter; defaults to the caller's argument expression.
        /// </param>
        public static void ThrowIfOutOfRange(this decimal value, decimal min, decimal max,
            [CallerArgumentExpression("value")] string paramName = null)
        {
            if (min > max)
                throw new ArgumentException(InvalidBoundsMessage, nameof(min));

            if (value < min || value > max)
                ThrowArgumentOutOfRange(paramName, value, min, max);
        }

        /// <summary>
        ///     Throws an <see cref="ArgumentOutOfRangeException" />; kept out of the guards so that the
        ///     non-throwing path stays small enough for the JIT to inline.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Always thrown.</exception>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="actualValue">The rejected value.</param>
        /// <param name="message">The exception message.</param>
        private static void ThrowArgumentOutOfRange(string paramName, object actualValue, string message)
        {
            throw new ArgumentOutOfRangeException(paramName, actualValue, message);
        }

        /// <summary>
        ///     Throws an <see cref="ArgumentOutOfRangeException" /> that names the inclusive range.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Always thrown.</exception>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="actualValue">The rejected value.</param>
        /// <param name="min">The inclusive lower bound.</param>
        /// <param name="max">The inclusive upper bound.</param>
        private static void ThrowArgumentOutOfRange(string paramName, object actualValue, object min, object max)
        {
            throw new ArgumentOutOfRangeException(paramName, actualValue,
                string.Format(CultureInfo.InvariantCulture, "Value must be between {0} and {1} (inclusive).", min, max));
        }
    }
}