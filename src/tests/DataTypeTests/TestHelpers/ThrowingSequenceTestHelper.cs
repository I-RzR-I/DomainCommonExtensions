using System;
using System.Collections.Generic;

namespace DataTypeTests.TestHelpers
{
    internal static class ThrowingSequenceTestHelper
    {
        internal static IEnumerable<T> FirstThenThrow<T>(T first)
        {
            yield return first;

            throw new InvalidOperationException("The source was read past the first item.");
        }
    }
}
