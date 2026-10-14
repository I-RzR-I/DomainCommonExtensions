// ***********************************************************************
//  Assembly          : RzR.Shared.Extensions.DomainCommonExtensions
//  Author            : RzR
//  Created On        : 2026-10-14 10:10
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-10-14 10:56
//  ***********************************************************************
//  <copyright file="CallerArgumentExpressionAttribute.cs" company="RzR SOFT & TECH">
//      Copyright (c) RzR. All rights reserved.
//  </copyright>
//  <contact>
//      https://iamrzr.dev/contact
//  </contact>
//  <summary></summary>
//  ***********************************************************************

#region U S I N G

using System;

#endregion

#if !NETCOREAPP3_0_OR_GREATER
// ReSharper disable once CheckNamespace
namespace System.Runtime.CompilerServices
{
    /// <summary>
    ///     Internal polyfill of the compiler attribute that captures the source text of the argument
    ///     passed for another parameter, for target frameworks whose reference assemblies lack it
    ///     (net40, net45, netstandard2.0 and netstandard2.1).
    /// </summary>
    /// <seealso cref="T:Attribute"/>
    [AttributeUsage(AttributeTargets.Parameter)]
    internal sealed class CallerArgumentExpressionAttribute : Attribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="CallerArgumentExpressionAttribute" /> class.
        /// </summary>
        /// <param name="parameterName">
        ///     The name of the parameter whose argument expression is captured.
        /// </param>
        public CallerArgumentExpressionAttribute(string parameterName)
        {
            ParameterName = parameterName;
        }

        /// <summary>
        ///     Gets the name of the parameter whose argument expression is captured.
        /// </summary>
        /// <value>
        ///     The name of the target parameter.
        /// </value>
        public string ParameterName { get; }
    }
}
#endif