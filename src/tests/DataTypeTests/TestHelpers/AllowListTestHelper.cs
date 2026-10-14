using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DataTypeTests.TestHelpers
{
    internal static class AllowListTestHelper
    {
        internal static List<AmbiguousCaseTestHelper> CreateCrossedCaseItems()
            => new List<AmbiguousCaseTestHelper>
            {
                new AmbiguousCaseTestHelper { Value = 2, VALUE = 10 },
                new AmbiguousCaseTestHelper { Value = 1, VALUE = 30 },
                new AmbiguousCaseTestHelper { Value = 3, VALUE = 20 }
            };
    }

    internal class CopyCountingDestinationTestModel
    {
        private string _name;

        public int Id { get; set; }

        public int NameSetCount { get; private set; }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                NameSetCount++;
            }
        }
    }

    internal sealed class ThrowingQueryableTestHelper<T> : IQueryable<T>
    {
        public Type ElementType => typeof(T);

        public Expression Expression => throw new InvalidOperationException("The query expression was read.");

        public IQueryProvider Provider => throw new InvalidOperationException("The query provider was read.");

        public IEnumerator<T> GetEnumerator() => throw new InvalidOperationException("The query was enumerated.");

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    internal sealed class ThrowingEnumerableTestHelper<T> : IEnumerable<T>
    {
        public IEnumerator<T> GetEnumerator() => throw new InvalidOperationException("The source was enumerated.");

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
