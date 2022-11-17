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
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

#endregion

namespace DomainCommonExtensions.CommonExtensions
{
    // ReSharper disable once InconsistentNaming
    /// <summary>
    ///     T type extensions
    /// </summary>
    /// <remarks></remarks>
    public static class TExtensions
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
        ///     Return data if not null
        /// </summary>
        /// <param name="source">Source data</param>
        /// <param name="action">Action</param>
        /// <returns></returns>
        /// <typeparam name="TInput">Input type</typeparam>
        /// <typeparam name="TResult">Result type</typeparam>
        /// <remarks></remarks>
        public static TResult IfNotNull<TInput, TResult>(this TInput source, Func<TInput, TResult> action)
        {
            if (action.IsNull())
                throw new ArgumentNullException(nameof(action));

            return !source.IsNull()
                ? action(source)
                : default;
        }

        /// <summary>
        ///     Return data if not null
        /// </summary>
        /// <param name="source">Source data</param>
        /// <param name="action">Action</param>
        /// <param name="defaultValue">Default result value</param>
        /// <returns></returns>
        /// <typeparam name="TInput">Input type</typeparam>
        /// <typeparam name="TResult">Result type</typeparam>
        /// <remarks></remarks>
        public static TResult IfNotNull<TInput, TResult>(this TInput source, Func<TInput, TResult> action, TResult defaultValue)
        {
            if (action.IsNull())
                throw new ArgumentNullException(nameof(action));

            return !source.IsNull()
                ? action(source)
                : defaultValue;
        }

        /// <summary>
        ///     Return data if not null
        /// </summary>
        /// <param name="source">Source data</param>
        /// <param name="action">Action</param>
        /// <typeparam name="TInput">Input type</typeparam>
        /// <remarks></remarks>
        public static void IfNotNull<TInput>(this TInput source, Action<TInput> action)
        {
            if (action.IsNull())
                throw new ArgumentNullException(nameof(action));

            if (source.IsNull())
                return;

            action(source);
        }
    }
}