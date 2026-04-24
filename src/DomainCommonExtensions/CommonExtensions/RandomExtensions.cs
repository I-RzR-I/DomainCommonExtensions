// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-08-10 21:39
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-12 23:39
// ***********************************************************************
//  <copyright file="RandomExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using RzR.Extensions.Domain.DataTypeExtensions;

#endregion

namespace RzR.Extensions.Domain.CommonExtensions
{
    /// <summary>
    ///     Random extensions
    /// </summary>
    /// <remarks></remarks>
    public static class RandomExtensions
    {
        /// <summary>
        ///     Generates a random boolean value
        /// </summary>
        /// <param name="random">Generator</param>
        /// <returns></returns>
        public static bool NextBool(this Random random)
        {
            return (random.Next() % 2).IsZero();
        }
    }
}