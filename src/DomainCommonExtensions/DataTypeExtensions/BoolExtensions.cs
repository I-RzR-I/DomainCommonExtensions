// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-08-10 21:39
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-12 23:39
// ***********************************************************************
//  <copyright file="BoolExtensions.cs" company="">
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
    ///     Bool extensions
    /// </summary>
    /// <remarks></remarks>
    public static class BoolExtensions
    {
        /// <summary>
        ///     Negate source value.
        /// </summary>
        /// <param name="source">Source object to be checked.</param>
        /// <returns></returns>
        public static bool Negate(this bool? source)
        {
            source ??= false;

            return (bool)!source;
        }

        /// <summary>
        ///     Negate source value.
        /// </summary>
        /// <param name="source">Source object to be checked.</param>
        /// <returns></returns>
        public static bool Negate(this bool source)
            => !source;

        /// <summary>
        ///     Check if source value is equals with true.
        /// </summary>
        /// <param name="source">Source object to be checked.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsTrue(this bool source)
            => source.IsNotNull() && source.Equals(true);

        /// <summary>
        ///     Check if source value is equals with true.
        /// </summary>
        /// <param name="source">Source object to be checked.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsTrue(this bool? source)
                => source.IsNotNull() && source.Equals(true);

        /// <summary>
        ///     Check if source value is equals with false.
        /// </summary>
        /// <param name="source">Source object to be checked.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsFalse(this bool source)
            => source.IsNull() || source.Equals(false);

        /// <summary>
        ///     Check if source value is equals with false.
        /// </summary>
        /// <param name="source">Source object to be checked.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsFalse(this bool? source)
                    => source.IsNull() || source.Equals(false);
    }
}