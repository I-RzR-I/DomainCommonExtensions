// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-12-08 22:55
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-12-08 22:57
// ***********************************************************************
//  <copyright file="HashExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Collections.Generic;

#endregion

namespace DomainCommonExtensions.ArraysExtensions
{
    /// <summary>
    ///     Hash extensions
    /// </summary>
    public static class HashExtensions
    {
        /// <summary>
        ///     Add range for hash list
        /// </summary>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="context"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static HashSet<TTarget> AddRange<TTarget>(this HashSet<TTarget> context, IEnumerable<TTarget> data)
        {
            context ??= new HashSet<TTarget>();
            foreach (var item in data) context.Add(item);

            return context;
        }
    }
}