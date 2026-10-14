using System.Collections.Generic;

namespace DataTypeTests.TestHelpers
{
    public sealed class AmbiguousCaseTestHelper
    {
        public int Value { get; set; }

        public int VALUE { get; set; }

        internal static List<AmbiguousCaseTestHelper> CreateItems()
            => new List<AmbiguousCaseTestHelper>
            {
                new AmbiguousCaseTestHelper { Value = 1, VALUE = 10 },
                new AmbiguousCaseTestHelper { Value = 2, VALUE = 20 }
            };
    }
}
