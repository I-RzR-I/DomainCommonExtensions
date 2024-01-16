// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-08-10 21:39
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-12 23:39
// ***********************************************************************
//  <copyright file="IntExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using System;
using System.Globalization;
using DomainCommonExtensions.CommonExtensions;

namespace DomainCommonExtensions.DataTypeExtensions
{
    /// <summary>
    ///     Int - Int32 extensions
    /// </summary>
    /// <remarks></remarks>
    public static class IntExtensions
    {
        /// <summary>
        ///     Check if value is NULL or 0
        /// </summary>
        /// <param name="value">Input value to check</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsNullOrZero(this int? value)
        {
            return value.IsNull() || value == 0;
        }

        /// <summary>
        ///     Check if the value is == 0
        /// </summary>
        /// <param name="value">Input value to check</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsZero(this int value)
        {
            return value == 0;
        }

        /// <summary>
        ///     Check if the value is less or equal with 0
        /// </summary>
        /// <param name="value">Input value to check</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsLessOrEqualZero(this int value)
        {
            return value <= 0;
        }

        /// <summary>
        ///     Check if the value is greater than or equal with 0
        /// </summary>
        /// <param name="value">Input value to check</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsGreaterThanOrEqualZero(this int value)
        {
            return value >= 0;
        }

        /// <summary>
        ///     Check if the value is greater than 0
        /// </summary>
        /// <param name="value">Input value to check</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsGreaterThanZero(this int value)
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
        public static long SetFlag(this int value, int flag, bool set)
        {
            return (value & ~flag) | (set ? flag : 0);
        }

        /// <summary>
        ///     Checks if the role flag is set
        /// </summary>
        /// <param name="value"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static bool IsFlagSet(this int value, int flag)
        {
            return (value & flag) != 0;
        }

        /// <summary>
        ///     Check current value is less than 0
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsLessZero(this int value)
        {
            return value < 0;
        }

        /// <summary>
        ///     Check current value is less than 0
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsLessZero(this int? value)
        {
            return  (value ?? 0) < 0;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Gets excel column name.</summary>
        /// <param name="columnIndex">Zero-based index of the column.</param>
        /// <returns>The excel column name.</returns>
        ///=================================================================================================
        public static string GetExcelColumnName(this int columnIndex)
        {
            // A - Z
            if (columnIndex >= 0 && columnIndex <= 25)
            {
                return ((char)('A' + columnIndex)).ToString();
            }

            // AA - ZZ
            if (columnIndex >= 26 && columnIndex <= 701)
            {
                var firstChar = (char)('A' + (columnIndex / 26) - 1);
                var secondChar = (char)('A' + (columnIndex % 26));

                return string.Format(CultureInfo.InvariantCulture, "{0}{1}", firstChar, secondChar);
            }

            // 17576
            // AAA - ZZZ
            if (columnIndex >= 702 && columnIndex <= 18277)
            {
                var fc = (columnIndex - 702) / 676;
                var sc = ((columnIndex - 702) % 676) / 26;
                var tc = ((columnIndex - 702) % 676) % 26;

                var firstChar = (char)('A' + fc);
                var secondChar = (char)('A' + sc);
                var thirdChar = (char)('A' + tc);

                return string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}", firstChar, secondChar, thirdChar);
            }
            
            throw new ArgumentOutOfRangeException($"Column reference ({columnIndex}) is out of range");
        }
    }
}