// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2026-04-24
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-04-24
// ***********************************************************************
//  <copyright file="FilesExtensionsTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// ***********************************************************************

#region U S A G E S

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Async;
using RzR.Extensions.Domain.Cryptography;
using RzR.Extensions.Domain.Data;
using RzR.Extensions.Domain.Diagnostics;
using RzR.Extensions.Domain.IO;
using RzR.Extensions.Domain.Linq;
using RzR.Extensions.Domain.Primitives;
using RzR.Extensions.Domain.Diagnostics;
using RzR.Extensions.Domain.Primitives;
using RzR.Extensions.Domain.Text;
#endregion

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class FilesExtensionsTests
    {
        private string _workDir;

        [TestInitialize]
        public void Setup()
        {
            _workDir = Path.Combine(Path.GetTempPath(), "DCE_Tests_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(_workDir);
        }

        [TestCleanup]
        public void Cleanup()
        {
            try
            {
                if (Directory.Exists(_workDir))
                    Directory.Delete(_workDir, true);
            }
            catch
            {
                // best-effort
            }
        }

        [TestMethod]
        public void SafeWriteAllText_CreatesFile_WhenNotExists()
        {
            var path = Path.Combine(_workDir, "new.txt");
            const string content = "hello world";

            path.SafeWriteAllText(content);

            Assert.IsTrue(File.Exists(path));
            Assert.AreEqual(content, File.ReadAllText(path, new UTF8Encoding(false)));
        }

        [TestMethod]
        public void SafeWriteAllText_OverwritesExistingFile()
        {
            var path = Path.Combine(_workDir, "existing.txt");
            File.WriteAllText(path, "old content");

            path.SafeWriteAllText("new content");

            Assert.AreEqual("new content", File.ReadAllText(path));
        }

        [TestMethod]
        public void SafeWriteAllText_NullContent_WritesEmptyFile()
        {
            var path = Path.Combine(_workDir, "empty.txt");

            path.SafeWriteAllText(null);

            Assert.IsTrue(File.Exists(path));
            Assert.AreEqual(0, new FileInfo(path).Length);
        }

        [TestMethod]
        public void SafeWriteAllText_DefaultEncoding_IsUtf8WithoutBom()
        {
            var path = Path.Combine(_workDir, "utf8.txt");

            path.SafeWriteAllText("abc");

            var bytes = File.ReadAllBytes(path);
            Assert.AreEqual(3, bytes.Length, "UTF-8 without BOM should produce no preamble.");
            Assert.AreEqual((byte)'a', bytes[0]);
        }

        [TestMethod]
        public void SafeWriteAllText_CustomEncoding_IsRespected()
        {
            var path = Path.Combine(_workDir, "utf16.txt");

            path.SafeWriteAllText("abc", Encoding.Unicode);

            var bytes = File.ReadAllBytes(path);
            // UTF-16 LE: 'a' (61 00) + 'b' (62 00) + 'c' (63 00) = 6 bytes (no BOM emitted by GetBytes).
            Assert.AreEqual(6, bytes.Length);
            Assert.AreEqual(0x61, bytes[0]);
            Assert.AreEqual(0x00, bytes[1]);
        }

        [TestMethod]
        public void SafeWriteAllText_CreatesMissingDirectory()
        {
            var nested = Path.Combine(_workDir, "a", "b", "c", "file.txt");

            nested.SafeWriteAllText("nested");

            Assert.IsTrue(File.Exists(nested));
            Assert.AreEqual("nested", File.ReadAllText(nested));
        }

        [TestMethod]
        public void SafeWriteAllText_DoesNotLeaveTempArtifacts()
        {
            var path = Path.Combine(_workDir, "clean.txt");

            path.SafeWriteAllText("payload");

            var leftovers = Directory.GetFiles(_workDir, "*.tmp");
            Assert.AreEqual(0, leftovers.Length, "No .tmp files must remain after a successful write.");
        }

        [TestMethod]
        public void SafeWriteAllText_NullPath_Throws()
        {
            string path = null;
            Assert.ThrowsException<ArgumentNullException>(() => path.SafeWriteAllText("x"));
        }

        [TestMethod]
        public void SafeWriteAllText_EmptyPath_Throws()
        {
            Assert.ThrowsException<ArgumentException>(() => string.Empty.SafeWriteAllText("x"));
        }

        [TestMethod]
        public void SafeWriteAllBytes_RoundTrips()
        {
            var path = Path.Combine(_workDir, "bytes.bin");
            var data = new byte[] { 1, 2, 3, 4, 5 };

            path.SafeWriteAllBytes(data);

            CollectionAssert.AreEqual(data, File.ReadAllBytes(path));
        }

        [TestMethod]
        public void SafeWriteAllBytes_OverwriteIsAtomic_ContentNeverEmpty()
        {
            var path = Path.Combine(_workDir, "atomic.bin");
            File.WriteAllBytes(path, Encoding.UTF8.GetBytes("ORIGINAL"));

            path.SafeWriteAllBytes(Encoding.UTF8.GetBytes("REPLACED"));

            Assert.AreEqual("REPLACED", File.ReadAllText(path));
        }

        [TestMethod]
        public void SafeWriteAllBytes_NullBytes_Throws()
        {
            var path = Path.Combine(_workDir, "x.bin");

            Assert.ThrowsException<ArgumentNullException>(() => path.SafeWriteAllBytes(null));
        }

        [TestMethod]
        public void SafeWriteAllBytes_EmptyBytes_CreatesEmptyFile()
        {
            var path = Path.Combine(_workDir, "empty.bin");

            path.SafeWriteAllBytes(new byte[0]);

            Assert.IsTrue(File.Exists(path));
            Assert.AreEqual(0, new FileInfo(path).Length);
        }

        [TestMethod]
        public void SafeWriteAllBytes_ConcurrentWriters_ResultIsOneOfPayloads()
        {
            var path = Path.Combine(_workDir, "concurrent.bin");
            var p1 = Encoding.UTF8.GetBytes(new string('A', 4096));
            var p2 = Encoding.UTF8.GetBytes(new string('B', 4096));

            Parallel.For(0, 50, i =>
            {
                try
                {
                    path.SafeWriteAllBytes(i % 2 == 0 ? p1 : p2);
                }
                catch (IOException)
                {
                    // Concurrent File.Replace can race; that's acceptable for this guarantee.
                }
            });

            Assert.IsTrue(File.Exists(path));
            var final = File.ReadAllBytes(path);
            var matchesA = final.SequenceEqual(p1);
            var matchesB = final.SequenceEqual(p2);
            Assert.IsTrue(matchesA || matchesB,
                "Final file must equal one full payload, never a partial write.");
        }

        [TestMethod]
        public void Sha256HexFromFile_KnownVector_EmptyFile()
        {
            // SHA-256 of "" = e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855
            var path = Path.Combine(_workDir, "empty.dat");
            File.WriteAllBytes(path, new byte[0]);

            var hash = path.Sha256HexFromFile();

            Assert.AreEqual("e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855", hash);
        }

        [TestMethod]
        public void Sha256HexFromFile_KnownVector_Abc()
        {
            // SHA-256 of "abc" = ba7816bf8f01cfea414140de5dae2223b00361a396177a9cb410ff61f20015ad
            var path = Path.Combine(_workDir, "abc.dat");
            File.WriteAllBytes(path, Encoding.ASCII.GetBytes("abc"));

            var hash = path.Sha256HexFromFile();

            Assert.AreEqual("ba7816bf8f01cfea414140de5dae2223b00361a396177a9cb410ff61f20015ad", hash);
        }

        [TestMethod]
        public void Sha256HexFromFile_MissingFile_Throws()
        {
            var path = Path.Combine(_workDir, "missing.dat");

            Assert.ThrowsException<FileNotFoundException>(() => path.Sha256HexFromFile());
        }

        [TestMethod]
        public void Sha256HexFromFile_NullPath_Throws()
        {
            string path = null;
            Assert.ThrowsException<ArgumentNullException>(() => path.Sha256HexFromFile());
        }

        [TestMethod]
        public void Sha256HexFromFile_EmptyPath_Throws()
        {
            Assert.ThrowsException<ArgumentException>(() => string.Empty.Sha256HexFromFile());
        }
    }
}
