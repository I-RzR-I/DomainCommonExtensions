using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Data;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class DataReaderReadAllTests
    {
        [TestMethod]
        public void ReadAll_MapsEveryRowInOrder_Test()
        {
            using (var reader = DataRecordTestHelper.IdRows(1, 2, 3))
            {
                var items = reader.ReadAll(record => record.GetInt32(0));

                CollectionAssert.AreEqual(new[] { 1, 2, 3 }, items);
            }
        }

        [TestMethod]
        public void ReadAll_ConsumesRemainingRows_Test()
        {
            using (var reader = DataRecordTestHelper.IdRows(1, 2, 3))
            {
                reader.ReadAll(record => record.GetInt32(0));

                Assert.IsFalse(reader.Read());
            }
        }

        [TestMethod]
        public void ReadAll_StartsFromCurrentPosition_Test()
        {
            using (var reader = DataRecordTestHelper.IdRows(1, 2, 3))
            {
                reader.Read();

                var items = reader.ReadAll(record => record.GetInt32(0));

                CollectionAssert.AreEqual(new[] { 2, 3 }, items);
            }
        }

        [TestMethod]
        public void ReadAll_EmptyReaderReturnsEmptyList_Test()
        {
            using (var reader = DataRecordTestHelper.IdRows())
            {
                var items = reader.ReadAll(record => record.GetInt32(0));

                Assert.IsNotNull(items);
                Assert.AreEqual(0, items.Count);
            }
        }

        [TestMethod]
        public void ReadAll_PassesReaderAsRecord_Test()
        {
            using (var reader = DataRecordTestHelper.IdRows(1))
            {
                var items = reader.ReadAll(record => record);

                Assert.AreSame(reader, items[0]);
            }
        }

        [TestMethod]
        public void ReadAll_DoesNotCloseReader_Test()
        {
            using (var reader = DataRecordTestHelper.IdRows(1, 2))
            {
                reader.ReadAll(record => record.GetInt32(0));

                Assert.IsFalse(reader.IsClosed);
            }
        }

        [TestMethod]
        public void ReadAll_ReadsCurrentResultSetOnly_Test()
        {
            using (var reader = DataRecordTestHelper.IdResultSets(new[] { 1, 2 }, new[] { 3 }))
            {
                var first = reader.ReadAll(record => record.GetInt32(0));
                var hasNext = reader.NextResult();
                var second = reader.ReadAll(record => record.GetInt32(0));

                CollectionAssert.AreEqual(new[] { 1, 2 }, first);
                Assert.IsTrue(hasNext);
                CollectionAssert.AreEqual(new[] { 3 }, second);
            }
        }

        [TestMethod]
        public void ReadAll_NullReaderThrowsArgumentNullException_Test()
        {
            var exception = Assert.ThrowsException<ArgumentNullException>(
                () => ((IDataReader)null).ReadAll(record => 1));

            Assert.AreEqual("reader", exception.ParamName);
        }

        [TestMethod]
        public void ReadAll_NullMapThrowsArgumentNullException_Test()
        {
            using (var reader = DataRecordTestHelper.IdRows(1))
            {
                var exception = Assert.ThrowsException<ArgumentNullException>(
                    () => reader.ReadAll<int>(null));

                Assert.AreEqual("map", exception.ParamName);
            }
        }

        [TestMethod]
        public async Task ReadAllAsync_MapsEveryRowInOrder_Test()
        {
            using (var reader = DataRecordTestHelper.IdRows(1, 2, 3))
            {
                var items = await reader.ReadAllAsync(record => record.GetInt32(0));

                CollectionAssert.AreEqual(new[] { 1, 2, 3 }, items);
            }
        }

        [TestMethod]
        public async Task ReadAllAsync_ConsumesRemainingRows_Test()
        {
            using (var reader = DataRecordTestHelper.IdRows(1, 2, 3))
            {
                await reader.ReadAllAsync(record => record.GetInt32(0));

                Assert.IsFalse(reader.Read());
            }
        }

        [TestMethod]
        public async Task ReadAllAsync_StartsFromCurrentPosition_Test()
        {
            using (var reader = DataRecordTestHelper.IdRows(1, 2, 3))
            {
                reader.Read();

                var items = await reader.ReadAllAsync(record => record.GetInt32(0));

                CollectionAssert.AreEqual(new[] { 2, 3 }, items);
            }
        }

        [TestMethod]
        public async Task ReadAllAsync_EmptyReaderReturnsEmptyList_Test()
        {
            using (var reader = DataRecordTestHelper.IdRows())
            {
                var items = await reader.ReadAllAsync(record => record.GetInt32(0));

                Assert.IsNotNull(items);
                Assert.AreEqual(0, items.Count);
            }
        }

        [TestMethod]
        public async Task ReadAllAsync_PassesReaderAsRecord_Test()
        {
            using (var reader = DataRecordTestHelper.IdRows(1))
            {
                var items = await reader.ReadAllAsync(record => record);

                Assert.AreSame(reader, items[0]);
            }
        }

        [TestMethod]
        public async Task ReadAllAsync_DoesNotCloseReader_Test()
        {
            using (var reader = DataRecordTestHelper.IdRows(1, 2))
            {
                await reader.ReadAllAsync(record => record.GetInt32(0));

                Assert.IsFalse(reader.IsClosed);
            }
        }

        [TestMethod]
        public async Task ReadAllAsync_ReadsCurrentResultSetOnly_Test()
        {
            using (var reader = DataRecordTestHelper.IdResultSets(new[] { 1, 2 }, new[] { 3 }))
            {
                var first = await reader.ReadAllAsync(record => record.GetInt32(0));
                var hasNext = reader.NextResult();
                var second = await reader.ReadAllAsync(record => record.GetInt32(0));

                CollectionAssert.AreEqual(new[] { 1, 2 }, first);
                Assert.IsTrue(hasNext);
                CollectionAssert.AreEqual(new[] { 3 }, second);
            }
        }

        [TestMethod]
        public async Task ReadAllAsync_PreCancelledTokenCancelsTask_Test()
        {
            using (var cancellation = new CancellationTokenSource())
            using (var reader = DataRecordTestHelper.IdRows(1, 2))
            {
                cancellation.Cancel();
                var task = reader.ReadAllAsync(record => record.GetInt32(0), cancellation.Token);

                OperationCanceledException caught = null;
                try
                {
                    await task;
                }
                catch (OperationCanceledException exception)
                {
                    caught = exception;
                }

                Assert.IsNotNull(caught);
                Assert.IsTrue(task.IsCanceled);
            }
        }

        [TestMethod]
        public async Task ReadAllAsync_CancelDuringReadCancelsTask_Test()
        {
            using (var cancellation = new CancellationTokenSource())
            using (var reader = DataRecordTestHelper.IdRows(1, 2, 3))
            {
                var task = reader.ReadAllAsync(record =>
                {
                    cancellation.Cancel();

                    return record.GetInt32(0);
                }, cancellation.Token);

                OperationCanceledException caught = null;
                try
                {
                    await task;
                }
                catch (OperationCanceledException exception)
                {
                    caught = exception;
                }

                Assert.IsNotNull(caught);
                Assert.IsTrue(task.IsCanceled);
            }
        }

        [TestMethod]
        public async Task ReadAllAsync_ProviderIgnoringTokenStillCancels_Test()
        {
            using (var cancellation = new CancellationTokenSource())
            using (var reader = new TokenIgnoringDataReaderTestHelper(DataRecordTestHelper.IdRows(1, 2, 3)))
            {
                var task = reader.ReadAllAsync(record =>
                {
                    cancellation.Cancel();

                    return record.GetInt32(0);
                }, cancellation.Token);

                Exception caught = null;
                try
                {
                    await task;
                }
                catch (Exception exception)
                {
                    caught = exception;
                }

                Assert.IsInstanceOfType(caught, typeof(OperationCanceledException));
            }
        }

        [TestMethod]
        public void ReadAllAsync_NullReaderThrowsBeforeTask_Test()
        {
            var exception = Assert.ThrowsException<ArgumentNullException>(
                () => ((DbDataReader)null).ReadAllAsync(record => 1));

            Assert.AreEqual("reader", exception.ParamName);
        }

        [TestMethod]
        public void ReadAllAsync_NullMapThrowsBeforeTask_Test()
        {
            using (var reader = DataRecordTestHelper.IdRows(1))
            {
                var exception = Assert.ThrowsException<ArgumentNullException>(
                    () => reader.ReadAllAsync<int>(null));

                Assert.AreEqual("map", exception.ParamName);
            }
        }
    }
}
