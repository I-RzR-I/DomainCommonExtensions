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
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using CodeSource;
using DomainCommonExtensions.CommonExtensions;
using DomainCommonExtensions.Utilities.Ensure;

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
            if (value.IsNull() || value.IsDbNull() || string.IsNullOrEmpty(value.ToString()))
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
            DomainEnsure.IsNotNull(obj, nameof(obj));

            foreach (var prop in obj.GetType().GetProperties())
            {
                var xmlNode = xmlDocument.SelectSingleNode("//" + prop.Name);
                if (xmlNode != null && xmlNode.ChildNodes.Count.IsZero() && prop.GetValue(obj, null) != null)
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
            DomainEnsure.IsNotNull(fromObject, nameof(fromObject));
            DomainEnsure.IsNotNull(toObject, nameof(toObject));

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
            if (properties.IsNotNull())
            {
                values = new Hashtable();
                var props = TypeDescriptor.GetProperties(properties);
                foreach (PropertyDescriptor prop in props) values.Add(prop.Name, prop.GetValue(properties));
            }

            return values;
        }

        /// <summary>
        ///     Check if object is inside the given collection
        /// </summary>
        /// <param name="obj">Object to check</param>
        /// <param name="collection">Where to check</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool In(this object obj, IEnumerable collection)
        {
            return collection.Cast<object>().Contains(obj);
        }

        /// <summary>
        ///     Returns a dictionary with all properties of the object and their values
        /// </summary>
        /// <param name="obj">The object to translate into a dictionary</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static IDictionary<string, object> ToDictionary(this object obj)
        {
            if (obj.IsNull()) return null;

            var result = new Dictionary<string, object>();
            foreach (var property in obj.GetType().GetProperties())
            {
#if NET40
                result[property.Name] = property.GetValue(obj, null);
#else
                result[property.Name] = property.GetValue(obj);
#endif
            }

            return result;
        }

        /// <summary>
        ///     Cast object to string
        /// </summary>
        /// <param name="source">Input source object</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToString(this object source)
        {
            if (source.IsDbNull() || source.IsNull())
                return null;
            else
                return source.ToString();
        }

        /// <summary>
        ///     Cast object to T
        /// </summary>
        /// <param name="source">Input source object</param>
        /// <returns></returns>
        /// <typeparam name="T">Type to cast</typeparam>
        /// <remarks></remarks>
        public static T To<T>(this object source)
        {
            if (source.IsDbNull() || source.IsNull())
                return default;
            else
                return (T)source;
        }

        /// <summary>
        ///     Serialize object to string
        /// </summary>
        /// <param name="obj">Source data object</param>
        /// <returns></returns>
        /// <remarks></remarks>
        [CodeSource("https://learn.microsoft.com/en-us/dotnet/api/system.runtime.serialization.datacontractserializer?view=netstandard-2.0", "", "MS", "2022-12-28", "Reference source")]
        public static string SerializeToString(this object obj)
        {
            if (obj.IsNull()) return null;

            using var memoryStream = new MemoryStream();
            using var reader = new StreamReader(memoryStream);
            var serializer = new DataContractSerializer(obj.GetType());
            serializer.WriteObject(memoryStream, obj);
            memoryStream.Position = 0;

            return reader.ReadToEnd();
        }
    }
}