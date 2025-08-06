// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2024-02-13 19:42
// 
//  Last Modified By : RzR
//  Last Modified On : 2024-02-13 20:02
// ***********************************************************************
//  <copyright file="TypeParamActionExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using DomainCommonExtensions.Utilities.Ensure;

#endregion

namespace DomainCommonExtensions.CommonExtensions.TypeParam
{
    /// -------------------------------------------------------------------------------------------------
    /// <content>
    ///     T type extensions.
    /// </content>
    /// =================================================================================================
    public static partial class TExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Return data if not null.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <typeparam name="TInput">Input type.</typeparam>
        /// <param name="source">Source data.</param>
        /// <param name="action">Action.</param>
        /// =================================================================================================
        public static void IfNotNull<TInput>(this TInput source, Action<TInput> action)
        {
            DomainEnsure.IsNotNull(action, nameof(action));

            if (source.IsNull())
                return;

            action(source);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Return data if null.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <typeparam name="TInput">Input type.</typeparam>
        /// <param name="source">Source data.</param>
        /// <param name="action">Action.</param>
        /// =================================================================================================
        public static void IfNull<TInput>(this TInput source, Action<TInput> action)
        {
            DomainEnsure.IsNotNull(action, nameof(action));

            if (source.IsNull())
                action(source);
        }
    }
}