using System.Collections.Generic;

namespace DataTypeTests.TestHelpers
{
    public sealed class DynamicMemberTestHelper
    {
        public string Code;

        public int Id { get; set; }

        public string Name { get; set; }

        public string Item { get; set; }

        private string Secret { get; set; } = "hidden";

        internal static List<DynamicMemberTestHelper> CreateItems()
            => new List<DynamicMemberTestHelper>
            {
                new DynamicMemberTestHelper { Id = 2, Name = "Beta", Item = "I-2", Code = "C-2" },
                new DynamicMemberTestHelper { Id = 1, Name = "Alpha", Item = "I-1", Code = "C-1" },
                new DynamicMemberTestHelper { Id = 3, Name = "Gamma", Item = "I-3", Code = "C-3" }
            };
    }
}
