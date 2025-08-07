// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2025-08-05 18:23
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-08-05 20:11
// ***********************************************************************
//  <copyright file="DomainEnsureExtensions.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using DomainCommonExtensions.CommonExtensions;
using DomainCommonExtensions.DataTypeExtensions;

#endregion

namespace DomainCommonExtensions.Utilities.Ensure
{
    /// <summary>
    ///     A domain ensures constraints extensions.
    /// </summary>
    public static class DomainEnsureExtensions
    {
        /// <summary>
        ///     Throw exception if source is null
        /// </summary>
        /// <param name="source">Source to check</param>
        /// <param name="message">Error message</param>
        /// <remarks></remarks>
        public static void ThrowIfNull(this object source, string message)
        {
            if (source.IsNull())
                throw new Exception(message);
        }

        /// <summary>
        ///     Throw argument null exception if source is null
        /// </summary>
        /// <param name="source">Source to check</param>
        /// <param name="parameterName">Name of source object</param>
        /// <remarks></remarks>
        public static void ThrowIfArgNull(this object source, string parameterName)
        {
            if (source.IsNull())
                throw new ArgumentNullException(parameterName);
        }

        /// <summary>
        ///     Throw argument exception if source is null
        /// </summary>
        /// <param name="source">Source to check</param>
        /// <param name="message">Error message.</param>
        /// <remarks></remarks>
        public static void ThrowArgIfNull(this object source, string message)
        {
            if (source.IsNull())
                throw new ArgumentException(message);
        }

        /// <summary>
        ///     Throw exception if source is null or empty
        /// </summary>
        /// <param name="source">Source to check</param>
        /// <param name="message">Error message</param>
        /// <remarks></remarks>
        public static void ThrowIfNullOrEmpty(this string source, string message)
        {
            if (source.IsNullOrEmpty())
                throw new Exception(message);
        }

        /// <summary>
        ///     Throw exception if source is null
        /// </summary>
        /// <param name="source">Source to check</param>
        /// <param name="message">Error message</param>
        /// <remarks></remarks>
        public static void ThrowIfNull(this string source, string message)
        {
            if (source.IsNull())
                throw new Exception(message);
        }

        /// <summary>
        ///     Throw argument exception if source is null
        /// </summary>
        /// <param name="source">Source to check</param>
        /// <param name="message">Error message.</param>
        /// <remarks></remarks>
        public static void ThrowArgIfNull(this string source, string message)
        {
            if (source.IsNull())
                throw new ArgumentException(message);
        }

        /// <summary>
        ///     Throw argument exception if source is null or empty
        /// </summary>
        /// <param name="source">Source to check</param>
        /// <param name="message">Error message.</param>
        /// <remarks></remarks>
        public static void ThrowArgIfNullOrEmpty(this string source, string message)
        {
            if (source.IsNullOrEmpty())
                throw new ArgumentException(message);
        }

        /// <summary>
        ///     Throw argument null exception if source is null
        /// </summary>
        /// <param name="source">Source to check</param>
        /// <param name="parameterName">Object name</param>
        /// <remarks></remarks>
        public static void ThrowIfArgNull(this string source, string parameterName)
        {
            if (source.IsNull())
                throw new ArgumentNullException(parameterName);
        }

        /// <summary>
        ///     Throw argument null exception if source is null or empty
        /// </summary>
        /// <param name="source">Source to check</param>
        /// <param name="parameterName">Object name</param>
        /// <remarks></remarks>
        public static void ThrowIfArgNullOrEmpty(this string source, string parameterName)
        {
            if (source.IsNullOrEmpty())
                throw new ArgumentNullException(parameterName);
        }

        /// <summary>
        ///     Throw exception if source is empty
        /// </summary>
        /// <param name="source">Source to check</param>
        /// <param name="message">Error message</param>
        /// <remarks></remarks>
        public static void ThrowIfEmpty(this string source, string message)
        {
            if (source.IsEmpty())
                throw new Exception(message);
        }
    }
}