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
            return value == null || value == 0;
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
    }
}