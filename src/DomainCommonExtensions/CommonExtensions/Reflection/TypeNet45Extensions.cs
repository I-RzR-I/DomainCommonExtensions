#if NET45_OR_GREATER || NET || NETSTANDARD1_0_OR_GREATER

// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2025-08-05 17:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-08-05 17:12
// ***********************************************************************
//  <copyright file="TypeExtensions.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
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
using DomainCommonExtensions.ArraysExtensions;
using DomainCommonExtensions.DataTypeExtensions;

#endregion

namespace DomainCommonExtensions.CommonExtensions.Reflection
{
    /// -------------------------------------------------------------------------------------------------
    /// <content>
    ///     A type extensions.
    /// </content>
    /// =================================================================================================
    public static partial class TypeExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A Type extension method that gets generic arguments.
        /// </summary>
        /// <param name="type">The type to act on.</param>
        /// <returns>
        ///     An array of type.
        /// </returns>
        /// =================================================================================================
        public static Type[] GetGenericArguments(this Type type)
        {
            return (type.GetTypeInfo().IsGenericTypeDefinition
                ? type.GetTypeInfo().GenericTypeParameters
                : type.GetTypeInfo().GenericTypeArguments).NotNull();
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A Type extension method that gets a method.
        /// </summary>
        /// <param name="type">The type to act on.</param>
        /// <param name="name">The name.</param>
        /// <returns>
        ///     The method.
        /// </returns>
        /// =================================================================================================
        public static MethodInfo GetMethod(this Type type, string name)
        {
            return type.GetTypeInfo().DeclaredMethods.FirstOrDefault(m => m.Name == name);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A Type extension method that gets a members.
        /// </summary>
        /// <param name="type">The type to act on.</param>
        /// <param name="name">The name.</param>
        /// <returns>
        ///     An array of member information.
        /// </returns>
        /// =================================================================================================
        public static IEnumerable<MemberInfo> GetMembers(this Type type, string name)
        {
            return type.GetTypeInfo().DeclaredMembers.Where(m => m.Name == name).NotNull();
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A Type extension method that gets the interfaces.
        /// </summary>
        /// <param name="type">The type to act on.</param>
        /// <returns>
        ///     An array of type.
        /// </returns>
        /// =================================================================================================
        public static IEnumerable<Type> GetInterfaces(this Type type)
        {
            return type.GetTypeInfo().ImplementedInterfaces.NotNull();
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A Type extension method that query if 'type' is generic type.
        /// </summary>
        /// <param name="type">The type to act on.</param>
        /// <returns>
        ///     True if generic type, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsGenericType(this Type type)
        {
            return type.GetTypeInfo().IsGenericType;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A Type extension method that query if 'type' is value type.
        /// </summary>
        /// <param name="type">The type to act on.</param>
        /// <returns>
        ///     True if value type, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsValueType(this Type type)
        {
            return type.GetTypeInfo().IsValueType;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A Type extension method that query if 'type' is abstract.
        /// </summary>
        /// <param name="type">The type to act on.</param>
        /// <returns>
        ///     True if abstract, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsAbstract(this Type type)
        {
            return type.GetTypeInfo().IsAbstract;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A Type extension method that query if 'type' is assignable from.
        /// </summary>
        /// <param name="type">The type to act on.</param>
        /// <param name="other">The other.</param>
        /// <returns>
        ///     True if assignable from, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsAssignableFrom(this Type type, Type other)
        {
            return type.GetTypeInfo().IsAssignableFrom(other.GetTypeInfo());
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A Type extension method that query if 'type' contains generic parameters.
        /// </summary>
        /// <param name="type">The type to act on.</param>
        /// <returns>
        ///     True if it succeeds, false if it fails.
        /// </returns>
        /// =================================================================================================
        public static bool ContainsGenericParameters(this Type type)
        {
            return type.GetTypeInfo().ContainsGenericParameters;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A Type extension method that base type.
        /// </summary>
        /// <param name="type">The type to act on.</param>
        /// <returns>
        ///     A Type.
        /// </returns>
        /// =================================================================================================
        public static Type BaseType(this Type type)
        {
            return type.GetTypeInfo().BaseType;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A Type extension method that query if 'type' is generic type definition.
        /// </summary>
        /// <param name="type">The type to act on.</param>
        /// <returns>
        ///     True if generic type definition, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsGenericTypeDefinition(this Type type)
        {
            return type.GetTypeInfo().IsGenericTypeDefinition;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A Type extension method that query if 'type' is primitive.
        /// </summary>
        /// <param name="type">The type to act on.</param>
        /// <returns>
        ///     True if primitive, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsPrimitive(this Type type)
        {
            return type.GetTypeInfo().IsPrimitive;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A Type extension method that query if 'type' is nested public.
        /// </summary>
        /// <param name="type">The type to act on.</param>
        /// <returns>
        ///     True if nested public, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsNestedPublic(this Type type)
        {
            return type.GetTypeInfo().IsNestedPublic;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A Type extension method that query if 'type' is public.
        /// </summary>
        /// <param name="type">The type to act on.</param>
        /// <returns>
        ///     True if public, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsPublic(this Type type)
        {
            return type.GetTypeInfo().IsPublic;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A Type extension method that query if 'type' is sealed.
        /// </summary>
        /// <param name="type">The type to act on.</param>
        /// <returns>
        ///     True if sealed, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsSealed(this Type type)
        {
            return type.GetTypeInfo().IsSealed;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A Type extension method that gets generic parameter constraints.
        /// </summary>
        /// <param name="type">The type to act on.</param>
        /// <returns>
        ///     An array of type.
        /// </returns>
        /// =================================================================================================
        public static Type[] GetGenericParameterConstraints(this Type type)
        {
            return type.GetTypeInfo().GetGenericParameterConstraints().NotNull();
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A Type extension method that query if 'type' is class.
        /// </summary>
        /// <param name="type">The type to act on.</param>
        /// <returns>
        ///     True if class, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsClass(this Type type)
        {
            return type.GetTypeInfo().IsClass;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A Type extension method that query if 'type' is interface.
        /// </summary>
        /// <param name="type">The type to act on.</param>
        /// <returns>
        ///     True if interface, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsInterface(this Type type)
        {
            return type.GetTypeInfo().IsInterface;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A Type extension method that query if 'type' is generic parameter.
        /// </summary>
        /// <param name="type">The type to act on.</param>
        /// <returns>
        ///     True if generic parameter, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsGenericParameter(this Type type)
        {
            return type.GetTypeInfo().IsGenericParameter;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A Type extension method that gets generic parameter attributes.
        /// </summary>
        /// <param name="type">The type to act on.</param>
        /// <returns>
        ///     The generic parameter attributes.
        /// </returns>
        /// =================================================================================================
        public static GenericParameterAttributes GetGenericParameterAttributes(this Type type)
        {
            return type.GetTypeInfo().GenericParameterAttributes;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A Type extension method that gets an assembly.
        /// </summary>
        /// <param name="type">The type to act on.</param>
        /// <returns>
        ///     The assembly.
        /// </returns>
        /// =================================================================================================
        public static Assembly GetAssembly(this Type type)
        {
            return type.GetTypeInfo().Assembly;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A Type extension method that gets the constructors.
        /// </summary>
        /// <param name="type">The type to act on.</param>
        /// <param name="includeNonPublic">True to include, false to exclude the non public.</param>
        /// <returns>
        ///     An array of constructor information.
        /// </returns>
        /// =================================================================================================
        public static IEnumerable<ConstructorInfo> GetConstructors(this Type type, bool includeNonPublic)
        {
            return type.GetTypeInfo().DeclaredConstructors
                .Where(ctor => ctor.IsStatic.IsFalse() && (includeNonPublic || ctor.IsPublic.IsTrue()))
                .NotNull();
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A Type extension method that gets the constructors.
        /// </summary>
        /// <param name="type">The type to act on.</param>
        /// <returns>
        ///     An array of constructor information.
        /// </returns>
        /// =================================================================================================
        public static IEnumerable<ConstructorInfo> GetConstructors(this Type type)
        {
            return type.GetConstructors(false).NotNull();
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A Type extension method that gets a constructor.
        /// </summary>
        /// <param name="type">The type to act on.</param>
        /// <param name="types">The types.</param>
        /// <returns>
        ///     The constructor.
        /// </returns>
        /// =================================================================================================
        public static ConstructorInfo GetConstructor(this Type type, Type[] types)
        {
            return (
                    from constructor in type.GetConstructors()
                    where types.SequenceEqual(constructor.GetParameters().Select(p => p.ParameterType))
                    select constructor)
                .FirstOrDefault();
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A Type extension method that query if 'type' is in namespace.
        /// </summary>
        /// <param name="type">The type to act on.</param>
        /// <param name="nameSpace">The name space.</param>
        /// <returns>
        ///     True if in namespace, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsInNamespace(this Type type, string nameSpace)
        {
            var subNameSpace = nameSpace + ".";

            return type.Namespace.IsNotNull() &&
                   (type.Namespace!.Equals(nameSpace) || type.Namespace.StartsWith(subNameSpace));
        }
    }
}
#endif