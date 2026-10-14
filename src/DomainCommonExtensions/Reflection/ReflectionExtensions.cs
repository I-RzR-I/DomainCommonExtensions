// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-08-10 21:39
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-12 23:39
// ***********************************************************************
//  <copyright file="ReflectionExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections.Generic;
using System.Reflection;
using RzR.Extensions.Domain.Primitives;
using RzR.Extensions.Domain.Validation;

#endregion

namespace RzR.Extensions.Domain.Reflection
{
    /// <summary>
    ///     Reflection extensions
    /// </summary>
    /// <remarks></remarks>
    public static class ReflectionExtensions
    {
        /// <summary>
        ///     Copy the properties to a destination object.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <remarks>
        ///     Only public, instance, non-indexer source properties with a public getter are copied to
        ///     destination properties with the same name that have a public setter and an assignable type.
        ///     Any other property is skipped; a hidden (new) property is resolved to its most-derived declaration.
        ///     Use the overload with an allow-list to copy only named properties.
        /// </remarks>
        public static void CopyProperties(this object source, object destination)
        {
            DomainEnsure.IsNotNullAll("Source or/and Destination Objects are null", source, destination);

            CopyPublicProperties(source, destination);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Copy only the properties named in an allow-list to a destination object.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="allowedProperties">
        ///     The exact names of the properties to copy, compared with <see cref="StringComparison.Ordinal"/>.
        ///     An empty list copies nothing; a duplicate name is copied once.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="source"/>, <paramref name="destination"/> or
        ///     <paramref name="allowedProperties"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     Thrown when <paramref name="allowedProperties"/> contains a null, empty or white-space name, or a
        ///     name that is not a property that can be copied from <paramref name="source"/> to
        ///     <paramref name="destination"/>.
        /// </exception>
        /// =================================================================================================
        public static void CopyProperties(this object source, object destination, IEnumerable<string> allowedProperties)
        {
            DomainEnsure.IsNotNullAll("Source or/and Destination Objects are null", source, destination);
            if (allowedProperties.IsNull())
                throw new ArgumentNullException(nameof(allowedProperties));

            var sourceProperties = GetMostDerivedProperties(source.GetType(), BindingFlags.Public | BindingFlags.Instance);
            var targetProperties = GetMostDerivedProperties(destination.GetType(), BindingFlags.Public | BindingFlags.Instance);
            var propertiesToCopy = new List<KeyValuePair<PropertyInfo, PropertyInfo>>();
            var copiedNames = new HashSet<string>(StringComparer.Ordinal);
            foreach (var propertyName in allowedProperties)
            {
                if (string.IsNullOrWhiteSpace(propertyName))
                    throw new ArgumentException("A property name must not be null, empty or white space.",
                        nameof(allowedProperties));
                if (!copiedNames.Add(propertyName))
                    continue;

                if (!sourceProperties.TryGetValue(propertyName, out var sourceProperty)
                    || sourceProperty.GetGetMethod() == null
                    || !targetProperties.TryGetValue(propertyName, out var targetProperty)
                    || targetProperty.GetSetMethod() == null
                    || !targetProperty.PropertyType.IsAssignableFrom(sourceProperty.PropertyType))
                    throw new ArgumentException(
                        $"The property '{propertyName}' cannot be copied from '{source.GetType().Name}' to '{destination.GetType().Name}'.",
                        nameof(allowedProperties));

                propertiesToCopy.Add(new KeyValuePair<PropertyInfo, PropertyInfo>(sourceProperty, targetProperty));
            }

            foreach (var pair in propertiesToCopy)
                pair.Value.SetValue(destination, pair.Key.GetValue(source, null), null);
        }

        /// <summary>
        ///     Copy the public readable source properties to the public writable destination properties
        ///     with the same name and an assignable type, skipping every other property.
        /// </summary>
        /// <param name="source">The source, not null.</param>
        /// <param name="destination">The destination, not null.</param>
        /// <param name="allowRuntimeConversion">
        ///     (Optional)
        ///     When true, a value whose declared type is not assignable is still set when the run-time
        ///     value is compatible (e.g. int to long); an incompatible value is skipped.
        /// </param>
        internal static void CopyPublicProperties(object source, object destination, bool allowRuntimeConversion = false)
        {
            var targetProperties = GetMostDerivedProperties(destination.GetType(), BindingFlags.Public | BindingFlags.Instance);
            foreach (var sourceProperty in GetMostDerivedProperties(source.GetType(), BindingFlags.Public | BindingFlags.Instance).Values)
            {
                if (sourceProperty.GetGetMethod().IsNull())
                    continue;
                if (!targetProperties.TryGetValue(sourceProperty.Name, out var targetProperty))
                    continue;
                if (targetProperty.GetSetMethod().IsNull())
                    continue;

                if (targetProperty.PropertyType.IsAssignableFrom(sourceProperty.PropertyType))
                {
                    targetProperty.SetValue(destination, sourceProperty.GetValue(source, null), null);
                    continue;
                }

                if (!allowRuntimeConversion)
                    continue;

                var value = sourceProperty.GetValue(source, null);
                try
                {
                    targetProperty.SetValue(destination, value, null);
                }
                catch (ArgumentException)
                {
                    // The run-time value is not compatible with the destination property type.
                }
            }
        }

        /// <summary>
        ///     Get the non-indexer properties of a type by name, keeping only the most-derived declaration
        ///     of a hidden (new) property so that the lookup never raises an ambiguous match.
        /// </summary>
        /// <param name="type">The type to inspect.</param>
        /// <param name="bindingFlags">The binding flags used to select the properties.</param>
        /// <returns>The properties keyed by their case-sensitive name.</returns>
        internal static Dictionary<string, PropertyInfo> GetMostDerivedProperties(Type type, BindingFlags bindingFlags)
        {
            var properties = new Dictionary<string, PropertyInfo>(StringComparer.Ordinal);
            foreach (var property in type.GetProperties(bindingFlags))
            {
                if (property.GetIndexParameters().Length != 0)
                    continue;
                if (properties.TryGetValue(property.Name, out var known)
                    && !property.DeclaringType!.IsSubclassOf(known.DeclaringType!))
                    continue;

                properties[property.Name] = property;
            }

            return properties;
        }
    }
}
