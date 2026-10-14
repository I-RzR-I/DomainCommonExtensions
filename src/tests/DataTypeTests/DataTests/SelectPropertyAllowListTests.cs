using System;
using System.Collections.Generic;
using System.Linq;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Collections;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class SelectPropertyAllowListTests
    {
        [TestMethod]
        public void SelectProperty_DefaultOverload_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            var selected = source.SelectProperty("Id").Cast<int>().ToList();

            CollectionAssert.AreEqual(new[] { 2, 1, 3 }, selected);
        }

        [TestMethod]
        public void SelectProperty_AllowedProperty_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            var query = source.SelectProperty("Name", new[] { "Id", "Name" });

            Assert.AreEqual(typeof(string), query.ElementType);
            CollectionAssert.AreEqual(new[] { "Beta", "Alpha", "Gamma" }, query.Cast<string>().ToList());
        }

        [TestMethod]
        public void SelectProperty_NotAllowedProperty_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            var exception = Assert.ThrowsException<ArgumentException>(
                () => source.SelectProperty("Name", new[] { "Id" }).Cast<object>().ToList());

            Assert.AreEqual("prop", exception.ParamName);
        }

        [TestMethod]
        public void SelectProperty_NotAllowedSameMessage_Test()
        {
            var existing = DynamicMemberTestHelper.CreateItems().AsQueryable();
            var missing = AmbiguousCaseTestHelper.CreateItems().AsQueryable();

            var existingException = Assert.ThrowsException<ArgumentException>(
                () => existing.SelectProperty("Name", new[] { "Id" }));
            var missingException = Assert.ThrowsException<ArgumentException>(
                () => missing.SelectProperty("Name", new[] { "Id" }));

            Assert.AreEqual(missingException.Message, existingException.Message);
        }

        [TestMethod]
        public void SelectProperty_UnknownAllowedProperty_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            Assert.ThrowsException<ArgumentException>(
                () => source.SelectProperty("Missing", new[] { "Missing" }).Cast<object>().ToList());
        }

        [TestMethod]
        public void SelectProperty_AllowedPrivateProperty_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            Assert.ThrowsException<ArgumentException>(
                () => source.SelectProperty("Secret", new[] { "Secret" }).Cast<object>().ToList());
        }

        [TestMethod]
        public void SelectProperty_AllowedPublicField_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            Assert.ThrowsException<ArgumentException>(
                () => source.SelectProperty("Code", new[] { "Code" }).Cast<object>().ToList());
        }

        [TestMethod]
        public void SelectProperty_LowerCaseRequest_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            var selected = source.SelectProperty("name", new[] { "Name" }).Cast<string>().ToList();

            CollectionAssert.AreEqual(new[] { "Beta", "Alpha", "Gamma" }, selected);
        }

        [TestMethod]
        public void SelectProperty_CaseVariantRequest_Test()
        {
            var source = AllowListTestHelper.CreateCrossedCaseItems().AsQueryable();

            var selected = source.SelectProperty("VALUE", new[] { "Value" }).Cast<int>().ToList();

            CollectionAssert.AreEqual(new[] { 2, 1, 3 }, selected);
        }

        [TestMethod]
        public void SelectProperty_ExactCaseVariant_Test()
        {
            var source = AllowListTestHelper.CreateCrossedCaseItems().AsQueryable();

            var selected = source.SelectProperty("VALUE", new[] { "Value", "VALUE" }).Cast<int>().ToList();

            CollectionAssert.AreEqual(new[] { 10, 30, 20 }, selected);
        }

        [TestMethod]
        public void SelectProperty_UpperCaseAllowList_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            Assert.ThrowsException<ArgumentException>(
                () => source.SelectProperty("NAME", new[] { "NAME" }).Cast<object>().ToList());
        }

        [TestMethod]
        public void SelectProperty_AmbiguousAllowList_Test()
        {
            var source = AllowListTestHelper.CreateCrossedCaseItems().AsQueryable();

            var ambiguousException = Assert.ThrowsException<ArgumentException>(
                () => source.SelectProperty("vAlUe", new[] { "Value", "VALUE" }));
            var unknownException = Assert.ThrowsException<ArgumentException>(
                () => source.SelectProperty("vAlUe", new[] { "Other" }));

            Assert.AreEqual(unknownException.Message, ambiguousException.Message);
        }

        [TestMethod]
        public void SelectProperty_NullAllowList_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();
            IEnumerable<string> allowed = null;

            Assert.ThrowsException<ArgumentNullException>(() => source.SelectProperty("Id", allowed));
        }

        [TestMethod]
        public void SelectProperty_NullAllowedEntry_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            var exception = Assert.ThrowsException<ArgumentException>(
                () => source.SelectProperty("Id", new[] { "Id", null }));

            Assert.AreEqual("allowedFields", exception.ParamName);
        }

        [TestMethod]
        public void SelectProperty_EmptyAllowedEntry_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            Assert.ThrowsException<ArgumentException>(() => source.SelectProperty("Id", new[] { "Id", "" }));
        }

        [TestMethod]
        public void SelectProperty_WhitespaceAllowedEntry_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            Assert.ThrowsException<ArgumentException>(() => source.SelectProperty("Id", new[] { "Id", "   " }));
        }

        [TestMethod]
        public void SelectProperty_EmptyAllowList_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            Assert.ThrowsException<ArgumentException>(() => source.SelectProperty("Id", new string[0]));
        }

        [TestMethod]
        public void SelectProperty_NullPropertyAllowList_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();
            string prop = null;

            Assert.ThrowsException<ArgumentException>(() => source.SelectProperty(prop, new[] { "Id", "Name" }));
        }

        [TestMethod]
        public void SelectProperty_EmptyPropertyAllowList_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            Assert.ThrowsException<ArgumentException>(() => source.SelectProperty("", new[] { "Id", "Name" }));
        }

        [TestMethod]
        public void SelectProperty_WhitespacePropertyAllowList_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            Assert.ThrowsException<ArgumentException>(() => source.SelectProperty("   ", new[] { "Id", "Name" }));
        }

        [TestMethod]
        public void SelectProperty_NullSourceAllowList_Test()
        {
            IQueryable source = null;

            Assert.ThrowsException<ArgumentNullException>(() => source.SelectProperty("Id", new[] { "Id" }));
        }

        [TestMethod]
        public void SelectProperty_RejectedBeforeQuery_Test()
        {
            var source = new ThrowingQueryableTestHelper<DynamicMemberTestHelper>();

            Assert.ThrowsException<ArgumentException>(() => source.SelectProperty("Name", new[] { "Id" }));
        }

        [TestMethod]
        public void SelectProperty_AllowedReadsQuery_Test()
        {
            var source = new ThrowingQueryableTestHelper<DynamicMemberTestHelper>();

            Assert.ThrowsException<InvalidOperationException>(() => source.SelectProperty("Id", new[] { "Id" }));
        }

        [TestMethod]
        public void SelectMultipleProperties_DefaultOverload_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            var selected = source.SelectMultipleProperties("Id", "Name").Cast<DynamicMemberTestHelper>().ToList();

            CollectionAssert.AreEqual(new[] { 2, 1, 3 }, selected.Select(x => x.Id).ToList());
            CollectionAssert.AreEqual(new[] { "Beta", "Alpha", "Gamma" }, selected.Select(x => x.Name).ToList());
            Assert.IsTrue(selected.All(x => x.Item == null));
        }

        [TestMethod]
        public void SelectMultipleProperties_AllowedProperties_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            var query = source.SelectMultipleProperties(new[] { "Id", "Name" }, new[] { "Id", "Name", "Item" });
            var selected = query.Cast<DynamicMemberTestHelper>().ToList();

            Assert.AreEqual(typeof(DynamicMemberTestHelper), query.ElementType);
            CollectionAssert.AreEqual(new[] { 2, 1, 3 }, selected.Select(x => x.Id).ToList());
            CollectionAssert.AreEqual(new[] { "Beta", "Alpha", "Gamma" }, selected.Select(x => x.Name).ToList());
            Assert.IsTrue(selected.All(x => x.Item == null));
        }

        [TestMethod]
        public void SelectMultipleProperties_NotAllowedProperty_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            var exception = Assert.ThrowsException<ArgumentException>(
                () => source.SelectMultipleProperties(new[] { "Id", "Name" }, new[] { "Id" }).Cast<object>().ToList());

            Assert.AreEqual("props", exception.ParamName);
        }

        [TestMethod]
        public void SelectMultipleProperties_NotAllowedSameMessage_Test()
        {
            var existing = DynamicMemberTestHelper.CreateItems().AsQueryable();
            var missing = AmbiguousCaseTestHelper.CreateItems().AsQueryable();

            var existingException = Assert.ThrowsException<ArgumentException>(
                () => existing.SelectMultipleProperties(new[] { "Name" }, new[] { "Id" }));
            var missingException = Assert.ThrowsException<ArgumentException>(
                () => missing.SelectMultipleProperties(new[] { "Name" }, new[] { "Id" }));

            Assert.AreEqual(missingException.Message, existingException.Message);
        }

        [TestMethod]
        public void SelectMultipleProperties_UnknownAllowedProperty_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            Assert.ThrowsException<ArgumentException>(
                () => source.SelectMultipleProperties(new[] { "Missing" }, new[] { "Id", "Missing" }).Cast<object>().ToList());
        }

        [TestMethod]
        public void SelectMultipleProperties_AllowedPrivateProperty_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            Assert.ThrowsException<ArgumentException>(
                () => source.SelectMultipleProperties(new[] { "Id", "Secret" }, new[] { "Id", "Secret" }).Cast<object>().ToList());
        }

        [TestMethod]
        public void SelectMultipleProperties_AllowedPublicField_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            Assert.ThrowsException<ArgumentException>(
                () => source.SelectMultipleProperties(new[] { "Id", "Code" }, new[] { "Id", "Code" }).Cast<object>().ToList());
        }

        [TestMethod]
        public void SelectMultipleProperties_LowerCaseRequest_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            var selected = source.SelectMultipleProperties(new[] { "name" }, new[] { "Name" })
                .Cast<DynamicMemberTestHelper>().ToList();

            CollectionAssert.AreEqual(new[] { "Beta", "Alpha", "Gamma" }, selected.Select(x => x.Name).ToList());
            Assert.IsTrue(selected.All(x => x.Id == 0));
        }

        [TestMethod]
        public void SelectMultipleProperties_CaseVariantRequest_Test()
        {
            var source = AllowListTestHelper.CreateCrossedCaseItems().AsQueryable();

            var selected = source.SelectMultipleProperties(new[] { "VALUE" }, new[] { "Value" })
                .Cast<AmbiguousCaseTestHelper>().ToList();

            CollectionAssert.AreEqual(new[] { 2, 1, 3 }, selected.Select(x => x.Value).ToList());
            Assert.IsTrue(selected.All(x => x.VALUE == 0));
        }

        [TestMethod]
        public void SelectMultipleProperties_ExactCaseVariant_Test()
        {
            var source = AllowListTestHelper.CreateCrossedCaseItems().AsQueryable();

            var selected = source.SelectMultipleProperties(new[] { "VALUE" }, new[] { "Value", "VALUE" })
                .Cast<AmbiguousCaseTestHelper>().ToList();

            CollectionAssert.AreEqual(new[] { 10, 30, 20 }, selected.Select(x => x.VALUE).ToList());
            Assert.IsTrue(selected.All(x => x.Value == 0));
        }

        [TestMethod]
        public void SelectMultipleProperties_UpperCaseAllowList_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            Assert.ThrowsException<ArgumentException>(
                () => source.SelectMultipleProperties(new[] { "NAME" }, new[] { "NAME" }).Cast<object>().ToList());
        }

        [TestMethod]
        public void SelectMultipleProperties_AmbiguousAllowList_Test()
        {
            var source = AllowListTestHelper.CreateCrossedCaseItems().AsQueryable();

            var ambiguousException = Assert.ThrowsException<ArgumentException>(
                () => source.SelectMultipleProperties(new[] { "vAlUe" }, new[] { "Value", "VALUE" }));
            var unknownException = Assert.ThrowsException<ArgumentException>(
                () => source.SelectMultipleProperties(new[] { "vAlUe" }, new[] { "Other" }));

            Assert.AreEqual(unknownException.Message, ambiguousException.Message);
        }

        [TestMethod]
        public void SelectMultipleProperties_NullAllowList_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();
            IEnumerable<string> props = new[] { "Id" };
            IEnumerable<string> allowed = null;

            Assert.ThrowsException<ArgumentNullException>(() => source.SelectMultipleProperties(props, allowed));
        }

        [TestMethod]
        public void SelectMultipleProperties_NullAllowedEntry_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            var exception = Assert.ThrowsException<ArgumentException>(
                () => source.SelectMultipleProperties(new[] { "Id" }, new[] { "Id", null }));

            Assert.AreEqual("allowedFields", exception.ParamName);
        }

        [TestMethod]
        public void SelectMultipleProperties_EmptyAllowedEntry_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            Assert.ThrowsException<ArgumentException>(
                () => source.SelectMultipleProperties(new[] { "Id" }, new[] { "Id", "" }));
        }

        [TestMethod]
        public void SelectMultipleProperties_WhitespaceAllowedEntry_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            Assert.ThrowsException<ArgumentException>(
                () => source.SelectMultipleProperties(new[] { "Id" }, new[] { "Id", "   " }));
        }

        [TestMethod]
        public void SelectMultipleProperties_EmptyAllowList_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            Assert.ThrowsException<ArgumentException>(
                () => source.SelectMultipleProperties(new[] { "Id" }, new string[0]));
        }

        [TestMethod]
        public void SelectMultipleProperties_NullPropsAllowList_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();
            IEnumerable<string> props = null;
            IEnumerable<string> allowed = new[] { "Id", "Name", "Item" };

            Assert.ThrowsException<ArgumentException>(() => source.SelectMultipleProperties(props, allowed));
        }

        [TestMethod]
        public void SelectMultipleProperties_EmptyPropsAllowList_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            Assert.ThrowsException<ArgumentException>(
                () => source.SelectMultipleProperties(new string[0], new[] { "Id", "Name", "Item" }));
        }

        [TestMethod]
        public void SelectMultipleProperties_EmptyPropNameAllowList_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            Assert.ThrowsException<ArgumentException>(
                () => source.SelectMultipleProperties(new[] { "Id", "" }, new[] { "Id", "Name" }));
        }

        [TestMethod]
        public void SelectMultipleProperties_DuplicatePropsAllowList_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            Assert.ThrowsException<ArgumentException>(
                () => source.SelectMultipleProperties(new[] { "Name", "Name" }, new[] { "Name" }));
        }

        [TestMethod]
        public void SelectMultipleProperties_NullSourceAllowList_Test()
        {
            IQueryable source = null;

            Assert.ThrowsException<ArgumentNullException>(
                () => source.SelectMultipleProperties(new[] { "Id" }, new[] { "Id" }));
        }

        [TestMethod]
        public void SelectMultipleProperties_RejectedBeforeQuery_Test()
        {
            var source = new ThrowingQueryableTestHelper<DynamicMemberTestHelper>();

            Assert.ThrowsException<ArgumentException>(
                () => source.SelectMultipleProperties(new[] { "Id", "Name" }, new[] { "Id" }));
        }

        [TestMethod]
        public void SelectMultipleProperties_AllowedReadsQuery_Test()
        {
            var source = new ThrowingQueryableTestHelper<DynamicMemberTestHelper>();

            Assert.ThrowsException<InvalidOperationException>(
                () => source.SelectMultipleProperties(new[] { "Id" }, new[] { "Id" }));
        }
    }
}
