using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Collections;

namespace DataTypeTests.CollectionTests
{
    [TestClass]
    public class ConcurrentQueueExtensionsTests
    {
        [TestMethod]
        public void DequeueAll_FifoOrder_Test()
        {
            var queue = new ConcurrentQueue<int>(new[] { 1, 2, 3, 4, 5 });

            var result = queue.DequeueAll();

            CollectionAssert.AreEqual(new[] { 1, 2, 3, 4, 5 }, result);
            Assert.AreEqual(0, queue.Count);
        }

        [TestMethod]
        public void DequeueAll_EmptyQueue_Test()
        {
            var queue = new ConcurrentQueue<int>();

            var result = queue.DequeueAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Length);
            Assert.AreEqual(0, queue.Count);
        }

        [TestMethod]
        public void DequeueBatch_TwoOfFive_Test()
        {
            var queue = new ConcurrentQueue<int>(new[] { 1, 2, 3, 4, 5 });

            var result = queue.DequeueBatch(2);

            CollectionAssert.AreEqual(new[] { 1, 2 }, result);
            Assert.AreEqual(3, queue.Count);
            CollectionAssert.AreEqual(new[] { 3, 4, 5 }, queue.ToArray());
        }

        [TestMethod]
        public void DequeueBatch_ZeroBatchSize_Test()
        {
            var queue = new ConcurrentQueue<int>(new[] { 1, 2, 3, 4, 5 });

            var result = queue.DequeueBatch(0);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Length);
            Assert.AreEqual(5, queue.Count);
        }

        [TestMethod]
        public void DequeueBatch_BatchSizeGreaterThanCount_Test()
        {
            var queue = new ConcurrentQueue<int>(new[] { 1, 2, 3, 4, 5 });

            var result = queue.DequeueBatch(10);

            CollectionAssert.AreEqual(new[] { 1, 2, 3, 4, 5 }, result);
            Assert.AreEqual(0, queue.Count);
        }

        [TestMethod]
        public void DequeueBatch_NegativeBatchSize_Test()
        {
            var queue = new ConcurrentQueue<int>(new[] { 1, 2, 3, 4, 5 });

            var result = queue.DequeueBatch(-1);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Length);
            Assert.AreEqual(5, queue.Count);
        }

        [TestMethod]
        public void DequeueBatch_ConcurrentConsumers_Test()
        {
            var input = Enumerable.Range(1, 10000).ToArray();
            var queue = new ConcurrentQueue<int>(input);
            var received = new ConcurrentBag<int>();

            var consumers = Enumerable.Range(0, 4)
                .Select(_ => Task.Run(() =>
                {
                    while (queue.IsEmpty.Equals(false))
                    {
                        foreach (var item in queue.DequeueBatch(100))
                            received.Add(item);
                    }
                }))
                .ToArray();
            Task.WaitAll(consumers);

            var all = received.ToArray();
            Assert.AreEqual(input.Length, all.Length);
            CollectionAssert.AreEquivalent(input, all);
            Assert.AreEqual(0, queue.Count);
        }

        [TestMethod]
        public void DequeueAndProcessBatches_EveryItemOnce_Test()
        {
            var input = Enumerable.Range(1, 25).ToArray();
            var queue = new ConcurrentQueue<int>(input);
            var batches = new ConcurrentQueue<int[]>();

            queue.DequeueAndProcessBatches(10, batch => batches.Enqueue(batch));

            var processed = batches.SelectMany(x => x).ToArray();
            CollectionAssert.AreEqual(input, processed);
            CollectionAssert.AreEqual(new[] { 10, 10, 5 }, batches.Select(x => x.Length).ToArray());
            Assert.AreEqual(0, queue.Count);
        }

        [TestMethod]
        public void DequeueAndProcessBatches_EmptyQueue_Test()
        {
            var queue = new ConcurrentQueue<int>();
            var calls = 0;

            queue.DequeueAndProcessBatches(10, _ => calls++);

            Assert.AreEqual(0, calls);
            Assert.AreEqual(0, queue.Count);
        }
    }
}
