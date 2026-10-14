using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Cryptography.Passwords;

namespace DataTypeTests.UtilsTests
{
    [TestClass]
    public class PasswordGenerateUtilsSecurityTests
    {
        private const string DefaultSpecialChars = "&!#@$_?-";

        [TestMethod]
        [Timeout(60000)]
        [TestCategory("Security")]
        public void Generate_CustomCharsDoNotLeakIntoLaterDefaultCalls_Test()
        {
            PasswordGenerateUtils.Generate(ownNonAlphanumericChars: new[] { "~" });

            var contaminated = Enumerable.Range(0, 200)
                .Select(_ => PasswordGenerateUtils.Generate())
                .Where(x => x.Contains("~"))
                .ToArray();

            Assert.AreEqual(0, contaminated.Length,
                $"{contaminated.Length} of 200 default calls contained a custom char from an earlier call, e.g. '{contaminated.FirstOrDefault()}'.");
        }

        [TestMethod]
        [Timeout(60000)]
        [TestCategory("Security")]
        public void Generate_DefaultSpecialAlphabetUnchangedAfterCustomCalls_Test()
        {
            for (var i = 0; i < 10; i++)
                PasswordGenerateUtils.Generate(64, 4, false, false, false, true, new[] { "%", "^", "|" });

            var observed = Enumerable.Range(0, 100)
                .SelectMany(_ => PasswordGenerateUtils.Generate(64, 4, false, false, false, true))
                .Distinct()
                .OrderBy(x => x)
                .ToArray();

            Assert.AreEqual(new string(DefaultSpecialChars.OrderBy(x => x).ToArray()), new string(observed),
                "The default special alphabet observed after calls with custom chars differs from the library defaults.");
        }

        [TestMethod]
        [Timeout(60000)]
        [TestCategory("Security")]
        public void Generate_CustomCharsSpecialSetIsDefaultsPlusCustom_Test()
        {
            PasswordGenerateUtils.Generate(64, 4, false, false, false, true, new[] { "^" });

            var observed = Enumerable.Range(0, 100)
                .SelectMany(_ => PasswordGenerateUtils.Generate(64, 4, false, false, false, true, new[] { "~", "&" }))
                .Distinct()
                .OrderBy(x => x)
                .ToArray();

            Assert.AreEqual(new string((DefaultSpecialChars + "~").OrderBy(x => x).ToArray()), new string(observed),
                "The special set with custom chars is not exactly the defaults plus this call's custom chars.");
        }

        [DataRow(8, 4)]
        [DataRow(0, 0)]
        [TestMethod]
        [Timeout(30000)]
        [TestCategory("Security")]
        public void Generate_AllRequiredFlagsFalse_ThrowsSameExceptionWithoutHang_Test(int requiredMinimumLength, int requiredUniqueChar)
        {
            var call = Task.Run(() => PasswordGenerateUtils.Generate(requiredMinimumLength, requiredUniqueChar, false, false, false, false));

            var completed = ((IAsyncResult)call).AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(10));

            Assert.IsTrue(completed, "Generate with all required flags false did not complete within 10 seconds.");
            Assert.IsTrue(call.IsFaulted, $"Generate with all required flags false returned '{(call.IsFaulted ? null : call.Result)}' instead of throwing.");
            Assert.AreEqual(typeof(IndexOutOfRangeException), call.Exception?.InnerException?.GetType());
        }

        [DataRow(0, 4)]
        [DataRow(0, 0)]
        [DataRow(0, 1)]
        [TestMethod]
        [Timeout(30000)]
        public void Generate_RequiredMinimumLengthZero_ReturnsAllClassesAtMinimumLength_Test(int requiredMinimumLength, int requiredUniqueChar)
        {
            for (var i = 0; i < 200; i++)
            {
                var pass = PasswordGenerateUtils.Generate(requiredMinimumLength, requiredUniqueChar);

                Assert.AreEqual(4, pass.Length, pass);
                Assert.IsTrue(pass.Any(char.IsUpper), pass);
                Assert.IsTrue(pass.Any(char.IsLower), pass);
                Assert.IsTrue(pass.Any(char.IsDigit), pass);
                Assert.IsTrue(pass.Any(x => char.IsLetterOrDigit(x).Equals(false)), pass);
                Assert.IsTrue(pass.Distinct().Count() >= requiredUniqueChar, pass);
            }
        }

        [TestMethod]
        [Timeout(120000)]
        [TestCategory("Security")]
        public void Generate_ParallelMixedCustomChars_NoCrossContamination_Test()
        {
            var customSets = new[] { null, new[] { "~" }, new[] { "^", "%" } };
            var violations = new ConcurrentBag<string>();

            Parallel.For(0, 600, i =>
            {
                var custom = customSets[i % customSets.Length];
                var allowed = DefaultSpecialChars + (custom == null ? string.Empty : string.Concat(custom));
                var pass = PasswordGenerateUtils.Generate(16, 4, true, true, true, true, custom);

                if (pass.Length != 16)
                    violations.Add($"length {pass.Length}: '{pass}'");

                var foreign = pass.Where(x => char.IsLetterOrDigit(x).Equals(false) && allowed.IndexOf(x) < 0).ToArray();
                if (foreign.Length > 0)
                    violations.Add($"set {i % customSets.Length}: '{pass}' contains '{new string(foreign)}'");
            });

            Assert.AreEqual(0, violations.Count,
                $"{violations.Count} of 600 parallel calls were contaminated, e.g. {violations.FirstOrDefault()}");
        }
    }
}
