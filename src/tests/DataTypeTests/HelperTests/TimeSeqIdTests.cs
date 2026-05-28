// ***********************************************************************
//  Assembly          : RzR.Shared.Extensions.DataTypeTests
//  Author            : RzR
//  Created           : 28-05-2026 22:05
// 
//  Last Modified By : RzR
//  Last Modified On : 28-05-2026 22:40
//  ***********************************************************************
//  <copyright file="TimeSeqIdTests.cs" company="RzR SOFT & TECH">
//      Copyright (c) RzR. All rights reserved.
//  </copyright>
//  <contact>
//      https://iamrzr.dev/contact
//  </contact>
//  <summary></summary>
//  ***********************************************************************

#region U S I N G

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Primitives;

#endregion

namespace DataTypeTests.HelperTests
{
    [TestClass]
    public class TimeSeqIdTests
    {
        private static string[] SplitBlocks(string id)
        {
            return id.Split('-');
        }

        private static string ComputeExpectedBlock5(string combinedBlocks)
        {
            using (var sha = SHA256.Create())
            {
                var hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(combinedBlocks));
                const string alphanumeric = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                var unbiasedCeiling = 256 - 256 % alphanumeric.Length; // 252
                var result = new StringBuilder(6);
                var byteIdx = 0;

                while (result.Length < 6)
                {
                    if (byteIdx >= hashBytes.Length)
                    {
                        hashBytes = sha.ComputeHash(hashBytes);
                        byteIdx = 0;
                    }

                    var b = hashBytes[byteIdx++];
                    if (b < unbiasedCeiling)
                        result.Append(alphanumeric[b % alphanumeric.Length]);
                }

                return result.ToString();
            }
        }

        [TestMethod]
        public void Generate_ReturnsNonNullNonEmpty()
        {
            var id = TimeSeqId.Generate();

            Assert.IsFalse(string.IsNullOrWhiteSpace(id));
        }

        [TestMethod]
        public void Generate_HasExactTotalLength()
        {
            // yyyy(4) - MMdd(4) - HHmmssfff(9) - SSSS(4) - hex×16(16) - hash(6)
            // + 5 dashes = 48 chars
            var id = TimeSeqId.Generate();

            Assert.AreEqual(48, id.Length, $"Unexpected length for id '{id}'");
        }

        [TestMethod]
        public void Generate_HasSixBlocksSeparatedByFiveDashes()
        {
            var blocks = SplitBlocks(TimeSeqId.Generate());

            Assert.AreEqual(6, blocks.Length);
        }

        [TestMethod]
        public void Generate_Block0_IsFourDigitCurrentYear()
        {
            var block0 = SplitBlocks(TimeSeqId.Generate())[0];

            Assert.AreEqual(4, block0.Length);
            Assert.IsTrue(int.TryParse(block0, out var year), "block0 is not numeric");
            Assert.IsTrue(year >= 2000 && year <= 9999, $"Year out of expected range: {year}");
        }

        [TestMethod]
        public void Generate_Block1_IsValidMonthDay()
        {
            var block1 = SplitBlocks(TimeSeqId.Generate())[1];

            Assert.AreEqual(4, block1.Length, "block1 must be exactly 4 digits");
            Assert.IsTrue(Regex.IsMatch(block1, @"^\d{4}$"), $"block1 not all digits: '{block1}'");

            var month = int.Parse(block1.Substring(0, 2));
            var day = int.Parse(block1.Substring(2, 2));

            Assert.IsTrue(month >= 1 && month <= 12, $"Invalid month: {month}");
            Assert.IsTrue(day >= 1 && day <= 31, $"Invalid day: {day}");
        }

        [TestMethod]
        public void Generate_Block2_IsValidNineDigitTimestamp()
        {
            var block2 = SplitBlocks(TimeSeqId.Generate())[2];

            Assert.AreEqual(9, block2.Length, "block2 must be exactly 9 digits");
            Assert.IsTrue(Regex.IsMatch(block2, @"^\d{9}$"), $"block2 not all digits: '{block2}'");

            var hh = int.Parse(block2.Substring(0, 2));
            var mm = int.Parse(block2.Substring(2, 2));
            var ss = int.Parse(block2.Substring(4, 2));
            var fff = int.Parse(block2.Substring(6, 3));

            Assert.IsTrue(hh >= 0 && hh <= 23, $"Invalid hour: {hh}");
            Assert.IsTrue(mm >= 0 && mm <= 59, $"Invalid minute: {mm}");
            Assert.IsTrue(ss >= 0 && ss <= 59, $"Invalid second: {ss}");
            Assert.IsTrue(fff >= 0 && fff <= 999, $"Invalid millisecond: {fff}");
        }

        [TestMethod]
        public void Generate_Block3_IsFourDigitSequenceInValidRange()
        {
            var block3 = SplitBlocks(TimeSeqId.Generate())[3];

            Assert.AreEqual(4, block3.Length, "block3 must be exactly 4 digits");
            Assert.IsTrue(int.TryParse(block3, out var seq), "block3 is not numeric");
            Assert.IsTrue(seq >= 0 && seq <= 9999, $"Sequence out of range: {seq}");
        }

        [TestMethod]
        public void Generate_Block4_Is16UppercaseHexCharacters()
        {
            var block4 = SplitBlocks(TimeSeqId.Generate())[4];

            Assert.AreEqual(16, block4.Length, $"block4 wrong length: '{block4}'");
            Assert.IsTrue(Regex.IsMatch(block4, @"^[0-9A-F]{16}$"),
                $"block4 contains non-hex characters: '{block4}'");
        }

        [TestMethod]
        public void Generate_Block5_IsSixUppercaseAlphanumericCharacters()
        {
            var block5 = SplitBlocks(TimeSeqId.Generate())[5];

            Assert.AreEqual(6, block5.Length, $"block5 wrong length: '{block5}'");
            Assert.IsTrue(Regex.IsMatch(block5, @"^[0-9A-Z]{6}$"),
                $"block5 contains invalid characters: '{block5}'");
        }

        [TestMethod]
        public void Generate_Block5_MatchesExpectedSha256Hash()
        {
            var id = TimeSeqId.Generate();
            var blocks = SplitBlocks(id);

            var combined = $"{blocks[0]}-{blocks[1]}-{blocks[2]}-{blocks[3]}-{blocks[4]}";
            var expected = ComputeExpectedBlock5(combined);

            Assert.AreEqual(expected, blocks[5],
                $"Integrity check failed — block5 does not match recomputed hash for id '{id}'");
        }

        [TestMethod]
        public void Generate_Block5_IsDeterministicForSameInput()
        {
            var id = TimeSeqId.Generate();
            var blocks = SplitBlocks(id);
            var combined = $"{blocks[0]}-{blocks[1]}-{blocks[2]}-{blocks[3]}-{blocks[4]}";

            var hash1 = ComputeExpectedBlock5(combined);
            var hash2 = ComputeExpectedBlock5(combined);

            Assert.AreEqual(hash1, hash2, "Block5 hash must be deterministic for the same input");
        }

        [TestMethod]
        public void Generate_Block5_ChangesWhenBlock4IsTampered()
        {
            var id = TimeSeqId.Generate();
            var blocks = SplitBlocks(id);

            var original = blocks[5];
            var tamperedBlock4 = (blocks[4][0] == 'A' ? 'B' : 'A') + blocks[4].Substring(1);
            var tamperedHash = ComputeExpectedBlock5(
                $"{blocks[0]}-{blocks[1]}-{blocks[2]}-{blocks[3]}-{tamperedBlock4}");

            Assert.AreNotEqual(original, tamperedHash,
                "Mutating block4 must produce a different block5 hash");
        }

        [TestMethod]
        public void Generate_Block5_ChangesWhenTimestampIsTampered()
        {
            var id = TimeSeqId.Generate();
            var blocks = SplitBlocks(id);

            var original = blocks[5];
            var tamperedBlock2 = blocks[2] == "000000000" ? "000000001" : "000000000";
            var tamperedHash = ComputeExpectedBlock5(
                $"{blocks[0]}-{blocks[1]}-{tamperedBlock2}-{blocks[3]}-{blocks[4]}");

            Assert.AreNotEqual(original, tamperedHash,
                "Mutating block2 (timestamp) must produce a different block5 hash");
        }

        [TestMethod]
        public void Generate_ProducesUniqueIds_Sequential()
        {
            const int count = 10000;
            var ids = new HashSet<string>(count);

            for (var i = 0; i < count; i++)
                Assert.IsTrue(ids.Add(TimeSeqId.Generate()), $"Duplicate detected at iteration {i}");
        }

        [TestMethod]
        public void Generate_ProducesUniqueIds_Concurrent()
        {
            const int threadCount = 16;
            const int perThread = 500;
            var bag = new ConcurrentBag<string>();

            Parallel.For(0, threadCount, _ =>
            {
                for (var i = 0; i < perThread; i++)
                    bag.Add(TimeSeqId.Generate());
            });

            var ids = bag.ToList();
            var distinct = new HashSet<string>(ids);

            Assert.AreEqual(threadCount * perThread, ids.Count, "Total ID count mismatch");
            Assert.AreEqual(ids.Count, distinct.Count,
                $"Duplicate IDs found under concurrent generation: {ids.Count - distinct.Count} collision(s)");
        }

        [TestMethod]
        public void Generate_IdsAreLexicographicallySorted_AcrossTimeWindows()
        {
            // 3 batches separated by a sleep so timestamps differ
            var ids = new List<string>();

            for (var batch = 0; batch < 3; batch++)
            {
                for (var i = 0; i < 10; i++)
                    ids.Add(TimeSeqId.Generate());
                if (batch < 2) Thread.Sleep(5);
            }

            var sorted = ids.OrderBy(x => x).ToList();
            CollectionAssert.AreEqual(ids, sorted,
                "IDs generated across distinct time windows must already be in lexicographic order");
        }

        [TestMethod]
        public void Generate_SequentialBurst_IsMonotonicallyOrdered()
        {
            const int count = 500;
            var ids = new string[count];

            for (var i = 0; i < count; i++)
                ids[i] = TimeSeqId.Generate();

            for (var i = 1; i < count; i++)
                Assert.IsTrue(
                    string.CompareOrdinal(ids[i - 1], ids[i]) <= 0,
                    $"Monotonicity violation at index {i}: '{ids[i - 1]}' > '{ids[i]}'");
        }
    }
}