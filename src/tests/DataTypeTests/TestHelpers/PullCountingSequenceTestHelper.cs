using System.Collections.Generic;

namespace DataTypeTests.TestHelpers
{
    internal sealed class PullCountingSequenceTestHelper
    {
        internal int EnumerationCount { get; private set; }

        internal int PulledCount { get; private set; }

        internal IEnumerable<int> Sequence(int count)
        {
            EnumerationCount++;
            for (var i = 0; i < count; i++)
            {
                PulledCount++;
                yield return i;
            }
        }
    }
}
