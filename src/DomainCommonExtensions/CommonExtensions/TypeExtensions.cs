// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-08-10 21:39
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-12 23:39
// ***********************************************************************
//  <copyright file="TypeExtensions.cs" company="">
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
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using DomainCommonExtensions.DataTypeExtensions;

#endregion

namespace DomainCommonExtensions.CommonExtensions
{
    /// <summary>
    ///     Type extensions
    /// </summary>
    /// <remarks></remarks>
    public static class TypeExtensions
    {
        private static readonly Dictionary<Type, Type> NullableTypeDict = new Dictionary<Type, Type>
        {
            [typeof(byte?)] = typeof(byte),
            [typeof(sbyte?)] = typeof(sbyte),
            [typeof(short?)] = typeof(short),
            [typeof(ushort?)] = typeof(ushort),
            [typeof(int?)] = typeof(int),
            [typeof(uint?)] = typeof(uint),
            [typeof(long?)] = typeof(long),
            [typeof(ulong?)] = typeof(ulong),
            [typeof(float?)] = typeof(float),
            [typeof(double?)] = typeof(double),
            [typeof(decimal?)] = typeof(decimal),
            [typeof(bool?)] = typeof(bool),
            [typeof(char?)] = typeof(char),
            [typeof(Guid?)] = typeof(Guid),
            [typeof(DateTime?)] = typeof(DateTime),
            [typeof(DateTimeOffset?)] = typeof(DateTimeOffset),
            [typeof(TimeSpan?)] = typeof(TimeSpan)
        };

        /// <summary>
        ///     Get property value as object
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static object GetPropertyValue(this object obj, string name)
        {
            return obj?.GetType()
                .GetProperty(name.Trim().FirstCharToUpper())
                ?.GetValue(obj, null);
        }

        /// <summary>
        ///     Get property value as string
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetStringPropertyValue(this object obj, string name)
        {
            return obj?.GetType()
                .GetProperty(name.Trim().FirstCharToUpper())
                ?.GetValue(obj, null)?.ToString() ?? string.Empty;
        }

        /// <summary>
        ///     Check if type of object is 'Anonymous'
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsAnonymousType(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false)
                   && type.IsGenericType && type.Name.Contains("AnonymousType")
                   && (type.Name.StartsWith("<>") || type.Name.StartsWith("VB$"))
                   && type.Attributes.HasFlag(TypeAttributes.NotPublic);
        }

        /// <summary>
        ///     Check if property is type of nullable
        /// </summary>
        /// <param name="type">Property type</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsNullablePropType(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        ///     Get value type by property typ
        /// </summary>
        /// <param name="type">Property type</param>
        /// <returns></returns>
        /// <typeparam name="T">Type result</typeparam>
        /// <remarks></remarks>
        public static T GetValue<T>(this Type type)
        {
            object value = null;

            var t = typeof(T);
            t = Nullable.GetUnderlyingType(t) ?? t;

            return value == null || DBNull.Value.Equals(value)
                ? default
                : (T)Convert.ChangeType(value, t);
        }

        /// <summary>
        ///     Get string with props from Entity
        /// </summary>
        /// <param name="type">Entity type</param>
        /// <param name="fields">Required fields</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetSelectedFieldFromEntity(this Type type, IEnumerable<string> fields = null)
        {
            var searchBuilder = new StringBuilder();
            var propsInfo = type.GetProperties();
            var props = propsInfo.Select(x => x.Name).ToList();

            if (fields != null && fields.Any())
            {
                //Return only specified fields
                foreach (var field in fields.ToList())
                    if (props.Contains(field))
                    {
                        var propType = propsInfo.FirstOrDefault(x => x.Name == field);
                        if (propType != null) searchBuilder.Append($"{field},");
                    }
            }
            else
            {
                //Return all fields
                foreach (var p in props) searchBuilder.Append($"{p},");
            }

            return searchBuilder.ToString().TrimEnd(',');
        }

        /// <summary>
        ///     Determine if the field type is corresponding with type in DB
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsSimpleType(this Type type)
        {
            var underlyingType = Nullable.GetUnderlyingType(type);
            type = underlyingType ?? type;
            var simpleTypes = new List<Type>
            {
                typeof(byte),
                typeof(sbyte),
                typeof(short),
                typeof(ushort),
                typeof(int),
                typeof(uint),
                typeof(long),
                typeof(ulong),
                typeof(float),
                typeof(double),
                typeof(decimal),
                typeof(bool),
                typeof(string),
                typeof(char),
                typeof(Guid),
                typeof(DateTime),
                typeof(DateTimeOffset),
                typeof(byte[])
            };

            return simpleTypes.Contains(type) || type.IsEnum;
        }

        /// <summary>
        ///     Check if data type is type on integer data
        /// </summary>
        /// <param name="type">Current type</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsTypeOnInt(this Type type)
        {
            var underlyingType = Nullable.GetUnderlyingType(type);
            type = underlyingType ?? type;
            var simpleTypes = new List<Type>
            {
                typeof(short),
                typeof(ushort),
                typeof(int),
                typeof(uint),
                typeof(long),
                typeof(ulong)
            };

            return simpleTypes.Contains(type);
        }

        /// <summary>
        ///     Check is property is Enum
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsEnumType(this Type type)
        {
            var underlyingType = Nullable.GetUnderlyingType(type);
            type = underlyingType ?? type;

            return type.IsEnum;
        }

        /// <summary>
        ///     Check is property is string
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsStringType(this Type type)
        {
            var underlyingType = Nullable.GetUnderlyingType(type);
            type = underlyingType ?? type;

            return type == typeof(string);
        }

        /// <summary>
        ///     Get non nullable type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Type GetNonNullableType(this Type type)
        {
            return type.IsNullablePropType()
                ? NullableTypeDict.FirstOrDefault(x => x.Key == type).Value
                : type;
        }
    }
}