// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-08-10 21:39
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-12 23:39
// ***********************************************************************
//  <copyright file="EnumExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

#endregion

namespace DomainCommonExtensions.DataTypeExtensions
{
    /// <summary>
    ///     Enum extensions
    /// </summary>
    /// <remarks></remarks>
    public static class EnumExtensions
    {
        /// <summary>
        ///     Get enum definition
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static Dictionary<int, string> GetEnumDefinition(this Type enumType)
        {
            if (!enumType.IsEnum) throw new Exception("Non valid enum");

            return Enum.GetNames(enumType).ToDictionary(x => (int)Enum.Parse(enumType, x), x => x);
        }

        /// <summary>
        ///     Get enum member value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumMemberValue<T>(this T value) where T : struct, IConvertible
        {
            var element = typeof(T).GetTypeInfo().DeclaredMembers
                .SingleOrDefault(x => x.Name == value.ToString(CultureInfo.InvariantCulture));

            return (object)element == null ? null : element.GetCustomAttribute<EnumMemberAttribute>(false)?.Value;
        }

        /// <summary>
        ///     Get enum value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T ToEnumMemberValue<T>(this string str)
        {
            var enumType = typeof(T);
            foreach (var name in Enum.GetNames(enumType))
            {
                var enumMemberAttribute =
                    ((EnumMemberAttribute[])enumType.GetField(name)
                        ?.GetCustomAttributes(typeof(EnumMemberAttribute), true) ?? Array.Empty<EnumMemberAttribute>())
                    .Single();
                if (enumMemberAttribute.Value == str) return (T)Enum.Parse(enumType, name);
            }

            return default;
        }

        /// <summary>
        ///     Get int from enum property
        /// </summary>
        /// <param name="source">Enum property</param>
        /// <returns></returns>
        /// <typeparam name="T"></typeparam>
        /// <remarks></remarks>
        public static int ToInt<T>(this T source) where T : IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type");

            return (int)(IConvertible)source;
        }

        /// <summary>
        ///     Convert enum property to string
        /// </summary>
        /// <param name="source">Enum property</param>
        /// <returns></returns>
        /// <typeparam name="T"></typeparam>
        /// <remarks></remarks>
        public static string ToString<T>(this T source) where T : IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type");

            return (string)(IConvertible)source;
        }

        /// <summary>
        ///     Get description of enum property if exist
        /// </summary>
        /// <param name="value">Enum</param>
        /// <param name="returnEmpty">In case when 'Description' not found, then return empty string</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetDescription(this Enum value, bool returnEmpty = false)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            if (fieldInfo == null) return null;

            var attribute = (DescriptionAttribute)fieldInfo.GetCustomAttribute(typeof(DescriptionAttribute));

            return attribute != null ? attribute.Description : (returnEmpty.Equals(true) ? "" : value.ToString());
        }

        /// <summary>
        ///     Method to get enumeration value from string value
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// <typeparam name="T"></typeparam>
        /// <remarks></remarks>
        public static T GetEnumValue<T>(this string str) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum) throw new Exception("T must be an Enumeration type.");
            var val = ((T[])Enum.GetValues(typeof(T)))[0];
            if (!string.IsNullOrEmpty(str))
                foreach (var enumValue in (T[])Enum.GetValues(typeof(T)))
                    if (enumValue.ToString().ToUpper().Equals(str.ToUpper()))
                    {
                        val = enumValue;
                        break;
                    }

            return val;
        }

        /// <summary>
        ///     Method to get enumeration value from int value
        /// </summary>
        /// <param name="intValue"></param>
        /// <returns></returns>
        /// <typeparam name="T"></typeparam>
        /// <remarks></remarks>
        public static T GetEnumValue<T>(this int intValue) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum) throw new Exception("T must be an Enumeration type.");
            var val = ((T[])Enum.GetValues(typeof(T)))[0];

            foreach (var enumValue in (T[])Enum.GetValues(typeof(T)))
                if (Convert.ToInt32(enumValue).Equals(intValue))
                {
                    val = enumValue;
                    break;
                }

            return val;
        }

        /// <summary>
        ///     Get 'Name' value from 'Display' attribute
        /// </summary>
        /// <param name="value">Enum value</param>
        /// <returns></returns>
        public static string GetDisplayName(this Enum value)
        {
            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString()).FirstOrDefault();

            if (memberInfo != null)
            {
                var attribute = (DisplayAttribute)
                    memberInfo.GetCustomAttributes(typeof(DisplayAttribute), false)
                        .FirstOrDefault();
                return attribute?.Name ?? value.ToString();
            }

            return value.ToString();
        }

        /// <summary>
        ///     Get 'Description' value from 'Display' attribute
        /// </summary>
        /// <param name="value">Enum value</param>
        /// <returns></returns>
        public static string GetDisplayDescription(this Enum value)
        {
            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString()).FirstOrDefault();

            if (memberInfo != null)
            {
                var attribute = (DisplayAttribute)
                    memberInfo.GetCustomAttributes(typeof(DisplayAttribute), false)
                        .FirstOrDefault();
                return attribute?.Description ?? value.ToString();
            }

            return value.ToString();
        }
    }
}