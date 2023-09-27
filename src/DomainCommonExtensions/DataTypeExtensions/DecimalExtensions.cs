// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-12-08 21:58
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-12-08 21:58
// ***********************************************************************
//  <copyright file="DecimalExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using DomainCommonExtensions.CommonExtensions;

namespace DomainCommonExtensions.DataTypeExtensions
{
    /// <summary>
    ///     Decimal extensions
    /// </summary>
    /// <remarks></remarks>
    public static class DecimalExtensions
    {
        /// <summary>
        ///     Check if value is NULL or 0
        /// </summary>
        /// <param name="value">Input value to check</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsNullOrZero(this decimal? value)
        {
            return value.IsNull() || value == 0;
        }

        /// <summary>
        ///     Check if the value is == 0
        /// </summary>
        /// <param name="value">Input value to check</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsZero(this decimal value)
        {
            return value == 0;
        }

        /// <summary>
        ///     Check if the value is less or equal with 0
        /// </summary>
        /// <param name="value">Input value to check</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsLessOrEqualZero(this decimal value)
        {
            return value <= 0;
        }

        /// <summary>
        ///     Check if the value is greater than or equal with 0
        /// </summary>
        /// <param name="value">Input value to check</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsGreaterThanOrEqualZero(this decimal value)
        {
            return value >= 0;
        }

        /// <summary>
        ///     Check if the value is greater than 0
        /// </summary>
        /// <param name="value">Input value to check</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsGreaterThanZero(this decimal value)
        {
            return value > 0;
        }
    }
}