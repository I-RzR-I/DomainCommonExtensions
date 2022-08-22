﻿// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-08-10 21:39
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-12 23:39
// ***********************************************************************
//  <copyright file="DynamicListExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using DomainCommonExtensions.CommonExtensions;

#endregion

namespace DomainCommonExtensions.ArraysExtensions
{
    /// <summary>
    ///     DynamicList extensions
    /// </summary>
    /// <remarks></remarks>
    public static class DynamicListExtensions
    {
        /// <summary>
        ///     Parse input data (List) to dynamic result (list)
        /// </summary>
        /// <param name="input">List of T input data</param>
        /// <param name="fields">Required fields</param>
        /// <returns></returns>
        /// <typeparam name="T"></typeparam>
        /// <remarks></remarks>
        public static IList<dynamic> ParseListOfTInDynamic<T>(this List<T> input, IEnumerable<string> fields = null)
        {
            var data = input.AsQueryable();
            var requiredFields = typeof(T).GetSelectedFieldFromEntity(fields);

            var result = data.Select("new {" + requiredFields + "}").ToDynamicList();

            return result;
        }

        /// <summary>
        ///     Parse input data (IEnumerable) to dynamic result (list)
        /// </summary>
        /// <param name="input">IEnumerable of T input data</param>
        /// <param name="fields">Required fields</param>
        /// <returns></returns>
        /// <typeparam name="T"></typeparam>
        /// <remarks></remarks>
        public static IList<dynamic> ParseEnumerableOfTInDynamic<T>(this IEnumerable<T> input,
            IEnumerable<string> fields = null)
        {
            var data = input.AsQueryable();
            var requiredFields = typeof(T).GetSelectedFieldFromEntity(fields);

            var result = data.Select("new {" + requiredFields + "}").ToDynamicList();

            return result;
        }

        /// <summary>
        ///     Generate select property
        /// </summary>
        /// <param name="source">Data source IQueryable</param>
        /// <param name="prop">Property name</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static IQueryable SelectProperty(this IQueryable source, string prop)
        {
            var parameter = Expression.Parameter(source.ElementType, "x");
            var property = prop.Split(',')
                .Aggregate((Expression)parameter, Expression.Property);
            var selector = Expression.Lambda(property, parameter);
            var selectCall = Expression.Call(
                typeof(Queryable), "Select", new[] { parameter.Type, property.Type },
                source.Expression, Expression.Quote(selector));

            return source.Provider.CreateQuery(selectCall);
        }
    }
}