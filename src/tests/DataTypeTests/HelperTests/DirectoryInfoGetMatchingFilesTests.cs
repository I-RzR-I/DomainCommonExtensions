using System;
using System.IO;
using System.Linq;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.IO;

namespace DataTypeTests.HelperTests
{
    [TestClass]
    public class DirectoryInfoGetMatchingFilesTests
    {
        private string _sandbox;

        [TestInitialize]
        public void Setup()
        {
            _sandbox = SandboxTestHelper.CreateSandbox();
            SandboxTestHelper.CreateFile(_sandbox, "a", "a.txt");
            SandboxTestHelper.CreateFile(_sandbox, "b", "b.log");
            SandboxTestHelper.CreateFile(_sandbox, "c", "c.cs");
            SandboxTestHelper.CreateFile(_sandbox, "d", "sub", "d.txt");
            SandboxTestHelper.CreateFile(_sandbox, "e", "sub", "e.cs");
        }

        [TestCleanup]
        public void Cleanup()
        {
            SandboxTestHelper.DeleteSandbox(_sandbox);
        }

        [TestMethod]
        public void GetMatchingFiles_TwoPatterns_Test()
        {
            var directory = new DirectoryInfo(_sandbox);

            var result = directory.GetMatchingFiles("*.txt;*.log", SearchOption.TopDirectoryOnly);

            CollectionAssert.AreEqual(
                new[] { "a.txt", "b.log" },
                result.Select(x => x.Name).OrderBy(x => x, StringComparer.Ordinal).ToArray());
        }

        [TestMethod]
        public void GetMatchingFiles_OverlappingPatterns_Test()
        {
            var directory = new DirectoryInfo(_sandbox);

            var result = directory.GetMatchingFiles("*.log;*.txt;a.*", SearchOption.TopDirectoryOnly);

            CollectionAssert.AreEqual(
                new[] { "b.log", "a.txt", "a.txt" },
                result.Select(x => x.Name).ToArray());
        }

        [TestMethod]
        public void GetMatchingFiles_EmptyPatternEntries_Test()
        {
            var directory = new DirectoryInfo(_sandbox);

            var result = directory.GetMatchingFiles(";*.txt;;", SearchOption.TopDirectoryOnly);

            CollectionAssert.AreEqual(new[] { "a.txt" }, result.Select(x => x.Name).ToArray());
        }

        [TestMethod]
        public void GetMatchingFiles_EmptyPatterns_Test()
        {
            var directory = new DirectoryInfo(_sandbox);

            var result = directory.GetMatchingFiles(string.Empty, SearchOption.TopDirectoryOnly);

            Assert.AreEqual(0, result.Length);
        }

        [TestMethod]
        public void GetMatchingFiles_SinglePattern_Test()
        {
            var directory = new DirectoryInfo(_sandbox);

            var result = directory.GetMatchingFiles("*.cs", SearchOption.TopDirectoryOnly);

            CollectionAssert.AreEqual(new[] { "c.cs" }, result.Select(x => x.Name).ToArray());
        }

        [TestMethod]
        public void GetMatchingFiles_AllDirectories_Test()
        {
            var directory = new DirectoryInfo(_sandbox);

            var result = directory.GetMatchingFiles("*.txt;*.log", SearchOption.AllDirectories);

            CollectionAssert.AreEqual(
                new[] { "a.txt", "b.log", "d.txt" },
                result.Select(x => x.Name).OrderBy(x => x, StringComparer.Ordinal).ToArray());
            Assert.AreEqual("sub", result.Single(x => x.Name == "d.txt").Directory?.Name);
        }

        [TestMethod]
        public void GetMatchingFiles_TopDirectoryOnly_Test()
        {
            var directory = new DirectoryInfo(_sandbox);

            var result = directory.GetMatchingFiles("*.cs;*.txt", SearchOption.TopDirectoryOnly);

            CollectionAssert.AreEqual(
                new[] { "a.txt", "c.cs" },
                result.Select(x => x.Name).OrderBy(x => x, StringComparer.Ordinal).ToArray());
        }

        [TestMethod]
        public void GetMatchingFiles_NoMatch_Test()
        {
            var directory = new DirectoryInfo(_sandbox);

            var result = directory.GetMatchingFiles("*.md;*.json", SearchOption.AllDirectories);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Length);
        }
    }
}
