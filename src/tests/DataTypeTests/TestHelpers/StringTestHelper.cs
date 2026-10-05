using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataTypeTests.TestHelpers
{
    internal static class StringTestHelper
    {
        internal const char ExpectedDelimiter = '|';
        internal const string Emoji = "\U0001F600";
        internal const string Nbsp = "\U000000A0";
        internal const string Zwsp = "\U0000200B";
        internal const string SoftHyphen = "\U000000AD";
        internal const string EmSpace = "\U00002003";
        internal const string IdeographicSpace = "\U00003000";
        internal const string LineSeparator = "\U00002028";
        internal const string ParagraphSeparator = "\U00002029";
        internal const string MongolianVowelSeparator = "\U0000180E";
        internal const string EAcutePrecomposed = "\U000000E9";
        internal const string CombiningAcute = "\U00000301";
        internal const string CapitalIWithDot = "\U00000130";
        internal const string DotlessI = "\U00000131";
        internal static readonly string LoneHighSurrogate = ((char)0xD83D).ToString();

        internal static string[] ParseExpected(string joined)
            => string.IsNullOrEmpty(joined) ? new string[0] : joined.Split(ExpectedDelimiter);

        internal static string Show(string value)
        {
            if (value == null)
                return "<null>";

            var sb = new StringBuilder("\"");
            foreach (var c in value)
            {
                if (c < 0x20 || c > 0x7E)
                    sb.Append("\\u").Append(((int)c).ToString("X4"));
                else
                    sb.Append(c);
            }

            return sb.Append('"').ToString();
        }

        internal static string ShowArray(string[] values)
        {
            if (values == null)
                return "<null>";

            var parts = new string[values.Length];
            for (var i = 0; i < values.Length; i++)
                parts[i] = Show(values[i]);

            return "[" + string.Join(", ", parts) + "]";
        }

        internal static List<string> Generate(int seed, string[] tokens, int count, int maxTokens)
        {
            var random = new Random(seed);
            var result = new List<string> { null, string.Empty };
            for (var i = 0; i < count; i++)
            {
                var length = random.Next(0, maxTokens + 1);
                var sb = new StringBuilder();
                for (var j = 0; j < length; j++)
                    sb.Append(tokens[random.Next(tokens.Length)]);
                result.Add(sb.ToString());
            }

            return result;
        }

        internal static void AssertSequence(string[] expected, string[] actual, string context)
        {
            Assert.IsNotNull(actual, "Result must never be null. " + context);
            Assert.AreEqual(expected.Length, actual.Length,
                $"Length mismatch. expected {ShowArray(expected)} actual {ShowArray(actual)}. {context}");
            for (var i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], actual[i],
                    $"Element {i} mismatch. expected {ShowArray(expected)} actual {ShowArray(actual)}. {context}");
        }
    }
}
