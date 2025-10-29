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
using System.Linq;
using DomainCommonExtensions.CommonExtensions;
using DomainCommonExtensions.Resources.Enums;
using DomainCommonExtensions.Utilities.Ensure;

#if NET45_OR_GREATER || NET || NETSTANDARD1_0_OR_GREATER
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Runtime.Serialization;
#endif

// ReSharper disable RedundantCast

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
            DomainEnsure.ThrowArgumentExceptionIfFuncIsFalse(
                () => enumType.IsEnum, "Non valid enum!", nameof(enumType));

            return Enum.GetNames(enumType).ToDictionary(x => (int)Enum.Parse(enumType, x), x => x);
        }

#if NET45_OR_GREATER || NET || NETSTANDARD1_0_OR_GREATER
        /// <summary>
        ///     Get enum member value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumMemberValue<T>(this T value) where T : struct, Enum, IConvertible
        {
            if (value.IsNull()) return null;

            var element = typeof(T).GetTypeInfo().DeclaredMembers
                .SingleOrDefault(x => x.Name == value.ToString());

            return ((object)element).IsNull() ? null : element!.GetCustomAttribute<EnumMemberAttribute>(false)?.Value;
        }
#endif

#if NET || NETSTANDARD1_0_OR_GREATER
        /// <summary>
        ///     Get enum value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T ToEnumMemberValue<T>(this string str) where T : struct, Enum, IConvertible
        {
            if (str.IsNull()) return default;

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
#endif

        /// <summary>
        ///     Get int from enum property
        /// </summary>
        /// <param name="source">Enum property</param>
        /// <returns></returns>
        /// <typeparam name="T"></typeparam>
        /// <remarks></remarks>
        public static int ToInt<T>(this T source) where T : Enum, IConvertible
        {
            DomainEnsure.IsNotNull(source, nameof(source));
            DomainEnsure.ThrowArgumentExceptionIfFuncIsFalse(
                () => typeof(T).IsEnum, "T must be an enumerated type!", typeof(T).Name);

            return (int)(IConvertible)source;
        }

        /// <summary>
        ///     Convert enum property to string
        /// </summary>
        /// <param name="source">Enum property</param>
        /// <returns></returns>
        /// <typeparam name="T"></typeparam>
        /// <remarks></remarks>
        public static string ToString<T>(this T source) where T : Enum, IConvertible
        {
            DomainEnsure.IsNotNull(source, nameof(source));
            DomainEnsure.ThrowArgumentExceptionIfFuncIsFalse(
                () => typeof(T).IsEnum, "T must be an enumerated type!", typeof(T).Name);

            return (string)(IConvertible)source;
        }

#if NET45_OR_GREATER || NET || NETSTANDARD1_0_OR_GREATER
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
            if (fieldInfo.IsNull()) return null;

            var attribute = (DescriptionAttribute)fieldInfo.GetCustomAttribute(typeof(DescriptionAttribute));

            return attribute.IsNotNull() ? attribute.Description : (returnEmpty.Equals(true) ? "" : value.ToString());
        }
#endif

        /// <summary>
        ///     Method to get enumeration value from string value
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// <typeparam name="T"></typeparam>
        /// <remarks></remarks>
        public static T GetEnumValue<T>(this string str) where T : struct, Enum, IConvertible
        {
            DomainEnsure.ThrowArgumentExceptionIfFuncIsFalse(
                () => typeof(T).IsEnum, "T must be an enumerated type!", typeof(T).Name);

            var val = ((T[])Enum.GetValues(typeof(T)))[0];
            if (str.IsPresent())
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
        public static T GetEnumValue<T>(this int intValue) where T : struct, Enum, IConvertible
        {
            DomainEnsure.ThrowArgumentExceptionIfFuncIsFalse(
                () => typeof(T).IsEnum, "T must be an enumerated type!", typeof(T).Name);

            var val = ((T[])Enum.GetValues(typeof(T)))[0];

            foreach (var enumValue in (T[])Enum.GetValues(typeof(T)))
                if (Convert.ToInt32(enumValue).Equals(intValue))
                {
                    val = enumValue;
                    break;
                }

            return val;
        }


#if NET45_OR_GREATER || NET || NETSTANDARD1_0_OR_GREATER
        /// <summary>
        ///     Get 'Name' value from 'Display' attribute
        /// </summary>
        /// <param name="value">Enum value</param>
        /// <returns></returns>
        public static string GetDisplayName(this Enum value)
        {
            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString()).FirstOrDefault();

            if (memberInfo.IsNotNull())
            {
                var attribute = (DisplayAttribute)
                    memberInfo!.GetCustomAttributes(typeof(DisplayAttribute), false)
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

            if (memberInfo.IsNotNull())
            {
                var attribute = (DisplayAttribute)
                    memberInfo!.GetCustomAttributes(typeof(DisplayAttribute), false)
                        .FirstOrDefault();
                return attribute?.Description ?? value.ToString();
            }

            return value.ToString();
        }
#endif

        /// <summary>
        ///     A T extension method that determine if we are equals.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="sourceEnumValue">The sourceEnumValue to act on.</param>
        /// <param name="compareEnumValue">The compare enum value.</param>
        /// <returns>
        ///     True if equals, false if not.
        /// </returns>
        public static bool AreEquals<T>(this T sourceEnumValue, T compareEnumValue) where T : Enum, IComparable, IFormattable
            => Equals(sourceEnumValue, compareEnumValue);

        /// <summary>
        ///     Is defined.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when one or more arguments are outside the required range.
        /// </exception>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns>
        ///     A T.
        /// </returns>
        public static T IsDefined<T>(this T value, string parameterName) where T : struct, Enum
        {
            if (Enum.IsDefined(typeof(T), value).IsFalse())
            {
                parameterName.NotAllowedEmpty(nameof(parameterName));

                DomainEnsure.ThrowException(nameof(value), ExceptionType.ArgumentOutOfRangeException);
            }

            return value;
        }
    }
}