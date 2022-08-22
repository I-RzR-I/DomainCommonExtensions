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
using DomainCommonExtensions.CommonExtensions;

#endregion

namespace DomainCommonExtensions.DataTypeExtensions
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
        ///     Milliseconds from minute
        /// </returns>
        public static int MinutesToMs(this double minutes)
        {
            if (minutes.IsNull())
                throw new ArgumentNullException(nameof(minutes));

            return (int)TimeSpan.FromMinutes(minutes).TotalMilliseconds;
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
            if (minutes.IsNull())
                throw new ArgumentNullException(nameof(minutes));

            return TimeSpan.FromMinutes(minutes).TotalSeconds;
        }
    }
}