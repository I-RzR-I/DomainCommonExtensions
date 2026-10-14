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

#region U S A G E S

using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using RzR.Extensions.Domain.Primitives;

#endregion

namespace RzR.Extensions.Domain.Data
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
            if (expandoDict.ContainsKey(propertyName).IsTrue())
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
        /// <remarks>
        ///     Walks <paramref name="expando"/> and every nested <see cref="ExpandoObject"/> value iteratively.
        ///     Each object is processed once, so cycles and shared nodes are handled.
        /// </remarks>
        public static void UpdateValue(this ExpandoObject expando, string propertyName, object propertyValue)
        {
            // ExpandoObject does not override Equals/GetHashCode, so the default comparer is reference equality.
            var visited = new HashSet<ExpandoObject>();
            var pending = new Stack<ExpandoObject>();
            pending.Push(expando);

            while (pending.Count > 0)
            {
                var current = pending.Pop();
                if (visited.Add(current).IsFalse())
                    continue;

                var map = (IDictionary<string, object>)current;
                if (map.ContainsKey(propertyName).IsTrue())
                    map[propertyName] = propertyValue;
                foreach (var val in map.Values)
                {
                    if (val is ExpandoObject)
                        pending.Push((ExpandoObject)val);
                }
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
