using System.Collections.Generic;

namespace DataTypeTests.TestHelpers
{
    public sealed class HiddenMemberDerivedTestHelper : HiddenMemberBaseTestHelper
    {
        public new string Id { get; set; }

        internal static List<HiddenMemberDerivedTestHelper> CreateItems()
        {
            var first = new HiddenMemberDerivedTestHelper { Id = "D-1" };
            ((HiddenMemberBaseTestHelper)first).Id = 10;

            var second = new HiddenMemberDerivedTestHelper { Id = "D-2" };
            ((HiddenMemberBaseTestHelper)second).Id = 20;

            return new List<HiddenMemberDerivedTestHelper> { first, second };
        }
    }
}
