// ***********************************************************************
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using RzR.Extensions.Domain.Primitives;
using RzR.Extensions.Domain.Internal.AnonymousSelect;

#region OLD Using System.Linq.Dynamic.Core
//using System.Linq.Dynamic.Core;
//using System.Linq.Dynamic.Core.Parser;
#endregion

#endregion

namespace RzR.Extensions.Domain.Collections
{
    /// <summary>
    ///     DynamicList extensions
    /// </summary>
    /// <remarks></remarks>
    public static class DynamicListExtensions
    {
        #region OLD Using System.Linq.Dynamic.Core

        /*
        //  Using System.Linq.Dynamic.Core
        // // <summary>
        // //     Parse input data (List) to dynamic result (list)
        // // </summary>
        // // <param name="input">List of T input data</param>
        // // <param name="fields">Required fields</param>
        // // <returns></returns>
        // // <typeparam name="T"></typeparam>
        // // <remarks></remarks>
        // public static IList<dynamic> ParseListOfTInDynamic<T>(this List<T> input, IEnumerable<string> fields = null)
        // {
        //    var data = input.AsQueryable();
        //    var requiredFields = typeof(T).GetSelectedFieldFromEntity(fields);
        //
        //    var result = data.Select("new {" + requiredFields + "}").ToDynamicList();
        //
        //    return result;
        // }
        
        */

        /*
        //  Using System.Linq.Dynamic.Core
        // /// <summary>
        // ///     Parse input data (IEnumerable) to dynamic result (list)
        // /// </summary>
        // /// <param name="input">IEnumerable of T input data</param>
        // /// <param name="fields">Required fields</param>
        // /// <returns></returns>
        // /// <typeparam name="T"></typeparam>
        // /// <remarks></remarks>
        // public static IList<dynamic> ParseEnumerableOfTInDynamic<T>(this IEnumerable<T> input,
        //     IEnumerable<string> fields = null)
        // {
        //     var data = input.AsQueryable();
        //     var requiredFields = typeof(T).GetSelectedFieldFromEntity(fields);

        //     var result = data.Select("new {" + requiredFields + "}").ToDynamicList();

        //     return result;
        // }

        */

        #endregion

        /// <summary>
        ///     Parse input data (IEnumerable) to dynamic result (list)
        /// </summary>
        /// <param name="input">List of T input data</param>
        /// <param name="fields">
        ///     Required fields: public instance property names, matched exactly first and then ignoring case.
        ///     Null (the default) selects every public instance property.
        /// </param>
        /// <returns></returns>
        /// <typeparam name="T"></typeparam>
        /// <remarks>At most 256 fields can be selected. Use the overload with an allow-list for caller-supplied field names.</remarks>
        /// <exception cref="ArgumentException">
        ///     Thrown when <paramref name="fields"/> is empty, contains a null, empty, white-space, unknown or
        ///     duplicate name, or has more than 256 fields.
        /// </exception>
        public static IList<dynamic> ParseListOfTInDynamic<T>(this List<T> input, IEnumerable<string> fields = null)
        {
            var data = input.AsQueryable();
            var properties = fields.IsNull()
                ? ExpressionHelper.GetAllSelectableProperties(data.ElementType, nameof(fields))
                : ExpressionHelper.ResolveProperties(data.ElementType, fields, nameof(fields));
            var parameter = Expression.Parameter(data.ElementType, "x");
            var lambdaExpression = ExpressionHelper.ParseLambda(parameter, data.ElementType, false, properties);

            var selectCall = Expression.Call(
                typeof(Queryable), 
                "Select", 
                new[] { parameter.Type, lambdaExpression.Body.Type/*source.ElementType*/ },
                data.Expression, 
                Expression.Quote(lambdaExpression));

            var result = data.Provider.CreateQuery(selectCall);

            return result.Cast<dynamic>().ToList();
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Parse input data (List) to dynamic result (list), selecting only fields that are in an allow-list.
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="input">List of T input data</param>
        /// <param name="fields">
        ///     Required fields: public instance property names, matched exactly first and then ignoring case.
        /// </param>
        /// <param name="allowedFields">
        ///     The property names that may be selected. Each resolved property name must be in the list with an
        ///     exact (ordinal) match.
        /// </param>
        /// <returns>
        ///     The selected fields of each element.
        /// </returns>
        /// <remarks>
        ///     At most 256 fields can be selected. Every field is checked against the allow-list before any
        ///     expression or type is built: a name that matches no allowed name, even ignoring case, is rejected
        ///     without inspecting <typeparamref name="T"/>; a name that resolves to a property whose exact name is
        ///     not allowed is also rejected.
        ///     The allow-list spelling is canonical: a requested name is mapped to the allow-list entry that
        ///     matches exactly, otherwise to the only entry that matches ignoring case (several such entries
        ///     reject it), and only that entry is resolved. With the allow-list {"Name"}, "NAME" and "nAmE"
        ///     resolve via "Name" to the Name property, never to a NAME sibling.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="input"/> or <paramref name="allowedFields"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     Thrown when <paramref name="allowedFields"/> contains a null, empty or white-space entry, or
        ///     <paramref name="fields"/> is null or empty, contains a null, empty, white-space, unknown, duplicate
        ///     or not allowed name, or has more than 256 fields.
        /// </exception>
        /// =================================================================================================
        public static IList<dynamic> ParseListOfTInDynamic<T>(this List<T> input, IEnumerable<string> fields,
            IEnumerable<string> allowedFields)
        {
            if (input.IsNull())
                throw new ArgumentNullException(nameof(input));

            var propertyNames = AllowedFieldsResolver.ResolveAllowedFields(typeof(T), fields, allowedFields,
                nameof(fields), nameof(allowedFields));

            return input.ParseListOfTInDynamic(propertyNames);
        }

        /// <summary>
        ///     Parse input data (List) to dynamic result (list)
        /// </summary>
        /// <param name="input">IEnumerable of T input data</param>
        /// <param name="fields">
        ///     Required fields: public instance property names, matched exactly first and then ignoring case.
        ///     Null (the default) selects every public instance property.
        /// </param>
        /// <returns></returns>
        /// <typeparam name="T"></typeparam>
        /// <remarks>At most 256 fields can be selected. Use the overload with an allow-list for caller-supplied field names.</remarks>
        /// <exception cref="ArgumentException">
        ///     Thrown when <paramref name="fields"/> is empty, contains a null, empty, white-space, unknown or
        ///     duplicate name, or has more than 256 fields.
        /// </exception>
        public static IList<dynamic> ParseEnumerableOfTInDynamic<T>(this IEnumerable<T> input, IEnumerable<string> fields = null)
        {
            var data = input.AsQueryable();
            var properties = fields.IsNull()
                ? ExpressionHelper.GetAllSelectableProperties(data.ElementType, nameof(fields))
                : ExpressionHelper.ResolveProperties(data.ElementType, fields, nameof(fields));
            var parameter = Expression.Parameter(data.ElementType, "x");
            var lambdaExpression = ExpressionHelper.ParseLambda(parameter, data.ElementType, false, properties);

            var selectCall = Expression.Call(
                typeof(Queryable), 
                "Select", 
                new[] { parameter.Type, lambdaExpression.Body.Type/*source.ElementType*/ },
                data.Expression,
                Expression.Quote(lambdaExpression));

            var result = data.Provider.CreateQuery(selectCall);

            return result.Cast<dynamic>().ToList();
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Parse input data (IEnumerable) to dynamic result (list), selecting only fields that are in an
        ///     allow-list.
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="input">IEnumerable of T input data</param>
        /// <param name="fields">
        ///     Required fields: public instance property names, matched exactly first and then ignoring case.
        /// </param>
        /// <param name="allowedFields">
        ///     The property names that may be selected. Each resolved property name must be in the list with an
        ///     exact (ordinal) match.
        /// </param>
        /// <returns>
        ///     The selected fields of each element.
        /// </returns>
        /// <remarks>
        ///     At most 256 fields can be selected. Every field is checked against the allow-list before any
        ///     expression or type is built: a name that matches no allowed name, even ignoring case, is rejected
        ///     without inspecting <typeparamref name="T"/>; a name that resolves to a property whose exact name is
        ///     not allowed is also rejected.
        ///     The allow-list spelling is canonical: a requested name is mapped to the allow-list entry that
        ///     matches exactly, otherwise to the only entry that matches ignoring case (several such entries
        ///     reject it), and only that entry is resolved. With the allow-list {"Name"}, "NAME" and "nAmE"
        ///     resolve via "Name" to the Name property, never to a NAME sibling.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="input"/> or <paramref name="allowedFields"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     Thrown when <paramref name="allowedFields"/> contains a null, empty or white-space entry, or
        ///     <paramref name="fields"/> is null or empty, contains a null, empty, white-space, unknown, duplicate
        ///     or not allowed name, or has more than 256 fields.
        /// </exception>
        /// =================================================================================================
        public static IList<dynamic> ParseEnumerableOfTInDynamic<T>(this IEnumerable<T> input,
            IEnumerable<string> fields, IEnumerable<string> allowedFields)
        {
            if (input.IsNull())
                throw new ArgumentNullException(nameof(input));

            var propertyNames = AllowedFieldsResolver.ResolveAllowedFields(input.AsQueryable().ElementType, fields,
                allowedFields, nameof(fields), nameof(allowedFields));

            return input.ParseEnumerableOfTInDynamic(propertyNames);
        }

        /// <summary>
        ///     Generate select property
        /// </summary>
        /// <param name="source">Data source IQueryable</param>
        /// <param name="prop">Property name: a public instance property, matched exactly first and then ignoring case</param>
        /// <returns></returns>
        /// <remarks>Use the overload with an allow-list for caller-supplied field names.</remarks>
        /// <exception cref="ArgumentException">
        ///     Thrown when <paramref name="prop"/> is null, empty, white space or not a public instance property.
        /// </exception>
        public static IQueryable SelectProperty(this IQueryable source, string prop)
        {
            var parameter = Expression.Parameter(source.ElementType, "x");
            var property = ExpressionHelper.ResolveProperty(source.ElementType, prop, nameof(prop));
            var propEx = Expression.Property(parameter, property);
            var selector = Expression.Lambda(propEx, parameter);
            var selectCall = Expression.Call(
                typeof(Queryable), 
                "Select", 
                new[] { parameter.Type, propEx.Type },
                source.Expression, 
                Expression.Quote(selector));

            return source.Provider.CreateQuery(selectCall);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Generate select property for a property that must be in an allow-list.
        /// </summary>
        /// <param name="source">Data source IQueryable</param>
        /// <param name="prop">Property name: a public instance property, matched exactly first and then ignoring case</param>
        /// <param name="allowedFields">
        ///     The property names that may be selected. The resolved property name must be in the list with an
        ///     exact (ordinal) match.
        /// </param>
        /// <returns>
        ///     The query that selects the property.
        /// </returns>
        /// <remarks>
        ///     The property name is checked against the allow-list before the query is built: a name that matches
        ///     no allowed name, even ignoring case, is rejected without inspecting the element type; a name that
        ///     resolves to a property whose exact name is not allowed is also rejected.
        ///     The allow-list spelling is canonical: a requested name is mapped to the allow-list entry that
        ///     matches exactly, otherwise to the only entry that matches ignoring case (several such entries
        ///     reject it), and only that entry is resolved. With the allow-list {"Name"}, "NAME" and "nAmE"
        ///     resolve via "Name" to the Name property, never to a NAME sibling.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="source"/> or <paramref name="allowedFields"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     Thrown when <paramref name="allowedFields"/> contains a null, empty or white-space entry, or
        ///     <paramref name="prop"/> is null, empty, white space, not allowed or not a public instance property.
        /// </exception>
        /// =================================================================================================
        public static IQueryable SelectProperty(this IQueryable source, string prop, IEnumerable<string> allowedFields)
        {
            if (source.IsNull())
                throw new ArgumentNullException(nameof(source));

            var propertyName = AllowedFieldsResolver.ResolveAllowedField(source.ElementType, prop, allowedFields,
                nameof(prop), nameof(allowedFields));

            return source.SelectProperty(propertyName);
        }

        /// <summary>
        ///     Generate select multiple properties
        /// </summary>
        /// <param name="source">Data source IQueryable</param>
        /// <param name="props">
        ///     Properties name: public instance properties, matched exactly first and then ignoring case
        /// </param>
        /// <returns></returns>
        /// <remarks>At most 256 properties can be selected. Use the overload with an allow-list for caller-supplied field names.</remarks>
        /// <exception cref="ArgumentException">
        ///     Thrown when <paramref name="props"/> is null or empty, contains a null, empty, white-space, unknown
        ///     or duplicate name, or has more than 256 names.
        /// </exception>
        public static IQueryable SelectMultipleProperties(this IQueryable source, params string[] props)
        {
            var parameter = Expression.Parameter(source.ElementType, "x");
            var properties = ExpressionHelper.ResolveProperties(source.ElementType, props, nameof(props));
            var lambdaExpression = ExpressionHelper.ParseLambda(parameter, source.ElementType, true, properties);

            var selectCall = Expression.Call(
                typeof(Queryable), 
                "Select", 
                new[] { parameter.Type, lambdaExpression.Body.Type/*source.ElementType*/ },
                source.Expression, 
                Expression.Quote(lambdaExpression));

            return source.Provider.CreateQuery(selectCall);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Generate select multiple properties that must be in an allow-list.
        /// </summary>
        /// <param name="source">Data source IQueryable</param>
        /// <param name="props">
        ///     Properties name: public instance properties, matched exactly first and then ignoring case
        /// </param>
        /// <param name="allowedFields">
        ///     The property names that may be selected. Each resolved property name must be in the list with an
        ///     exact (ordinal) match.
        /// </param>
        /// <returns>
        ///     The query that selects the properties.
        /// </returns>
        /// <remarks>
        ///     At most 256 properties can be selected. Every name is checked against the allow-list before any
        ///     expression is built: a name that matches no allowed name, even ignoring case, is rejected without
        ///     inspecting the element type; a name that resolves to a property whose exact name is not allowed is
        ///     also rejected.
        ///     The allow-list spelling is canonical: a requested name is mapped to the allow-list entry that
        ///     matches exactly, otherwise to the only entry that matches ignoring case (several such entries
        ///     reject it), and only that entry is resolved. With the allow-list {"Name"}, "NAME" and "nAmE"
        ///     resolve via "Name" to the Name property, never to a NAME sibling.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="source"/> or <paramref name="allowedFields"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     Thrown when <paramref name="allowedFields"/> contains a null, empty or white-space entry, or
        ///     <paramref name="props"/> is null or empty, contains a null, empty, white-space, unknown, duplicate or
        ///     not allowed name, or has more than 256 names.
        /// </exception>
        /// =================================================================================================
        public static IQueryable SelectMultipleProperties(this IQueryable source, IEnumerable<string> props,
            IEnumerable<string> allowedFields)
        {
            if (source.IsNull())
                throw new ArgumentNullException(nameof(source));

            var propertyNames = AllowedFieldsResolver.ResolveAllowedFields(source.ElementType, props, allowedFields,
                nameof(props), nameof(allowedFields));

            return source.SelectMultipleProperties(propertyNames);
        }
    }
}
