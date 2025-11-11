// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2025-11-05 16:11
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-11-05 16:10
// ***********************************************************************
//  <copyright file="TaskRunnerHelper.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace DomainCommonExtensions.Helpers
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A task runner helper.
    /// </summary>
    /// =================================================================================================
    public static class TaskRunnerHelper
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Runs the given delegate function in a new task. Adapted to work on net40.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="delegateFunc">The delegate function.</param>
        /// <param name="cancellationToken">
        ///     (Optional) A token that allows processing to be cancelled.
        /// </param>
        /// <returns>
        ///     A Task.
        /// </returns>
        /// =================================================================================================
        public static Task<T> Run<T>(Func<T> delegateFunc, CancellationToken cancellationToken = default)
        {
#if NET40
            var task = new Task<T>(delegateFunc, cancellationToken);
            task.Start();

            return task;
#else
            return Task.Run(delegateFunc, cancellationToken);
#endif
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Runs the given delegate action in a new task. Adapted to work on net40.
        /// </summary>
        /// <param name="delegateAction">The delegate action.</param>
        /// <param name="cancellationToken">
        ///     (Optional) A token that allows processing to be cancelled.
        /// </param>
        /// <returns>
        ///     A Task.
        /// </returns>
        /// =================================================================================================
        public static Task Run(Action delegateAction, CancellationToken cancellationToken = default)
        {
#if NET40
            var task = new Task(delegateAction, cancellationToken);
            task.Start();

            return task;
#else
            return Task.Run(delegateAction, cancellationToken);
#endif
        }
    }
}