using System.Collections.Generic;
using DataTypeTests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Collections;
using RzR.Extensions.Domain.Models;

namespace DataTypeTests.HelperTests
{
    [TestClass]
    public class WithIndexModelDeconstructTests
    {
        [TestMethod]
        public void WithIndexModel_Deconstruct_ItemThenIndex_Test()
        {
            var model = new WithIndexModel<int> { Item = 42, Index = 7 };

            var (item, index) = model;

            Assert.AreEqual(42, item);
            Assert.AreEqual(7, index);
        }

        [TestMethod]
        public void WithIndexModel_Deconstruct_ExplicitCall_Test()
        {
            var model = new WithIndexModel<int> { Item = 42, Index = 7 };

            model.Deconstruct(out var item, out var index);

            Assert.AreEqual(42, item);
            Assert.AreEqual(7, index);
        }

        [TestMethod]
        public void WithIndexModel_Deconstruct_ReferenceItem_Test()
        {
            var instance = new IdNameActiveModel { Id = 1, Name = "Name", IsActive = true };
            var model = new WithIndexModel<IdNameActiveModel> { Item = instance, Index = 3 };

            var (item, index) = model;

            Assert.AreSame(instance, item);
            Assert.AreEqual(3, index);
        }

        [TestMethod]
        public void WithIndexModel_Deconstruct_DefaultReferenceType_Test()
        {
            var model = default(WithIndexModel<string>);

            var (item, index) = model;

            Assert.IsNull(item);
            Assert.AreEqual(0, index);
        }

        [TestMethod]
        public void WithIndexModel_Deconstruct_NullItem_Test()
        {
            var model = new WithIndexModel<string> { Item = null, Index = 5 };

            var (item, index) = model;

            Assert.IsNull(item);
            Assert.AreEqual(5, index);
        }

        [TestMethod]
        public void WithIndexModel_Deconstruct_DefaultValueType_Test()
        {
            var model = default(WithIndexModel<int>);

            var (item, index) = model;

            Assert.AreEqual(0, item);
            Assert.AreEqual(0, index);
        }

        [TestMethod]
        public void WithIndexModel_Deconstruct_Foreach_Test()
        {
            var source = new List<string> { "a", "b", "c" };
            var pairs = new List<string>();

            foreach (var (item, index) in source.WithIndexModel())
                pairs.Add($"{item}:{index}");

            CollectionAssert.AreEqual(new List<string> { "a:0", "b:1", "c:2" }, pairs);
        }

        [TestMethod]
        public void WithIndexModel_Deconstruct_ForeachNullSource_Test()
        {
            List<string> source = null;
            var pairs = new List<string>();

            foreach (var (item, index) in source.WithIndexModel())
                pairs.Add($"{item}:{index}");

            Assert.AreEqual(0, pairs.Count);
        }

        [TestMethod]
        public void WithIndexModel_Deconstruct_ForeachEmptySource_Test()
        {
            var source = new List<string>();
            var pairs = new List<string>();

            foreach (var (item, index) in source.WithIndexModel())
                pairs.Add($"{item}:{index}");

            Assert.AreEqual(0, pairs.Count);
        }

        [TestMethod]
        public void WithIndexModel_Deconstruct_Discards_Test()
        {
            var model = new WithIndexModel<string> { Item = "x", Index = 9 };

            var (item, _) = model;
            var (_, index) = model;

            Assert.AreEqual("x", item);
            Assert.AreEqual(9, index);
        }

        [TestMethod]
        public void WithIndexModel_Deconstruct_CopySemantics_Test()
        {
            var model = new WithIndexModel<int> { Item = 42, Index = 7 };

            var (item, index) = model;
            model.Item = 100;
            model.Index = 200;

            Assert.AreEqual(42, item);
            Assert.AreEqual(7, index);

            item = 1;
            index = 2;

            Assert.AreEqual(100, model.Item);
            Assert.AreEqual(200, model.Index);
        }

        [TestMethod]
        public void WithIndexModel_Deconstruct_ParityWithWithIndex_Test()
        {
            var source = new List<int> { 10, 20, 30 };
            var expected = new List<(int, int)>();
            var actual = new List<(int, int)>();

            foreach (var (item, index) in source.WithIndex())
                expected.Add((item, index));

            foreach (var (item, index) in source.WithIndexModel())
                actual.Add((item, index));

            Assert.AreEqual(3, actual.Count);
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
