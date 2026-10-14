using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Linq;

namespace DataTypeTests.UtilsTests
{
    [TestClass]
    public class ExpressionCombineAndOrTests
    {
        private static readonly int[] TruthTableInputs = { -1, 0, 1, 2, 3, 4, 5, 6, 7 };

        private static readonly Expression<Func<int, bool>>[] AndPredicates =
        {
            x => x > 1,
            y => y < 6,
            z => z % 2 == 0
        };

        private static readonly Expression<Func<int, bool>>[] OrPredicates =
        {
            x => x == 1,
            y => y == 3,
            z => z > 5
        };

        [TestMethod]
        public void CombineAnd_OnePredicateTruthTable_Test()
        {
            var compiled = AndPredicates.Take(1).CombineAnd().Compile();

            foreach (var value in TruthTableInputs)
                Assert.AreEqual(value > 1, compiled(value), $"value {value}");
        }

        [TestMethod]
        public void CombineAnd_TwoPredicatesTruthTable_Test()
        {
            var compiled = AndPredicates.Take(2).CombineAnd().Compile();

            foreach (var value in TruthTableInputs)
                Assert.AreEqual(value > 1 && value < 6, compiled(value), $"value {value}");
        }

        [TestMethod]
        public void CombineAnd_ThreePredicatesTruthTable_Test()
        {
            var compiled = AndPredicates.CombineAnd().Compile();

            foreach (var value in TruthTableInputs)
                Assert.AreEqual(value > 1 && value < 6 && value % 2 == 0, compiled(value), $"value {value}");
        }

        [TestMethod]
        public void CombineOr_OnePredicateTruthTable_Test()
        {
            var compiled = OrPredicates.Take(1).CombineOr().Compile();

            foreach (var value in TruthTableInputs)
                Assert.AreEqual(value == 1, compiled(value), $"value {value}");
        }

        [TestMethod]
        public void CombineOr_TwoPredicatesTruthTable_Test()
        {
            var compiled = OrPredicates.Take(2).CombineOr().Compile();

            foreach (var value in TruthTableInputs)
                Assert.AreEqual(value == 1 || value == 3, compiled(value), $"value {value}");
        }

        [TestMethod]
        public void CombineOr_ThreePredicatesTruthTable_Test()
        {
            var compiled = OrPredicates.CombineOr().Compile();

            foreach (var value in TruthTableInputs)
                Assert.AreEqual(value == 1 || value == 3 || value > 5, compiled(value), $"value {value}");
        }

        [TestMethod]
        public void CombineAnd_EmptySourceAlwaysTrue_Test()
        {
            var compiled = new List<Expression<Func<int, bool>>>().CombineAnd().Compile();

            foreach (var value in TruthTableInputs)
                Assert.IsTrue(compiled(value), $"value {value}");
        }

        [TestMethod]
        public void CombineOr_EmptySourceAlwaysFalse_Test()
        {
            var compiled = new List<Expression<Func<int, bool>>>().CombineOr().Compile();

            foreach (var value in TruthTableInputs)
                Assert.IsFalse(compiled(value), $"value {value}");
        }

        [TestMethod]
        public void CombineAnd_NullSource_Test()
        {
            IEnumerable<Expression<Func<int, bool>>> source = null;

            Assert.ThrowsException<ArgumentNullException>(() => source.CombineAnd());
        }

        [TestMethod]
        public void CombineOr_NullSource_Test()
        {
            IEnumerable<Expression<Func<int, bool>>> source = null;

            Assert.ThrowsException<ArgumentNullException>(() => source.CombineOr());
        }

        [TestMethod]
        public void CombineAnd_NullFirstElement_Test()
        {
            var source = new Expression<Func<int, bool>>[] { null, x => x > 1 };

            Assert.ThrowsException<ArgumentNullException>(() => source.CombineAnd());
        }

        [TestMethod]
        public void CombineAnd_NullMiddleElement_Test()
        {
            var source = new Expression<Func<int, bool>>[] { x => x > 1, null, y => y < 6 };

            Assert.ThrowsException<ArgumentNullException>(() => source.CombineAnd());
        }

        [TestMethod]
        public void CombineOr_NullFirstElement_Test()
        {
            var source = new Expression<Func<int, bool>>[] { null, x => x == 1 };

            Assert.ThrowsException<ArgumentNullException>(() => source.CombineOr());
        }

        [TestMethod]
        public void CombineOr_NullMiddleElement_Test()
        {
            var source = new Expression<Func<int, bool>>[] { x => x == 1, null, y => y == 3 };

            Assert.ThrowsException<ArgumentNullException>(() => source.CombineOr());
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        public void CombineAnd_MatchesAll_Test(int count)
        {
            var predicates = AndPredicates.Take(count).ToArray();

            var expected = TruthTableInputs.Where(ExpressionExtensions.All(predicates).Compile()).ToArray();
            var actual = TruthTableInputs.Where(predicates.CombineAnd().Compile()).ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        public void CombineOr_MatchesAny_Test(int count)
        {
            var predicates = OrPredicates.Take(count).ToArray();

            var expected = TruthTableInputs.Where(ExpressionExtensions.Any(predicates).Compile()).ToArray();
            var actual = TruthTableInputs.Where(predicates.CombineOr().Compile()).ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(3)]
        public void CombineAnd_SingleLambdaNoInvocation_Test(int count)
        {
            var combined = AndPredicates.Take(count).CombineAnd();
            var shape = ExpressionShapeTestHelper.Inspect(combined);

            Assert.AreEqual(1, combined.Parameters.Count);
            Assert.AreEqual(0, shape.InvocationCount);
            Assert.AreSame(combined.Parameters[0], shape.Parameters.Single());
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(3)]
        public void CombineOr_SingleLambdaNoInvocation_Test(int count)
        {
            var combined = OrPredicates.Take(count).CombineOr();
            var shape = ExpressionShapeTestHelper.Inspect(combined);

            Assert.AreEqual(1, combined.Parameters.Count);
            Assert.AreEqual(0, shape.InvocationCount);
            Assert.AreSame(combined.Parameters[0], shape.Parameters.Single());
        }

        [TestMethod]
        public void CombineAnd_SameInstanceTwice_Test()
        {
            Expression<Func<int, bool>> positive = x => x > 0;
            var combined = new[] { positive, positive }.CombineAnd();
            var shape = ExpressionShapeTestHelper.Inspect(combined);

            Assert.AreEqual(0, shape.InvocationCount);
            Assert.AreSame(combined.Parameters[0], shape.Parameters.Single());
            CollectionAssert.AreEqual(new[] { 1, 2, 3, 4, 5, 6, 7 }, TruthTableInputs.Where(combined.Compile()).ToArray());
        }

        [TestMethod]
        public void CombineAnd_AsQueryableWhere_Test()
        {
            var result = TruthTableInputs.AsQueryable().Where(AndPredicates.CombineAnd()).ToArray();

            CollectionAssert.AreEqual(new[] { 2, 4 }, result);
        }

        [TestMethod]
        public void CombineOr_AsQueryableWhere_Test()
        {
            var result = TruthTableInputs.AsQueryable().Where(OrPredicates.CombineOr()).ToArray();

            CollectionAssert.AreEqual(new[] { 1, 3, 6, 7 }, result);
        }

        [TestMethod]
        public void CombineAnd_ShortCircuitsLaterOperand_Test()
        {
            var source = new Expression<Func<int, bool>>[] { x => x != 0, y => 10 / y > 1 };

            var compiled = source.CombineAnd().Compile();

            Assert.IsFalse(compiled(0));
            Assert.IsTrue(compiled(2));
            Assert.IsFalse(compiled(20));
        }

        [TestMethod]
        public void CombineOr_ShortCircuitsLaterOperand_Test()
        {
            var source = new Expression<Func<int, bool>>[] { x => x == 0, y => 10 / y > 1 };

            var compiled = source.CombineOr().Compile();

            Assert.IsTrue(compiled(0));
            Assert.IsTrue(compiled(2));
            Assert.IsFalse(compiled(20));
        }

        [TestMethod]
        public void CombineOr_ListExtensionSyntax_Test()
        {
            var list = new List<Expression<Func<int, bool>>> { x => x == 1, y => y > 5 };

            Expression<Func<int, bool>> combined = list.CombineOr();

            CollectionAssert.AreEqual(new[] { 1, 6, 7 }, TruthTableInputs.Where(combined.Compile()).ToArray());
        }
    }
}
