// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2025-07-11 12:38
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-07-11 13:01
// ***********************************************************************
//  <copyright file="FuncExtensions.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Threading.Tasks;
using DomainCommonExtensions.DataTypeExtensions;
using DomainCommonExtensions.Utilities.Ensure;

#endregion

namespace DomainCommonExtensions.CommonExtensions
{
    /// <summary>
    ///     A function extensions.
    /// </summary>
    public static class FuncExtensions
    {
        /// <summary>
        ///     A Func&lt;bool&gt; extension method that query if 'func' result is true.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="func">The func to act on.</param>
        /// <returns>
        ///     True if true, false if not.
        /// </returns>
        public static bool IsTrue(this Func<bool> func)
        {
            if (func.IsNull())
                throw new ArgumentNullException(nameof(func));

            return func.Invoke().IsTrue();
        }

        /// <summary>
        ///     A Func&lt;bool&gt; extension method that query if 'func' result is false.
        /// </summary>
        /// <param name="func">The func to act on.</param>
        /// <returns>
        ///     True if false, false if not.
        /// </returns>
        public static bool IsFalse(this Func<bool> func)
        {
            return func.IsTrue().IsFalse();
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A Func&lt;bool&gt; extension method that query if 'func' result is true.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="func">The func to act on.</param>
        /// <returns>
        ///     True if true, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsTrue(this Func<Task<bool>> func)
        {
            DomainEnsure.IsNotNull(func, nameof(func));

            return func.Invoke().Result.IsTrue();
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A Func&lt;bool&gt; extension method that query if 'func' result is false.
        /// </summary>
        /// <param name="func">The func to act on.</param>
        /// <returns>
        ///     True if false, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsFalse(this Func<Task<bool>> func)
        {
            return func.IsTrue().IsFalse();
        }
    }
}