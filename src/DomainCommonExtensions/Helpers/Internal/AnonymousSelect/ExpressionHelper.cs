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
using CodeSource;
using DomainCommonExtensions.CommonExtensions;
using DomainCommonExtensions.CommonExtensions.Reflection;
using DomainCommonExtensions.DataTypeExtensions;
using DomainCommonExtensions.Helpers.Internal.AnonymousSelect.Factory;

#endregion

namespace DomainCommonExtensions.Helpers.Internal.AnonymousSelect
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     An internal expression helper.
    /// </summary>
    /// =================================================================================================
    [CodeSource(SourceUrl = "https://stackoverflow.com/questions/3740532/how-to-use-expression-to-build-an-anonymous-type", Version = 1.0D, Comment = "Access the source URL from more info.")]
    [CodeSource(SourceUrl = "https://stackoverflow.com/questions/606104/how-to-create-linq-expression-tree-to-select-an-anonymous-type", Version = 1.0D, Comment = "Access the source URL from more info.")]
    [CodeSource(SourceUrl = "https://stackoverflow.com/questions/57524986/selector-expression-dynamic-on-iqueryable", Version = 1.0D, Comment = "Access the source URL from more info.")]
    internal static class ExpressionHelper
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Parse lambda.
        /// </summary>
        /// <param name="paramExpression">The parameter expression.</param>
        /// <param name="elementType">Type of the element.</param>
        /// <param name="buildLambdaBaseOnSourceType">True to build lambda base on source type.</param>
        /// <param name="props">A variable-length parameters list containing properties.</param>
        /// <returns>
        ///     A LambdaExpression.
        /// </returns>
        /// =================================================================================================
        internal static LambdaExpression ParseLambda(ParameterExpression paramExpression, Type elementType,
            bool buildLambdaBaseOnSourceType, params string[] props)
        {
            var propsX = new List<PropertyInfo>();
            var memberAssignmentList = new List<MemberAssignment>();
            var propExpressionList = new List<Expression>();
            foreach (var prop in props)
            {
                var propInfo = elementType.GetProperty(prop);
                var propExpression = Expression.Property(paramExpression, prop);

                propExpressionList.Add(propExpression);
                propsX.Add(propInfo);
                memberAssignmentList.Add(Expression.Bind(propInfo!, propExpression));
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
                var resultType = AnonymousClassFactory.CreateType(propsX);
                var resultDataType = BuildCtorParamExpression(resultType, propExpressionList, propsX);
                //initExpression = Expression.MemberInit(resultDataType, memberAssignmentList);
                var lambda = Expression.Lambda(resultDataType, paramExpression);

                return lambda;
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Builds constructor parameter expression.
        /// </summary>
        /// <param name="anonymousType">Type of the anonymous.</param>
        /// <param name="expressions">The expressions.</param>
        /// <param name="properties">The properties.</param>
        /// <returns>
        ///     An Expression.
        /// </returns>
        /// =================================================================================================
        private static NewExpression BuildCtorParamExpression(Type anonymousType, IList<Expression> expressions,
            IList<PropertyInfo> properties)
        {
            var propertyInfos = anonymousType.GetProperties().Where(x => x.Name != "Item").ToArray();
            var propertyTypes = propertyInfos.Select(p => p.PropertyType).ToArray();
            var ctor = anonymousType.GetConstructor(propertyTypes);

            if (ctor.IsNull()) return null;

            var constructorParameters = ctor!.GetParameters();
            if (constructorParameters.Length == expressions.Count)
            {
                var expressionsPromoted = new List<Expression>();
                for (var i = 0; i < constructorParameters.Length; i++)
                {
                    var bindParametersSequentially = !properties.All(p => constructorParameters
                        .Any(cp => cp.Name == p.Name && (cp.ParameterType == p.PropertyType ||
                                                         p.PropertyType == Nullable.GetUnderlyingType(cp.ParameterType))));
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

            return null;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Validates the expression.
        /// </summary>
        /// <param name="sourceExpression">Source expression.</param>
        /// <param name="destinationType">Type of the destination.</param>
        /// <returns>
        ///     An Expression.
        /// </returns>
        /// =================================================================================================
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