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

namespace DomainCommonExtensions.DataTypeExtensions
{
    /// <summary>
    ///     Bool extensions
    /// </summary>
    /// <remarks></remarks>
    public static class BoolExtensions
    {
        /// <summary>
        ///     Negate
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool Negate(this bool? source)
        {
            source ??= false;

            return (bool)!source;
        }

        /// <summary>
        ///     Negate
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool Negate(this bool source)
        {
            return !source;
        }
    }
}