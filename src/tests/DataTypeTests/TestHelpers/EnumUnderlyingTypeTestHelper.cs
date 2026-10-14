namespace DataTypeTests.TestHelpers
{
    internal enum OrdinalTestEnum
    {
        First,
        Second,
        Third
    }

    internal enum LevelTestEnum
    {
        NONE,
        INFO
    }

    internal enum ByteBasedTestEnum : byte
    {
        Zero = 0,
        Seven = 7
    }

    internal enum LongBasedTestEnum : long
    {
        Small = 42,
        Large = (long)int.MaxValue + 1
    }

    [System.Flags]
    internal enum FlagsTestEnum
    {
        None = 0,
        A = 1,
        B = 2
    }

    internal enum NegativeTestEnum
    {
        Neg = -5,
        Zero = 0,
        Pos = 5
    }

    internal enum UIntBasedTestEnum : uint
    {
        Zero = 0,
        One = 1,
        Big = 3000000000
    }
}
