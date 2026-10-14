namespace DataTypeTests.TestHelpers
{
    internal class CopyPlainTestModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    internal class CopyRestrictedSourceTestModel
    {
        public int ProtectedValue { get; set; }

        public int InternalValue { get; set; }

        public int PrivateValue { get; set; }

        public string Name { get; set; }
    }

    internal class CopyRestrictedDestinationTestModel
    {
        public CopyRestrictedDestinationTestModel(int protectedValue, int internalValue, int privateValue)
        {
            ProtectedValue = protectedValue;
            InternalValue = internalValue;
            PrivateValue = privateValue;
        }

        public int ProtectedValue { get; protected set; }

        public int InternalValue { get; internal set; }

        public int PrivateValue { get; private set; }

        public string Name { get; set; }
    }

    internal class CopyIndexedTestModel
    {
        private readonly int[] _items = new int[4];

        public int Id { get; set; }

        public string Name { get; set; }

        public int this[int index]
        {
            get => _items[index];
            set => _items[index] = value;
        }
    }

    internal class CopyStringNameTestModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    internal class CopyIntNameTestModel
    {
        public int Id { get; set; }

        public int Name { get; set; }
    }

    internal class CopyReadOnlyDestinationTestModel
    {
        public int Id { get; } = 5;

        public string Name { get; set; }
    }

    internal class CopyGetOnlySourceTestModel
    {
        public int Id => 42;

        public string Name { get; set; }
    }

    internal class CopyHiddenBaseTestModel
    {
        public int Value { get; set; }
    }

    internal class CopyHiddenDerivedTestModel : CopyHiddenBaseTestModel
    {
        public new string Value { get; set; }
    }

    internal class CopyHiddenSourceTestModel
    {
        public string Value { get; set; }
    }

    internal class CopyNarrowSourceTestModel
    {
        public int Number { get; set; }

        public string Name { get; set; }
    }

    internal class CopyWideDestinationTestModel
    {
        public long Number { get; set; }

        public string Name { get; set; }
    }

    internal class CopyStaticSourceTestModel
    {
        public static int Counter { get; set; }

        public int Id { get; set; }
    }

    internal class CopyInstanceCounterTestModel
    {
        public int Counter { get; set; }

        public int Id { get; set; }

        public static int Shared { get; set; }
    }

    internal class CopySharedSourceTestModel
    {
        public int Shared { get; set; }
    }

    internal class CopyPrivateGetterSourceTestModel
    {
        public int Secret { private get; set; }

        public string Name { get; set; }
    }

    internal class CopySecretDestinationTestModel
    {
        public int Secret { get; set; }

        public string Name { get; set; }
    }
}
