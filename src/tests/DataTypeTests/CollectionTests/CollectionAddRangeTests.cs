using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Collections;

namespace DataTypeTests.CollectionTests
{
    [TestClass]
    public class CollectionAddRangeTests
    {
        [TestMethod]
        public void AddRange_NullContext_Test()
        {
            ICollection<int> context = null;

            var result = context.AddRange(new[] { 1, 2 });

            Assert.IsNull(result);
        }

        [TestMethod]
        public void AddRange_NullData_Test()
        {
            ICollection<int> context = new List<int> { 1, 2 };

            var result = context.AddRange((IEnumerable<int>)null);

            Assert.AreSame(context, result);
            CollectionAssert.AreEqual(new[] { 1, 2 }, context.ToArray());
        }

        [TestMethod]
        public void AddRange_EmptyCollection_Test()
        {
            ICollection<int> context = new Collection<int>();

            var result = context.AddRange(new[] { 1, 2, 3 });

            Assert.AreSame(context, result);
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, context.ToArray());
        }

        [TestMethod]
        public void AddRange_EmptyListTypedAsCollection_Test()
        {
            ICollection<int> context = new List<int>();

            var result = context.AddRange(new[] { 1, 2, 3 });

            Assert.AreSame(context, result);
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, context.ToArray());
        }

        [TestMethod]
        public void AddRange_NonEmptyCollection_Test()
        {
            ICollection<int> context = new Collection<int> { 1, 2 };

            var result = context.AddRange(new[] { 3, 4 });

            Assert.AreSame(context, result);
            CollectionAssert.AreEqual(new[] { 1, 2, 3, 4 }, context.ToArray());
        }

        [TestMethod]
        public void AddRange_EmptyReadOnlyCollection_Test()
        {
            ICollection<int> context = new int[0];

            var result = context.AddRange(new[] { 1, 2 });

            Assert.IsNull(result);
            Assert.AreEqual(0, context.Count);
        }

        [TestMethod]
        public void With_NullSourceAndItems_Test()
        {
            ICollection<int> source = null;

            var result = source.With(new[] { 1, 2 });

            Assert.IsNotNull(result);
            CollectionAssert.AreEqual(new[] { 1, 2 }, result.ToArray());
        }

        [TestMethod]
        public void With_EmptySourceAndItems_Test()
        {
            ICollection<int> source = new Collection<int>();

            var result = source.With(new[] { 1, 2 });

            Assert.AreSame(source, result);
            CollectionAssert.AreEqual(new[] { 1, 2 }, source.ToArray());
        }

        [TestMethod]
        public void WithMany_NullSourceAndItems_Test()
        {
            ICollection<int> source = null;

            var result = source.WithMany(new List<int> { 1, 2 });

            Assert.IsNotNull(result);
            CollectionAssert.AreEqual(new[] { 1, 2 }, result.ToArray());
        }

        [TestMethod]
        public void WithMany_EmptySourceAndItems_Test()
        {
            ICollection<int> source = new Collection<int>();

            var result = source.WithMany(new List<int> { 1, 2 });

            Assert.AreSame(source, result);
            CollectionAssert.AreEqual(new[] { 1, 2 }, source.ToArray());
        }

        [TestMethod]
        public void WithMany_NonEmptySourceAndItems_Test()
        {
            ICollection<int> source = new Collection<int> { 1 };

            var result = source.WithMany(new List<int> { 2, 3 });

            Assert.AreSame(source, result);
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, source.ToArray());
        }
    }
}
