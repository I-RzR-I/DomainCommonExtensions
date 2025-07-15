// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2025-07-15 18:33
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-07-15 18:33
// ***********************************************************************
//  <copyright file="WithIndexModel.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

namespace DomainCommonExtensions.Models
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A data Model for the with index enumerable extension method.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// =================================================================================================
    public struct WithIndexModel<T>
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the zero-based index of this object.
        /// </summary>
        /// <value>
        ///     The index.
        /// </value>
        /// =================================================================================================
        public int Index { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the item.
        /// </summary>
        /// <value>
        ///     The item.
        /// </value>
        /// =================================================================================================
        public T Item { get; set; }
    }
}