#if NET || NETSTANDARD2_0_OR_GREATER
// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2025-06-27 19:03
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-06-27 19:03
// ***********************************************************************
//  <copyright file="RandomHelper.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************
// 
#region U S A G E S

using System.Text.Json;
using System.Text.Json.Serialization;

#endregion

namespace DomainCommonExtensions.Helpers.Internal
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     An object for persisting JSON object data.
    /// </summary>
    /// =================================================================================================
    internal static class JsonObjectSerializer
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) options for controlling the operation.
        /// </summary>
        /// =================================================================================================
        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            //DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Convert this object into a string representation.
        /// </summary>
        /// <param name="o">An object to process.</param>
        /// <returns>
        ///     A string that represents this object.
        /// </returns>
        /// =================================================================================================
        internal static string ToString(object o) => JsonSerializer.Serialize(o, Options);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Creates a new object from the given string.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     A T.
        /// </returns>
        /// =================================================================================================
        internal static T FromString<T>(string value) where T : class => JsonSerializer.Deserialize<T>(value, Options);
    }
}
#endif