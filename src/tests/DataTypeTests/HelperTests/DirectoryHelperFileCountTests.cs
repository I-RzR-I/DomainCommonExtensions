using System.IO;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.IO;

namespace DataTypeTests.HelperTests
{
    [TestClass]
    public class DirectoryHelperFileCountTests
    {
        private string _sandbox;

        [TestInitialize]
        public void Setup()
        {
            _sandbox = SandboxTestHelper.CreateSandbox();
        }

        [TestCleanup]
        public void Cleanup()
        {
            SandboxTestHelper.DeleteSandbox(_sandbox);
        }

        [TestMethod]
        public void FileCount_TopLevelAndNestedFiles_Test()
        {
            var root = SandboxTestHelper.CreateDirectory(_sandbox, "root");
            SandboxTestHelper.CreateFile(_sandbox, "a", "root", "a.txt");
            SandboxTestHelper.CreateFile(_sandbox, "b", "root", "b.txt");
            SandboxTestHelper.CreateFile(_sandbox, "c", "root", "sub", "c.txt");
            SandboxTestHelper.CreateFile(_sandbox, "d", "root", "sub", "sub2", "d.txt");

            var result = DirectoryHelper.FileCount(root);

            Assert.AreEqual(4, result);
        }

        [TestMethod]
        public void FileCount_EmptyDirectory_Test()
        {
            var root = SandboxTestHelper.CreateDirectory(_sandbox, "root");

            var result = DirectoryHelper.FileCount(root);

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void FileCount_OnlyTopLevelFiles_Test()
        {
            var root = SandboxTestHelper.CreateDirectory(_sandbox, "root");
            SandboxTestHelper.CreateFile(_sandbox, "a", "root", "a.txt");
            SandboxTestHelper.CreateFile(_sandbox, "b", "root", "b.txt");
            SandboxTestHelper.CreateFile(_sandbox, "c", "root", "c.txt");

            var result = DirectoryHelper.FileCount(root);

            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void FileCount_TopDirectoryOnlyOption_Test()
        {
            var root = SandboxTestHelper.CreateDirectory(_sandbox, "root");
            SandboxTestHelper.CreateFile(_sandbox, "a", "root", "a.txt");
            SandboxTestHelper.CreateFile(_sandbox, "b", "root", "b.log");
            SandboxTestHelper.CreateFile(_sandbox, "c", "root", "sub", "c.txt");

            Assert.AreEqual(2, DirectoryHelper.FileCount(root, SearchOption.TopDirectoryOnly));
            Assert.AreEqual(3, DirectoryHelper.FileCount(root, SearchOption.AllDirectories));
        }

        [TestMethod]
        public void FileCount_SearchPatternOption_Test()
        {
            var root = SandboxTestHelper.CreateDirectory(_sandbox, "root");
            SandboxTestHelper.CreateFile(_sandbox, "a", "root", "a.txt");
            SandboxTestHelper.CreateFile(_sandbox, "b", "root", "b.log");
            SandboxTestHelper.CreateFile(_sandbox, "c", "root", "sub", "c.txt");

            Assert.AreEqual(2, DirectoryHelper.FileCount(root, SearchOption.AllDirectories, "*.txt"));
            Assert.AreEqual(1, DirectoryHelper.FileCount(root, SearchOption.TopDirectoryOnly, "*.txt"));
        }
    }
}
