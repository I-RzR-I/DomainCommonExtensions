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
    internal static class AnonymousClassFactory
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the current assembly version.
        /// </summary>
        /// =================================================================================================
        private static readonly string CurrentAssemblyVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) name of the anonymous assembly.
        /// </summary>
        /// =================================================================================================
        private static readonly string AnonymousAssemblyName = $"DomainCommonExtensions.Helpers.Factory.Internal.AnonymousClassFactory, Version={CurrentAssemblyVersion}";

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) name of the anonymous module.
        /// </summary>
        /// =================================================================================================
        private const string AnonymousModuleName = "DomainCommonExtensions.Helpers.Factory.Internal.AnonymousClassFactory";

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the compiler generated attribute builder.
        /// </summary>
        /// =================================================================================================
        private static readonly CustomAttributeBuilder CompilerGeneratedAttributeBuilder = new CustomAttributeBuilder(typeof(CompilerGeneratedAttribute).GetConstructor(Type.EmptyTypes)!, new object[0]);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the debugger hidden attribute builder.
        /// </summary>
        /// =================================================================================================
        private static readonly CustomAttributeBuilder DebuggerHiddenAttributeBuilder = new CustomAttributeBuilder(typeof(DebuggerHiddenAttribute).GetConstructor(Type.EmptyTypes)!, new object[0]);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the debugger browsable attribute builder.
        /// </summary>
        /// =================================================================================================
        private static readonly CustomAttributeBuilder DebuggerBrowsableAttributeBuilder = new CustomAttributeBuilder(typeof(DebuggerBrowsableAttribute).GetConstructor(new[] { typeof(DebuggerBrowsableState) })!, new object[] { DebuggerBrowsableState.Never });

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the object constructor.
        /// </summary>
        /// =================================================================================================
        private static readonly ConstructorInfo ObjectCtor = typeof(object).GetConstructor(Type.EmptyTypes)!;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the string builder constructor.
        /// </summary>
        /// =================================================================================================
        private static readonly ConstructorInfo StringBuilderCtor = typeof(StringBuilder).GetConstructor(Type.EmptyTypes)!;

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
        public static Type CreateType(IList<PropertyInfo> properties, bool createParameterCtor = true)
        {
            var key = GenerateKey(properties, createParameterCtor);

            // ReSharper disable once InconsistentlySynchronizedField
            if (!GeneratedTypes.TryGetValue(key, out var type))
                // We create only a single class at a time, through this lock.
                // Note that this is a variant of the double-checked locking.
                // It is safe because we are using a thread safe class.
                lock (GeneratedTypes)
                {
                    return GeneratedTypes.GetOrAdd(key, _ => EmitType(properties, createParameterCtor));
                }

            return type;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Emit type.
        /// </summary>
        /// <param name="properties">The properties.</param>
        /// <param name="createParameterCtor">True to create parameter constructor.</param>
        /// <returns>
        ///     A Type.
        /// </returns>
        /// =================================================================================================
        [CodeSource(SourceUrl = "https://stackoverflow.com/questions/29413942/c-sharp-anonymous-object-with-properties-from-dictionary", Version = 1.0D)]
        private static Type EmitType(IList<PropertyInfo> properties, bool createParameterCtor)
        {
            var typeIndex = Interlocked.Increment(ref _index);
            var typeName = properties.Any()
                ? $"<>f__AnonymousType{typeIndex}`{properties.Count}"
                : $"<>f__AnonymousType{typeIndex}";

            var typeBuilder = ModuleBuilder.DefineType(typeName,
                TypeAttributes.AnsiClass | TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.AutoLayout |
                TypeAttributes.BeforeFieldInit, typeof(AnonymousClass));
            typeBuilder.SetCustomAttribute(CompilerGeneratedAttributeBuilder);

            var fieldBuilders = new FieldBuilder[properties.Count];

            // There are two for-loops because we want to have all the getter methods before all the other methods
            for (var i = 0; i < properties.Count; i++)
            {
                var fieldName = properties[i].Name;
                var fieldType = properties[i].PropertyType;

                // field
                fieldBuilders[i] = typeBuilder.DefineField($"<{fieldName}>i__Field", fieldType,
                    FieldAttributes.Private | FieldAttributes.InitOnly);
                fieldBuilders[i].SetCustomAttribute(DebuggerBrowsableAttributeBuilder);

                var propertyBuilder = typeBuilder.DefineProperty(fieldName, PropertyAttributes.None,
                    CallingConventions.HasThis, fieldType, Type.EmptyTypes);

                // getter
                var getter = typeBuilder.DefineMethod($"get_{fieldName}",
                    MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName,
                    CallingConventions.HasThis, fieldType, null);
                getter.SetCustomAttribute(CompilerGeneratedAttributeBuilder);

                var ilgeneratorGetter = getter.GetILGenerator();
                ilgeneratorGetter.Emit(OpCodes.Ldarg_0);
                ilgeneratorGetter.Emit(OpCodes.Ldfld, fieldBuilders[i]);
                ilgeneratorGetter.Emit(OpCodes.Ret);
                propertyBuilder.SetGetMethod(getter);

                // setter
                var setter = typeBuilder.DefineMethod($"set_{fieldName}",
                    MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName,
                    CallingConventions.HasThis, null, new[] { fieldType });
                setter.SetCustomAttribute(CompilerGeneratedAttributeBuilder);

                // workaround for https://github.com/dotnet/corefx/issues/7792
                setter.DefineParameter(1, ParameterAttributes.In, properties[i].Name);

                var ilgeneratorSetter = setter.GetILGenerator();
                ilgeneratorSetter.Emit(OpCodes.Ldarg_0);
                ilgeneratorSetter.Emit(OpCodes.Ldarg_1);
                ilgeneratorSetter.Emit(OpCodes.Stfld, fieldBuilders[i]);
                ilgeneratorSetter.Emit(OpCodes.Ret);
                propertyBuilder.SetSetMethod(setter);
            }

            // ToString()
            var toString = typeBuilder.DefineMethod(nameof(ToString),
                MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig,
                CallingConventions.HasThis, typeof(string), Type.EmptyTypes);
            toString.SetCustomAttribute(DebuggerHiddenAttributeBuilder);

            var ilgeneratorToString = toString.GetILGenerator();
            ilgeneratorToString.DeclareLocal(typeof(StringBuilder));
            ilgeneratorToString.Emit(OpCodes.Newobj, StringBuilderCtor);
            ilgeneratorToString.Emit(OpCodes.Stloc_0);

            // Equals()
            var equals = typeBuilder.DefineMethod(nameof(Equals),
                MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig,
                CallingConventions.HasThis, typeof(bool), new[] { typeof(object) });
            equals.DefineParameter(1, ParameterAttributes.In, "value");
            equals.SetCustomAttribute(DebuggerHiddenAttributeBuilder);

            var ilgeneratorEquals = equals.GetILGenerator();
            ilgeneratorEquals.DeclareLocal(typeBuilder.AsType());
            ilgeneratorEquals.Emit(OpCodes.Ldarg_1);
            ilgeneratorEquals.Emit(OpCodes.Isinst, typeBuilder.AsType());
            ilgeneratorEquals.Emit(OpCodes.Stloc_0);
            ilgeneratorEquals.Emit(OpCodes.Ldloc_0);

            // GetHashCode()
            var getHashCode = typeBuilder.DefineMethod(nameof(GetHashCode),
                MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig,
                CallingConventions.HasThis, typeof(int), Type.EmptyTypes);
            getHashCode.SetCustomAttribute(DebuggerHiddenAttributeBuilder);

            var ilgeneratorGetHashCode = getHashCode.GetILGenerator();
            ilgeneratorGetHashCode.DeclareLocal(typeof(int));

            if (properties.Count == 0)
            {
                ilgeneratorGetHashCode.Emit(OpCodes.Ldc_I4_0);
            }
            else
            {
                // As done by Roslyn
                // Note that initHash can vary, because string.GetHashCode() isn't "stable" for different compilation of the code
                var initHash = 0;

                for (var i = 0; i < properties.Count; i++)
                    initHash = unchecked(initHash * -1521134295 + fieldBuilders[i].Name.GetHashCode());

                // Note that the CSC seems to generate a different seed for every anonymous class
                ilgeneratorGetHashCode.Emit(OpCodes.Ldc_I4, initHash);
            }

            var equalsLabel = ilgeneratorEquals.DefineLabel();

            for (var i = 0; i < properties.Count; i++)
            {
                var fieldName = properties[i].Name;
                var fieldType = properties[i].PropertyType;
                var equalityComparerT = EqualityComparer.MakeGenericType(fieldType);

                // Equals()
                var equalityComparerTDefault =
                    equalityComparerT.GetMethod("get_Default", BindingFlags.Static | BindingFlags.Public)!;
                var equalityComparerTEquals = equalityComparerT.GetMethod(nameof(EqualityComparer.Equals),
                    BindingFlags.Instance | BindingFlags.Public, null, new[] { fieldType, fieldType }, null)!;

                // Illegal one-byte branch at position: 9. Requested branch was: 143.
                // So replace OpCodes.Brfalse_S to OpCodes.Brfalse
                ilgeneratorEquals.Emit(OpCodes.Brfalse, equalsLabel);
                ilgeneratorEquals.Emit(OpCodes.Call, equalityComparerTDefault);
                ilgeneratorEquals.Emit(OpCodes.Ldarg_0);
                ilgeneratorEquals.Emit(OpCodes.Ldfld, fieldBuilders[i]);
                ilgeneratorEquals.Emit(OpCodes.Ldloc_0);
                ilgeneratorEquals.Emit(OpCodes.Ldfld, fieldBuilders[i]);
                ilgeneratorEquals.Emit(OpCodes.Callvirt, equalityComparerTEquals);

                // GetHashCode();
                var equalityComparerTGetHashCode = equalityComparerT.GetMethod(nameof(EqualityComparer.GetHashCode),
                    BindingFlags.Instance | BindingFlags.Public, null, new[] { fieldType }, null)!;
                ilgeneratorGetHashCode.Emit(OpCodes.Stloc_0);
                ilgeneratorGetHashCode.Emit(OpCodes.Ldc_I4, -1521134295);
                ilgeneratorGetHashCode.Emit(OpCodes.Ldloc_0);
                ilgeneratorGetHashCode.Emit(OpCodes.Mul);
                ilgeneratorGetHashCode.Emit(OpCodes.Call, equalityComparerTDefault);
                ilgeneratorGetHashCode.Emit(OpCodes.Ldarg_0);
                ilgeneratorGetHashCode.Emit(OpCodes.Ldfld, fieldBuilders[i]);
                ilgeneratorGetHashCode.Emit(OpCodes.Callvirt, equalityComparerTGetHashCode);
                ilgeneratorGetHashCode.Emit(OpCodes.Add);

                // ToString();
                ilgeneratorToString.Emit(OpCodes.Ldloc_0);
                ilgeneratorToString.Emit(OpCodes.Ldstr, i == 0 ? $"{{ {fieldName} = " : $", {fieldName} = ");
                ilgeneratorToString.Emit(OpCodes.Callvirt, StringBuilderAppendString);
                ilgeneratorToString.Emit(OpCodes.Pop);
                ilgeneratorToString.Emit(OpCodes.Ldloc_0);
                ilgeneratorToString.Emit(OpCodes.Ldarg_0);
                ilgeneratorToString.Emit(OpCodes.Ldfld, fieldBuilders[i]);
                ilgeneratorToString.Emit(OpCodes.Box, properties[i].PropertyType);
                ilgeneratorToString.Emit(OpCodes.Callvirt, StringBuilderAppendObject);
                ilgeneratorToString.Emit(OpCodes.Pop);
            }

            // Only create the default and with params constructor when there are any params.
            // Otherwise default constructor is not needed because it matches the default
            // one provided by the runtime when no constructor is present
            if (createParameterCtor && properties.Any())
            {
                // .ctor default
                var constructorDef = typeBuilder.DefineConstructor(MethodAttributes.Public | MethodAttributes.HideBySig,
                    CallingConventions.HasThis, Type.EmptyTypes);
                constructorDef.SetCustomAttribute(DebuggerHiddenAttributeBuilder);

                var ilgeneratorConstructorDef = constructorDef.GetILGenerator();
                ilgeneratorConstructorDef.Emit(OpCodes.Ldarg_0);
                ilgeneratorConstructorDef.Emit(OpCodes.Call, ObjectCtor);
                ilgeneratorConstructorDef.Emit(OpCodes.Ret);

                // .ctor with params
                var types = properties.Select(p => p.PropertyType).ToArray();
                var constructor = typeBuilder.DefineConstructor(MethodAttributes.Public | MethodAttributes.HideBySig,
                    CallingConventions.HasThis, types);
                constructor.SetCustomAttribute(DebuggerHiddenAttributeBuilder);

                var ilgeneratorConstructor = constructor.GetILGenerator();
                ilgeneratorConstructor.Emit(OpCodes.Ldarg_0);
                ilgeneratorConstructor.Emit(OpCodes.Call, ObjectCtor);

                for (var i = 0; i < properties.Count; i++)
                {
                    constructor.DefineParameter(i + 1, ParameterAttributes.None, properties[i].Name);
                    ilgeneratorConstructor.Emit(OpCodes.Ldarg_0);

                    if (i == 0)
                        ilgeneratorConstructor.Emit(OpCodes.Ldarg_1);
                    else if (i == 1)
                        ilgeneratorConstructor.Emit(OpCodes.Ldarg_2);
                    else if (i == 2)
                        ilgeneratorConstructor.Emit(OpCodes.Ldarg_3);
                    else if (i < 255)
                        ilgeneratorConstructor.Emit(OpCodes.Ldarg_S, (byte)(i + 1));
                    else
                        // Ldarg uses an ushort, but the Emit only accepts short, so we use a unchecked(...), cast to short and let the CLR interpret it as ushort.
                        ilgeneratorConstructor.Emit(OpCodes.Ldarg, unchecked((short)(i + 1)));

                    ilgeneratorConstructor.Emit(OpCodes.Stfld, fieldBuilders[i]);
                }

                ilgeneratorConstructor.Emit(OpCodes.Ret);
            }

            // Equals()
            if (properties.Count == 0)
            {
                ilgeneratorEquals.Emit(OpCodes.Ldnull);
                ilgeneratorEquals.Emit(OpCodes.Ceq);
                ilgeneratorEquals.Emit(OpCodes.Ldc_I4_0);
                ilgeneratorEquals.Emit(OpCodes.Ceq);
            }
            else
            {
                ilgeneratorEquals.Emit(OpCodes.Ret);
                ilgeneratorEquals.MarkLabel(equalsLabel);
                ilgeneratorEquals.Emit(OpCodes.Ldc_I4_0);
            }

            ilgeneratorEquals.Emit(OpCodes.Ret);

            // GetHashCode()
            ilgeneratorGetHashCode.Emit(OpCodes.Stloc_0);
            ilgeneratorGetHashCode.Emit(OpCodes.Ldloc_0);
            ilgeneratorGetHashCode.Emit(OpCodes.Ret);

            // ToString()
            ilgeneratorToString.Emit(OpCodes.Ldloc_0);
            ilgeneratorToString.Emit(OpCodes.Ldstr, properties.Count == 0 ? "{ }" : " }");
            ilgeneratorToString.Emit(OpCodes.Callvirt, StringBuilderAppendString);
            ilgeneratorToString.Emit(OpCodes.Pop);
            ilgeneratorToString.Emit(OpCodes.Ldloc_0);
            ilgeneratorToString.Emit(OpCodes.Callvirt, ObjectToString);
            ilgeneratorToString.Emit(OpCodes.Ret);

            EmitEqualityOperators(typeBuilder, equals);

            return typeBuilder.CreateType();
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Emit equality operators.
        /// </summary>
        /// <param name="typeBuilder">The type builder.</param>
        /// <param name="equals">The equals.</param>
        /// =================================================================================================
        private static void EmitEqualityOperators(TypeBuilder typeBuilder, MethodBuilder equals)
        {
            // Define the '==' operator
            var equalityOperator = typeBuilder.DefineMethod(
                "op_Equality",
                MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.SpecialName |
                MethodAttributes.HideBySig,
                typeof(bool),
                new[] { typeBuilder.AsType(), typeBuilder.AsType() });

            var ilgeneratorEqualityOperator = equalityOperator.GetILGenerator();

            // if (left == null || right == null) return ReferenceEquals(left, right);
            var endLabel = ilgeneratorEqualityOperator.DefineLabel();
            ilgeneratorEqualityOperator.Emit(OpCodes.Ldarg_0);
            ilgeneratorEqualityOperator.Emit(OpCodes.Brfalse_S, endLabel);
            ilgeneratorEqualityOperator.Emit(OpCodes.Ldarg_1);
            ilgeneratorEqualityOperator.Emit(OpCodes.Brfalse_S, endLabel);

            // return left.Equals(right);
            ilgeneratorEqualityOperator.Emit(OpCodes.Ldarg_0);
            ilgeneratorEqualityOperator.Emit(OpCodes.Ldarg_1);
            ilgeneratorEqualityOperator.Emit(OpCodes.Callvirt, equals);
            ilgeneratorEqualityOperator.Emit(OpCodes.Ret);

            // Return false if one is null
            ilgeneratorEqualityOperator.MarkLabel(endLabel);
            ilgeneratorEqualityOperator.Emit(OpCodes.Ldarg_0);
            ilgeneratorEqualityOperator.Emit(OpCodes.Ldarg_1);
            ilgeneratorEqualityOperator.Emit(OpCodes.Call, typeof(object).GetMethod("ReferenceEquals")!);
            ilgeneratorEqualityOperator.Emit(OpCodes.Ret);

            // Define the '!=' operator
            var inequalityOperator = typeBuilder.DefineMethod(
                "op_Inequality",
                MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.SpecialName |
                MethodAttributes.HideBySig,
                typeof(bool),
                new[] { typeBuilder.AsType(), typeBuilder.AsType() });

            var ilNeq = inequalityOperator.GetILGenerator();

            // return !(left == right);
            ilNeq.Emit(OpCodes.Ldarg_0);
            ilNeq.Emit(OpCodes.Ldarg_1);
            ilNeq.Emit(OpCodes.Call, equalityOperator);
            ilNeq.Emit(OpCodes.Ldc_I4_0);
            ilNeq.Emit(OpCodes.Ceq);
            ilNeq.Emit(OpCodes.Ret);
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
        private static string GenerateKey(IEnumerable<PropertyInfo> dynamicProperties, bool createParameterCtor)
        => $"{string.Join("|", dynamicProperties.Select(p => p.Name.EscapeBackSlash() + "~" + p.PropertyType).ToArray())}_{(createParameterCtor ? "c" : string.Empty)}";
    }
}