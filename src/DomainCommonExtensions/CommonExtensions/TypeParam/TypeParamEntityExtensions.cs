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

using System.Collections.Generic;
using System.Linq;
using DomainCommonExtensions.ArraysExtensions;
using DomainCommonExtensions.Utilities.Ensure;

#endregion

namespace DomainCommonExtensions.CommonExtensions.TypeParam
{
    /// -------------------------------------------------------------------------------------------------
    /// <content>
    ///     T type extensions.
    /// </content>
    /// =================================================================================================
    public static partial class TExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the property values in this collection.
        /// </summary>
        /// <typeparam name="TPropResultType">Type of the property result type.</typeparam>
        /// <typeparam name="TSourceEntity">Type of the source entity.</typeparam>
        /// <param name="source">The source to act on.</param>
        /// <param name="fieldNames">A variable-length parameters list containing field names.</param>
        /// <returns>
        ///     An enumerator that allows foreach to be used to process the property values in this
        ///     collection.
        /// </returns>
        /// =================================================================================================
        public static IEnumerable<TPropResultType> GetPropertyValue<TPropResultType, TSourceEntity>(this TSourceEntity source,
            params string[] fieldNames)
            where TPropResultType : struct
            where TSourceEntity : class
        {
            DomainEnsure.IsNotNull(source, nameof(source));

            var resultList = new List<TPropResultType>();
            var props = source!.GetType().GetProperties().Where(x => fieldNames.Contains(x.Name));
            foreach (var prop in props.NotNull())
            {
                var value = prop.GetValue(source, null);
                if (value.IsNotNull())
                    resultList.Add((TPropResultType)value!);
            }

            return resultList;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the property string values in this collection.
        /// </summary>
        /// <typeparam name="TSourceEntity">Type of the source entity.</typeparam>
        /// <param name="source">The source to act on.</param>
        /// <param name="fieldNames">A variable-length parameters list containing field names.</param>
        /// <returns>
        ///     An enumerator that allows foreach to be used to process the property string values in
        ///     this collection.
        /// </returns>
        /// =================================================================================================
        public static IEnumerable<string> GetPropertyStringValue<TSourceEntity>(this TSourceEntity source,
            params string[] fieldNames)
            where TSourceEntity : class
        {
            DomainEnsure.IsNotNull(source, nameof(source));

            var resultList = new List<string>();
            var props = source!.GetType().GetProperties().Where(x => fieldNames.Contains(x.Name));
            foreach (var prop in props.NotNull())
            {
                var value = prop.GetValue(source, null);
                if (value.IsNotNull())
                    resultList.Add(value!.ToString()!);
            }

            return resultList;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the property values in this collection.
        /// </summary>
        /// <typeparam name="TSourceEntity">Type of the source entity.</typeparam>
        /// <param name="source">The source to act on.</param>
        /// <param name="fieldNames">A variable-length parameters list containing field names.</param>
        /// <returns>
        ///     An enumerator that allows foreach to be used to process the property values in this
        ///     collection.
        /// </returns>
        /// =================================================================================================
        public static IEnumerable<object> GetPropertyValue<TSourceEntity>(this TSourceEntity source,
            params string[] fieldNames)
            where TSourceEntity : class
        {
            DomainEnsure.IsNotNull(source, nameof(source));

            var resultList = new List<object>();
            var props = source!.GetType().GetProperties().Where(x => fieldNames.Contains(x.Name));
            foreach (var prop in props.NotNull())
            {
                var value = prop.GetValue(source, null);
                if (value.IsNotNull())
                    resultList.Add(value!);
            }

            return resultList;
        }

        /// -------------------------------------------------------------------------------------------------
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
        /// =================================================================================================
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