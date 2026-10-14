using System;
using System.Collections.Generic;
using System.Linq;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Collections;
using RzR.Extensions.Domain.Linq;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class DynamicMemberAccessTests
    {
        [TestMethod]
        [Timeout(10000)]
        public void ParseListOfTInDynamic_EmptyFieldList_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems();

            Assert.ThrowsException<ArgumentException>(() => source.ParseListOfTInDynamic(new string[0]));
        }

        [TestMethod]
        [Timeout(10000)]
        public void ParseListOfTInDynamic_EmptyFieldName_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems();

            var exception = Assert.ThrowsException<ArgumentException>(
                () => source.ParseListOfTInDynamic(new[] { "Id", "" }));

            Assert.AreEqual("fields", exception.ParamName);
        }

        [TestMethod]
        [Timeout(10000)]
        public void ParseListOfTInDynamic_WhitespaceFieldName_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems();

            Assert.ThrowsException<ArgumentException>(() => source.ParseListOfTInDynamic(new[] { "Id", "   " }));
        }

        [TestMethod]
        [Timeout(10000)]
        public void ParseListOfTInDynamic_PrivateProperty_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems();

            Assert.ThrowsException<ArgumentException>(() => source.ParseListOfTInDynamic(new[] { "Id", "Secret" }));
        }

        [TestMethod]
        [Timeout(10000)]
        public void ParseListOfTInDynamic_PublicField_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems();

            Assert.ThrowsException<ArgumentException>(() => source.ParseListOfTInDynamic(new[] { "Id", "Code" }));
        }

        [TestMethod]
        [Timeout(10000)]
        public void ParseListOfTInDynamic_UnknownField_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems();

            var exception = Assert.ThrowsException<ArgumentException>(
                () => source.ParseListOfTInDynamic(new[] { "Id", "Missing" }));

            StringAssert.Contains(exception.Message, "Missing");
        }

        [TestMethod]
        [Timeout(10000)]
        public void ParseListOfTInDynamic_DuplicateField_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems();

            Assert.ThrowsException<ArgumentException>(() => source.ParseListOfTInDynamic(new[] { "Name", "Name" }));
        }

        [TestMethod]
        [Timeout(10000)]
        public void ParseListOfTInDynamic_ItemProperty_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems();

            var selected = source.ParseListOfTInDynamic(new[] { "Id", "Item" });

            Assert.AreEqual(3, selected.Count);
            Assert.AreEqual(2, (int)selected[0].Id);
            Assert.AreEqual("I-2", (string)selected[0].Item);
            Assert.AreEqual("I-1", (string)selected[1].Item);
            Assert.AreEqual("I-3", (string)selected[2].Item);
        }

        [TestMethod]
        [Timeout(10000)]
        public void ParseListOfTInDynamic_IdAndName_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems();

            var selected = source.ParseListOfTInDynamic(new[] { "Id", "Name" });

            Assert.AreEqual(3, selected.Count);
            Assert.AreEqual(2, (int)selected[0].Id);
            Assert.AreEqual("Beta", (string)selected[0].Name);
            Assert.AreEqual(1, (int)selected[1].Id);
            Assert.AreEqual("Alpha", (string)selected[1].Name);
            Assert.AreEqual(3, (int)selected[2].Id);
            Assert.AreEqual("Gamma", (string)selected[2].Name);
        }

        [TestMethod]
        [Timeout(10000)]
        public void ParseEnumerableOfTInDynamic_EmptyFieldList_Test()
        {
            IEnumerable<DynamicMemberTestHelper> source = DynamicMemberTestHelper.CreateItems();

            Assert.ThrowsException<ArgumentException>(() => source.ParseEnumerableOfTInDynamic(new string[0]));
        }

        [TestMethod]
        [Timeout(10000)]
        public void ParseEnumerableOfTInDynamic_EmptyFieldName_Test()
        {
            IEnumerable<DynamicMemberTestHelper> source = DynamicMemberTestHelper.CreateItems();

            var exception = Assert.ThrowsException<ArgumentException>(
                () => source.ParseEnumerableOfTInDynamic(new[] { "Id", "" }));

            Assert.AreEqual("fields", exception.ParamName);
        }

        [TestMethod]
        [Timeout(10000)]
        public void ParseEnumerableOfTInDynamic_WhitespaceFieldName_Test()
        {
            IEnumerable<DynamicMemberTestHelper> source = DynamicMemberTestHelper.CreateItems();

            Assert.ThrowsException<ArgumentException>(() => source.ParseEnumerableOfTInDynamic(new[] { "Id", "   " }));
        }

        [TestMethod]
        [Timeout(10000)]
        public void ParseEnumerableOfTInDynamic_PrivateProperty_Test()
        {
            IEnumerable<DynamicMemberTestHelper> source = DynamicMemberTestHelper.CreateItems();

            Assert.ThrowsException<ArgumentException>(() => source.ParseEnumerableOfTInDynamic(new[] { "Id", "Secret" }));
        }

        [TestMethod]
        [Timeout(10000)]
        public void ParseEnumerableOfTInDynamic_PublicField_Test()
        {
            IEnumerable<DynamicMemberTestHelper> source = DynamicMemberTestHelper.CreateItems();

            Assert.ThrowsException<ArgumentException>(() => source.ParseEnumerableOfTInDynamic(new[] { "Id", "Code" }));
        }

        [TestMethod]
        [Timeout(10000)]
        public void ParseEnumerableOfTInDynamic_UnknownField_Test()
        {
            IEnumerable<DynamicMemberTestHelper> source = DynamicMemberTestHelper.CreateItems();

            var exception = Assert.ThrowsException<ArgumentException>(
                () => source.ParseEnumerableOfTInDynamic(new[] { "Id", "Missing" }));

            StringAssert.Contains(exception.Message, "Missing");
        }

        [TestMethod]
        [Timeout(10000)]
        public void ParseEnumerableOfTInDynamic_DuplicateField_Test()
        {
            IEnumerable<DynamicMemberTestHelper> source = DynamicMemberTestHelper.CreateItems();

            Assert.ThrowsException<ArgumentException>(() => source.ParseEnumerableOfTInDynamic(new[] { "Name", "Name" }));
        }

        [TestMethod]
        [Timeout(10000)]
        public void ParseEnumerableOfTInDynamic_ItemProperty_Test()
        {
            IEnumerable<DynamicMemberTestHelper> source = DynamicMemberTestHelper.CreateItems();

            var selected = source.ParseEnumerableOfTInDynamic(new[] { "Id", "Item" });

            Assert.AreEqual(3, selected.Count);
            Assert.AreEqual(2, (int)selected[0].Id);
            Assert.AreEqual("I-2", (string)selected[0].Item);
            Assert.AreEqual("I-1", (string)selected[1].Item);
            Assert.AreEqual("I-3", (string)selected[2].Item);
        }

        [TestMethod]
        [Timeout(10000)]
        public void ParseEnumerableOfTInDynamic_IdAndName_Test()
        {
            IEnumerable<DynamicMemberTestHelper> source = DynamicMemberTestHelper.CreateItems();

            var selected = source.ParseEnumerableOfTInDynamic(new[] { "Id", "Name" });

            Assert.AreEqual(3, selected.Count);
            Assert.AreEqual(2, (int)selected[0].Id);
            Assert.AreEqual("Beta", (string)selected[0].Name);
            Assert.AreEqual(1, (int)selected[1].Id);
            Assert.AreEqual("Alpha", (string)selected[1].Name);
            Assert.AreEqual(3, (int)selected[2].Id);
            Assert.AreEqual("Gamma", (string)selected[2].Name);
        }

        [TestMethod]
        [Timeout(10000)]
        public void SelectProperty_EmptyName_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            var exception = Assert.ThrowsException<ArgumentException>(
                () => source.SelectProperty("").Cast<object>().ToList());

            Assert.AreEqual("prop", exception.ParamName);
        }

        [TestMethod]
        [Timeout(10000)]
        public void SelectProperty_WhitespaceName_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            Assert.ThrowsException<ArgumentException>(() => source.SelectProperty("   ").Cast<object>().ToList());
        }

        [TestMethod]
        [Timeout(10000)]
        public void SelectProperty_PrivateProperty_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            Assert.ThrowsException<ArgumentException>(() => source.SelectProperty("Secret").Cast<object>().ToList());
        }

        [TestMethod]
        [Timeout(10000)]
        public void SelectProperty_PublicField_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            Assert.ThrowsException<ArgumentException>(() => source.SelectProperty("Code").Cast<object>().ToList());
        }

        [TestMethod]
        [Timeout(10000)]
        public void SelectProperty_UnknownName_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            var exception = Assert.ThrowsException<ArgumentException>(
                () => source.SelectProperty("Missing").Cast<object>().ToList());

            StringAssert.Contains(exception.Message, "Missing");
        }

        [TestMethod]
        [Timeout(10000)]
        public void SelectProperty_LowerCaseName_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            var selected = source.SelectProperty("name").Cast<string>().ToList();

            CollectionAssert.AreEqual(new[] { "Beta", "Alpha", "Gamma" }, selected);
        }

        [TestMethod]
        [Timeout(10000)]
        public void SelectProperty_ItemProperty_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            var selected = source.SelectProperty("Item").Cast<string>().ToList();

            CollectionAssert.AreEqual(new[] { "I-2", "I-1", "I-3" }, selected);
        }

        [TestMethod]
        [Timeout(10000)]
        public void SelectProperty_IdProperty_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            var selected = source.SelectProperty("Id").Cast<int>().ToList();

            CollectionAssert.AreEqual(new[] { 2, 1, 3 }, selected);
        }

        [TestMethod]
        [Timeout(10000)]
        public void SelectMultipleProperties_EmptyFieldList_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            Assert.ThrowsException<ArgumentException>(() => source.SelectMultipleProperties().Cast<object>().ToList());
        }

        [TestMethod]
        [Timeout(10000)]
        public void SelectMultipleProperties_EmptyFieldName_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            var exception = Assert.ThrowsException<ArgumentException>(
                () => source.SelectMultipleProperties("Id", "").Cast<object>().ToList());

            Assert.AreEqual("props", exception.ParamName);
        }

        [TestMethod]
        [Timeout(10000)]
        public void SelectMultipleProperties_PrivateProperty_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            Assert.ThrowsException<ArgumentException>(
                () => source.SelectMultipleProperties("Id", "Secret").Cast<object>().ToList());
        }

        [TestMethod]
        [Timeout(10000)]
        public void SelectMultipleProperties_PublicField_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            Assert.ThrowsException<ArgumentException>(
                () => source.SelectMultipleProperties("Id", "Code").Cast<object>().ToList());
        }

        [TestMethod]
        [Timeout(10000)]
        public void SelectMultipleProperties_UnknownField_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            var exception = Assert.ThrowsException<ArgumentException>(
                () => source.SelectMultipleProperties("Id", "Missing").Cast<object>().ToList());

            StringAssert.Contains(exception.Message, "Missing");
        }

        [TestMethod]
        [Timeout(10000)]
        public void SelectMultipleProperties_DuplicateField_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            Assert.ThrowsException<ArgumentException>(
                () => source.SelectMultipleProperties("Name", "Name").Cast<object>().ToList());
        }

        [TestMethod]
        [Timeout(10000)]
        public void SelectMultipleProperties_IdAndName_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            var selected = source.SelectMultipleProperties("Id", "Name").Cast<DynamicMemberTestHelper>().ToList();

            CollectionAssert.AreEqual(new[] { 2, 1, 3 }, selected.Select(x => x.Id).ToList());
            CollectionAssert.AreEqual(new[] { "Beta", "Alpha", "Gamma" }, selected.Select(x => x.Name).ToList());
            Assert.IsTrue(selected.All(x => x.Item == null));
        }

        [TestMethod]
        [Timeout(10000)]
        public void SelectMultipleProperties_ItemProperty_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            var selected = source.SelectMultipleProperties("Id", "Item").Cast<DynamicMemberTestHelper>().ToList();

            CollectionAssert.AreEqual(new[] { "I-2", "I-1", "I-3" }, selected.Select(x => x.Item).ToList());
        }

        [TestMethod]
        [Timeout(10000)]
        public void OrderByDynamic_EmptyMember_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            var exception = Assert.ThrowsException<ArgumentException>(
                () => source.OrderByDynamic("", OrderType.Asc).ToList());

            Assert.AreEqual("orderByMember", exception.ParamName);
        }

        [TestMethod]
        [Timeout(10000)]
        public void OrderByDynamic_WhitespaceMember_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            Assert.ThrowsException<ArgumentException>(() => source.OrderByDynamic("   ", OrderType.Asc).ToList());
        }

        [TestMethod]
        [Timeout(10000)]
        public void OrderByDynamic_PrivateProperty_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            Assert.ThrowsException<ArgumentException>(() => source.OrderByDynamic("Secret", OrderType.Asc).ToList());
        }

        [TestMethod]
        [Timeout(10000)]
        public void OrderByDynamic_PublicField_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            Assert.ThrowsException<ArgumentException>(() => source.OrderByDynamic("Code", OrderType.Asc).ToList());
        }

        [TestMethod]
        [Timeout(10000)]
        public void OrderByDynamic_UnknownMember_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            var exception = Assert.ThrowsException<ArgumentException>(
                () => source.OrderByDynamic("Missing", OrderType.Asc).ToList());

            StringAssert.Contains(exception.Message, "Missing");
        }

        [TestMethod]
        [Timeout(10000)]
        public void OrderByDynamic_LowerCaseMember_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            var ordered = source.OrderByDynamic("name", OrderType.Asc).ToList();

            CollectionAssert.AreEqual(new[] { "Alpha", "Beta", "Gamma" }, ordered.Select(x => x.Name).ToList());
        }

        [TestMethod]
        [Timeout(10000)]
        public void OrderByDynamic_IdAscending_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            var ordered = source.OrderByDynamic("Id", OrderType.Asc).ToList();

            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, ordered.Select(x => x.Id).ToList());
        }

        [TestMethod]
        [Timeout(10000)]
        public void OrderByDynamic_NameDescending_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            var ordered = source.OrderByDynamic("Name", OrderType.Desc).ToList();

            CollectionAssert.AreEqual(new[] { "Gamma", "Beta", "Alpha" }, ordered.Select(x => x.Name).ToList());
        }

        [TestMethod]
        [Timeout(10000)]
        public void OrderByDynamic_ItemProperty_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            var ordered = source.OrderByDynamic("Item", OrderType.Desc).ToList();

            CollectionAssert.AreEqual(new[] { "I-3", "I-2", "I-1" }, ordered.Select(x => x.Item).ToList());
        }

        [TestMethod]
        [Timeout(10000)]
        public void ParseListOfTInDynamic_NullFields_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems();

            var selected = source.ParseListOfTInDynamic();

            object first = selected[0];
            var names = first.GetType().GetProperties()
                .Where(x => x.GetIndexParameters().Length == 0)
                .Select(x => x.Name)
                .OrderBy(x => x, StringComparer.Ordinal)
                .ToList();
            CollectionAssert.AreEqual(new[] { "Id", "Item", "Name" }, names);
            Assert.AreEqual(3, selected.Count);
            Assert.AreEqual(2, (int)selected[0].Id);
            Assert.AreEqual("Beta", (string)selected[0].Name);
            Assert.AreEqual("I-2", (string)selected[0].Item);
            Assert.AreEqual(3, (int)selected[2].Id);
            Assert.AreEqual("Gamma", (string)selected[2].Name);
            Assert.AreEqual("I-3", (string)selected[2].Item);
        }

        [TestMethod]
        [Timeout(10000)]
        public void ParseEnumerableOfTInDynamic_NullFields_Test()
        {
            IEnumerable<DynamicMemberTestHelper> source = DynamicMemberTestHelper.CreateItems();

            var selected = source.ParseEnumerableOfTInDynamic();

            object first = selected[0];
            var names = first.GetType().GetProperties()
                .Where(x => x.GetIndexParameters().Length == 0)
                .Select(x => x.Name)
                .OrderBy(x => x, StringComparer.Ordinal)
                .ToList();
            CollectionAssert.AreEqual(new[] { "Id", "Item", "Name" }, names);
            Assert.AreEqual(3, selected.Count);
            Assert.AreEqual(2, (int)selected[0].Id);
            Assert.AreEqual("Beta", (string)selected[0].Name);
            Assert.AreEqual("I-2", (string)selected[0].Item);
            Assert.AreEqual(3, (int)selected[2].Id);
            Assert.AreEqual("Gamma", (string)selected[2].Name);
            Assert.AreEqual("I-3", (string)selected[2].Item);
        }

        [TestMethod]
        [Timeout(10000)]
        public void ParseListOfTInDynamic_NameThenId_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems();

            var selected = source.ParseListOfTInDynamic(new[] { "Name", "Id" });

            Assert.AreEqual(3, selected.Count);
            Assert.AreEqual(2, (int)selected[0].Id);
            Assert.AreEqual("Beta", (string)selected[0].Name);
            Assert.AreEqual(1, (int)selected[1].Id);
            Assert.AreEqual("Alpha", (string)selected[1].Name);
            Assert.AreEqual(3, (int)selected[2].Id);
            Assert.AreEqual("Gamma", (string)selected[2].Name);
        }

        [TestMethod]
        [Timeout(10000)]
        public void ParseEnumerableOfTInDynamic_NameThenId_Test()
        {
            IEnumerable<DynamicMemberTestHelper> source = DynamicMemberTestHelper.CreateItems();

            var selected = source.ParseEnumerableOfTInDynamic(new[] { "Name", "Id" });

            Assert.AreEqual(3, selected.Count);
            Assert.AreEqual(2, (int)selected[0].Id);
            Assert.AreEqual("Beta", (string)selected[0].Name);
            Assert.AreEqual(1, (int)selected[1].Id);
            Assert.AreEqual("Alpha", (string)selected[1].Name);
            Assert.AreEqual(3, (int)selected[2].Id);
            Assert.AreEqual("Gamma", (string)selected[2].Name);
        }

        [TestMethod]
        [Timeout(10000)]
        public void SelectMultipleProperties_NameThenId_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            var selected = source.SelectMultipleProperties("Name", "Id").Cast<DynamicMemberTestHelper>().ToList();

            CollectionAssert.AreEqual(new[] { 2, 1, 3 }, selected.Select(x => x.Id).ToList());
            CollectionAssert.AreEqual(new[] { "Beta", "Alpha", "Gamma" }, selected.Select(x => x.Name).ToList());
        }

        [TestMethod]
        [Timeout(10000)]
        public void OrderByDynamic_UndefinedDirection_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => source.OrderByDynamic("Id", (OrderType)99).ToList());
        }

        [TestMethod]
        [Timeout(10000)]
        public void ParseListOfTInDynamic_CaseVariantDuplicate_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems();

            Assert.ThrowsException<ArgumentException>(() => source.ParseListOfTInDynamic(new[] { "Name", "name" }));
        }

        [TestMethod]
        [Timeout(10000)]
        public void ParseEnumerableOfTInDynamic_CaseVariantDuplicate_Test()
        {
            IEnumerable<DynamicMemberTestHelper> source = DynamicMemberTestHelper.CreateItems();

            Assert.ThrowsException<ArgumentException>(() => source.ParseEnumerableOfTInDynamic(new[] { "Name", "name" }));
        }

        [TestMethod]
        [Timeout(10000)]
        public void SelectMultipleProperties_CaseVariantDuplicate_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            Assert.ThrowsException<ArgumentException>(
                () => source.SelectMultipleProperties("Name", "name").Cast<object>().ToList());
        }

        [TestMethod]
        [Timeout(10000)]
        public void ParseListOfTInDynamic_AmbiguousCaseField_Test()
        {
            var source = AmbiguousCaseTestHelper.CreateItems();

            Assert.ThrowsException<ArgumentException>(() => source.ParseListOfTInDynamic(new[] { "value" }));
        }

        [TestMethod]
        [Timeout(10000)]
        public void ParseEnumerableOfTInDynamic_AmbiguousCaseField_Test()
        {
            IEnumerable<AmbiguousCaseTestHelper> source = AmbiguousCaseTestHelper.CreateItems();

            Assert.ThrowsException<ArgumentException>(() => source.ParseEnumerableOfTInDynamic(new[] { "value" }));
        }

        [TestMethod]
        [Timeout(10000)]
        public void SelectProperty_AmbiguousCaseName_Test()
        {
            var source = AmbiguousCaseTestHelper.CreateItems().AsQueryable();

            Assert.ThrowsException<ArgumentException>(() => source.SelectProperty("value").Cast<object>().ToList());
        }

        [TestMethod]
        [Timeout(10000)]
        public void SelectMultipleProperties_AmbiguousCaseField_Test()
        {
            var source = AmbiguousCaseTestHelper.CreateItems().AsQueryable();

            Assert.ThrowsException<ArgumentException>(
                () => source.SelectMultipleProperties("value").Cast<object>().ToList());
        }

        [TestMethod]
        [Timeout(10000)]
        public void OrderByDynamic_AmbiguousCaseMember_Test()
        {
            var source = AmbiguousCaseTestHelper.CreateItems().AsQueryable();

            Assert.ThrowsException<ArgumentException>(() => source.OrderByDynamic("value", OrderType.Asc).ToList());
        }

        [TestMethod]
        [Timeout(10000)]
        public void ParseListOfTInDynamic_LowerCaseField_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems();

            var selected = source.ParseListOfTInDynamic(new[] { "Id", "name" });

            Assert.AreEqual(3, selected.Count);
            Assert.AreEqual(2, (int)selected[0].Id);
            Assert.AreEqual("Beta", (string)selected[0].Name);
            Assert.AreEqual("Alpha", (string)selected[1].Name);
            Assert.AreEqual("Gamma", (string)selected[2].Name);
        }

        [TestMethod]
        [Timeout(10000)]
        public void ParseEnumerableOfTInDynamic_LowerCaseField_Test()
        {
            IEnumerable<DynamicMemberTestHelper> source = DynamicMemberTestHelper.CreateItems();

            var selected = source.ParseEnumerableOfTInDynamic(new[] { "Id", "name" });

            Assert.AreEqual(3, selected.Count);
            Assert.AreEqual(2, (int)selected[0].Id);
            Assert.AreEqual("Beta", (string)selected[0].Name);
            Assert.AreEqual("Alpha", (string)selected[1].Name);
            Assert.AreEqual("Gamma", (string)selected[2].Name);
        }

        [TestMethod]
        [Timeout(10000)]
        public void SelectMultipleProperties_LowerCaseField_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            var selected = source.SelectMultipleProperties("Id", "name").Cast<DynamicMemberTestHelper>().ToList();

            CollectionAssert.AreEqual(new[] { 2, 1, 3 }, selected.Select(x => x.Id).ToList());
            CollectionAssert.AreEqual(new[] { "Beta", "Alpha", "Gamma" }, selected.Select(x => x.Name).ToList());
        }

        [TestMethod]
        [Timeout(10000)]
        public void ParseListOfTInDynamic_HiddenMemberAllFields_Test()
        {
            var source = HiddenMemberDerivedTestHelper.CreateItems();

            var selected = source.ParseListOfTInDynamic();

            object first = selected[0];
            var idProperties = first.GetType().GetProperties()
                .Where(x => x.Name == "Id" && x.GetIndexParameters().Length == 0)
                .ToList();
            Assert.AreEqual(1, idProperties.Count);
            Assert.AreEqual(typeof(string), idProperties[0].PropertyType);
            Assert.AreEqual(2, selected.Count);
            Assert.AreEqual("D-1", (string)selected[0].Id);
            Assert.AreEqual("D-2", (string)selected[1].Id);
        }

        [TestMethod]
        [Timeout(10000)]
        public void ParseListOfTInDynamic_HiddenMemberField_Test()
        {
            var source = HiddenMemberDerivedTestHelper.CreateItems();

            var selected = source.ParseListOfTInDynamic(new[] { "Id" });

            object first = selected[0];
            var idProperties = first.GetType().GetProperties()
                .Where(x => x.Name == "Id" && x.GetIndexParameters().Length == 0)
                .ToList();
            Assert.AreEqual(1, idProperties.Count);
            Assert.AreEqual(typeof(string), idProperties[0].PropertyType);
            Assert.AreEqual(2, selected.Count);
            Assert.AreEqual("D-1", (string)selected[0].Id);
            Assert.AreEqual("D-2", (string)selected[1].Id);
        }
    }
}
