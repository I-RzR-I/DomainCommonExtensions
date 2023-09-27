// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-09-14 21:15
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-09-14 21:15
// ***********************************************************************
//  <copyright file="ExpandoObjectExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace DomainCommonExtensions.CommonExtensions
{
    /// <summary>
    ///     Expando object extensions
    /// </summary>
    /// <remarks></remarks>
    public static class ExpandoObjectExtensions
    {
        /// <summary>
        ///     Add property
        /// </summary>
        /// <param name="expando">Object</param>
        /// <param name="propertyName">Property name</param>
        /// <param name="propertyValue">Property value</param>
        /// <remarks></remarks>
        public static void AddProperty(this ExpandoObject expando, string propertyName, object propertyValue)
        {
            // ExpandoObject supports IDictionary so we can extend it like this
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }

        /// <summary>
        ///     Update property value
        /// </summary>
        /// <param name="expando">Object</param>
        /// <param name="propertyName">Property name</param>
        /// <param name="propertyValue">Property value</param>
        /// <remarks></remarks>
        public static void UpdateValue(this ExpandoObject expando, string propertyName, object propertyValue)
        {
            var map = (IDictionary<string, object>)expando;
            if (map.ContainsKey(propertyName))
                map[propertyName] = propertyValue;
            foreach (var val in map.Values)
            {
                if (val is ExpandoObject)
                    UpdateValue((ExpandoObject)val, propertyName, propertyValue);
            }
        }

        /// <summary>
        ///     Get value by key
        /// </summary>
        /// <param name="expando">Object</param>
        /// <param name="key">Key for find</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static object GetValue(this ExpandoObject expando, string key)
        {
            object r = expando.FirstOrDefault(x => x.Key == key).Value;

            return r;
        }
    }
}