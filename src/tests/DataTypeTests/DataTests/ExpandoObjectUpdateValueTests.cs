using System.Collections.Generic;
using System.Dynamic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Data;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class ExpandoObjectUpdateValueTests
    {
        [TestMethod]
        public void UpdateValue_NestedChildHasKey_Test()
        {
            var root = new ExpandoObject();
            var child = new ExpandoObject();
            var rootMap = (IDictionary<string, object>)root;
            var childMap = (IDictionary<string, object>)child;
            childMap["x"] = 2;
            childMap["y"] = 3;
            rootMap["x"] = 1;
            rootMap["child"] = child;

            root.UpdateValue("x", 9);

            Assert.AreEqual(9, rootMap["x"]);
            Assert.AreEqual(9, childMap["x"]);
            Assert.AreEqual(3, childMap["y"]);
            Assert.AreSame(child, rootMap["child"]);
        }

        [TestMethod]
        public void UpdateValue_KeyMissingAtRoot_Test()
        {
            var root = new ExpandoObject();
            var child = new ExpandoObject();
            var rootMap = (IDictionary<string, object>)root;
            var childMap = (IDictionary<string, object>)child;
            childMap["x"] = 2;
            childMap["y"] = 3;
            rootMap["x"] = 1;
            rootMap["child"] = child;

            root.UpdateValue("y", 7);

            Assert.IsFalse(rootMap.ContainsKey("y"));
            Assert.AreEqual(2, rootMap.Count);
            Assert.AreEqual(1, rootMap["x"]);
            Assert.AreEqual(7, childMap["y"]);
        }

        [TestMethod]
        public void UpdateValue_KeyMissingEverywhere_Test()
        {
            var root = new ExpandoObject();
            var child = new ExpandoObject();
            var rootMap = (IDictionary<string, object>)root;
            var childMap = (IDictionary<string, object>)child;
            childMap["x"] = 2;
            rootMap["x"] = 1;
            rootMap["child"] = child;

            root.UpdateValue("z", 7);

            Assert.IsFalse(rootMap.ContainsKey("z"));
            Assert.IsFalse(childMap.ContainsKey("z"));
            Assert.AreEqual(1, rootMap["x"]);
            Assert.AreEqual(2, childMap["x"]);
        }

        [TestMethod]
        [Timeout(10000)]
        [TestCategory("RedByConstruction")]
        public void UpdateValue_ParentChildCycle_Test()
        {
            var a = new ExpandoObject();
            var b = new ExpandoObject();
            var aMap = (IDictionary<string, object>)a;
            var bMap = (IDictionary<string, object>)b;
            aMap["x"] = 1;
            bMap["x"] = 2;
            aMap["child"] = b;
            bMap["parent"] = a;

            a.UpdateValue("x", 5);

            Assert.AreEqual(5, aMap["x"]);
            Assert.AreEqual(5, bMap["x"]);
            Assert.AreSame(b, aMap["child"]);
            Assert.AreSame(a, bMap["parent"]);
        }

        [TestMethod]
        [Timeout(10000)]
        [TestCategory("RedByConstruction")]
        public void UpdateValue_SelfReference_Test()
        {
            var a = new ExpandoObject();
            var aMap = (IDictionary<string, object>)a;
            aMap["x"] = 1;
            aMap["self"] = a;

            a.UpdateValue("x", 5);

            Assert.AreEqual(5, aMap["x"]);
            Assert.AreSame(a, aMap["self"]);
        }

        [TestMethod]
        [Timeout(10000)]
        public void UpdateValue_SharedNode_Test()
        {
            var a = new ExpandoObject();
            var s = new ExpandoObject();
            var aMap = (IDictionary<string, object>)a;
            var sMap = (IDictionary<string, object>)s;
            aMap["x"] = 1;
            sMap["x"] = 2;
            aMap["l"] = s;
            aMap["r"] = s;

            a.UpdateValue("x", 5);

            Assert.AreEqual(5, aMap["x"]);
            Assert.AreEqual(5, sMap["x"]);
            Assert.AreSame(s, aMap["l"]);
            Assert.AreSame(s, aMap["r"]);
        }

        [TestMethod]
        [Timeout(30000)]
        [TestCategory("RedByConstruction")]
        public void UpdateValue_DeepNesting_Test()
        {
            var root = new ExpandoObject();
            var rootMap = (IDictionary<string, object>)root;
            rootMap["x"] = 0;
            var currentMap = rootMap;
            for (var i = 1; i < 100000; i++)
            {
                var next = new ExpandoObject();
                var nextMap = (IDictionary<string, object>)next;
                nextMap["x"] = i;
                currentMap["child"] = next;
                currentMap = nextMap;
            }

            root.UpdateValue("x", -1);

            Assert.AreEqual(-1, rootMap["x"]);
            Assert.AreEqual(-1, currentMap["x"]);
            Assert.IsFalse(currentMap.ContainsKey("child"));
        }
    }
}
