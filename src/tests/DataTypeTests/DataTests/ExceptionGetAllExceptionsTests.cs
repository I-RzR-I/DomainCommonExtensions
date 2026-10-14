using System;
using System.Linq;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Diagnostics;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class ExceptionGetAllExceptionsTests
    {
        [TestMethod]
        public void GetAllExceptions_NullException_Test()
        {
            Exception ex = null;

            var exception = Assert.ThrowsException<ArgumentNullException>(() => ex.GetAllExceptions().ToList());

            Assert.AreEqual("ex", exception.ParamName);
        }

        [TestMethod]
        public void GetAllExceptions_SingleException_Test()
        {
            var ex = new InvalidOperationException("single");

            var result = ex.GetAllExceptions().ToList();

            CollectionAssert.AreEqual(new Exception[] { ex }, result);
        }

        [TestMethod]
        public void GetAllExceptions_InnerChain_Test()
        {
            var innermost = new FormatException("c");
            var middle = new InvalidOperationException("b", innermost);
            var outer = new Exception("a", middle);

            var result = outer.GetAllExceptions().ToList();

            CollectionAssert.AreEqual(new Exception[] { outer, middle, innermost }, result);
        }

        [TestMethod]
        public void GetAllExceptions_AggregateDepthFirstPreOrder_Test()
        {
            var first = new InvalidOperationException("e1");
            var nested = new FormatException("e3");
            var second = new ArgumentException("e2", nested);
            var third = new NotSupportedException("e4");
            var aggregate = new AggregateException(first, second, third);

            var result = aggregate.GetAllExceptions().ToList();

            CollectionAssert.AreEqual(new Exception[] { aggregate, first, second, nested, third }, result);
        }

        [TestMethod]
        public void GetAllExceptions_AggregateInnerExceptionNotDuplicated_Test()
        {
            var first = new InvalidOperationException("e1");
            var second = new FormatException("e2");
            var aggregate = new AggregateException(first, second);
            Assert.AreSame(first, aggregate.InnerException);

            var result = aggregate.GetAllExceptions().ToList();

            Assert.AreEqual(1, result.Count(x => ReferenceEquals(x, first)));
            CollectionAssert.AreEqual(new Exception[] { aggregate, first, second }, result);
        }

        [TestMethod]
        public void GetAllExceptions_NestedAggregates_Test()
        {
            var x = new InvalidOperationException("x");
            var y = new FormatException("y");
            var z = new NotSupportedException("z");
            var innerAggregate = new AggregateException(x, y);
            var wrapper = new Exception("wrapper", innerAggregate);
            var outerAggregate = new AggregateException(wrapper, z);

            var result = outerAggregate.GetAllExceptions().ToList();

            CollectionAssert.AreEqual(new Exception[] { outerAggregate, wrapper, innerAggregate, x, y, z }, result);
        }

        [TestMethod]
        public void GetAllExceptions_RepeatedReferences_Test()
        {
            var shared = new InvalidOperationException("shared");
            var other = new FormatException("other", shared);
            var aggregate = new AggregateException(shared, shared, other);

            var result = aggregate.GetAllExceptions().ToList();

            CollectionAssert.AreEqual(new Exception[] { aggregate, shared, other }, result);
        }

        [TestMethod]
        public void GetAllExceptions_SharedExceptionListedAtFirstPosition_Test()
        {
            var shared = new InvalidOperationException("shared");
            var wrapper = new FormatException("wrapper", shared);
            var last = new NotSupportedException("last");
            var aggregate = new AggregateException(wrapper, shared, last);

            var result = aggregate.GetAllExceptions().ToList();

            CollectionAssert.AreEqual(new Exception[] { aggregate, wrapper, shared, last }, result);
        }

        [TestMethod]
        public void GetAllExceptions_EqualButDistinctInstances_Test()
        {
            var first = new EqualityOverridingException();
            var second = new EqualityOverridingException();
            var aggregate = new AggregateException(first, second);

            var result = aggregate.GetAllExceptions().ToList();

            Assert.AreEqual(3, result.Count);
            Assert.AreSame(first, result[1]);
            Assert.AreSame(second, result[2]);
        }

        [TestMethod]
        public void GetAllExceptions_DeepChain_Test()
        {
            const int depth = 1000;
            var innermost = new InvalidOperationException("innermost");
            Exception current = innermost;
            for (var i = 0; i < depth; i++)
                current = new Exception("level", current);

            var result = current.GetAllExceptions().ToList();

            Assert.AreEqual(depth + 1, result.Count);
            Assert.AreSame(current, result[0]);
            Assert.AreSame(innermost, result[depth]);
        }

        [TestMethod]
        public void GetAllExceptions_ReturnsNewListPerCall_Test()
        {
            var ex = new Exception("a", new Exception("b"));

            var first = ex.GetAllExceptions();
            var second = ex.GetAllExceptions();

            Assert.AreNotSame(first, second);
            CollectionAssert.AreEqual(first.ToList(), second.ToList());
        }
    }
}
