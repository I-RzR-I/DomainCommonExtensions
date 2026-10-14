using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Primitives;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class NullKeyValuePairTests
    {
        [TestMethod]
        public void IsNull_DefaultPair_Test()
        {
            var pair = default(KeyValuePair<string, int>);

            var result = pair.IsNull();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsNull_MissingLookup_Test()
        {
            var source = new Dictionary<string, int> { { "a", 1 } };

            var result = source.FirstOrDefault(x => x.Key == "b").IsNull();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsNull_FoundDefaultValuedPair_Test()
        {
            var source = new Dictionary<int, int> { { 0, 0 } };

            var result = source.First().IsNull();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsNull_NonDefaultPair_Test()
        {
            var keyOnly = new KeyValuePair<string, int>("a", 0).IsNull();
            var valueOnly = new KeyValuePair<string, int>(null, 1).IsNull();
            var numeric = new KeyValuePair<int, int>(0, 1).IsNull();

            Assert.IsFalse(keyOnly);
            Assert.IsFalse(valueOnly);
            Assert.IsFalse(numeric);
        }
    }
}
