// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2025-01-08 09:30
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-01-08 09:32
// ***********************************************************************
//  <copyright file="AssemblyBuilderFactory.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#if !UAP10_0

#region U S A G E S

#if NET40
using System;
#endif

using System.Reflection;
using System.Reflection.Emit;

#endregion

namespace DomainCommonExtensions.Helpers.Internal.AnonymousSelect.Factory
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     An assembly builder factory.
    /// </summary>
    /// =================================================================================================
    internal static class AssemblyBuilderFactory
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Defines a dynamic assembly that has the specified name and access rights.
        /// </summary>
        /// <param name="name">The name of the assembly.</param>
        /// <param name="access">The access rights of the assembly.</param>
        /// <returns>
        ///     An object that represents the new assembly.
        /// </returns>
        /// =================================================================================================
        internal static AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access)
        {
#if (NET35 || NET40 || SILVERLIGHT)
            return AppDomain.CurrentDomain.DefineDynamicAssembly(name, access);
#else
            return AssemblyBuilder.DefineDynamicAssembly(name, access);
#endif
        }
    }
}
#endif