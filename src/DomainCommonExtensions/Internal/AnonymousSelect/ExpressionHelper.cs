// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2025-01-08 13:30
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-01-08 17:55
// ***********************************************************************
//  <copyright file="ExpressionHelper.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using RzR.Core.CodeSource;
using RzR.Extensions.Domain.Primitives;
using RzR.Extensions.Domain.Reflection;
using RzR.Extensions.Domain.Internal.AnonymousSelect.Factory;

#endregion

namespace RzR.Extensions.Domain.Internal.AnonymousSelect
{
    /// <summary>
    ///     An internal expression helper.
    /// </summary>
    [CodeSource(SourceUrl = "https://stackoverflow.com/questions/3740532/how-to-use-expression-to-build-an-anonymous-type", Version = "1.0", Comment = "Access the source URL from more info.")]
    [CodeSource(SourceUrl = "https://stackoverflow.com/questions/606104/how-to-create-linq-expression-tree-to-select-an-anonymous-type", Version = "1.0", Comment = "Access the source URL from more info.")]
    [CodeSource(SourceUrl = "https://stackoverflow.com/questions/57524986/selector-expression-dynamic-on-iqueryable", Version = "1.0", Comment = "Access the source URL from more info.")]
    internal static class ExpressionHelper
    {
        /// <summary>
        ///     Parse lambda.
        /// </summary>
        /// <param name="paramExpression">The parameter expression.</param>
        /// <param name="elementType">Type of the element.</param>
        /// <param name="buildLambdaBaseOnSourceType">True to build lambda base on source type.</param>
        /// <param name="properties">
        ///     The properties to select, already resolved by <see cref="ResolveProperties"/> or
        ///     <see cref="GetAllSelectableProperties"/>.
        /// </param>
        /// <returns>
        ///     A LambdaExpression.
        /// </returns>
        internal static LambdaExpression ParseLambda(ParameterExpression paramExpression, Type elementType,
            bool buildLambdaBaseOnSourceType, IList<PropertyInfo> properties)
        {
            var memberAssignmentList = new List<MemberAssignment>();
            var propExpressionList = new List<Expression>();
            foreach (var propInfo in properties)
            {
                var propExpression = Expression.Property(paramExpression, propInfo);

                propExpressionList.Add(propExpression);
                memberAssignmentList.Add(Expression.Bind(propInfo, propExpression));
            }

            if (buildLambdaBaseOnSourceType.IsTrue())
            {
                var resultDataType = Expression.New(elementType);
                var initExpression = Expression.MemberInit(resultDataType, memberAssignmentList);
                var lambda = Expression.Lambda(initExpression, paramExpression);

                return lambda;
            }
            else
            {
                var resultType = AnonymousClassFactory.CreateType(properties);
                var resultDataType = BuildCtorParamExpression(resultType, propExpressionList, properties);
                //initExpression = Expression.MemberInit(resultDataType, memberAssignmentList);
                var lambda = Expression.Lambda(resultDataType, paramExpression);

                return lambda;
            }
        }

        /// <summary>
        ///     Builds constructor parameter expression.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when the generated type has no constructor that matches its properties and the
        ///     selected expressions.
        /// </exception>
        /// <param name="anonymousType">Type of the anonymous.</param>
        /// <param name="expressions">The expressions.</param>
        /// <param name="properties">The properties.</param>
        /// <returns>
        ///     An Expression.
        /// </returns>
        private static NewExpression BuildCtorParamExpression(Type anonymousType, IList<Expression> expressions,
            IList<PropertyInfo> properties)
        {
            // Indexers are excluded by signature, not by name, so a selected property named "Item" is kept.
            var propertyInfos = anonymousType.GetProperties().Where(x => x.GetIndexParameters().Length == 0).ToArray();
            var propertyTypes = propertyInfos.Select(p => p.PropertyType).ToArray();
            var ctor = anonymousType.GetConstructor(propertyTypes);

            if (ctor.IsNull())
                throw new InvalidOperationException(
                    $"The generated type '{anonymousType.Name}' has no constructor that matches its properties.");

            var constructorParameters = ctor!.GetParameters();
            if (constructorParameters.Length != expressions.Count)
                throw new InvalidOperationException(
                    $"The generated type '{anonymousType.Name}' expects {constructorParameters.Length} constructor arguments, but {expressions.Count} were selected.");

            var bindParametersSequentially = !properties.All(p => constructorParameters
                .Any(cp => cp.Name == p.Name && (cp.ParameterType == p.PropertyType ||
                                                 p.PropertyType == Nullable.GetUnderlyingType(cp.ParameterType))));
            var expressionsPromoted = new List<Expression>();
            for (var i = 0; i < constructorParameters.Length; i++)
            {
                if (bindParametersSequentially)
                {
                    expressionsPromoted.Add(ValidateExpression(expressions[i], propertyTypes[i]));
                }
                else
                {
                    var propertyType = constructorParameters[i].ParameterType;
                    var cParameterName = constructorParameters[i].Name;
                    var propertyAndIndex = properties.Select((p, index) => new { p, index })
                        .First(p => p.p.Name == cParameterName && (p.p.PropertyType == propertyType ||
                                                                   p.p.PropertyType == Nullable.GetUnderlyingType(propertyType)));

                    expressionsPromoted.Add(ValidateExpression(expressions[propertyAndIndex.index], propertyType));
                }
            }

            return Expression.New(ctor, expressionsPromoted, (IEnumerable<MemberInfo>)propertyInfos);
        }

        /// <summary>
        ///     Resolves one dynamic field name to a public instance property of <paramref name="type"/>.
        /// </summary>
        /// <remarks>
        ///     Only public, readable, non-indexed instance properties are reachable; non-public members
        ///     and fields never are. An exact (ordinal) name match wins; otherwise a single case-
        ///     insensitive match is used.
        /// </remarks>
        /// <exception cref="ArgumentException">
        ///     Thrown when the name is null, empty, white space, unknown or ambiguous.
        /// </exception>
        /// <param name="type">The type that declares the property.</param>
        /// <param name="fieldName">The field name to resolve.</param>
        /// <param name="paramName">The public parameter name reported in the exception.</param>
        /// <returns>
        ///     The resolved property.
        /// </returns>
        internal static PropertyInfo ResolveProperty(Type type, string fieldName, string paramName)
            => ResolveProperty(GetSelectableProperties(type), type, fieldName, paramName);

        /// <summary>
        ///     Resolves a dynamic field list to public instance properties of <paramref name="type"/>.
        /// </summary>
        /// <remarks>
        ///     The list is limited to <see cref="AnonymousClassFactory.MaxFieldCount"/> fields; the
        ///     limit is checked before any expression or type is built.
        /// </remarks>
        /// <exception cref="ArgumentException">
        ///     Thrown when the list is null or empty, has too many or duplicate fields, or a name cannot
        ///     be resolved.
        /// </exception>
        /// <param name="type">The type that declares the properties.</param>
        /// <param name="fieldNames">The field names to resolve.</param>
        /// <param name="paramName">The public parameter name reported in the exception.</param>
        /// <returns>
        ///     The resolved properties, in the requested order.
        /// </returns>
        internal static IList<PropertyInfo> ResolveProperties(Type type, IEnumerable<string> fieldNames,
            string paramName)
        {
            if (fieldNames.IsNull())
                throw new ArgumentException("The field list must not be null.", paramName);

            var candidates = GetSelectableProperties(type);
            var properties = new List<PropertyInfo>();
            var resolvedNames = new HashSet<string>(StringComparer.Ordinal);
            foreach (var fieldName in fieldNames!)
            {
                EnsureFieldCount(properties.Count + 1, paramName);

                var property = ResolveProperty(candidates, type, fieldName, paramName);
                if (!resolvedNames.Add(property.Name))
                    throw new ArgumentException($"The field '{fieldName}' is selected more than once.", paramName);

                properties.Add(property);
            }

            if (properties.Count == 0)
                throw new ArgumentException("The field list must contain at least one field name.", paramName);

            return properties;
        }

        /// <summary>
        ///     Gets every public instance property of <paramref name="type"/> that a dynamic select can
        ///     read.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Thrown when the type has no selectable property or more than
        ///     <see cref="AnonymousClassFactory.MaxFieldCount"/> of them.
        /// </exception>
        /// <param name="type">The type that declares the properties.</param>
        /// <param name="paramName">The public parameter name reported in the exception.</param>
        /// <returns>
        ///     The selectable properties.
        /// </returns>
        internal static IList<PropertyInfo> GetAllSelectableProperties(Type type, string paramName)
        {
            var properties = GetSelectableProperties(type);
            if (properties.Length == 0)
                throw new ArgumentException($"The type '{type.Name}' has no public instance property to select.",
                    paramName);

            EnsureFieldCount(properties.Length, paramName);

            return properties;
        }

        /// <summary>
        ///     Gets the public, readable, non-indexed instance properties of a type.
        /// </summary>
        /// <remarks>
        ///     A property hidden by a "new" declaration in a derived type is dropped, so each name
        ///     appears once (the most-derived declaration), in reflection order. Properties whose type
        ///     cannot be a field of a generated type (by-ref, pointer, byref-like) are not selectable.
        /// 
        /// </remarks>
        /// <param name="type">The type to inspect.</param>
        /// <returns>
        ///     The selectable properties.
        /// </returns>
        private static PropertyInfo[] GetSelectableProperties(Type type)
        {
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.GetIndexParameters().Length == 0)
                .ToArray();

            return properties
                .Where(p => !properties.Any(other => other.Name == p.Name
                                                     && other.DeclaringType != p.DeclaringType
                                                     && other.DeclaringType!.IsSubclassOf(p.DeclaringType!)))
                .Where(p => p.GetGetMethod().IsNotNull()
                            && AnonymousClassFactory.IsSupportedFieldType(p.PropertyType))
                .ToArray();
        }

        /// <summary>
        ///     Resolves one field name against the selectable properties of a type.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Thrown when one or more arguments have unsupported or illegal values.
        /// </exception>
        /// <param name="candidates">The selectable properties.</param>
        /// <param name="type">The type that declares the properties.</param>
        /// <param name="fieldName">The field name to resolve.</param>
        /// <param name="paramName">The public parameter name reported in the exception.</param>
        /// <returns>
        ///     The resolved property.
        /// </returns>
        private static PropertyInfo ResolveProperty(PropertyInfo[] candidates, Type type, string fieldName,
            string paramName)
        {
            if (string.IsNullOrWhiteSpace(fieldName))
                throw new ArgumentException("A field name must not be null, empty or white space.", paramName);

            var exactMatch = candidates.FirstOrDefault(p => string.Equals(p.Name, fieldName, StringComparison.Ordinal));
            if (exactMatch.IsNotNull())
                return exactMatch;

            var matches = candidates
                .Where(p => string.Equals(p.Name, fieldName, StringComparison.OrdinalIgnoreCase))
                .ToArray();
            if (matches.Select(p => p.Name).Distinct(StringComparer.Ordinal).Count() > 1)
                throw new ArgumentException($"The field '{fieldName}' matches more than one property of '{type.Name}'.",
                    paramName);

            if (matches.Length == 0)
                throw new ArgumentException($"The field '{fieldName}' is not a public instance property of '{type.Name}'.",
                    paramName);

            return matches[0];
        }

        /// <summary>
        ///     Ensures that a field count does not exceed <see cref="AnonymousClassFactory.MaxFieldCount"/>
        ///     .
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Thrown when one or more arguments have unsupported or illegal values.
        /// </exception>
        /// <param name="fieldCount">The field count.</param>
        /// <param name="paramName">The public parameter name reported in the exception.</param>
        private static void EnsureFieldCount(int fieldCount, string paramName)
        {
            if (fieldCount > AnonymousClassFactory.MaxFieldCount)
                throw new ArgumentException(
                    $"A dynamic select supports at most {AnonymousClassFactory.MaxFieldCount} fields.", paramName);
        }

        /// <summary>
        ///     Validates the expression.
        /// </summary>
        /// <param name="sourceExpression">Source expression.</param>
        /// <param name="destinationType">Type of the destination.</param>
        /// <returns>
        ///     An Expression.
        /// </returns>
        private static Expression ValidateExpression(Expression sourceExpression, Type destinationType)
        {
            Type returnType;
            if (sourceExpression is LambdaExpression lambdaExpression)
            {
#if !NET35
                returnType = lambdaExpression.ReturnType;
#else
                returnType = lambdaExpression.Body.Type;
#endif
            }
            else
            {
                returnType = sourceExpression.Type;
            }

            if (returnType == destinationType || destinationType.IsGenericParameter) return sourceExpression;

            if ((destinationType == typeof(decimal) || destinationType == typeof(int)) &&
                sourceExpression.Type.IsEnumType())
                return Expression.Convert(
                    Expression.Convert(sourceExpression, Enum.GetUnderlyingType(sourceExpression.Type)),
                    destinationType);

            return sourceExpression;
        }
    }
}
