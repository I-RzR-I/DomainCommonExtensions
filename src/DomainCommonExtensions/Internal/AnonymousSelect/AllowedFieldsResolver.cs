// ***********************************************************************
//  Assembly          : RzR.Shared.Extensions.DomainCommonExtensions
//  Author            : RzR
//  Created On        : 2026-10-10 20:10
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-10-14 10:25
//  ***********************************************************************
//  <copyright file="AllowedFieldsResolver.cs" company="RzR SOFT & TECH">
//      Copyright (c) RzR. All rights reserved.
//  </copyright>
//  <contact>
//      https://iamrzr.dev/contact
//  </contact>
//  <summary></summary>
//  ***********************************************************************

#region U S I N G

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RzR.Extensions.Domain.Internal.AnonymousSelect.Factory;
using RzR.Extensions.Domain.Primitives;
using RzR.Extensions.Domain.Text;

#endregion

namespace RzR.Extensions.Domain.Internal.AnonymousSelect
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Resolves dynamic field names against a caller-supplied allow-list.
    /// </summary>
    /// =================================================================================================
    internal static class AllowedFieldsResolver
    {
        /// <summary>
        ///     Resolves one field name and ensures that the resolved property is allowed.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="allowedFields" /> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     Thrown when the allow-list contains a null, empty or white-space entry, or the field name
        ///     is not allowed or cannot be resolved.
        /// </exception>
        /// <param name="type">The type that declares the property.</param>
        /// <param name="fieldName">The requested field name.</param>
        /// <param name="allowedFields">The allowed property names.</param>
        /// <param name="paramName">The public parameter name of the requested field.</param>
        /// <param name="allowedFieldsParamName">The public parameter name of the allow-list.</param>
        /// <returns>
        ///     The exact name of the resolved property.
        /// </returns>
        internal static string ResolveAllowedField(Type type, string fieldName, IEnumerable<string> allowedFields,
            string paramName, string allowedFieldsParamName)
        {
            var allowList = CreateAllowList(allowedFields, allowedFieldsParamName);
            var allowedEntry = MapToAllowedEntry(allowList, fieldName, paramName);

            var property = ExpressionHelper.ResolveProperty(type, allowedEntry, paramName);
            EnsureResolvedNameAllowed(allowList, property, fieldName, paramName);

            return property.Name;
        }

        /// <summary>
        ///     Resolves a field list and ensures that every resolved property is allowed.
        /// </summary>
        /// <remarks>
        ///     The requested list is read once and at most
        ///     <see cref="AnonymousClassFactory.MaxFieldCount" /> + 1 names are taken, so the checked
        ///     names
        ///     are the resolved names and an unbounded sequence is still rejected by the field limit.
        ///     Two requested names that map to the same allow-list entry are duplicates.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="allowedFields" /> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     Thrown when the allow-list contains a null, empty or white-space entry, or the field list
        ///     is null or empty, has too many or duplicate fields, or a name is not allowed or cannot be
        ///     resolved.
        /// </exception>
        /// <param name="type">The type that declares the properties.</param>
        /// <param name="fieldNames">The requested field names.</param>
        /// <param name="allowedFields">The allowed property names.</param>
        /// <param name="paramName">The public parameter name of the requested fields.</param>
        /// <param name="allowedFieldsParamName">The public parameter name of the allow-list.</param>
        /// <returns>
        ///     The exact names of the resolved properties, in the requested order.
        /// </returns>
        internal static string[] ResolveAllowedFields(Type type, IEnumerable<string> fieldNames,
            IEnumerable<string> allowedFields, string paramName, string allowedFieldsParamName)
        {
            var allowList = CreateAllowList(allowedFields, allowedFieldsParamName);
            var requestedNames = fieldNames?.Take(AnonymousClassFactory.MaxFieldCount + 1).ToArray();
            var allowedEntries = requestedNames?.Select(fieldName => MapToAllowedEntry(allowList, fieldName, paramName))
                .ToArray();

            var properties = ExpressionHelper.ResolveProperties(type, allowedEntries, paramName);
            var resolvedNames = new string[properties.Count];
            for (var i = 0; i < properties.Count; i++)
            {
                EnsureResolvedNameAllowed(allowList, properties[i], requestedNames![i], paramName);
                resolvedNames[i] = properties[i].Name;
            }

            return resolvedNames;
        }

        /// <summary>
        ///     Reads the allow-list once into an exact (ordinal) set.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="allowedFields" /> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     Thrown when the allow-list contains a null, empty or white-space entry.
        /// </exception>
        /// <param name="allowedFields">The allowed property names.</param>
        /// <param name="allowedFieldsParamName">The public parameter name of the allow-list.</param>
        /// <returns>
        ///     The allowed names; duplicates are kept once.
        /// </returns>
        private static HashSet<string> CreateAllowList(IEnumerable<string> allowedFields, string allowedFieldsParamName)
        {
            if (allowedFields.IsNull())
                throw new ArgumentNullException(allowedFieldsParamName);

            var allowList = new HashSet<string>(StringComparer.Ordinal);
            foreach (var allowedField in allowedFields)
            {
                if (string.IsNullOrWhiteSpace(allowedField))
                    throw new ArgumentException("An allowed field name must not be null, empty or white space.",
                        allowedFieldsParamName);

                allowList.Add(allowedField);
            }

            return allowList;
        }

        /// <summary>
        ///     Maps a requested name to its allow-list entry before the type is inspected, so that the
        ///     caller's spelling never drives reflection.
        /// </summary>
        /// <remarks>
        ///     The entry that matches exactly (ordinal) wins; otherwise the only entry that matches
        ///     ignoring case is used. A null, empty or white-space name is returned unchanged and left
        ///     to the default resolution, which rejects it.
        /// </remarks>
        /// <exception cref="ArgumentException">
        ///     Thrown when no entry matches, even ignoring case, or more than one entry matches ignoring
        ///     case.
        /// </exception>
        /// <param name="allowList">The allowed names.</param>
        /// <param name="fieldName">The requested field name.</param>
        /// <param name="paramName">The public parameter name reported in the exception.</param>
        /// <returns>
        ///     The allow-list entry to resolve.
        /// </returns>
        private static string MapToAllowedEntry(HashSet<string> allowList, string fieldName, string paramName)
        {
            if (fieldName.IsMissing() || allowList.Contains(fieldName))
                return fieldName;

            var matches = allowList
                .Where(allowed => string.Equals(allowed, fieldName, StringComparison.OrdinalIgnoreCase))
                .Take(2)
                .ToArray();

            if (matches.Length != 1)
                throw NotAllowed(fieldName, paramName);

            return matches[0];
        }

        /// <summary>
        ///     Rejects a resolved property whose exact name is not allowed.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Thrown when the resolved property name is not allowed.
        /// </exception>
        /// <param name="allowList">The allowed names.</param>
        /// <param name="property">The resolved property.</param>
        /// <param name="fieldName">The requested field name.</param>
        /// <param name="paramName">The public parameter name reported in the exception.</param>
        private static void EnsureResolvedNameAllowed(HashSet<string> allowList, PropertyInfo property,
            string fieldName, string paramName)
        {
            if (allowList.Contains(property.Name).IsFalse())
                throw NotAllowed(fieldName, paramName);
        }

        /// <summary>
        ///     Creates the exception for a field name that is not allowed.
        /// </summary>
        /// <param name="fieldName">The requested field name.</param>
        /// <param name="paramName">The public parameter name reported in the exception.</param>
        /// <returns>
        ///     The exception to throw.
        /// </returns>
        private static ArgumentException NotAllowed(string fieldName, string paramName)
        {
            return new ArgumentException($"The field '{fieldName}' is not in the allowed field list.", paramName);
        }
    }
}