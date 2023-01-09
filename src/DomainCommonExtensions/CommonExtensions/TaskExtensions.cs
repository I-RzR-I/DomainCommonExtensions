// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-08-10 21:39
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-12 23:39
// ***********************************************************************
//  <copyright file="TaskExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Threading.Tasks;

#endregion

namespace DomainCommonExtensions.CommonExtensions
{
    /// <summary>
    ///     Task extensions
    /// </summary>
    /// <remarks></remarks>
    public static class TaskExtensions
    {
#if NET45_OR_GREATER || NET || NETSTANDARD1_0_OR_GREATER
        /// <summary>
        ///     Execute async
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        /// <returns></returns>
        public static T ExecuteAsync<T>(this Task<T> task)
        {
            return task.GetAwaiter().GetResult();
        }

        /// <summary>
        ///     Execute
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static void ExecuteAsync(this Task task)
        {
            task.GetAwaiter().GetResult();
        }
#endif
    }
}