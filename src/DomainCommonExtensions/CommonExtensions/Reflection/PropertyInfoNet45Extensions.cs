#if NET45_OR_GREATER || NET || NETSTANDARD1_0_OR_GREATER
// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2025-08-06 21:38
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-08-06 21:38
// ***********************************************************************
//  <copyright file="PropertyInfoExtensions.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Reflection;
using DomainCommonExtensions.DataTypeExtensions;

#endregion

namespace DomainCommonExtensions.CommonExtensions.Reflection
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A property information extensions.
    /// </summary>
    /// <content>
    ///     A property information extensions.
    /// </content>
    /// =================================================================================================
    public static partial class PropertyInfoExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A PropertyInfo extension method that gets 'SetMethod' method.
        /// </summary>
        /// <param name="property">The property to act on.</param>
        /// <param name="nonPublic">(Optional) True to non public.</param>
        /// <returns>
        ///     The method 'SetMethod'.
        /// </returns>
        /// =================================================================================================
        public static MethodInfo GetSetMethod(this PropertyInfo property, bool nonPublic = true)
        {
            return nonPublic || (property.SetMethod?.IsPublic).IsTrue() ? property.SetMethod : null;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A PropertyInfo extension method that gets 'GetMethod' method.
        /// </summary>
        /// <param name="property">The property to act on.</param>
        /// <param name="nonPublic">(Optional) True to non public.</param>
        /// <returns>
        ///     The method 'GetMethod'.
        /// </returns>
        /// =================================================================================================
        public static MethodInfo GetGetMethod(this PropertyInfo property, bool nonPublic = true)
        {
            return nonPublic || (property.GetMethod?.IsPublic).IsTrue() ? property.GetMethod : null;
        }
    }
}
#endif