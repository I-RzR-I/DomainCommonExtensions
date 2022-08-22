// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-08-10 21:39
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-12 23:39
// ***********************************************************************
//  <copyright file="ObjectExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections;
using System.ComponentModel;
using System.Xml;

#endregion

namespace DomainCommonExtensions.DataTypeExtensions
{
    /// <summary>
    ///     Object extensions
    /// </summary>
    /// <remarks></remarks>
    public static class ObjectExtensions
    {
        /// <summary>
        ///     Gets the DateTime? from reader object. Return null if value empty.
        /// </summary>
        /// <param name="value">Input DateTime object</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DateTime? TryGetDateTimeFromDbObj(this object value)
        {
            if (value == null || value == DBNull.Value || string.IsNullOrEmpty(value.ToString()))
                return null;
            try
            {
                return Convert.ToDateTime(value);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///     Tries to cast initial object to T value
        /// </summary>
        /// <param name="obj">Object to cast</param>
        /// <param name="result">Result object</param>
        /// <returns></returns>
        /// <typeparam name="T">Type of result data</typeparam>
        /// <remarks></remarks>
        public static bool TryCast<T>(this object obj, out T result)
        {
            if (obj is T)
            {
                result = (T)obj;
                return true;
            }

            result = default;

            return false;
        }

        /// <summary>
        ///     Get xml with object property
        /// </summary>
        /// <param name="obj">The object</param>
        /// <param name="xmlDocument">The xml document</param>
        public static void GetXmlParams(this object obj, ref XmlDocument xmlDocument)
        {
            foreach (var prop in obj.GetType().GetProperties())
            {
                var xmlNode = xmlDocument.SelectSingleNode("//" + prop.Name);
                if (xmlNode != null && xmlNode.ChildNodes.Count == 0 && prop.GetValue(obj, null) != null)
                    if (xmlNode.Attributes != null)
                        xmlNode.Attributes["value"].Value = prop.GetValue(obj, null).ToString();
            }
        }

        /// <summary>
        ///     Copy properties from object to another
        /// </summary>
        /// <param name="fromObject">Copy from</param>
        /// <param name="toObject">Copy to</param>
        /// <remarks></remarks>
        public static void CopyPropertiesTo(this object fromObject, object toObject)
        {
            var toObjectProperties = toObject.GetType().GetProperties();
            foreach (var propTo in toObjectProperties)
            {
                var propFrom = fromObject.GetType().GetProperty(propTo.Name);
                if (propFrom != null && propFrom.CanWrite)
                    propTo.SetValue(toObject, propFrom.GetValue(fromObject, null), null);
            }
        }

        /// <summary>
        ///     Get properties hash
        /// </summary>
        /// <param name="properties">Property</param>
        /// <returns>Hash table</returns>
        /// <remarks></remarks>
        public static Hashtable GetPropertyHash(this object properties)
        {
            Hashtable values = null;
            if (properties != null)
            {
                values = new Hashtable();
                var props = TypeDescriptor.GetProperties(properties);
                foreach (PropertyDescriptor prop in props) values.Add(prop.Name, prop.GetValue(properties));
            }

            return values;
        }
    }
}