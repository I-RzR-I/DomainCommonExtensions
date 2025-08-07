#if NET45_OR_GREATER || NET || NETSTANDARD1_0_OR_GREATER
// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2025-08-06 21:42
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-08-06 21:43
// ***********************************************************************
//  <copyright file="AssemblyExtensions.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#endregion

namespace DomainCommonExtensions.CommonExtensions.Reflection
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     An assembly extensions.
    /// </summary>
    /// <content>
    ///     An assembly extensions.
    /// </content>
    /// =================================================================================================
    public static partial class AssemblyExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     An Assembly extension method that gets the types.
        /// </summary>
        /// <param name="assembly">The assembly to act on.</param>
        /// <returns>
        ///     An array of type.
        /// </returns>
        /// =================================================================================================
        public static IEnumerable<Type> GetTypes(this Assembly assembly)
        {
            return assembly.DefinedTypes.Select(i => i.AsType());
        }
    }
}
#endif