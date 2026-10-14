using System;
using System.Collections.Generic;
using System.Linq;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Collections.Types;

namespace DataTypeTests.CollectionTests
{
    [TestClass]
    public class DisposableStackCollectionDisposeTests
    {
        [TestMethod]
        public void DisposableStackDispose_NoThrowLifoOrder_Test()
        {
            var log = new List<string>();
            var stack = new DisposableStackCollection();
            stack.Add(new RecordingDisposableTestHelper("first", log));
            stack.Add(new RecordingDisposableTestHelper("second", log));
            stack.Add(new RecordingDisposableTestHelper("third", log));

            stack.Dispose();

            CollectionAssert.AreEqual(new[] { "third", "second", "first" }, log);
            Assert.AreEqual(0, stack.Length);
        }

        [TestMethod]
        public void DisposableStackDispose_MiddleThrowDisposesAll_Test()
        {
            var log = new List<string>();
            var stack = new DisposableStackCollection();
            var first = stack.Add(new RecordingDisposableTestHelper("first", log));
            var second = stack.Add(new RecordingDisposableTestHelper("second", log, new InvalidOperationException("second")));
            var third = stack.Add(new RecordingDisposableTestHelper("third", log));

            try
            {
                stack.Dispose();
            }
            catch (Exception)
            {
            }

            Assert.AreEqual(1, first.DisposeCount);
            Assert.AreEqual(1, second.DisposeCount);
            Assert.AreEqual(1, third.DisposeCount);
        }

        [TestMethod]
        public void DisposableStackDispose_MiddleThrowKeepsLifoOrder_Test()
        {
            var log = new List<string>();
            var stack = new DisposableStackCollection();
            stack.Add(new RecordingDisposableTestHelper("first", log));
            stack.Add(new RecordingDisposableTestHelper("second", log, new InvalidOperationException("second")));
            stack.Add(new RecordingDisposableTestHelper("third", log));

            try
            {
                stack.Dispose();
            }
            catch (Exception)
            {
            }

            CollectionAssert.AreEqual(new[] { "third", "second", "first" }, log);
        }

        [TestMethod]
        public void DisposableStackDispose_MiddleThrowRethrowsSameException_Test()
        {
            var log = new List<string>();
            var failure = new InvalidOperationException("second");
            var stack = new DisposableStackCollection();
            stack.Add(new RecordingDisposableTestHelper("first", log));
            stack.Add(new RecordingDisposableTestHelper("second", log, failure));
            stack.Add(new RecordingDisposableTestHelper("third", log));

            var thrown = Assert.ThrowsException<InvalidOperationException>(() => stack.Dispose());

            Assert.AreSame(failure, thrown);
        }

        [TestMethod]
        public void DisposableStackDispose_TwoThrowAggregates_Test()
        {
            var log = new List<string>();
            var firstFailure = new InvalidOperationException("first");
            var thirdFailure = new NotSupportedException("third");
            var stack = new DisposableStackCollection();
            stack.Add(new RecordingDisposableTestHelper("first", log, firstFailure));
            stack.Add(new RecordingDisposableTestHelper("second", log));
            stack.Add(new RecordingDisposableTestHelper("third", log, thirdFailure));

            var thrown = Assert.ThrowsException<AggregateException>(() => stack.Dispose());

            Assert.AreEqual(2, thrown.InnerExceptions.Count);
            CollectionAssert.AreEquivalent(new Exception[] { firstFailure, thirdFailure }, thrown.InnerExceptions.ToList());
        }

        [TestMethod]
        public void DisposableStackDispose_TwoThrowDisposesAll_Test()
        {
            var log = new List<string>();
            var stack = new DisposableStackCollection();
            var first = stack.Add(new RecordingDisposableTestHelper("first", log, new InvalidOperationException("first")));
            var second = stack.Add(new RecordingDisposableTestHelper("second", log));
            var third = stack.Add(new RecordingDisposableTestHelper("third", log, new NotSupportedException("third")));

            try
            {
                stack.Dispose();
            }
            catch (Exception)
            {
            }

            Assert.AreEqual(1, first.DisposeCount);
            Assert.AreEqual(1, second.DisposeCount);
            Assert.AreEqual(1, third.DisposeCount);
        }

        [TestMethod]
        public void DisposableStackDispose_NullItemSkipped_Test()
        {
            var log = new List<string>();
            var stack = new DisposableStackCollection();
            var first = stack.Add(new RecordingDisposableTestHelper("first", log));
            stack.Add<IDisposable>(null);
            var third = stack.Add(new RecordingDisposableTestHelper("third", log));

            stack.Dispose();

            Assert.AreEqual(1, first.DisposeCount);
            Assert.AreEqual(1, third.DisposeCount);
            CollectionAssert.AreEqual(new[] { "third", "first" }, log);
        }

        [TestMethod]
        public void DisposableStackDispose_SecondCallNoOp_Test()
        {
            var log = new List<string>();
            var stack = new DisposableStackCollection();
            var first = stack.Add(new RecordingDisposableTestHelper("first", log));
            var second = stack.Add(new RecordingDisposableTestHelper("second", log));

            stack.Dispose();
            stack.Dispose();

            Assert.AreEqual(1, first.DisposeCount);
            Assert.AreEqual(1, second.DisposeCount);
            Assert.AreEqual(2, log.Count);
        }

        [TestMethod]
        public void DisposableStackDispose_SecondCallAfterThrowNoOp_Test()
        {
            var log = new List<string>();
            var stack = new DisposableStackCollection();
            var first = stack.Add(new RecordingDisposableTestHelper("first", log));
            var second = stack.Add(new RecordingDisposableTestHelper("second", log, new InvalidOperationException("second")));
            var third = stack.Add(new RecordingDisposableTestHelper("third", log));

            try
            {
                stack.Dispose();
            }
            catch (Exception)
            {
            }

            stack.Dispose();

            Assert.AreEqual(1, first.DisposeCount);
            Assert.AreEqual(1, second.DisposeCount);
            Assert.AreEqual(1, third.DisposeCount);
            Assert.AreEqual(3, log.Count);
        }
    }
}
