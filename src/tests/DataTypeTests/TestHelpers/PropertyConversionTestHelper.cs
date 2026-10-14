using System;

namespace DataTypeTests.TestHelpers
{
    internal class PropertyConversionTestModel
    {
        public int A { get; set; } = 1;

        public int B { get; set; } = 2;

        public long C { get; set; } = 3;

        public string S { get; set; } = "s";

        public DayOfWeek D { get; set; } = DayOfWeek.Monday;

        public int? N { get; set; }
    }

    internal struct TestPoint
    {
        public int X { get; set; }

        public int Y { get; set; }
    }
}
