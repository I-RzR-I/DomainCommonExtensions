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
    public class OrderByDynamicAllowListTests
    {
        [TestMethod]
        public void OrderByDynamic_DefaultOverload_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            var ordered = source.OrderByDynamic("Id", OrderType.Desc).ToList();

            CollectionAssert.AreEqual(new[] { 3, 2, 1 }, ordered.Select(x => x.Id).ToList());
        }

        [TestMethod]
        public void OrderByDynamic_AllowedAscending_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            var ordered = source.OrderByDynamic("Id", OrderType.Asc, new[] { "Id", "Name" }).ToList();

            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, ordered.Select(x => x.Id).ToList());
            CollectionAssert.AreEqual(new[] { "Alpha", "Beta", "Gamma" }, ordered.Select(x => x.Name).ToList());
        }

        [TestMethod]
        public void OrderByDynamic_AllowedDescending_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            var ordered = source.OrderByDynamic("Name", OrderType.Desc, new[] { "Id", "Name" }).ToList();

            CollectionAssert.AreEqual(new[] { "Gamma", "Beta", "Alpha" }, ordered.Select(x => x.Name).ToList());
            CollectionAssert.AreEqual(new[] { 3, 2, 1 }, ordered.Select(x => x.Id).ToList());
        }

        [TestMethod]
        public void OrderByDynamic_NotAllowedMember_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            var exception = Assert.ThrowsException<ArgumentException>(
                () => source.OrderByDynamic("Name", OrderType.Asc, new[] { "Id" }).ToList());

            Assert.AreEqual("orderByMember", exception.ParamName);
        }

        [TestMethod]
        public void OrderByDynamic_NotAllowedSameMessage_Test()
        {
            var existing = DynamicMemberTestHelper.CreateItems().AsQueryable();
            var missing = AmbiguousCaseTestHelper.CreateItems().AsQueryable();

            var existingException = Assert.ThrowsException<ArgumentException>(
                () => existing.OrderByDynamic("Name", OrderType.Asc, new[] { "Id" }));
            var missingException = Assert.ThrowsException<ArgumentException>(
                () => missing.OrderByDynamic("Name", OrderType.Asc, new[] { "Id" }));

            Assert.AreEqual(missingException.Message, existingException.Message);
        }

        [TestMethod]
        public void OrderByDynamic_UnknownAllowedMember_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            Assert.ThrowsException<ArgumentException>(
                () => source.OrderByDynamic("Missing", OrderType.Asc, new[] { "Missing" }).ToList());
        }

        [TestMethod]
        public void OrderByDynamic_AllowedPrivateProperty_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            Assert.ThrowsException<ArgumentException>(
                () => source.OrderByDynamic("Secret", OrderType.Asc, new[] { "Secret" }).ToList());
        }

        [TestMethod]
        public void OrderByDynamic_AllowedPublicField_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            Assert.ThrowsException<ArgumentException>(
                () => source.OrderByDynamic("Code", OrderType.Asc, new[] { "Code" }).ToList());
        }

        [TestMethod]
        public void OrderByDynamic_LowerCaseRequest_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            var ordered = source.OrderByDynamic("name", OrderType.Asc, new[] { "Name" }).ToList();

            CollectionAssert.AreEqual(new[] { "Alpha", "Beta", "Gamma" }, ordered.Select(x => x.Name).ToList());
        }

        [TestMethod]
        public void OrderByDynamic_CaseVariantRequest_Test()
        {
            var source = AllowListTestHelper.CreateCrossedCaseItems().AsQueryable();

            var ordered = source.OrderByDynamic("VALUE", OrderType.Asc, new[] { "Value" }).ToList();

            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, ordered.Select(x => x.Value).ToList());
        }

        [TestMethod]
        public void OrderByDynamic_ExactCaseVariant_Test()
        {
            var source = AllowListTestHelper.CreateCrossedCaseItems().AsQueryable();

            var ordered = source.OrderByDynamic("VALUE", OrderType.Asc, new[] { "Value", "VALUE" }).ToList();

            CollectionAssert.AreEqual(new[] { 10, 20, 30 }, ordered.Select(x => x.VALUE).ToList());
        }

        [TestMethod]
        public void OrderByDynamic_UpperCaseAllowList_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            Assert.ThrowsException<ArgumentException>(
                () => source.OrderByDynamic("NAME", OrderType.Asc, new[] { "NAME" }).ToList());
        }

        [TestMethod]
        public void OrderByDynamic_AmbiguousAllowList_Test()
        {
            var source = AllowListTestHelper.CreateCrossedCaseItems().AsQueryable();

            var ambiguousException = Assert.ThrowsException<ArgumentException>(
                () => source.OrderByDynamic("vAlUe", OrderType.Asc, new[] { "Value", "VALUE" }));
            var unknownException = Assert.ThrowsException<ArgumentException>(
                () => source.OrderByDynamic("vAlUe", OrderType.Asc, new[] { "Other" }));

            Assert.AreEqual(unknownException.Message, ambiguousException.Message);
        }

        [TestMethod]
        public void OrderByDynamic_NullAllowList_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();
            IEnumerable<string> allowed = null;

            Assert.ThrowsException<ArgumentNullException>(() => source.OrderByDynamic("Id", OrderType.Asc, allowed));
        }

        [TestMethod]
        public void OrderByDynamic_NullAllowedEntry_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            var exception = Assert.ThrowsException<ArgumentException>(
                () => source.OrderByDynamic("Id", OrderType.Asc, new[] { "Id", null }));

            Assert.AreEqual("allowedFields", exception.ParamName);
        }

        [TestMethod]
        public void OrderByDynamic_EmptyAllowedEntry_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            Assert.ThrowsException<ArgumentException>(
                () => source.OrderByDynamic("Id", OrderType.Asc, new[] { "Id", "" }));
        }

        [TestMethod]
        public void OrderByDynamic_WhitespaceAllowedEntry_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            Assert.ThrowsException<ArgumentException>(
                () => source.OrderByDynamic("Id", OrderType.Asc, new[] { "Id", "   " }));
        }

        [TestMethod]
        public void OrderByDynamic_EmptyAllowList_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            Assert.ThrowsException<ArgumentException>(
                () => source.OrderByDynamic("Id", OrderType.Asc, new string[0]));
        }

        [TestMethod]
        public void OrderByDynamic_NullMemberAllowList_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();
            string member = null;

            Assert.ThrowsException<ArgumentException>(
                () => source.OrderByDynamic(member, OrderType.Asc, new[] { "Id", "Name" }));
        }

        [TestMethod]
        public void OrderByDynamic_EmptyMemberAllowList_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            Assert.ThrowsException<ArgumentException>(
                () => source.OrderByDynamic("", OrderType.Asc, new[] { "Id", "Name" }));
        }

        [TestMethod]
        public void OrderByDynamic_WhitespaceMemberAllowList_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            Assert.ThrowsException<ArgumentException>(
                () => source.OrderByDynamic("   ", OrderType.Asc, new[] { "Id", "Name" }));
        }

        [TestMethod]
        public void OrderByDynamic_UndefinedDirectionAllowList_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => source.OrderByDynamic("Id", (OrderType)99, new[] { "Id" }));
        }

        [TestMethod]
        public void OrderByDynamic_UndefinedDirectionBeforeAllowCheck_Test()
        {
            var source = DynamicMemberTestHelper.CreateItems().AsQueryable();

            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => source.OrderByDynamic("Name", (OrderType)99, new[] { "Id" }));

            Assert.AreEqual("direction", exception.ParamName);
        }

        [TestMethod]
        public void OrderByDynamic_NullQueryAllowList_Test()
        {
            IQueryable<DynamicMemberTestHelper> source = null;

            Assert.ThrowsException<ArgumentNullException>(
                () => source.OrderByDynamic("Id", OrderType.Asc, new[] { "Id" }));
        }

        [TestMethod]
        public void OrderByDynamic_RejectedBeforeQuery_Test()
        {
            IQueryable<DynamicMemberTestHelper> source = new ThrowingQueryableTestHelper<DynamicMemberTestHelper>();

            Assert.ThrowsException<ArgumentException>(
                () => source.OrderByDynamic("Name", OrderType.Asc, new[] { "Id" }));
        }

        [TestMethod]
        public void OrderByDynamic_AllowedReadsQuery_Test()
        {
            IQueryable<DynamicMemberTestHelper> source = new ThrowingQueryableTestHelper<DynamicMemberTestHelper>();

            Assert.ThrowsException<InvalidOperationException>(
                () => source.OrderByDynamic("Id", OrderType.Asc, new[] { "Id" }));
        }
    }
}
