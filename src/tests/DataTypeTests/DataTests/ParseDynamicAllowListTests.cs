using System;
using System.Collections.Generic;
using System.Linq;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Collections;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class ParseDynamicAllowListTests
    {
        [TestMethod]
        public void ParseListOfTInDynamic_DefaultOverload_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems();

            var selected = source.ParseListOfTInDynamic(new[] { "Id" });

            object first = selected[0];
            var names = first.GetType().GetProperties()
                .Where(x => x.GetIndexParameters().Length == 0)
                .Select(x => x.Name)
                .ToList();
            CollectionAssert.AreEqual(new[] { "Id" }, names);
            Assert.AreEqual(3, selected.Count);
            Assert.AreEqual(2, (int)selected[0].Id);
            Assert.AreEqual(1, (int)selected[1].Id);
            Assert.AreEqual(3, (int)selected[2].Id);
        }

        [TestMethod]
        public void ParseListOfTInDynamic_AllowedFields_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems();

            var selected = source.ParseListOfTInDynamic(new[] { "Id", "Name" }, new[] { "Id", "Name", "Item" });

            object first = selected[0];
            var names = first.GetType().GetProperties()
                .Where(x => x.GetIndexParameters().Length == 0)
                .Select(x => x.Name)
                .OrderBy(x => x, StringComparer.Ordinal)
                .ToList();
            CollectionAssert.AreEqual(new[] { "Id", "Name" }, names);
            Assert.AreEqual(3, selected.Count);
            Assert.AreEqual(2, (int)selected[0].Id);
            Assert.AreEqual("Beta", (string)selected[0].Name);
            Assert.AreEqual(1, (int)selected[1].Id);
            Assert.AreEqual("Alpha", (string)selected[1].Name);
            Assert.AreEqual(3, (int)selected[2].Id);
            Assert.AreEqual("Gamma", (string)selected[2].Name);
        }

        [TestMethod]
        public void ParseListOfTInDynamic_NotAllowedField_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems();

            var exception = Assert.ThrowsException<ArgumentException>(
                () => source.ParseListOfTInDynamic(new[] { "Id", "Name" }, new[] { "Id" }));

            Assert.AreEqual("fields", exception.ParamName);
        }

        [TestMethod]
        public void ParseListOfTInDynamic_NotAllowedSameMessage_Test()
        {
            var existing = DynamicMemberTestHelper.CreateItems();
            var missing = AmbiguousCaseTestHelper.CreateItems();

            var existingException = Assert.ThrowsException<ArgumentException>(
                () => existing.ParseListOfTInDynamic(new[] { "Name" }, new[] { "Id" }));
            var missingException = Assert.ThrowsException<ArgumentException>(
                () => missing.ParseListOfTInDynamic(new[] { "Name" }, new[] { "Id" }));

            Assert.AreEqual(missingException.Message, existingException.Message);
        }

        [TestMethod]
        public void ParseListOfTInDynamic_UnknownAllowedField_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems();

            Assert.ThrowsException<ArgumentException>(
                () => source.ParseListOfTInDynamic(new[] { "Missing" }, new[] { "Id", "Missing" }));
        }

        [TestMethod]
        public void ParseListOfTInDynamic_AllowedPrivateProperty_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems();

            Assert.ThrowsException<ArgumentException>(
                () => source.ParseListOfTInDynamic(new[] { "Id", "Secret" }, new[] { "Id", "Secret" }));
        }

        [TestMethod]
        public void ParseListOfTInDynamic_AllowedPublicField_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems();

            Assert.ThrowsException<ArgumentException>(
                () => source.ParseListOfTInDynamic(new[] { "Id", "Code" }, new[] { "Id", "Code" }));
        }

        [TestMethod]
        public void ParseListOfTInDynamic_LowerCaseRequest_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems();

            var selected = source.ParseListOfTInDynamic(new[] { "name" }, new[] { "Name" });

            Assert.AreEqual(3, selected.Count);
            Assert.AreEqual("Beta", (string)selected[0].Name);
            Assert.AreEqual("Alpha", (string)selected[1].Name);
            Assert.AreEqual("Gamma", (string)selected[2].Name);
        }

        [TestMethod]
        public void ParseListOfTInDynamic_CaseVariantRequest_Test()
        {
            var source = AllowListTestHelper.CreateCrossedCaseItems();

            var selected = source.ParseListOfTInDynamic(new[] { "VALUE" }, new[] { "Value" });

            object first = selected[0];
            var names = first.GetType().GetProperties()
                .Where(x => x.GetIndexParameters().Length == 0)
                .Select(x => x.Name)
                .ToList();
            CollectionAssert.AreEqual(new[] { "Value" }, names);
            Assert.AreEqual(3, selected.Count);
            Assert.AreEqual(2, (int)selected[0].Value);
            Assert.AreEqual(1, (int)selected[1].Value);
            Assert.AreEqual(3, (int)selected[2].Value);
        }

        [TestMethod]
        public void ParseListOfTInDynamic_ExactCaseVariant_Test()
        {
            var source = AllowListTestHelper.CreateCrossedCaseItems();

            var selected = source.ParseListOfTInDynamic(new[] { "VALUE" }, new[] { "Value", "VALUE" });

            object first = selected[0];
            var names = first.GetType().GetProperties()
                .Where(x => x.GetIndexParameters().Length == 0)
                .Select(x => x.Name)
                .ToList();
            CollectionAssert.AreEqual(new[] { "VALUE" }, names);
            Assert.AreEqual(10, (int)selected[0].VALUE);
            Assert.AreEqual(30, (int)selected[1].VALUE);
            Assert.AreEqual(20, (int)selected[2].VALUE);
        }

        [TestMethod]
        public void ParseListOfTInDynamic_UpperCaseAllowList_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems();

            Assert.ThrowsException<ArgumentException>(
                () => source.ParseListOfTInDynamic(new[] { "NAME" }, new[] { "NAME" }));
        }

        [TestMethod]
        public void ParseListOfTInDynamic_AmbiguousAllowList_Test()
        {
            var source = AllowListTestHelper.CreateCrossedCaseItems();

            var ambiguousException = Assert.ThrowsException<ArgumentException>(
                () => source.ParseListOfTInDynamic(new[] { "vAlUe" }, new[] { "Value", "VALUE" }));
            var unknownException = Assert.ThrowsException<ArgumentException>(
                () => source.ParseListOfTInDynamic(new[] { "vAlUe" }, new[] { "Other" }));

            Assert.AreEqual(unknownException.Message, ambiguousException.Message);
        }

        [TestMethod]
        public void ParseListOfTInDynamic_NullAllowList_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems();
            IEnumerable<string> allowed = null;

            Assert.ThrowsException<ArgumentNullException>(() => source.ParseListOfTInDynamic(new[] { "Id" }, allowed));
        }

        [TestMethod]
        public void ParseListOfTInDynamic_NullAllowedEntry_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems();

            var exception = Assert.ThrowsException<ArgumentException>(
                () => source.ParseListOfTInDynamic(new[] { "Id" }, new[] { "Id", null }));

            Assert.AreEqual("allowedFields", exception.ParamName);
        }

        [TestMethod]
        public void ParseListOfTInDynamic_EmptyAllowedEntry_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems();

            Assert.ThrowsException<ArgumentException>(
                () => source.ParseListOfTInDynamic(new[] { "Id" }, new[] { "Id", "" }));
        }

        [TestMethod]
        public void ParseListOfTInDynamic_WhitespaceAllowedEntry_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems();

            Assert.ThrowsException<ArgumentException>(
                () => source.ParseListOfTInDynamic(new[] { "Id" }, new[] { "Id", "   " }));
        }

        [TestMethod]
        public void ParseListOfTInDynamic_EmptyAllowList_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems();

            Assert.ThrowsException<ArgumentException>(
                () => source.ParseListOfTInDynamic(new[] { "Id" }, new string[0]));
        }

        [TestMethod]
        public void ParseListOfTInDynamic_NullFieldsAllowList_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems();
            IEnumerable<string> fields = null;

            Assert.ThrowsException<ArgumentException>(
                () => source.ParseListOfTInDynamic(fields, new[] { "Id", "Name", "Item" }));
        }

        [TestMethod]
        public void ParseListOfTInDynamic_EmptyFieldsAllowList_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems();

            Assert.ThrowsException<ArgumentException>(
                () => source.ParseListOfTInDynamic(new string[0], new[] { "Id", "Name", "Item" }));
        }

        [TestMethod]
        public void ParseListOfTInDynamic_EmptyFieldNameAllowList_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems();

            Assert.ThrowsException<ArgumentException>(
                () => source.ParseListOfTInDynamic(new[] { "Id", "" }, new[] { "Id", "Name" }));
        }

        [TestMethod]
        public void ParseListOfTInDynamic_DuplicateFieldsAllowList_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems();

            Assert.ThrowsException<ArgumentException>(
                () => source.ParseListOfTInDynamic(new[] { "Name", "Name" }, new[] { "Name" }));
        }

        [TestMethod]
        public void ParseListOfTInDynamic_NullInputAllowList_Test()
        {
            List<DynamicMemberTestHelper> source = null;

            Assert.ThrowsException<ArgumentNullException>(
                () => source.ParseListOfTInDynamic(new[] { "Id" }, new[] { "Id" }));
        }

        [TestMethod]
        public void ParseListOfTInDynamic_TooManyAllowedFields_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems();
            var fields = Enumerable.Range(0, 257).Select(x => "F" + x).ToArray();

            Assert.ThrowsException<ArgumentException>(() => source.ParseListOfTInDynamic(fields, fields));
        }

        [TestMethod]
        public void ParseEnumerableOfTInDynamic_DefaultOverload_Test()
        {
            IEnumerable<DynamicMemberTestHelper> source = DynamicMemberTestHelper.CreateItems();

            var selected = source.ParseEnumerableOfTInDynamic(new[] { "Id" });

            object first = selected[0];
            var names = first.GetType().GetProperties()
                .Where(x => x.GetIndexParameters().Length == 0)
                .Select(x => x.Name)
                .ToList();
            CollectionAssert.AreEqual(new[] { "Id" }, names);
            Assert.AreEqual(3, selected.Count);
            Assert.AreEqual(2, (int)selected[0].Id);
            Assert.AreEqual(1, (int)selected[1].Id);
            Assert.AreEqual(3, (int)selected[2].Id);
        }

        [TestMethod]
        public void ParseEnumerableOfTInDynamic_AllowedFields_Test()
        {
            IEnumerable<DynamicMemberTestHelper> source = DynamicMemberTestHelper.CreateItems();

            var selected = source.ParseEnumerableOfTInDynamic(new[] { "Id", "Name" }, new[] { "Id", "Name", "Item" });

            object first = selected[0];
            var names = first.GetType().GetProperties()
                .Where(x => x.GetIndexParameters().Length == 0)
                .Select(x => x.Name)
                .OrderBy(x => x, StringComparer.Ordinal)
                .ToList();
            CollectionAssert.AreEqual(new[] { "Id", "Name" }, names);
            Assert.AreEqual(3, selected.Count);
            Assert.AreEqual(2, (int)selected[0].Id);
            Assert.AreEqual("Beta", (string)selected[0].Name);
            Assert.AreEqual(1, (int)selected[1].Id);
            Assert.AreEqual("Alpha", (string)selected[1].Name);
            Assert.AreEqual(3, (int)selected[2].Id);
            Assert.AreEqual("Gamma", (string)selected[2].Name);
        }

        [TestMethod]
        public void ParseEnumerableOfTInDynamic_NotAllowedField_Test()
        {
            IEnumerable<DynamicMemberTestHelper> source = DynamicMemberTestHelper.CreateItems();

            var exception = Assert.ThrowsException<ArgumentException>(
                () => source.ParseEnumerableOfTInDynamic(new[] { "Id", "Name" }, new[] { "Id" }));

            Assert.AreEqual("fields", exception.ParamName);
        }

        [TestMethod]
        public void ParseEnumerableOfTInDynamic_NotAllowedSameMessage_Test()
        {
            IEnumerable<DynamicMemberTestHelper> existing = DynamicMemberTestHelper.CreateItems();
            IEnumerable<AmbiguousCaseTestHelper> missing = AmbiguousCaseTestHelper.CreateItems();

            var existingException = Assert.ThrowsException<ArgumentException>(
                () => existing.ParseEnumerableOfTInDynamic(new[] { "Name" }, new[] { "Id" }));
            var missingException = Assert.ThrowsException<ArgumentException>(
                () => missing.ParseEnumerableOfTInDynamic(new[] { "Name" }, new[] { "Id" }));

            Assert.AreEqual(missingException.Message, existingException.Message);
        }

        [TestMethod]
        public void ParseEnumerableOfTInDynamic_UnknownAllowedField_Test()
        {
            IEnumerable<DynamicMemberTestHelper> source = DynamicMemberTestHelper.CreateItems();

            Assert.ThrowsException<ArgumentException>(
                () => source.ParseEnumerableOfTInDynamic(new[] { "Missing" }, new[] { "Id", "Missing" }));
        }

        [TestMethod]
        public void ParseEnumerableOfTInDynamic_AllowedPrivateProperty_Test()
        {
            IEnumerable<DynamicMemberTestHelper> source = DynamicMemberTestHelper.CreateItems();

            Assert.ThrowsException<ArgumentException>(
                () => source.ParseEnumerableOfTInDynamic(new[] { "Id", "Secret" }, new[] { "Id", "Secret" }));
        }

        [TestMethod]
        public void ParseEnumerableOfTInDynamic_AllowedPublicField_Test()
        {
            IEnumerable<DynamicMemberTestHelper> source = DynamicMemberTestHelper.CreateItems();

            Assert.ThrowsException<ArgumentException>(
                () => source.ParseEnumerableOfTInDynamic(new[] { "Id", "Code" }, new[] { "Id", "Code" }));
        }

        [TestMethod]
        public void ParseEnumerableOfTInDynamic_LowerCaseRequest_Test()
        {
            IEnumerable<DynamicMemberTestHelper> source = DynamicMemberTestHelper.CreateItems();

            var selected = source.ParseEnumerableOfTInDynamic(new[] { "name" }, new[] { "Name" });

            Assert.AreEqual(3, selected.Count);
            Assert.AreEqual("Beta", (string)selected[0].Name);
            Assert.AreEqual("Alpha", (string)selected[1].Name);
            Assert.AreEqual("Gamma", (string)selected[2].Name);
        }

        [TestMethod]
        public void ParseEnumerableOfTInDynamic_CaseVariantRequest_Test()
        {
            IEnumerable<AmbiguousCaseTestHelper> source = AllowListTestHelper.CreateCrossedCaseItems();

            var selected = source.ParseEnumerableOfTInDynamic(new[] { "VALUE" }, new[] { "Value" });

            object first = selected[0];
            var names = first.GetType().GetProperties()
                .Where(x => x.GetIndexParameters().Length == 0)
                .Select(x => x.Name)
                .ToList();
            CollectionAssert.AreEqual(new[] { "Value" }, names);
            Assert.AreEqual(3, selected.Count);
            Assert.AreEqual(2, (int)selected[0].Value);
            Assert.AreEqual(1, (int)selected[1].Value);
            Assert.AreEqual(3, (int)selected[2].Value);
        }

        [TestMethod]
        public void ParseEnumerableOfTInDynamic_ExactCaseVariant_Test()
        {
            IEnumerable<AmbiguousCaseTestHelper> source = AllowListTestHelper.CreateCrossedCaseItems();

            var selected = source.ParseEnumerableOfTInDynamic(new[] { "VALUE" }, new[] { "Value", "VALUE" });

            object first = selected[0];
            var names = first.GetType().GetProperties()
                .Where(x => x.GetIndexParameters().Length == 0)
                .Select(x => x.Name)
                .ToList();
            CollectionAssert.AreEqual(new[] { "VALUE" }, names);
            Assert.AreEqual(10, (int)selected[0].VALUE);
            Assert.AreEqual(30, (int)selected[1].VALUE);
            Assert.AreEqual(20, (int)selected[2].VALUE);
        }

        [TestMethod]
        public void ParseEnumerableOfTInDynamic_UpperCaseAllowList_Test()
        {
            IEnumerable<DynamicMemberTestHelper> source = DynamicMemberTestHelper.CreateItems();

            Assert.ThrowsException<ArgumentException>(
                () => source.ParseEnumerableOfTInDynamic(new[] { "NAME" }, new[] { "NAME" }));
        }

        [TestMethod]
        public void ParseEnumerableOfTInDynamic_AmbiguousAllowList_Test()
        {
            IEnumerable<AmbiguousCaseTestHelper> source = AllowListTestHelper.CreateCrossedCaseItems();

            var ambiguousException = Assert.ThrowsException<ArgumentException>(
                () => source.ParseEnumerableOfTInDynamic(new[] { "vAlUe" }, new[] { "Value", "VALUE" }));
            var unknownException = Assert.ThrowsException<ArgumentException>(
                () => source.ParseEnumerableOfTInDynamic(new[] { "vAlUe" }, new[] { "Other" }));

            Assert.AreEqual(unknownException.Message, ambiguousException.Message);
        }

        [TestMethod]
        public void ParseEnumerableOfTInDynamic_NullAllowList_Test()
        {
            IEnumerable<DynamicMemberTestHelper> source = DynamicMemberTestHelper.CreateItems();
            IEnumerable<string> allowed = null;

            Assert.ThrowsException<ArgumentNullException>(
                () => source.ParseEnumerableOfTInDynamic(new[] { "Id" }, allowed));
        }

        [TestMethod]
        public void ParseEnumerableOfTInDynamic_NullAllowedEntry_Test()
        {
            IEnumerable<DynamicMemberTestHelper> source = DynamicMemberTestHelper.CreateItems();

            var exception = Assert.ThrowsException<ArgumentException>(
                () => source.ParseEnumerableOfTInDynamic(new[] { "Id" }, new[] { "Id", null }));

            Assert.AreEqual("allowedFields", exception.ParamName);
        }

        [TestMethod]
        public void ParseEnumerableOfTInDynamic_EmptyAllowedEntry_Test()
        {
            IEnumerable<DynamicMemberTestHelper> source = DynamicMemberTestHelper.CreateItems();

            Assert.ThrowsException<ArgumentException>(
                () => source.ParseEnumerableOfTInDynamic(new[] { "Id" }, new[] { "Id", "" }));
        }

        [TestMethod]
        public void ParseEnumerableOfTInDynamic_WhitespaceAllowedEntry_Test()
        {
            IEnumerable<DynamicMemberTestHelper> source = DynamicMemberTestHelper.CreateItems();

            Assert.ThrowsException<ArgumentException>(
                () => source.ParseEnumerableOfTInDynamic(new[] { "Id" }, new[] { "Id", "   " }));
        }

        [TestMethod]
        public void ParseEnumerableOfTInDynamic_EmptyAllowList_Test()
        {
            IEnumerable<DynamicMemberTestHelper> source = DynamicMemberTestHelper.CreateItems();

            Assert.ThrowsException<ArgumentException>(
                () => source.ParseEnumerableOfTInDynamic(new[] { "Id" }, new string[0]));
        }

        [TestMethod]
        public void ParseEnumerableOfTInDynamic_NullFieldsAllowList_Test()
        {
            IEnumerable<DynamicMemberTestHelper> source = DynamicMemberTestHelper.CreateItems();
            IEnumerable<string> fields = null;

            Assert.ThrowsException<ArgumentException>(
                () => source.ParseEnumerableOfTInDynamic(fields, new[] { "Id", "Name", "Item" }));
        }

        [TestMethod]
        public void ParseEnumerableOfTInDynamic_EmptyFieldsAllowList_Test()
        {
            IEnumerable<DynamicMemberTestHelper> source = DynamicMemberTestHelper.CreateItems();

            Assert.ThrowsException<ArgumentException>(
                () => source.ParseEnumerableOfTInDynamic(new string[0], new[] { "Id", "Name", "Item" }));
        }

        [TestMethod]
        public void ParseEnumerableOfTInDynamic_EmptyFieldNameAllowList_Test()
        {
            IEnumerable<DynamicMemberTestHelper> source = DynamicMemberTestHelper.CreateItems();

            Assert.ThrowsException<ArgumentException>(
                () => source.ParseEnumerableOfTInDynamic(new[] { "Id", "" }, new[] { "Id", "Name" }));
        }

        [TestMethod]
        public void ParseEnumerableOfTInDynamic_DuplicateFieldsAllowList_Test()
        {
            IEnumerable<DynamicMemberTestHelper> source = DynamicMemberTestHelper.CreateItems();

            Assert.ThrowsException<ArgumentException>(
                () => source.ParseEnumerableOfTInDynamic(new[] { "Name", "Name" }, new[] { "Name" }));
        }

        [TestMethod]
        public void ParseEnumerableOfTInDynamic_NullInputAllowList_Test()
        {
            IEnumerable<DynamicMemberTestHelper> source = null;

            Assert.ThrowsException<ArgumentNullException>(
                () => source.ParseEnumerableOfTInDynamic(new[] { "Id" }, new[] { "Id" }));
        }

        [TestMethod]
        public void ParseEnumerableOfTInDynamic_RejectedBeforeEnumeration_Test()
        {
            var source = new ThrowingEnumerableTestHelper<DynamicMemberTestHelper>();

            Assert.ThrowsException<ArgumentException>(
                () => source.ParseEnumerableOfTInDynamic(new[] { "Name" }, new[] { "Id" }));
        }

        [TestMethod]
        public void ParseEnumerableOfTInDynamic_AllowedEnumeratesSource_Test()
        {
            var source = new ThrowingEnumerableTestHelper<DynamicMemberTestHelper>();

            Assert.ThrowsException<InvalidOperationException>(
                () => source.ParseEnumerableOfTInDynamic(new[] { "Id" }, new[] { "Id" }));
        }
    }
}
