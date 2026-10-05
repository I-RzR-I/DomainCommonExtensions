using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RzR.Extensions.Domain.Collections;
using RzR.Extensions.Domain.Internal.AnonymousSelect.Base;

using static DataTypeTests.TestHelpers.AssemblyMetadataTestHelper;

namespace DataTypeTests.AssemblyTests
{
    [TestClass]
    public class CodeSourceEmissionTests
    {
        [TestMethod]
        public void DomainAssembly_CodeSourceReference_ExistsOnlyWithAttributeRowsAndOnlyForCodeSourceAttribute()
        {
            var hasReference = HasCodeSourceAssemblyReference();
            var rows = CountCodeSourceAttributeRows();
            var typeReferences = GetCodeSourceTypeReferences();

            Assert.AreEqual(hasReference, rows > 0, $"AssemblyRef={hasReference}, attribute rows={rows}");

            if (hasReference)
                CollectionAssert.AreEqual(new[] { CodeSourceAttributeFullName }, typeReferences.ToArray(), string.Join(", ", typeReferences));
            else
                Assert.AreEqual(0, typeReferences.Count, string.Join(", ", typeReferences));
        }

        [TestMethod]
        public void DomainAssembly_CodeSourceReference_IsLoadableWhenReferenced()
        {
            var reference = FindCodeSourceReference();
            if (reference == null)
            {
                Assert.IsFalse(IsEmitCodeSourceBuild(typeof(CodeSourceEmissionTests).Assembly));
                return;
            }

            var loaded = Assembly.Load(reference);

            Assert.AreEqual(CodeSourceAssemblyName, loaded.GetName().Name);
        }

        [TestMethod]
        public void DomainAssembly_AllTypesAndDeclaredMembers_SurviveCustomAttributeLookups()
        {
            var types = DomainAssembly.GetTypes();
            var observedCodeSourceAttributes = 0;

            foreach (var type in types)
            {
                ProbeCustomAttributes(type);
                observedCodeSourceAttributes += CountObservedCodeSourceAttributes(type);

                foreach (var member in GetDeclaredMembers(type))
                {
                    ProbeCustomAttributes(member);
                    observedCodeSourceAttributes += CountObservedCodeSourceAttributes(member);
                }
            }

            Assert.IsTrue(types.Length > 0);
            Assert.AreEqual(CountCodeSourceAttributeRows(), observedCodeSourceAttributes);
        }

        [TestMethod]
        public void EmittedAnonymousType_InheritedAttributeLookups_Succeed()
        {
            var selected = CreateAnonymousSelectSource().ParseEnumerableOfTInDynamic(AnonymousSelectFields);
            var emittedType = ((object)selected.First()).GetType();

            Assert.IsTrue(emittedType.IsSubclassOf(typeof(AnonymousClass)), emittedType.FullName);
            Assert.AreNotEqual(DomainAssembly, emittedType.Assembly);

            ProbeCustomAttributes(emittedType);
            foreach (var property in emittedType.GetProperties())
                ProbeCustomAttributes(property);
        }

        [TestMethod]
        public void ParseListOfTInDynamic_NewtonsoftSerialization_Succeeds()
        {
            var selected = CreateAnonymousSelectSource().ParseListOfTInDynamic(AnonymousSelectFields);

            Assert.IsTrue(((object)selected.First()).GetType().IsSubclassOf(typeof(AnonymousClass)));

            var json = JsonConvert.SerializeObject(selected);

            StringAssert.Contains(json, "Code-001");
            StringAssert.Contains(json, "Code-002");
        }

        [TestMethod]
        public void DomainAssembly_CodeSourceAttributeRows_ExistOnlyInEmitCodeSourceBuild()
        {
            var emitMode = IsEmitCodeSourceBuild(typeof(CodeSourceEmissionTests).Assembly);
            var rows = CountCodeSourceAttributeRows();

            Assert.AreEqual(emitMode, rows > 0, $"EmitCodeSource={emitMode}, attribute rows={rows}");
        }
    }
}
