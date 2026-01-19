// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2025-02-16 19:03
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-02-19 18:23
// ***********************************************************************
//  <copyright file="RandomHelper.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Threading;
using DomainCommonExtensions.ArraysExtensions;
using DomainCommonExtensions.Resources.Enums;
using DomainCommonExtensions.Utilities.Ensure;

#endregion

namespace DomainCommonExtensions.Helpers
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A random helper.
    /// </summary>
    /// =================================================================================================
    public class RandomHelper
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the letter alphabet.
        /// </summary>
        /// =================================================================================================
        private const string LetterAlphabet = LetterUpperAlphabet + LetterLowerAlphabet;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the letter lower alphabet.
        /// </summary>
        /// =================================================================================================
        private const string LetterLowerAlphabet = "abcdefghijklmnopqrstuvwxyz";

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the letter upper alphabet.
        /// </summary>
        /// =================================================================================================
        private const string LetterUpperAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the alphanumeric alphabet.
        /// </summary>
        /// =================================================================================================
        private const string AlphaNumericAlphabet = LetterAlphabet + "0123456789";

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the URL safe alphabet.
        /// </summary>
        /// =================================================================================================
        private const string UrlSafeAlphabet = AlphaNumericAlphabet + "-_";

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the locker.
        /// </summary>
        /// =================================================================================================
        private static readonly Lazy<object> Locker = new Lazy<object>(() => new object(), LazyThreadSafetyMode.ExecutionAndPublication);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the local seed.
        /// </summary>
        /// =================================================================================================
        private readonly Random _localSeed;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the seed.
        /// </summary>
        /// =================================================================================================
        private readonly Random _seed = new Random();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="RandomHelper" /> class.
        /// </summary>
        /// =================================================================================================
        private RandomHelper() => _localSeed = _seed ?? new Random();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="RandomHelper" /> class.
        /// </summary>
        /// <param name="localSeed">Random seed.</param>
        /// =================================================================================================
        public RandomHelper(Random localSeed) => _localSeed = localSeed;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Random instance.
        /// </summary>
        /// <value>
        ///     The instance.
        /// </value>
        /// =================================================================================================
        public static RandomHelper Instance { get; } = new RandomHelper();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Generate new random number.
        /// </summary>
        /// <param name="min">(Optional) Optional. The default value is 0 (inclusive).</param>
        /// <param name="max">(Optional) Optional. The default value is 1 (inclusive).</param>
        /// <returns>
        ///     An int.
        /// </returns>
        /// =================================================================================================
        public int Number(int min = 0, int max = 1)
        {
            lock (Locker.Value)
            {
                if (max < int.MaxValue) return _localSeed.Next(min, max + 1);
                if (min > int.MinValue) return 1 + _localSeed.Next(min - 1, max);

                var num = _localSeed.Next();
                var num2 = _localSeed.Next();
                var num3 = (num >> 8) & 0xFFFF;
                var num4 = (num2 >> 8) & 0xFFFF;

                return (num3 << 16) | num4;
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Generates a random long value within a range.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when one or more arguments are outside the required range.
        /// </exception>
        /// <param name="min">Optional. The default value is 0 (inclusive).</param>
        /// <param name="max">Optional. The default value is 1 (inclusive).</param>
        /// <returns>
        ///     A long.
        /// </returns>
        /// =================================================================================================
        public long Long(long min, long max)
        {
            if (min > max)
                throw new ArgumentOutOfRangeException(nameof(min));

            lock (Locker.Value)
            {
                var buffer = new byte[8];
                _localSeed.NextBytes(buffer);
                var value = BitConverter.ToInt64(buffer, 0);

                return Math.Abs(value % (max - min + 1)) + min;
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Generates a random double between 0.0 and 1.0.
        /// </summary>
        /// <returns>
        ///     A double.
        /// </returns>
        /// =================================================================================================
        public double Double()
        {
            lock (Locker.Value)
            {
                return _localSeed.NextDouble();
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Generates a random decimal between 0 and 1.
        /// </summary>
        /// <returns>
        ///     A decimal.
        /// </returns>
        /// =================================================================================================
        public decimal Decimal() => (decimal)Double();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Generate new random bool value.
        /// </summary>
        /// <returns>
        ///     New bool value.
        /// </returns>
        /// =================================================================================================
        public bool Bool()
        {
            lock (Locker.Value)
            {
                return _localSeed.Next() > (int.MaxValue / 2);
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the random letter.
        /// </summary>
        /// <remarks>
        ///     Random letter form [a-zA-Z].
        /// </remarks>
        /// <returns>
        ///     A random letter (string).
        /// </returns>
        /// =================================================================================================
        public string Letter()
        {
            lock (Locker.Value)
            {
                var idx = _localSeed.Next(LetterAlphabet.Length);
                var xChar = LetterAlphabet[idx];

                return new string(new[] { xChar });
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the random letters.
        /// </summary>
        /// <remarks>
        ///     Random letter form [a-zA-Z].
        /// </remarks>
        /// <param name="length">Number of letters.</param>
        /// <returns>
        ///     A random letters (string).
        /// </returns>
        /// =================================================================================================
        public string Letters(int length)
        {
            lock (Locker.Value)
            {
                var charArray = new char[length];

                for (var i = 0; i < length; i++)
                {
                    var idx = _localSeed.Next(LetterAlphabet.Length);
                    var xChar = LetterAlphabet[idx];
                    charArray[i] = xChar;
                }

                return new string(charArray);
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Returns a random lowercase letter.
        /// </summary>
        /// <remarks>
        ///     Random letter form [a-z].
        /// </remarks>
        /// <returns>
        ///     A char.
        /// </returns>
        /// =================================================================================================
        public char LowerLetter()
        {
            lock (Locker.Value)
            {
                var idx = _localSeed.Next(LetterLowerAlphabet.Length);

                return LetterLowerAlphabet[idx];
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Returns a random uppercase letter.
        /// </summary>
        /// <remarks>
        ///     Random letter form [A-Z].
        /// </remarks>
        /// <returns>
        ///     A char.
        /// </returns>
        /// =================================================================================================
        public char UpperLetter()
        {
            lock (Locker.Value)
            {
                var idx = _localSeed.Next(LetterUpperAlphabet.Length);

                return LetterUpperAlphabet[idx];
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Generates a random alphanumeric string.
        /// </summary>
        /// <param name="length">Number of letters.</param>
        /// <returns>
        ///     A string.
        /// </returns>
        /// =================================================================================================
        public string AlphaNumeric(int length)
        {
            lock (Locker.Value)
            {
                var buffer = new char[length];

                for (var i = 0; i < length; i++)
                    buffer[i] = AlphaNumericAlphabet[_localSeed.Next(AlphaNumericAlphabet.Length)];

                return new string(buffer);
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Generates a random numeric string.
        /// </summary>
        /// <param name="length">Number of letters.</param>
        /// <returns>
        ///     A string.
        /// </returns>
        /// =================================================================================================
        public string Digits(int length)
        {
            lock (Locker.Value)
            {
                var buffer = new char[length];

                for (var i = 0; i < length; i++)
                    buffer[i] = (char)('0' + _localSeed.Next(10));

                return new string(buffer);
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Generates a new Guid.
        /// </summary>
        /// <returns>
        ///     A GUID.
        /// </returns>
        /// =================================================================================================
        public Guid Guid() => System.Guid.NewGuid();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Generates a short, URL-safe token.
        /// </summary>
        /// <param name="length">(Optional) Number of letters.</param>
        /// <returns>
        ///     A string.
        /// </returns>
        /// =================================================================================================
        public string Token(int length = 16)
        {
            lock (Locker.Value)
            {
                var buffer = new char[length];

                for (var i = 0; i < length; i++)
                    buffer[i] = UrlSafeAlphabet[_localSeed.Next(UrlSafeAlphabet.Length)];

                return new string(buffer);
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Generates a random DateTime between two dates.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Thrown when one or more arguments have unsupported or illegal values.
        /// </exception>
        /// <param name="min">Optional. The default value is 0 (inclusive).</param>
        /// <param name="max">Optional. The default value is 1 (inclusive).</param>
        /// <returns>
        ///     A DateTime.
        /// </returns>
        /// =================================================================================================
        public DateTime DateTime(DateTime min, DateTime max)
        {
            if (min > max)
                DomainEnsure.ThrowException("min <= max", ExceptionType.ArgumentException);

            var range = (max - min).Ticks;
            var randTicks = Long(0, range);

            return min.AddTicks(randTicks);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Generates a random TimeSpan within a range.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Thrown when one or more arguments have unsupported or illegal values.
        /// </exception>
        /// <param name="min">Optional. The default value is 0 (inclusive).</param>
        /// <param name="max">Optional. The default value is 1 (inclusive).</param>
        /// <returns>
        ///     A TimeSpan.
        /// </returns>
        /// =================================================================================================
        public TimeSpan TimeSpan(TimeSpan min, TimeSpan max)
        {
            if (min > max)
                DomainEnsure.ThrowException("min <= max", ExceptionType.ArgumentException);

            var ticks = Long(min.Ticks, max.Ticks);

            return System.TimeSpan.FromTicks(ticks);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Picks a random item from an array.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Thrown when one or more arguments have unsupported or illegal values.
        /// </exception>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="values">A variable-length parameters list containing values.</param>
        /// <returns>
        ///     A T.
        /// </returns>
        /// =================================================================================================
        public T Pick<T>(params T[] values)
        {
            if (values.IsNullOrEmptyEnumerable())
                DomainEnsure.ThrowException("Values cannot be null or empty", ExceptionType.ArgumentException);

            lock (Locker.Value)
            {
                return values[_localSeed.Next(values.Length)];
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Shuffles an array in-place.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="array">The array.</param>
        /// =================================================================================================
        public void Shuffle<T>(T[] array)
        {
            if (array.IsNullOrEmptyEnumerable())
                DomainEnsure.ThrowException("Array cannot be null or empty", ExceptionType.ArgumentNullException);

            lock (Locker.Value)
            {
                for (var i = array.Length - 1; i > 0; i--)
                {
                    var j = _localSeed.Next(i + 1);

                    T temp = array[i];
                    array[i] = array[j];
                    array[j] = temp;
                }
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Returns a random enum value.
        /// </summary>
        /// <typeparam name="TEnum">Type of the enum.</typeparam>
        /// <returns>
        ///     A TEnum.
        /// </returns>
        /// =================================================================================================
        public TEnum Enum<TEnum>() where TEnum : struct, Enum
        {
            lock (Locker.Value)
            {
                var values = System.Enum.GetValues(typeof(TEnum));

                return (TEnum)values.GetValue(_localSeed.Next(values.Length))!;
            }
        }
    }
}