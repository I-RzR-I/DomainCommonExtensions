// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2025-01-08 13:29
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-01-08 17:59
// ***********************************************************************
//  <copyright file="AnonymousClassFactory.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using CodeSource;
using DomainCommonExtensions.CommonExtensions;
using DomainCommonExtensions.DataTypeExtensions;
using DomainCommonExtensions.Helpers.Internal.AnonymousSelect.Base;
using DomainCommonExtensions.Models;

// ReSharper disable RedundantExplicitArrayCreation
// ReSharper disable UseArrayEmptyMethod
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

#endregion

namespace DomainCommonExtensions.Helpers.Internal.AnonymousSelect.Factory
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     The anonymous class factory.
    /// </summary>
    /// =================================================================================================
    [CodeSource(SourceUrl = "https://stackoverflow.com/questions/29413942/c-sharp-anonymous-object-with-properties-from-dictionary", Version = 1.0D, Comment = "Access the source URL from more info.")]
    [CodeSource(SourceUrl = "https://stackoverflow.com/questions/606104/how-to-create-linq-expression-tree-to-select-an-anonymous-type", Version = 1.0D, Comment = "Access the source URL from more info.")]
    internal static partial class AnonymousClassFactory
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the current assembly version.
        /// </summary>
        /// =================================================================================================
        private static readonly string CurrentAssemblyVersion 
            = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) name of the anonymous assembly.
        /// </summary>
        /// =================================================================================================
        private static readonly string AnonymousAssemblyName 
            = $"DomainCommonExtensions.Helpers.Factory.Internal.AnonymousClassFactory, Version={CurrentAssemblyVersion}";

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) name of the anonymous module.
        /// </summary>
        /// =================================================================================================
        private const string AnonymousModuleName 
            = "DomainCommonExtensions.Helpers.Factory.Internal.AnonymousClassFactory";

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the compiler generated attribute builder.
        /// </summary>
        /// =================================================================================================
        private static readonly CustomAttributeBuilder CompilerGeneratedAttributeBuilder 
            = new CustomAttributeBuilder(typeof(CompilerGeneratedAttribute).GetConstructor(Type.EmptyTypes)!, new object[0]);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the debugger hidden attribute builder.
        /// </summary>
        /// =================================================================================================
        private static readonly CustomAttributeBuilder DebuggerHiddenAttributeBuilder 
            = new CustomAttributeBuilder(typeof(DebuggerHiddenAttribute).GetConstructor(Type.EmptyTypes)!, new object[0]);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the debugger browsable attribute builder.
        /// </summary>
        /// =================================================================================================
        private static readonly CustomAttributeBuilder DebuggerBrowsableAttributeBuilder
            = new CustomAttributeBuilder(typeof(DebuggerBrowsableAttribute).GetConstructor(new[] { typeof(DebuggerBrowsableState) })!, new object[] { DebuggerBrowsableState.Never });

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the object constructor.
        /// </summary>
        /// =================================================================================================
        private static readonly ConstructorInfo ObjectCtor 
            = typeof(object).GetConstructor(Type.EmptyTypes)!;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the string builder constructor.
        /// </summary>
        /// =================================================================================================
        private static readonly ConstructorInfo StringBuilderCtor 
            = typeof(StringBuilder).GetConstructor(Type.EmptyTypes)!;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the equality comparer.
        /// </summary>
        /// =================================================================================================
        private static readonly Type EqualityComparer = typeof(EqualityComparer<>);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) list of types of the generated.
        /// </summary>
        /// =================================================================================================
        private static readonly ConcurrentDictionary<string, Type> GeneratedTypes = new ConcurrentDictionary<string, Type>();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the module builder.
        /// </summary>
        /// =================================================================================================
        private static readonly ModuleBuilder ModuleBuilder;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Zero-based index of the.
        /// </summary>
        /// =================================================================================================
        private static int _index = -1;

#if UAP10_0 || NETSTANDARD
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the object to string.
        /// </summary>
        /// =================================================================================================
        private static readonly MethodInfo ObjectToString = typeof(object).GetMethod(nameof(ToString), BindingFlags.Instance | BindingFlags.Public)!;
#else
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the object to string.
        /// </summary>
        /// =================================================================================================
        private static readonly MethodInfo ObjectToString = typeof(object).GetMethod(nameof(ToString), BindingFlags.Instance | BindingFlags.Public, null, Type.EmptyTypes, null)!;
#endif

#if UAP10_0 || NETSTANDARD
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the string builder append string.
        /// </summary>
        /// =================================================================================================
        private static readonly MethodInfo StringBuilderAppendString = typeof(StringBuilder).GetMethod(nameof(StringBuilder.Append), new[] { typeof(string) })!;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the string builder append object.
        /// </summary>
        /// =================================================================================================
        private static readonly MethodInfo StringBuilderAppendObject = typeof(StringBuilder).GetMethod(nameof(StringBuilder.Append), new[] { typeof(object) })!;
#else
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the string builder append string.
        /// </summary>
        /// =================================================================================================
        private static readonly MethodInfo StringBuilderAppendString = typeof(StringBuilder).GetMethod(nameof(StringBuilder.Append), BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(string) }, null)!;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the string builder append object.
        /// </summary>
        /// =================================================================================================
        private static readonly MethodInfo StringBuilderAppendObject = typeof(StringBuilder).GetMethod(nameof(StringBuilder.Append), BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(object) }, null)!;
#endif

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes static members of the <see cref="AnonymousClassFactory"/> class.
        /// </summary>
        /// =================================================================================================
        static AnonymousClassFactory()
        {
            var assemblyName = new AssemblyName(AnonymousAssemblyName);
            var assemblyBuilder = AssemblyBuilderFactory.DefineDynamicAssembly
            (
                assemblyName,
#if NET35
            AssemblyBuilderAccess.Run
#else
                AssemblyBuilderAccess.RunAndCollect
#endif
            );

            ModuleBuilder = assemblyBuilder.DefineDynamicModule(AnonymousModuleName);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Creates a type.
        /// </summary>
        /// <param name="properties">The properties.</param>
        /// <param name="createParameterCtor">(Optional) True to create parameter constructor.</param>
        /// <returns>
        ///     The new type.
        /// </returns>
        /// =================================================================================================
        public static Type CreateType(IEnumerable<PropertyInfo> properties, bool createParameterCtor = true)
        {
            var fields = properties.Select(x => new AnonymousFieldGeneratorModel(x.PropertyType, x.Name)).ToArray();
            var key = GenerateKey(fields, createParameterCtor);

            // ReSharper disable once InconsistentlySynchronizedField
            if (!GeneratedTypes.TryGetValue(key, out var type))
                // We create only a single class at a time, through this lock.
                // Note that this is a variant of the double-checked locking.
                // It is safe because we are using a thread safe class.
            {
                lock (GeneratedTypes)
                {
                    return GeneratedTypes.GetOrAdd(key, _ => EmitType(fields, createParameterCtor));
                }
            }

            return type;
        }

        public static Type CreateType(Type[] sourceTypes, string[] sourceNames, bool createParameterCtor = true)
        {
            var fields = sourceNames.Select((t, i) => new AnonymousFieldGeneratorModel(sourceTypes[i], t)).ToArray();
            var key = GenerateKey(fields, createParameterCtor);

            // ReSharper disable once InconsistentlySynchronizedField
            if (!GeneratedTypes.TryGetValue(key, out var type))
                // We create only a single class at a time, through this lock.
                // Note that this is a variant of the double-checked locking.
                // It is safe because we are using a thread safe class.
            {
                lock (GeneratedTypes)
                {
                    return GeneratedTypes.GetOrAdd(key, _ => EmitType(fields, createParameterCtor));
                }
            }

            return type;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Emit type.
        /// </summary>
        /// <param name="fields">The properties.</param>
        /// <param name="createParameterCtor">True to create parameter constructor.</param>
        /// <returns>
        ///     A Type.
        /// </returns>
        /// =================================================================================================
        [CodeSource(SourceUrl = "https://stackoverflow.com/questions/29413942/c-sharp-anonymous-object-with-properties-from-dictionary", Version = 1.0D)]
        private static Type EmitType(AnonymousFieldGeneratorModel[] fields, bool createParameterCtor)
        {
            var fieldCount = fields.Count();
            var typeIndex = Interlocked.Increment(ref _index);
            var typeName = fields.Any()
                ? $"<>f__AnonymousType{typeIndex}`{fieldCount}"
                : $"<>f__AnonymousType{typeIndex}";

            var typeBuilder = ModuleBuilder.DefineType(typeName,
                TypeAttributes.AnsiClass | TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.AutoLayout |
                TypeAttributes.BeforeFieldInit, typeof(AnonymousClass));
            typeBuilder.SetCustomAttribute(CompilerGeneratedAttributeBuilder);

            var fieldBuilders = new FieldBuilder[fieldCount];

            // There are two for-loops because we want to have all the getter methods before all the other methods
            for (var i = 0; i < fieldCount; i++)
            {
                var fieldName = fields[i].FieldName;
                var fieldType = fields[i].FieldType;

                // field
                fieldBuilders[i] = typeBuilder.DefineField($"<{fieldName}>i__Field", fieldType,
                    FieldAttributes.Private | FieldAttributes.InitOnly);
                fieldBuilders[i].SetCustomAttribute(DebuggerBrowsableAttributeBuilder);

                BuildFieldGetAndSetMethods(ref typeBuilder, ref fieldBuilders[i], fieldName, fieldType);
            }

            // ToString()
            var toString = GenerateIlGeneratorToString(ref typeBuilder);

            // Equals()
            var equals = GenerateIlGeneratorEquals(ref typeBuilder);

            // GetHashCode()
            var getHashCode = GenerateIlGeneratorGetHashCode(ref typeBuilder, ref fieldBuilders, fieldCount);

            var equalsLabel = equals.IlGenerator.DefineLabel();

            for (var i = 0; i < fieldCount; i++)
            {
                var fieldName = fields[i].FieldName;
                var fieldType = fields[i].FieldType;
                var equalityComparerT = EqualityComparer.MakeGenericType(fieldType);

                // Equals()
                var equalityComparerTDefault =
                    equalityComparerT.GetMethod("get_Default", BindingFlags.Static | BindingFlags.Public)!;
                var equalityComparerTEquals = equalityComparerT.GetMethod(nameof(EqualityComparer.Equals),
                    BindingFlags.Instance | BindingFlags.Public, null, new Type[] { fieldType, fieldType }, null)!;

                EmitIlGeneratorEquals(equals.IlGenerator, equalsLabel, equalityComparerTDefault, equalityComparerTEquals,
                    ref fieldBuilders[i]);

                // GetHashCode();
                EmitIlGeneratorGetHashCode(getHashCode.IlGenerator, equalityComparerTDefault, equalityComparerT,
                    fieldType, ref fieldBuilders[i]);

                // ToString();
                EmitIlGeneratorToString(toString.IlGenerator, fieldType, fieldName, i, ref fieldBuilders[i]);
            }

            // Only create the default and with params constructor when there are any params.
            // Otherwise default constructor is not needed because it matches the default
            // one provided by the runtime when no constructor is present
            if (createParameterCtor.IsTrue() && fields.Any())
                GenerateConstructor(ref typeBuilder, ref fieldBuilders, createParameterCtor, fields);

            // Equals()
            if (fieldCount == 0)
            {
                equals.IlGenerator.Emit(OpCodes.Ldnull);
                equals.IlGenerator.Emit(OpCodes.Ceq);
                equals.IlGenerator.Emit(OpCodes.Ldc_I4_0);
                equals.IlGenerator.Emit(OpCodes.Ceq);
            }
            else
            {
                equals.IlGenerator.Emit(OpCodes.Ret);
                equals.IlGenerator.MarkLabel(equalsLabel);
                equals.IlGenerator.Emit(OpCodes.Ldc_I4_0);
            }

            equals.IlGenerator.Emit(OpCodes.Ret);

            // GetHashCode()
            getHashCode.IlGenerator.Emit(OpCodes.Stloc_0);
            getHashCode.IlGenerator.Emit(OpCodes.Ldloc_0);
            getHashCode.IlGenerator.Emit(OpCodes.Ret);

            // ToString()
            toString.IlGenerator.Emit(OpCodes.Ldloc_0);
            toString.IlGenerator.Emit(OpCodes.Ldstr, fieldCount == 0 ? "{ }" : " }");
            toString.IlGenerator.Emit(OpCodes.Callvirt, StringBuilderAppendString);
            toString.IlGenerator.Emit(OpCodes.Pop);
            toString.IlGenerator.Emit(OpCodes.Ldloc_0);
            toString.IlGenerator.Emit(OpCodes.Callvirt, ObjectToString);
            toString.IlGenerator.Emit(OpCodes.Ret);

            var equalsMethodBuilder = equals.MethodBuilder;
            EmitEqualityOperators(ref typeBuilder, ref equalsMethodBuilder);

            return typeBuilder.CreateType();

        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Generates a key.
        /// </summary>
        /// <param name="dynamicProperties">The dynamic properties.</param>
        /// <param name="createParameterCtor">True to create parameter constructor.</param>
        /// <returns>
        ///     The key.
        /// </returns>
        /// =================================================================================================
        private static string GenerateKey(IEnumerable<AnonymousFieldGeneratorModel> dynamicProperties, bool createParameterCtor)
            => $"{string.Join("|", dynamicProperties.Select(p => p.FieldName.EscapeBackSlash() + "~" + p.FieldType).ToArray())}_{(createParameterCtor ? "c" : string.Empty)}";
    }
}