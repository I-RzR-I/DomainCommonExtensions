namespace DataTypeTests.TestHelpers
{
    internal enum NoZeroTestEnum
    {
        One = 1,
        Two = 2
    }

    [System.Flags]
    internal enum NoZeroFlagsTestEnum
    {
        X = 1,
        Y = 2,
        Z = 4
    }

    [System.Flags]
    internal enum HighBitFlagsTestEnum
    {
        Low = 1,
        High = int.MinValue
    }

    [System.Flags]
    internal enum LowOnlyFlagsTestEnum
    {
        Low = 1
    }

    [System.Flags]
    internal enum ULongFlagsTestEnum : ulong
    {
        Low = 1UL,
        Top = 1UL << 63
    }
}
