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

namespace DomainCommonExtensions.Utilities
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
        /// <param name="stepSize">Optional. The default value is 1.</param>
        /// <returns>Enumerate.FromTo(0, 3) => 0, 1, 2, 3</returns>
        /// <remarks></remarks>
        public static IEnumerable<int> FromTo(int from, int upToIncluded, int stepSize = 1)
        {
            for (var i = from; i <= upToIncluded; i += stepSize) yield return i;
        }

        /// <summary>
        ///     Get enumerate from specified range
        /// </summary>
        /// <param name="from">Required. Start range</param>
        /// <param name="upToIncluded">Required. End of range</param>
        /// <param name="stepSize">Range step size</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static IEnumerable<DateTime> FromTo(DateTime from, DateTime upToIncluded, TimeSpan stepSize)
        {
            var i = from;
            while (i <= upToIncluded)
            {
                yield return i;
                i += stepSize;
            }
        }

        /// <summary>
        ///     Return power on N
        /// </summary>
        /// <param name="number"></param>
        /// <param name="upToIncluded"></param>
        /// <returns></returns>
        /// <remarks>(n^0, n^1, n^2, n^3,..., n^X)</remarks>
        public static IEnumerable<double> PowersOf(double number, double upToIncluded)
        {
            var i = 0.0;
            while (true)
            {
                var item = Math.Pow(number, i);
                if (item > upToIncluded) break;
                yield return item;
                i += 1.0;
            }
        }
    }
}