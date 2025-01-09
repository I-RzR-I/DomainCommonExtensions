// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2025-01-08 09:27
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-01-08 09:28
// ***********************************************************************
//  <copyright file="TypeBuilderExtensions.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

// ReSharper disable RedundantUsingDirective

using System;
using System.Reflection;
using System.Reflection.Emit;

#endregion

namespace DomainCommonExtensions.CommonExtensions
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A type builder extensions.
    /// </summary>
    /// =================================================================================================
    public static class TypeBuilderExtensions
    {

#if !(NET35 || NET40 || SILVERLIGHT || WPSL || UAP10_0)

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Creates a System.Type object for the class. After defining fields and methods 
        ///     on the class, CreateType is called in order to load its Type object.
        /// </summary>
        /// <param name="tb">The TypeBuilder to act on.</param>
        /// <returns>
        ///     Returns the new System.Type object for this class.
        /// </returns>
        /// =================================================================================================
        public static Type CreateType(this TypeBuilder tb)
        {
            return tb.CreateTypeInfo().AsType();
        }
#endif

#if NET35 || NET40 || SILVERLIGHT || WPSL || NETCOREAPP || NETSTANDARD2_1

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A TypeBuilder extension method that define property.
        /// </summary>
        /// <param name="tb">The TypeBuilder to act on.</param>
        /// <param name="name">The name.</param>
        /// <param name="attributes">The attributes.</param>
        /// <param name="callingConvention">The calling convention.</param>
        /// <param name="returnType">Type of the return.</param>
        /// <param name="parameterTypes">List of types of the parameters.</param>
        /// <returns>
        ///     A PropertyBuilder.
        /// </returns>
        /// =================================================================================================
        public static PropertyBuilder DefineProperty(this TypeBuilder tb, string name, PropertyAttributes attributes, 
            CallingConventions callingConvention, Type returnType, Type[] parameterTypes)
        {
            return tb.DefineProperty(name, attributes, returnType, parameterTypes);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A TypeBuilder extension method that converts a builder to a type.
        /// </summary>
        /// <param name="builder">The builder to act on.</param>
        /// <returns>
        ///     A Type.
        /// </returns>
        /// =================================================================================================
        public static Type AsType(this TypeBuilder builder)
        {
            return builder;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A GenericTypeParameterBuilder extension method that converts a builder to a type.
        /// </summary>
        /// <param name="builder">The builder to act on.</param>
        /// <returns>
        ///     A Type.
        /// </returns>
        /// =================================================================================================
        public static Type AsType(this GenericTypeParameterBuilder builder)
        {
            return builder;
        }
#endif
    }
}