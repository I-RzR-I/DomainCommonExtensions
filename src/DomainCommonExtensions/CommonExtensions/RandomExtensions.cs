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

#endregion

namespace DomainCommonExtensions.CommonExtensions
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
            return random.Next() % 2 == 0;
        }
    }
}