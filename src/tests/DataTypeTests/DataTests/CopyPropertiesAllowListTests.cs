using System;
using System.Collections.Generic;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Reflection;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class CopyPropertiesAllowListTests
    {
        [TestMethod]
        public void CopyProperties_DefaultOverload_Test()
        {
            var source = new CopyPlainTestModel { Id = 10, Name = "abc" };
            var destination = new CopyPlainTestModel { Id = 1, Name = "old" };

            source.CopyProperties(destination);

            Assert.AreEqual(10, destination.Id);
            Assert.AreEqual("abc", destination.Name);
        }

        [TestMethod]
        public void CopyProperties_AllowedSubset_Test()
        {
            var source = new CopyPlainTestModel { Id = 10, Name = "abc" };
            var destination = new CopyPlainTestModel { Id = 1, Name = "old" };

            source.CopyProperties(destination, new[] { "Name" });

            Assert.AreEqual("abc", destination.Name);
            Assert.AreEqual(1, destination.Id);
        }

        [TestMethod]
        public void CopyProperties_AllAllowed_Test()
        {
            var source = new CopyPlainTestModel { Id = 10, Name = "abc" };
            var destination = new CopyPlainTestModel { Id = 1, Name = "old" };

            source.CopyProperties(destination, new List<string> { "Id", "Name" });

            Assert.AreEqual(10, destination.Id);
            Assert.AreEqual("abc", destination.Name);
        }

        [TestMethod]
        public void CopyProperties_AllowedDifferentTypes_Test()
        {
            var source = new CopyPlainTestModel { Id = 10, Name = "abc" };
            var destination = new CopySecretDestinationTestModel { Secret = 4, Name = "old" };

            source.CopyProperties(destination, new[] { "Name" });

            Assert.AreEqual("abc", destination.Name);
            Assert.AreEqual(4, destination.Secret);
        }

        [TestMethod]
        public void CopyProperties_EmptyAllowList_Test()
        {
            var source = new CopyPlainTestModel { Id = 10, Name = "abc" };
            var destination = new CopyPlainTestModel { Id = 1, Name = "old" };

            source.CopyProperties(destination, new string[0]);

            Assert.AreEqual(1, destination.Id);
            Assert.AreEqual("old", destination.Name);
        }

        [TestMethod]
        public void CopyProperties_NullAllowList_Test()
        {
            var source = new CopyPlainTestModel { Id = 10, Name = "abc" };
            var destination = new CopyPlainTestModel { Id = 1, Name = "old" };
            IEnumerable<string> allowed = null;

            Assert.ThrowsException<ArgumentNullException>(() => source.CopyProperties(destination, allowed));

            Assert.AreEqual(1, destination.Id);
            Assert.AreEqual("old", destination.Name);
        }

        [TestMethod]
        public void CopyProperties_AllowListNullSource_Test()
        {
            object source = null;

            Assert.ThrowsException<ArgumentNullException>(
                () => source.CopyProperties(new CopyPlainTestModel(), new[] { "Name" }));
        }

        [TestMethod]
        public void CopyProperties_AllowListNullDestination_Test()
        {
            var source = new CopyPlainTestModel { Id = 10, Name = "abc" };

            Assert.ThrowsException<ArgumentNullException>(() => source.CopyProperties(null, new[] { "Name" }));
        }

        [TestMethod]
        public void CopyProperties_DuplicateAllowedName_Test()
        {
            var source = new CopyPlainTestModel { Id = 10, Name = "abc" };
            var destination = new CopyCountingDestinationTestModel { Id = 1 };

            source.CopyProperties(destination, new[] { "Name", "Name" });

            Assert.AreEqual("abc", destination.Name);
            Assert.AreEqual(1, destination.NameSetCount);
            Assert.AreEqual(1, destination.Id);
        }

        [TestMethod]
        public void CopyProperties_OrdinalCaseMismatch_Test()
        {
            var source = new CopyPlainTestModel { Id = 10, Name = "abc" };
            var destination = new CopyPlainTestModel { Id = 1, Name = "old" };

            Assert.ThrowsException<ArgumentException>(() => source.CopyProperties(destination, new[] { "name" }));

            Assert.AreEqual("old", destination.Name);
            Assert.AreEqual(1, destination.Id);
        }

        [TestMethod]
        public void CopyProperties_UpperCaseVariant_Test()
        {
            var source = new AmbiguousCaseTestHelper { Value = 1, VALUE = 10 };
            var destination = new AmbiguousCaseTestHelper { Value = 5, VALUE = 50 };

            source.CopyProperties(destination, new[] { "VALUE" });

            Assert.AreEqual(10, destination.VALUE);
            Assert.AreEqual(5, destination.Value);
        }

        [TestMethod]
        public void CopyProperties_ExactCaseVariant_Test()
        {
            var source = new AmbiguousCaseTestHelper { Value = 1, VALUE = 10 };
            var destination = new AmbiguousCaseTestHelper { Value = 5, VALUE = 50 };

            source.CopyProperties(destination, new[] { "Value" });

            Assert.AreEqual(1, destination.Value);
            Assert.AreEqual(50, destination.VALUE);
        }

        [TestMethod]
        public void CopyProperties_UnknownAllowedName_Test()
        {
            var source = new CopyPlainTestModel { Id = 10, Name = "abc" };
            var destination = new CopyPlainTestModel { Id = 1, Name = "old" };

            var exception = Assert.ThrowsException<ArgumentException>(
                () => source.CopyProperties(destination, new[] { "Missing" }));

            Assert.AreEqual("allowedProperties", exception.ParamName);
            Assert.AreEqual(1, destination.Id);
            Assert.AreEqual("old", destination.Name);
        }

        [TestMethod]
        public void CopyProperties_NoPartialCopy_Test()
        {
            var source = new CopyPlainTestModel { Id = 10, Name = "abc" };
            var destination = new CopyPlainTestModel { Id = 1, Name = "old" };

            Assert.ThrowsException<ArgumentException>(
                () => source.CopyProperties(destination, new[] { "Id", "Name", "Missing" }));

            Assert.AreEqual(1, destination.Id);
            Assert.AreEqual("old", destination.Name);
        }

        [TestMethod]
        public void CopyProperties_AllowedNoSetter_Test()
        {
            var source = new CopyPlainTestModel { Id = 10, Name = "abc" };
            var destination = new CopyReadOnlyDestinationTestModel { Name = "old" };

            Assert.ThrowsException<ArgumentException>(() => source.CopyProperties(destination, new[] { "Name", "Id" }));

            Assert.AreEqual("old", destination.Name);
            Assert.AreEqual(5, destination.Id);
        }

        [TestMethod]
        public void CopyProperties_AllowedProtectedSetter_Test()
        {
            var source = new CopyRestrictedSourceTestModel { ProtectedValue = 10, Name = "abc" };
            var destination = new CopyRestrictedDestinationTestModel(1, 2, 3) { Name = "old" };

            Assert.ThrowsException<ArgumentException>(
                () => source.CopyProperties(destination, new[] { "Name", "ProtectedValue" }));

            Assert.AreEqual(1, destination.ProtectedValue);
            Assert.AreEqual("old", destination.Name);
        }

        [TestMethod]
        public void CopyProperties_AllowedInternalSetter_Test()
        {
            var source = new CopyRestrictedSourceTestModel { InternalValue = 20, Name = "abc" };
            var destination = new CopyRestrictedDestinationTestModel(1, 2, 3) { Name = "old" };

            Assert.ThrowsException<ArgumentException>(
                () => source.CopyProperties(destination, new[] { "Name", "InternalValue" }));

            Assert.AreEqual(2, destination.InternalValue);
            Assert.AreEqual("old", destination.Name);
        }

        [TestMethod]
        public void CopyProperties_AllowedPrivateSetter_Test()
        {
            var source = new CopyRestrictedSourceTestModel { PrivateValue = 30, Name = "abc" };
            var destination = new CopyRestrictedDestinationTestModel(1, 2, 3) { Name = "old" };

            Assert.ThrowsException<ArgumentException>(
                () => source.CopyProperties(destination, new[] { "Name", "PrivateValue" }));

            Assert.AreEqual(3, destination.PrivateValue);
            Assert.AreEqual("old", destination.Name);
        }

        [TestMethod]
        public void CopyProperties_AllowedPrivateGetter_Test()
        {
            var source = new CopyPrivateGetterSourceTestModel { Secret = 8, Name = "abc" };
            var destination = new CopySecretDestinationTestModel { Secret = 1, Name = "old" };

            Assert.ThrowsException<ArgumentException>(
                () => source.CopyProperties(destination, new[] { "Name", "Secret" }));

            Assert.AreEqual(1, destination.Secret);
            Assert.AreEqual("old", destination.Name);
        }

        [TestMethod]
        public void CopyProperties_AllowedIndexer_Test()
        {
            var source = new CopyIndexedTestModel { Id = 10, Name = "abc" };
            source[0] = 99;
            var destination = new CopyIndexedTestModel { Id = 1, Name = "old" };

            Assert.ThrowsException<ArgumentException>(() => source.CopyProperties(destination, new[] { "Name", "Item" }));

            Assert.AreEqual(0, destination[0]);
            Assert.AreEqual("old", destination.Name);
        }

        [TestMethod]
        public void CopyProperties_AllowedNonPublicProperty_Test()
        {
            var source = new DynamicMemberTestHelper { Id = 10, Name = "abc" };
            var destination = new DynamicMemberTestHelper { Id = 1, Name = "old" };

            Assert.ThrowsException<ArgumentException>(
                () => source.CopyProperties(destination, new[] { "Name", "Secret" }));

            Assert.AreEqual("old", destination.Name);
        }

        [TestMethod]
        public void CopyProperties_AllowedPublicField_Test()
        {
            var source = new DynamicMemberTestHelper { Id = 10, Name = "abc", Code = "C-10" };
            var destination = new DynamicMemberTestHelper { Id = 1, Name = "old", Code = "C-1" };

            Assert.ThrowsException<ArgumentException>(() => source.CopyProperties(destination, new[] { "Name", "Code" }));

            Assert.AreEqual("C-1", destination.Code);
            Assert.AreEqual("old", destination.Name);
        }

        [TestMethod]
        public void CopyProperties_AllowedTypeMismatch_Test()
        {
            var source = new CopyStringNameTestModel { Id = 10, Name = "abc" };
            var destination = new CopyIntNameTestModel { Id = 1, Name = 7 };

            Assert.ThrowsException<ArgumentException>(() => source.CopyProperties(destination, new[] { "Id", "Name" }));

            Assert.AreEqual(1, destination.Id);
            Assert.AreEqual(7, destination.Name);
        }

        [TestMethod]
        public void CopyProperties_AllowedStaticSource_Test()
        {
            var source = new CopyStaticSourceTestModel { Id = 3 };
            var destination = new CopyInstanceCounterTestModel { Counter = 2, Id = 1 };

            Assert.ThrowsException<ArgumentException>(() => source.CopyProperties(destination, new[] { "Id", "Counter" }));

            Assert.AreEqual(2, destination.Counter);
            Assert.AreEqual(1, destination.Id);
        }

        [TestMethod]
        public void CopyProperties_AllowedHiddenProperty_Test()
        {
            var source = new CopyHiddenSourceTestModel { Value = "abc" };
            var destination = new CopyHiddenDerivedTestModel { Value = "old" };
            ((CopyHiddenBaseTestModel)destination).Value = 7;

            source.CopyProperties(destination, new[] { "Value" });

            Assert.AreEqual("abc", destination.Value);
            Assert.AreEqual(7, ((CopyHiddenBaseTestModel)destination).Value);
        }

        [TestMethod]
        public void CopyProperties_NullAllowedName_Test()
        {
            var source = new CopyPlainTestModel { Id = 10, Name = "abc" };
            var destination = new CopyPlainTestModel { Id = 1, Name = "old" };

            var exception = Assert.ThrowsException<ArgumentException>(
                () => source.CopyProperties(destination, new[] { "Name", null }));

            Assert.AreEqual("allowedProperties", exception.ParamName);
            Assert.AreEqual("old", destination.Name);
            Assert.AreEqual(1, destination.Id);
        }

        [TestMethod]
        public void CopyProperties_EmptyAllowedName_Test()
        {
            var source = new CopyPlainTestModel { Id = 10, Name = "abc" };
            var destination = new CopyPlainTestModel { Id = 1, Name = "old" };

            Assert.ThrowsException<ArgumentException>(() => source.CopyProperties(destination, new[] { "Name", "" }));

            Assert.AreEqual("old", destination.Name);
            Assert.AreEqual(1, destination.Id);
        }

        [TestMethod]
        public void CopyProperties_WhitespaceAllowedName_Test()
        {
            var source = new CopyPlainTestModel { Id = 10, Name = "abc" };
            var destination = new CopyPlainTestModel { Id = 1, Name = "old" };

            Assert.ThrowsException<ArgumentException>(() => source.CopyProperties(destination, new[] { "Name", "   " }));

            Assert.AreEqual("old", destination.Name);
            Assert.AreEqual(1, destination.Id);
        }
    }
}
