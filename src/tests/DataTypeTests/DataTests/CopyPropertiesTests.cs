using System;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Primitives;
using RzR.Extensions.Domain.Reflection;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class CopyPropertiesTests
    {
        [TestMethod]
        public void CopyProperties_PlainProperties_Test()
        {
            var source = new CopyPlainTestModel { Id = 10, Name = "abc" };
            var destination = new CopyPlainTestModel { Id = 1, Name = "old" };

            source.CopyProperties(destination);

            Assert.AreEqual(10, destination.Id);
            Assert.AreEqual("abc", destination.Name);
        }

        [TestMethod]
        public void CopyPropertiesTo_PlainProperties_Test()
        {
            var source = new CopyPlainTestModel { Id = 10, Name = "abc" };
            var destination = new CopyPlainTestModel { Id = 1, Name = "old" };

            source.CopyPropertiesTo(destination);

            Assert.AreEqual(10, destination.Id);
            Assert.AreEqual("abc", destination.Name);
        }

        [TestMethod]
        public void CopyProperties_NonPublicSetters_Test()
        {
            var source = new CopyRestrictedSourceTestModel { ProtectedValue = 10, InternalValue = 20, PrivateValue = 30, Name = "abc" };
            var destination = new CopyRestrictedDestinationTestModel(1, 2, 3);

            source.CopyProperties(destination);

            Assert.AreEqual(1, destination.ProtectedValue);
            Assert.AreEqual(2, destination.InternalValue);
            Assert.AreEqual(3, destination.PrivateValue);
            Assert.AreEqual("abc", destination.Name);
        }

        [TestMethod]
        public void CopyPropertiesTo_NonPublicSetters_Test()
        {
            var source = new CopyRestrictedSourceTestModel { ProtectedValue = 10, InternalValue = 20, PrivateValue = 30, Name = "abc" };
            var destination = new CopyRestrictedDestinationTestModel(1, 2, 3);

            source.CopyPropertiesTo(destination);

            Assert.AreEqual(1, destination.ProtectedValue);
            Assert.AreEqual(2, destination.InternalValue);
            Assert.AreEqual(3, destination.PrivateValue);
            Assert.AreEqual("abc", destination.Name);
        }

        [TestMethod]
        public void CopyProperties_IndexerProperty_Test()
        {
            var source = new CopyIndexedTestModel { Id = 10, Name = "abc" };
            source[0] = 99;
            var destination = new CopyIndexedTestModel { Id = 1, Name = "old" };

            source.CopyProperties(destination);

            Assert.AreEqual(10, destination.Id);
            Assert.AreEqual("abc", destination.Name);
            Assert.AreEqual(0, destination[0]);
        }

        [TestMethod]
        public void CopyPropertiesTo_IndexerProperty_Test()
        {
            var source = new CopyIndexedTestModel { Id = 10, Name = "abc" };
            source[0] = 99;
            var destination = new CopyIndexedTestModel { Id = 1, Name = "old" };

            source.CopyPropertiesTo(destination);

            Assert.AreEqual(10, destination.Id);
            Assert.AreEqual("abc", destination.Name);
            Assert.AreEqual(0, destination[0]);
        }

        [TestMethod]
        public void CopyProperties_TypeMismatch_Test()
        {
            var source = new CopyStringNameTestModel { Id = 10, Name = "abc" };
            var destination = new CopyIntNameTestModel { Id = 1, Name = 7 };

            source.CopyProperties(destination);

            Assert.AreEqual(10, destination.Id);
            Assert.AreEqual(7, destination.Name);
        }

        [TestMethod]
        public void CopyPropertiesTo_TypeMismatch_Test()
        {
            var source = new CopyStringNameTestModel { Id = 10, Name = "abc" };
            var destination = new CopyIntNameTestModel { Id = 1, Name = 7 };

            source.CopyPropertiesTo(destination);

            Assert.AreEqual(10, destination.Id);
            Assert.AreEqual(7, destination.Name);
        }

        [TestMethod]
        public void CopyProperties_ReadOnlyDestination_Test()
        {
            var source = new CopyPlainTestModel { Id = 10, Name = "abc" };
            var destination = new CopyReadOnlyDestinationTestModel { Name = "old" };

            source.CopyProperties(destination);

            Assert.AreEqual(5, destination.Id);
            Assert.AreEqual("abc", destination.Name);
        }

        [TestMethod]
        public void CopyPropertiesTo_ReadOnlyDestination_Test()
        {
            var source = new CopyPlainTestModel { Id = 10, Name = "abc" };
            var destination = new CopyReadOnlyDestinationTestModel { Name = "old" };

            source.CopyPropertiesTo(destination);

            Assert.AreEqual(5, destination.Id);
            Assert.AreEqual("abc", destination.Name);
        }

        [TestMethod]
        public void CopyProperties_GetOnlySource_Test()
        {
            var source = new CopyGetOnlySourceTestModel { Name = "abc" };
            var destination = new CopyPlainTestModel { Id = 1, Name = "old" };

            source.CopyProperties(destination);

            Assert.AreEqual(42, destination.Id);
            Assert.AreEqual("abc", destination.Name);
        }

        [TestMethod]
        public void CopyPropertiesTo_GetOnlySource_Test()
        {
            var source = new CopyGetOnlySourceTestModel { Name = "abc" };
            var destination = new CopyPlainTestModel { Id = 1, Name = "old" };

            source.CopyPropertiesTo(destination);

            Assert.AreEqual(42, destination.Id);
            Assert.AreEqual("abc", destination.Name);
        }

        [TestMethod]
        public void CopyProperties_HiddenProperty_Test()
        {
            var source = new CopyHiddenSourceTestModel { Value = "abc" };
            var destination = new CopyHiddenDerivedTestModel { Value = "old" };
            ((CopyHiddenBaseTestModel)destination).Value = 7;

            source.CopyProperties(destination);

            Assert.AreEqual("abc", destination.Value);
            Assert.AreEqual(7, ((CopyHiddenBaseTestModel)destination).Value);
        }

        [TestMethod]
        public void CopyPropertiesTo_HiddenProperty_Test()
        {
            var source = new CopyHiddenSourceTestModel { Value = "abc" };
            var destination = new CopyHiddenDerivedTestModel { Value = "old" };
            ((CopyHiddenBaseTestModel)destination).Value = 7;

            source.CopyPropertiesTo(destination);

            Assert.AreEqual("abc", destination.Value);
            Assert.AreEqual(7, ((CopyHiddenBaseTestModel)destination).Value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CopyProperties_NullSource_Test()
        {
            object source = null;

            source.CopyProperties(new CopyPlainTestModel());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CopyProperties_NullDestination_Test()
        {
            var source = new CopyPlainTestModel { Id = 10, Name = "abc" };

            source.CopyProperties(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CopyPropertiesTo_NullSource_Test()
        {
            object source = null;

            source.CopyPropertiesTo(new CopyPlainTestModel());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CopyPropertiesTo_NullDestination_Test()
        {
            var source = new CopyPlainTestModel { Id = 10, Name = "abc" };

            source.CopyPropertiesTo(null);
        }

        [TestMethod]
        public void CopyPropertiesTo_RuntimeCompatibleValue_Test()
        {
            var source = new CopyNarrowSourceTestModel { Number = 5, Name = "abc" };
            var destination = new CopyWideDestinationTestModel();

            source.CopyPropertiesTo(destination);

            Assert.AreEqual(5L, destination.Number);
            Assert.AreEqual("abc", destination.Name);
        }

        [TestMethod]
        public void CopyProperties_RuntimeCompatibleValue_Test()
        {
            var source = new CopyNarrowSourceTestModel { Number = 5, Name = "abc" };
            var destination = new CopyWideDestinationTestModel();

            source.CopyProperties(destination);

            Assert.AreEqual(0L, destination.Number);
            Assert.AreEqual("abc", destination.Name);
        }

        [TestMethod]
        public void CopyProperties_StaticSource_Test()
        {
            CopyStaticSourceTestModel.Counter = 9;
            var source = new CopyStaticSourceTestModel { Id = 3 };
            var destination = new CopyInstanceCounterTestModel();

            source.CopyProperties(destination);

            Assert.AreEqual(0, destination.Counter);
            Assert.AreEqual(3, destination.Id);
        }

        [TestMethod]
        public void CopyPropertiesTo_StaticDestination_Test()
        {
            CopyInstanceCounterTestModel.Shared = 1;
            var source = new CopySharedSourceTestModel { Shared = 4 };

            source.CopyPropertiesTo(new CopyInstanceCounterTestModel());

            Assert.AreEqual(1, CopyInstanceCounterTestModel.Shared);
        }

        [TestMethod]
        public void CopyProperties_PrivateGetterSource_Test()
        {
            var source = new CopyPrivateGetterSourceTestModel { Secret = 8, Name = "abc" };
            var destination = new CopySecretDestinationTestModel();

            source.CopyProperties(destination);

            Assert.AreEqual(0, destination.Secret);
            Assert.AreEqual("abc", destination.Name);
        }

        [TestMethod]
        public void CopyPropertiesTo_PrivateGetterSource_Test()
        {
            var source = new CopyPrivateGetterSourceTestModel { Secret = 8, Name = "abc" };
            var destination = new CopySecretDestinationTestModel();

            source.CopyPropertiesTo(destination);

            Assert.AreEqual(0, destination.Secret);
            Assert.AreEqual("abc", destination.Name);
        }
    }
}
