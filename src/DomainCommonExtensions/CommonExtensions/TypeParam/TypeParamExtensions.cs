// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-08-10 21:39
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-12 23:39
// ***********************************************************************
//  <copyright file="TExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using CodeSource;
using DomainCommonExtensions.DataTypeExtensions;

#pragma warning disable SCS0007

#endregion

namespace DomainCommonExtensions.CommonExtensions.TypeParam
{
    // ReSharper disable once InconsistentNaming
    /// <summary>
    ///     T type extensions
    /// </summary>
    /// <remarks></remarks>
    public static partial class TExtensions
    {
        /// <summary>
        ///     Cloning an object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T Clone<T>(this T source)
        {
            if (!typeof(T).IsSerializable)
                throw new ArgumentException("The type must be serializable.", nameof(source));

            // Don't serialize a null object, simply return the default for that object
            if (ReferenceEquals(source, null)) return default;

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);

                return (T)formatter.Deserialize(stream);
            }
        }

        /// <summary>
        ///     Serialize source data to XML document
        /// </summary>
        /// <param name="source">Required. Source data</param>
        /// <param name="rootName">Optional. The default value is null. XML root tag name</param>
        /// <param name="rootNameSpaceName">Optional. The default value is null. XML root namespace name</param>
        /// <returns></returns>
        /// <typeparam name="T">Source data type</typeparam>
        /// <remarks></remarks>
        [CodeSource("https://learn.microsoft.com/en-us/dotnet/api/system.runtime.serialization.datacontractserializer?view=netstandard-2.0", "", "MS", "2022-12-28", "Reference source")]
        public static XmlDocument SerializeToXmlDoc<T>(this T source, string rootName = null, string rootNameSpaceName = null)
        {
            if (source.IsNull()) return null;

            rootName = rootName.IsNullOrEmpty() ? "Root" : rootName;
            rootNameSpaceName = rootNameSpaceName.IsNullOrEmpty() ? "RootNs" : rootNameSpaceName;
            var dataContractSerializer = new DataContractSerializer(source.GetType(), rootName!, rootNameSpaceName!);

            var ms = new MemoryStream();
            dataContractSerializer.WriteObject(ms, source);
            ms.Position = 0;

            var doc = new XmlDocument();
            doc.Load(ms);

            return doc;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>A T extension method that appends to.</summary>
        /// <remarks></remarks>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="first">The first to act on.</param>
        /// <param name="items">A variable-length parameters list containing items.</param>
        /// <returns>A T[].</returns>
        ///=================================================================================================
        public static T[] AppendTo<T>(this T first, params T[] items)
        {
            var result = new T[items.Length + 1];
            result[0] = first;
            items.CopyTo(result, 1);

            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A TSource extension method that gets properties information from source.
        /// </summary>
        /// <remarks></remarks>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <param name="source">.</param>
        /// <returns>The properties information from source.</returns>
        ///=================================================================================================
        public static IList<PropertyInfo> GetPropertiesInfoFromSource<TSource>(this TSource source) where TSource : class
        {
            try
            {
                return typeof(TSource).GetProperties()
                    .ToList();
            }
            catch { return null; }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A TSource extension method that if is null.
        /// </summary>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <param name="source">Source data.</param>
        /// <param name="defaultValue">Default result value.</param>
        /// <returns>
        ///     A TSource.
        /// </returns>
        /// =================================================================================================
        public static TSource IfIsNull<TSource>(this TSource source, TSource defaultValue)
        {
            if (source.IsNull())
                return defaultValue;

            return source;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A TSource extension method that if is not null.
        /// </summary>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <param name="source">Source data.</param>
        /// <param name="defaultValue">Default result value.</param>
        /// <returns>
        ///     A TSource.
        /// </returns>
        /// =================================================================================================
        public static TSource IfIsNotNull<TSource>(this TSource source, TSource defaultValue)
        {
            if (source.IsNotNull())
                return defaultValue;

            return source;
        }
    }
}