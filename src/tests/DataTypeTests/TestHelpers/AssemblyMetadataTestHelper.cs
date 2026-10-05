using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using DataTypeTests.Models;
using RzR.Extensions.Domain.Collections;

namespace DataTypeTests.TestHelpers
{
    internal static class AssemblyMetadataTestHelper
    {
        internal const string CodeSourceAssemblyName = "RzR.Core.CodeSource";
        internal const string CodeSourceAttributeFullName = "RzR.Core.CodeSource.CodeSourceAttribute";
        internal const string EmitCodeSourceMetadataKey = "EmitCodeSource";

        internal static readonly string[] AnonymousSelectFields = { nameof(TempModel.Id), nameof(TempModel.Code) };

        private const BindingFlags DeclaredMembers = BindingFlags.DeclaredOnly | BindingFlags.Public
            | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        internal static Assembly DomainAssembly => typeof(DynamicListExtensions).Assembly;

        internal static List<TempModel> CreateAnonymousSelectSource()
            => new List<TempModel>
            {
                new TempModel { Id = 1, Code = "Code-001", Name = "Name-001" },
                new TempModel { Id = 2, Code = "Code-002", Name = "Name-002" }
            };

        internal static bool HasCodeSourceAssemblyReference()
            => ReadDomainMetadata(reader => reader.AssemblyReferences.Any(handle => IsCodeSourceAssembly(reader, handle)));

        internal static int CountCodeSourceAttributeRows()
            => ReadDomainMetadata(reader => reader.CustomAttributes
                .Select(handle => GetAttributeTypeReference(reader, reader.GetCustomAttribute(handle)))
                .Count(typeReference => typeReference.HasValue
                    && IsCodeSourceScoped(reader, typeReference.Value)
                    && string.Equals(GetFullName(reader, typeReference.Value), CodeSourceAttributeFullName, StringComparison.Ordinal)));

        internal static IReadOnlyList<string> GetCodeSourceTypeReferences()
            => ReadDomainMetadata(reader => reader.TypeReferences
                .Where(handle => IsCodeSourceScoped(reader, handle))
                .Select(handle => GetFullName(reader, handle))
                .OrderBy(name => name, StringComparer.Ordinal)
                .ToList());

        internal static AssemblyName FindCodeSourceReference()
            => DomainAssembly.GetReferencedAssemblies()
                .FirstOrDefault(name => string.Equals(name.Name, CodeSourceAssemblyName, StringComparison.OrdinalIgnoreCase));

        internal static bool IsEmitCodeSourceBuild(Assembly testAssembly)
        {
            var value = testAssembly.GetCustomAttributes<AssemblyMetadataAttribute>()
                .Where(attribute => string.Equals(attribute.Key, EmitCodeSourceMetadataKey, StringComparison.Ordinal))
                .Select(attribute => attribute.Value)
                .FirstOrDefault();

            return string.Equals(value, bool.TrueString, StringComparison.OrdinalIgnoreCase);
        }

        internal static IEnumerable<MemberInfo> GetDeclaredMembers(Type type)
            => type.GetMembers(DeclaredMembers).Where(member => member.MemberType != MemberTypes.NestedType);

        internal static void ProbeCustomAttributes(MemberInfo member)
        {
            member.GetCustomAttributes(false);
            member.GetCustomAttributes(true);
            member.GetCustomAttributes(typeof(ObsoleteAttribute), true);
            member.IsDefined(typeof(ObsoleteAttribute), true);
            member.GetCustomAttributesData();
            Attribute.GetCustomAttributes(member, true);
        }

        internal static int CountObservedCodeSourceAttributes(MemberInfo member)
            => member.GetCustomAttributesData()
                .Count(data => string.Equals(data.AttributeType.FullName, CodeSourceAttributeFullName, StringComparison.Ordinal));

        private static T ReadDomainMetadata<T>(Func<MetadataReader, T> read)
        {
            using (var stream = new FileStream(DomainAssembly.Location, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var peReader = new PEReader(stream))
                return read(peReader.GetMetadataReader());
        }

        private static TypeReferenceHandle? GetAttributeTypeReference(MetadataReader reader, CustomAttribute attribute)
        {
            if (attribute.Constructor.Kind != HandleKind.MemberReference)
                return null;

            var parent = reader.GetMemberReference((MemberReferenceHandle)attribute.Constructor).Parent;

            return parent.Kind == HandleKind.TypeReference ? (TypeReferenceHandle)parent : (TypeReferenceHandle?)null;
        }

        private static bool IsCodeSourceScoped(MetadataReader reader, TypeReferenceHandle handle)
        {
            var scope = reader.GetTypeReference(handle).ResolutionScope;
            while (scope.Kind == HandleKind.TypeReference)
                scope = reader.GetTypeReference((TypeReferenceHandle)scope).ResolutionScope;

            return scope.Kind == HandleKind.AssemblyReference && IsCodeSourceAssembly(reader, (AssemblyReferenceHandle)scope);
        }

        private static bool IsCodeSourceAssembly(MetadataReader reader, AssemblyReferenceHandle handle)
            => string.Equals(reader.GetString(reader.GetAssemblyReference(handle).Name), CodeSourceAssemblyName, StringComparison.OrdinalIgnoreCase);

        private static string GetFullName(MetadataReader reader, TypeReferenceHandle handle)
        {
            var typeReference = reader.GetTypeReference(handle);
            var name = reader.GetString(typeReference.Name);

            if (typeReference.ResolutionScope.Kind == HandleKind.TypeReference)
                return GetFullName(reader, (TypeReferenceHandle)typeReference.ResolutionScope) + "+" + name;

            var ns = reader.GetString(typeReference.Namespace);

            return string.IsNullOrEmpty(ns) ? name : ns + "." + name;
        }
    }
}
