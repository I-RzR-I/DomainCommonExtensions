using System;

namespace DataTypeTests.TestHelpers
{
    internal sealed class EqualityOverridingException : Exception
    {
        public override bool Equals(object obj) => obj is EqualityOverridingException;

        public override int GetHashCode() => 1;
    }
}
