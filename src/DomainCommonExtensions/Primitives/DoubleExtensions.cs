// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-08-12 23:30
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-12 23:39
// ***********************************************************************
//  <copyright file="DoubleExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using RzR.Extensions.Domain.Validation;

#endregion

namespace RzR.Extensions.Domain.Primitives
{
    /// <summary>
    ///     Double extensions
    /// </summary>
    /// <remarks></remarks>
    public static class DoubleExtensions
    {
        /// <summary>
        ///     Convert from minute to milliseconds
        /// </summary>
        /// <param name="minutes">Minute to convert</param>
        /// <returns>
        ///     Milliseconds from minute, saturated to the <see cref="int"/> range
        /// </returns>
        public static int MinutesToMs(this double minutes)
        {
            DomainEnsure.IsNotNull(minutes, nameof(minutes));

            return TimeSpan.FromMinutes(minutes).TotalMilliseconds.ToSaturatedInt();
        }

        /// <summary>
        ///     Truncate a double to int, clamping values outside the int range to int.MinValue / int.MaxValue
        /// </summary>
        /// <param name="value">Value to convert (NaN gives 0)</param>
        /// <returns>
        ///     Truncated value saturated to the <see cref="int"/> range
        /// </returns>
        internal static int ToSaturatedInt(this double value)
        {
            if (double.IsNaN(value))
                return 0;

            if (value >= int.MaxValue)
                return int.MaxValue;

            if (value <= int.MinValue)
                return int.MinValue;

            return (int)value;
        }

        /// <summary>
        ///     Convert from minute to seconds
        /// </summary>
        /// <param name="minutes">Minute to convert</param>
        /// <returns>
        ///     Seconds from minute
        /// </returns>
        public static double MinutesToSeconds(this double minutes)
        {
            DomainEnsure.IsNotNull(minutes, nameof(minutes));

            return TimeSpan.FromMinutes(minutes).TotalSeconds;
        }
    }
}
