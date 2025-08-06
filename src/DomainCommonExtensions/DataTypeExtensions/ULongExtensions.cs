// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-12-08 21:47
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-12-08 21:47
// ***********************************************************************
//  <copyright file="LongExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using DomainCommonExtensions.CommonExtensions;

#endregion

namespace DomainCommonExtensions.DataTypeExtensions
{
    /// <summary>
    ///     ULong extensions
    /// </summary>
    /// <remarks></remarks>
    public static class ULongExtensions
    {
        /// <summary>
        ///     Check if value is NULL or 0
        /// </summary>
        /// <param name="value">Input value to check</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsNullOrZero(this ulong? value)
        {
            return value.IsNull() || value == 0;
        }

        /// <summary>
        ///     Check if the value is == 0
        /// </summary>
        /// <param name="value">Input value to check</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsZero(this ulong value)
        {
            return value == 0;
        }

        /// <summary>
        ///     Check if the value is greater than 0
        /// </summary>
        /// <param name="value">Input value to check</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsGreaterThanZero(this ulong value)
        {
            return value > 0;
        }
    }
}