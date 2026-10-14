using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Async.LazyLoad;
using RzR.Extensions.Domain.Collections;
using RzR.Extensions.Domain.Diagnostics;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class DocumentationExtensionsTests
    {
        [TestMethod]
        public void GetSummary_TopLevelLibraryType_Test()
        {
            var libraryAssembly = typeof(DocumentationExtensions).Assembly;
            if (!File.Exists(Path.ChangeExtension(libraryAssembly.Location, ".xml")))
                Assert.Inconclusive("Library XML documentation file is not present next to the assembly.");

            var summary = typeof(ConcurrentQueueExtensions).GetSummary();

            Assert.IsFalse(string.IsNullOrWhiteSpace(summary));
        }

        [TestMethod]
        public void GetSummary_NestedTypeMember_Test()
        {
            var libraryAssembly = typeof(DocumentationExtensions).Assembly;
            if (!File.Exists(Path.ChangeExtension(libraryAssembly.Location, ".xml")))
                Assert.Inconclusive("Library XML documentation file is not present next to the assembly.");
            var nestedType = typeof(AsyncLazy<>).GetNestedType("LazyState", BindingFlags.NonPublic);
            var member = nestedType?.GetField("Task", BindingFlags.Public | BindingFlags.Instance);
            if (member == null)
                Assert.Inconclusive("The private nested type AsyncLazy<T>.LazyState.Task is not available.");

            var summary = member.GetSummary();

            Assert.IsFalse(string.IsNullOrWhiteSpace(summary));
        }

        [TestMethod]
        public void GetSummary_NestedType_Test()
        {
            var libraryAssembly = typeof(DocumentationExtensions).Assembly;
            if (!File.Exists(Path.ChangeExtension(libraryAssembly.Location, ".xml")))
                Assert.Inconclusive("Library XML documentation file is not present next to the assembly.");
            var nestedType = typeof(AsyncLazy<>).GetNestedType("LazyState", BindingFlags.NonPublic);
            if (nestedType == null)
                Assert.Inconclusive("The private nested type AsyncLazy<T>.LazyState is not available.");

            var summary = nestedType.GetSummary();

            Assert.IsFalse(string.IsNullOrWhiteSpace(summary));
        }

        [TestMethod]
        public void XmlFromAssembly_ParallelCalls_Test()
        {
            var libraryAssembly = typeof(DocumentationExtensions).Assembly;
            if (!File.Exists(Path.ChangeExtension(libraryAssembly.Location, ".xml")))
                Assert.Inconclusive("Library XML documentation file is not present next to the assembly.");
            const int workers = 8;
            using var barrier = new Barrier(workers);
            var tasks = Enumerable.Range(0, workers)
                .Select(_ => Task.Factory.StartNew(() =>
                {
                    barrier.SignalAndWait(TimeSpan.FromSeconds(30));

                    return libraryAssembly.XmlFromAssembly();
                }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default))
                .ToArray();

            Assert.IsTrue(Task.WaitAll(tasks, TimeSpan.FromSeconds(60)));
            var results = tasks.Select(t => t.Result).ToArray();

            Assert.IsNotNull(results[0]);
            Assert.IsTrue(results.All(r => ReferenceEquals(r, results[0])));
        }

        [TestMethod]
        public void XmlFromAssembly_CoreLibAssembly_Test()
        {
            var coreAssembly = typeof(string).Assembly;

            var document = coreAssembly.XmlFromAssembly();

            Assert.IsTrue(document == null || document.DocumentElement != null);
        }

        [TestMethod]
        public void XmlFromAssembly_DynamicAssembly_Test()
        {
            var dynamicAssembly = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("DynamicDocTest"), AssemblyBuilderAccess.Run);

            var document = dynamicAssembly.XmlFromAssembly();

            Assert.IsNull(document);
        }
    }
}
