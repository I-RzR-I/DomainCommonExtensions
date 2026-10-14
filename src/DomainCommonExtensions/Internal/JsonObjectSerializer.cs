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

#region U S A G E S

using System;
using System.IO;
using System.Text.Json;

#endregion

namespace RzR.Extensions.Domain.Internal
{
    /// <summary>
    ///     An object for persisting JSON object data.
    /// </summary>
    internal static class JsonObjectSerializer
    {
        /// <summary>
        ///     (Immutable) options for controlling the operation.
        /// </summary>
        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            //DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        /// <summary>
        ///     Convert this object into a string representation.
        /// </summary>
        /// <param name="o">An object to process.</param>
        /// <returns>
        ///     A string that represents this object.
        /// </returns>
        internal static string ToString(object o) => JsonSerializer.Serialize(o, Options);

        /// <summary>
        ///     Creates a new object from the given string.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     A T.
        /// </returns>
        internal static T FromString<T>(string value) where T : class => JsonSerializer.Deserialize<T>(value, Options);

        /// <summary>
        ///     Gets the comparer the serializer uses to detect colliding property names.
        /// </summary>
        /// <value>
        ///     The property name comparer.
        /// </value>
        internal static StringComparer PropertyNameComparer
            => Options.PropertyNameCaseInsensitive ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;

        /// <summary>
        ///     Converts a member name with the property naming policy.
        /// </summary>
        /// <param name="name">The member name.</param>
        /// <returns>
        ///     The JSON property name.
        /// </returns>
        internal static string ConvertName(string name) 
            => Options.PropertyNamingPolicy.ConvertName(name);

        /// <summary>
        ///     Encodes a JSON property name with the serializer encoder.
        /// </summary>
        /// <param name="name">The JSON property name.</param>
        /// <returns>
        ///     The encoded name.
        /// </returns>
        internal static JsonEncodedText EncodeName(string name) 
            => JsonEncodedText.Encode(name, Options.Encoder);

        /// <summary>
        ///     Creates a writer with the same encoder and indentation as <see cref="ToString(object)"/>.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <returns>
        ///     The writer.
        /// </returns>
        internal static Utf8JsonWriter CreateWriter(Stream stream)
            => new Utf8JsonWriter(stream,
                new JsonWriterOptions { Encoder = Options.Encoder, Indented = Options.WriteIndented });

        /// <summary>
        ///     Writes a value to a writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="value">The value, or null.</param>
        /// <param name="type">The declared type of the value.</param>
        internal static void Serialize(Utf8JsonWriter writer, object value, Type type)
            => JsonSerializer.Serialize(writer, value, type, Options);
    }
}
#endif
