// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2025-06-27 16:55
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-06-27 20:33
// ***********************************************************************
//  <copyright file="AnonymousGeneratorHelper.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using DomainCommonExtensions.CommonExtensions;
using DomainCommonExtensions.Models;

#endregion

namespace DomainCommonExtensions.Helpers.Internal.AnonymousSelect.Factory
{
    /// -------------------------------------------------------------------------------------------------
    /// <content>
    ///     The anonymous class factory.
    /// </content>
    /// =================================================================================================
    internal static partial class AnonymousClassFactory
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Emit equality operators.
        /// </summary>
        /// <param name="typeBuilder">[in,out] The type builder.</param>
        /// <param name="equals">[in,out] The equals.</param>
        /// =================================================================================================
        private static void EmitEqualityOperators(ref TypeBuilder typeBuilder, ref MethodBuilder equals)
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
        ///     Generates an il generator to string.
        /// </summary>
        /// <param name="typeBuilder">[in,out] The type builder.</param>
        /// <returns>
        ///     The il generator to string.
        /// </returns>
        /// =================================================================================================
        private static GenerateILGeneratorModel GenerateIlGeneratorToString(ref TypeBuilder typeBuilder)
        {
            // ToString()
            var toString = typeBuilder.DefineMethod(nameof(ToString),
                MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig,
                CallingConventions.HasThis, typeof(string), Type.EmptyTypes);
            toString.SetCustomAttribute(DebuggerHiddenAttributeBuilder);

            var ilgeneratorToString = toString.GetILGenerator();
            ilgeneratorToString.DeclareLocal(typeof(StringBuilder));
            ilgeneratorToString.Emit(OpCodes.Newobj, StringBuilderCtor);
            ilgeneratorToString.Emit(OpCodes.Stloc_0);

            return new GenerateILGeneratorModel(ilgeneratorToString, toString);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Generates an il generator equals.
        /// </summary>
        /// <param name="typeBuilder">[in,out] The type builder.</param>
        /// <returns>
        ///     The il generator equals.
        /// </returns>
        /// =================================================================================================
        private static GenerateILGeneratorModel GenerateIlGeneratorEquals(ref TypeBuilder typeBuilder)
        {
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

            return new GenerateILGeneratorModel(ilgeneratorEquals, equals);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Generates an il generator get hash code.
        /// </summary>
        /// <param name="typeBuilder">[in,out] The type builder.</param>
        /// <param name="fieldBuilders">[in,out] The field builders.</param>
        /// <param name="propCount">Number of properties.</param>
        /// <returns>
        ///     The il generator get hash code.
        /// </returns>
        /// =================================================================================================
        private static GenerateILGeneratorModel GenerateIlGeneratorGetHashCode(
            ref TypeBuilder typeBuilder,
            ref FieldBuilder[] fieldBuilders,
            int propCount)
        {
            // GetHashCode()
            var getHashCode = typeBuilder.DefineMethod(nameof(GetHashCode),
                MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig,
                CallingConventions.HasThis, typeof(int), Type.EmptyTypes);
            getHashCode.SetCustomAttribute(DebuggerHiddenAttributeBuilder);

            var ilgeneratorGetHashCode = getHashCode.GetILGenerator();
            ilgeneratorGetHashCode.DeclareLocal(typeof(int));

            if (propCount == 0)
            {
                ilgeneratorGetHashCode.Emit(OpCodes.Ldc_I4_0);
            }
            else
            {
                // As done by Roslyn
                // Note that initHash can vary, because string.GetHashCode() isn't "stable" for different compilation of the code
                var initHash = 0;

                for (var i = 0; i < propCount; i++)
                    initHash = unchecked(initHash * -1521134295 + fieldBuilders[i].Name.GetHashCode());

                // Note that the CSC seems to generate a different seed for every anonymous class
                ilgeneratorGetHashCode.Emit(OpCodes.Ldc_I4, initHash);
            }

            return new GenerateILGeneratorModel(ilgeneratorGetHashCode, getHashCode);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Generates a constructor.
        /// </summary>
        /// <param name="typeBuilder">[in,out] The type builder.</param>
        /// <param name="fieldBuilders">[in,out] The field builders.</param>
        /// <param name="creteCtor">True to crete constructor.</param>
        /// <param name="fields">The fields.</param>
        /// =================================================================================================
        private static void GenerateConstructor(
            ref TypeBuilder typeBuilder,
            ref FieldBuilder[] fieldBuilders,
            bool creteCtor,
            AnonymousFieldGeneratorModel[] fields)
        {
            // Only create the default and with params constructor when there are any params.
            // Otherwise default constructor is not needed because it matches the default
            // one provided by the runtime when no constructor is present
            if (creteCtor && fields.Any())
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
                var types = fields.Select(x => x.FieldType).ToArray();
                var constructor = typeBuilder.DefineConstructor(MethodAttributes.Public | MethodAttributes.HideBySig,
                    CallingConventions.HasThis, types);
                constructor.SetCustomAttribute(DebuggerHiddenAttributeBuilder);

                var ilgeneratorConstructor = constructor.GetILGenerator();
                ilgeneratorConstructor.Emit(OpCodes.Ldarg_0);
                ilgeneratorConstructor.Emit(OpCodes.Call, ObjectCtor);

                for (var i = 0; i < fields.Length; i++)
                {
                    constructor.DefineParameter(i + 1, ParameterAttributes.None, fields[i].FieldName);
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
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Builds field get and set methods.
        /// </summary>
        /// <param name="typeBuilder">[in,out] The type builder.</param>
        /// <param name="fieldBuilder">[in,out] The field builder.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldType">Type of the field.</param>
        /// =================================================================================================
        private static void BuildFieldGetAndSetMethods(
            ref TypeBuilder typeBuilder,
            ref FieldBuilder fieldBuilder,
            string fieldName,
            Type fieldType)
        {
            var propertyBuilder = typeBuilder.DefineProperty(fieldName, PropertyAttributes.None,
                CallingConventions.HasThis, fieldType, Type.EmptyTypes);

            // getter
            var getter = typeBuilder.DefineMethod($"get_{fieldName}",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName,
                CallingConventions.HasThis, fieldType, null);
            getter.SetCustomAttribute(CompilerGeneratedAttributeBuilder);

            var ilgeneratorGetter = getter.GetILGenerator();
            ilgeneratorGetter.Emit(OpCodes.Ldarg_0);
            ilgeneratorGetter.Emit(OpCodes.Ldfld, fieldBuilder);
            ilgeneratorGetter.Emit(OpCodes.Ret);
            propertyBuilder.SetGetMethod(getter);

            // setter
            var setter = typeBuilder.DefineMethod($"set_{fieldName}",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName,
                CallingConventions.HasThis, null, new[] { fieldType });
            setter.SetCustomAttribute(CompilerGeneratedAttributeBuilder);

            // workaround for https://github.com/dotnet/corefx/issues/7792
            setter.DefineParameter(1, ParameterAttributes.In, fieldName);

            var ilgeneratorSetter = setter.GetILGenerator();
            ilgeneratorSetter.Emit(OpCodes.Ldarg_0);
            ilgeneratorSetter.Emit(OpCodes.Ldarg_1);
            ilgeneratorSetter.Emit(OpCodes.Stfld, fieldBuilder);
            ilgeneratorSetter.Emit(OpCodes.Ret);
            propertyBuilder.SetSetMethod(setter);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Emit il generator equals.
        /// </summary>
        /// <param name="equals">The equals.</param>
        /// <param name="equalsLabel">The equals label.</param>
        /// <param name="equalityComparerTDefault">The equality comparer t default.</param>
        /// <param name="equalityComparerTEquals">The equality comparer t equals.</param>
        /// <param name="fieldBuilder">[in,out] The field builder.</param>
        /// =================================================================================================
        private static void EmitIlGeneratorEquals(
            ILGenerator equals,
            Label equalsLabel,
            MethodInfo equalityComparerTDefault,
            MethodInfo equalityComparerTEquals,
            ref FieldBuilder fieldBuilder)
        {
            // Illegal one-byte branch at position: 9. Requested branch was: 143.
            // So replace OpCodes.Brfalse_S to OpCodes.Brfalse
            equals.Emit(OpCodes.Brfalse, equalsLabel);
            equals.Emit(OpCodes.Call, equalityComparerTDefault);
            equals.Emit(OpCodes.Ldarg_0);
            equals.Emit(OpCodes.Ldfld, fieldBuilder);
            equals.Emit(OpCodes.Ldloc_0);
            equals.Emit(OpCodes.Ldfld, fieldBuilder);
            equals.Emit(OpCodes.Callvirt, equalityComparerTEquals);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Emit il generator get hash code.
        /// </summary>
        /// <param name="getHashCode">The get hash code.</param>
        /// <param name="equalityComparerTDefault">The equality comparer t default.</param>
        /// <param name="equalityComparerT">The equality comparer t.</param>
        /// <param name="fieldType">Type of the field.</param>
        /// <param name="fieldBuilder">[in,out] The field builder.</param>
        /// =================================================================================================
        private static void EmitIlGeneratorGetHashCode(
            ILGenerator getHashCode,
            MethodInfo equalityComparerTDefault,
            Type equalityComparerT,
            Type fieldType,
            ref FieldBuilder fieldBuilder)
        {
            // GetHashCode();
            var equalityComparerTGetHashCode = equalityComparerT.GetMethod(nameof(EqualityComparer.GetHashCode),
                BindingFlags.Instance | BindingFlags.Public, null, new[] { fieldType }, null)!;
            getHashCode.Emit(OpCodes.Stloc_0);
            getHashCode.Emit(OpCodes.Ldc_I4, -1521134295);
            getHashCode.Emit(OpCodes.Ldloc_0);
            getHashCode.Emit(OpCodes.Mul);
            getHashCode.Emit(OpCodes.Call, equalityComparerTDefault);
            getHashCode.Emit(OpCodes.Ldarg_0);
            getHashCode.Emit(OpCodes.Ldfld, fieldBuilder);
            getHashCode.Emit(OpCodes.Callvirt, equalityComparerTGetHashCode);
            getHashCode.Emit(OpCodes.Add);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Emit il generator to string.
        /// </summary>
        /// <param name="toString">to string.</param>
        /// <param name="fieldType">Type of the field.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="currentIndex">The current index.</param>
        /// <param name="fieldBuilder">[in,out] The field builder.</param>
        /// =================================================================================================
        private static void EmitIlGeneratorToString(
            ILGenerator toString,
            Type fieldType,
            string fieldName,
            int currentIndex,
            ref FieldBuilder fieldBuilder)
        {
            // ToString();
            toString.Emit(OpCodes.Ldloc_0);
            toString.Emit(OpCodes.Ldstr, currentIndex == 0 ? $"{{ {fieldName} = " : $", {fieldName} = ");
            toString.Emit(OpCodes.Callvirt, StringBuilderAppendString);
            toString.Emit(OpCodes.Pop);
            toString.Emit(OpCodes.Ldloc_0);
            toString.Emit(OpCodes.Ldarg_0);
            toString.Emit(OpCodes.Ldfld, fieldBuilder);
            toString.Emit(OpCodes.Box, fieldType);
            toString.Emit(OpCodes.Callvirt, StringBuilderAppendObject);
            toString.Emit(OpCodes.Pop);
        }
    }
}