// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-08-10 21:39
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-12 23:39
// ***********************************************************************
//  <copyright file="NullExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Collections.Generic;

#endregion

namespace DomainCommonExtensions.CommonExtensions
{
    /// <summary>
    ///     Null extensions
    /// </summary>
    /// <remarks></remarks>
    public static class NullExtensions
    {
        /// <summary>
        ///     Is null
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNull(this object obj)
        {
            return obj == null;
        }

        /// <summary>
        ///     Check if KeyValue is null
        /// </summary>
        /// <typeparam name="TK"></typeparam>
        /// <typeparam name="TV"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNull<TK, TV>(this KeyValuePair<TK, TV> source)
        {
            return source.Equals(default(KeyValuePair<TK, TV>));
        }
    }
}