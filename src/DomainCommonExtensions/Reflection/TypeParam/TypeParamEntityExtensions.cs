// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2026-01-19 22:01
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-01-19 22:42
// ***********************************************************************
//  <copyright file="TypeParamEntityExtensions.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using RzR.Extensions.Domain.Collections;
using RzR.Extensions.Domain.Validation;
using RzR.Extensions.Domain.Primitives;

#endregion

namespace RzR.Extensions.Domain.Reflection.TypeParam
{
    /// <content>
    ///     T type extensions.
    /// </content>
    public static partial class TExtensions
    {
        /// <summary>
        ///     Gets the property values in this collection.
        /// </summary>
        /// <remarks>
        ///     Values are returned in the order of <paramref name="fieldNames"/>; a repeated name is
        ///     read once, missing properties and null values are skipped and a null list gives an empty
        ///     result. Values of another convertible type are converted with the invariant culture, so
        ///     an out-of-range narrowing throws <see cref="OverflowException"/>, an enum result from a
        ///     non-integral value throws <see cref="ArgumentException"/> and any other unsupported
        ///     conversion throws
        ///     <see cref="InvalidCastException"/>.
        /// </remarks>
        /// <typeparam name="TPropResultType">Type of the property result type.</typeparam>
        /// <typeparam name="TSourceEntity">Type of the source entity.</typeparam>
        /// <param name="source">The source to act on.</param>
        /// <param name="fieldNames">A variable-length parameters list containing field names.</param>
        /// <returns>
        ///     An enumerator that allows foreach to be used to process the property values in this
        ///     collection.
        /// </returns>
        public static IEnumerable<TPropResultType> GetPropertyValue<TPropResultType, TSourceEntity>(this TSourceEntity source,
            params string[] fieldNames)
            where TPropResultType : struct
            where TSourceEntity : class
        {
            DomainEnsure.IsNotNull(source, nameof(source));

            return GetNonNullPropertyValuesInOrder(source, fieldNames)
                .Select(ConvertPropertyValue<TPropResultType>)
                .ToList();
        }

        /// <summary>
        ///     Gets the property string values in this collection.
        /// </summary>
        /// <remarks>
        ///     Values are returned in the order of <paramref name="fieldNames"/>; a repeated name is
        ///     read once, missing properties and null values are skipped and a null list gives an empty
        ///     result.
        /// </remarks>
        /// <typeparam name="TSourceEntity">Type of the source entity.</typeparam>
        /// <param name="source">The source to act on.</param>
        /// <param name="fieldNames">A variable-length parameters list containing field names.</param>
        /// <returns>
        ///     An enumerator that allows foreach to be used to process the property string values in
        ///     this collection.
        /// </returns>
        public static IEnumerable<string> GetPropertyStringValue<TSourceEntity>(this TSourceEntity source,
            params string[] fieldNames)
            where TSourceEntity : class
        {
            DomainEnsure.IsNotNull(source, nameof(source));

            return GetNonNullPropertyValuesInOrder(source, fieldNames)
                .Select(x => x.ToString()!)
                .ToList();
        }

        /// <summary>
        ///     Gets the property values in this collection.
        /// </summary>
        /// <remarks>
        ///     Values are returned in the order of <paramref name="fieldNames"/>; a repeated name is
        ///     read once, missing properties and null values are skipped and a null list gives an empty
        ///     result.
        /// </remarks>
        /// <typeparam name="TSourceEntity">Type of the source entity.</typeparam>
        /// <param name="source">The source to act on.</param>
        /// <param name="fieldNames">A variable-length parameters list containing field names.</param>
        /// <returns>
        ///     An enumerator that allows foreach to be used to process the property values in this
        ///     collection.
        /// </returns>
        public static IEnumerable<object> GetPropertyValue<TSourceEntity>(this TSourceEntity source,
            params string[] fieldNames)
            where TSourceEntity : class
        {
            DomainEnsure.IsNotNull(source, nameof(source));

            return GetNonNullPropertyValuesInOrder(source, fieldNames);
        }

        /// <summary>
        ///     Read the non-null values of the named readable properties in the order of the names,
        ///     reading each name once and skipping missing properties.
        /// </summary>
        /// <param name="source">The source to act on, not null.</param>
        /// <param name="fieldNames">The property names; null gives an empty result.</param>
        /// <returns>
        ///     The non-null property values.
        /// </returns>
        private static List<object> GetNonNullPropertyValuesInOrder(object source, string[] fieldNames)
        {
            var resultList = new List<object>();
            if (fieldNames.IsNull())
                return resultList;

            var props = ReflectionExtensions.GetMostDerivedProperties(source.GetType(),
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            var readNames = new HashSet<string>(StringComparer.Ordinal);
            foreach (var fieldName in fieldNames)
            {
                if (fieldName.IsNull() || !readNames.Add(fieldName))
                    continue;
                if (!props.TryGetValue(fieldName, out var prop) || !prop.CanRead)
                    continue;

                var value = prop.GetValue(source, null);
                if (value.IsNotNull())
                    resultList.Add(value!);
            }

            return resultList;
        }

        /// <summary>
        ///     Convert a non-null property value to the requested value type.
        /// </summary>
        /// <typeparam name="TPropResultType">Type of the property result type.</typeparam>
        /// <param name="value">The property value, not null.</param>
        /// <returns>
        ///     The converted value.
        /// </returns>
        private static TPropResultType ConvertPropertyValue<TPropResultType>(object value)
            where TPropResultType : struct
        {
            if (value is TPropResultType typedValue)
                return typedValue;

            var resultType = typeof(TPropResultType);
            if (resultType.IsEnum)
                return (TPropResultType)Enum.ToObject(resultType, value);
            if (value is IConvertible && !(value is string))
                return (TPropResultType)Convert.ChangeType(value, resultType, CultureInfo.InvariantCulture);

            return (TPropResultType)value;
        }

        /// <summary>
        ///     A TEntityResult extension method that change property value.
        /// </summary>
        /// <typeparam name="TEntityResult">Type of the entity result.</typeparam>
        /// <typeparam name="TEntitySource">Type of the entity source.</typeparam>
        /// <param name="sourceResultEntry">The sourceResultEntry to act on.</param>
        /// <param name="sourceDataEntry">Source data entry.</param>
        /// <param name="propsName">A variable-length parameters list containing properties name.</param>
        /// <returns>
        ///     A TEntityResult.
        /// </returns>
        public static TEntityResult ChangePropertyValue<TEntityResult, TEntitySource>(
            this TEntityResult sourceResultEntry,
            TEntitySource sourceDataEntry, params string[] propsName)
            where TEntitySource : class
            where TEntityResult : class
        {
            DomainEnsure.IsNotNull(sourceResultEntry, nameof(sourceResultEntry));
            DomainEnsure.IsNotNull(sourceDataEntry, nameof(sourceDataEntry));

            var props = sourceResultEntry!.GetType().GetProperties().Where(x => propsName.Contains(x.Name));
            foreach (var prop in props.NotNull())
            {
                var sourceEntryProp = sourceDataEntry.GetType().GetProperty(prop.Name);
                if (sourceEntryProp.IsNotNull())
                {
                    var value = sourceEntryProp!.GetValue(sourceDataEntry, null);
                    prop!.SetValue(sourceResultEntry, value, null);
                }
            }

            return sourceResultEntry;
        }
    }
}
