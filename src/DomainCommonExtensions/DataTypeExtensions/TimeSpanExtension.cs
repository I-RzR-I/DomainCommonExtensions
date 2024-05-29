// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-12-08 18:58
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-12-08 18:59
// ***********************************************************************
//  <copyright file="TimeSpanExtension.cs" company="">
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
    ///     Time span extensions
    /// </summary>
    public static class TimeSpanExtension
    {
        /// <summary>
        ///     Returns the absolute value of the timestamp
        /// </summary>
        /// <param name="source">Source TimeSpan</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static TimeSpan Absolute(this TimeSpan source)
        {
            if (source.IsNull()) return TimeSpan.Zero;

            return source.Ticks.IsGreaterThanOrEqualZero() ? source : source.Negate();
        }
    }
}