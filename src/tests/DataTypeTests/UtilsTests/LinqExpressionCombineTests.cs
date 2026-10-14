using System;
using System.Linq;
using System.Linq.Expressions;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Linq;

namespace DataTypeTests.UtilsTests
{
    [TestClass]
    public class LinqExpressionCombineTests
    {
        private static readonly int[] Source = { 1, 2, 3, 4, 5, 6 };

        private static readonly int[] TruthTableInputs = { -1, 0, 1, 2, 3, 4, 5, 6, 7 };

        private static readonly Expression<Func<int, bool>>[] ThreePredicates =
        {
            x => x > 1,
            y => y < 6,
            z => z % 2 == 0
        };

        private static readonly Expression<Func<int, bool>>[] ThreeAlternatives =
        {
            x => x == 1,
            y => y == 3,
            z => z > 5
        };

        [TestMethod]
        public void AndAlso_DistinctParametersTruthTable_Test()
        {
            Expression<Func<int, bool>> left = x => x > 1;
            Expression<Func<int, bool>> right = y => y < 5;

            var compiled = ExpressionExtensions.AndAlso(left, right).Compile();

            foreach (var value in TruthTableInputs)
                Assert.AreEqual(value > 1 && value < 5, compiled(value), $"value {value}");
        }

        [TestMethod]
        public void AndAlso_DistinctParametersNoInvocation_Test()
        {
            Expression<Func<int, bool>> left = x => x > 1;
            Expression<Func<int, bool>> right = y => y < 5;

            var combined = ExpressionExtensions.AndAlso(left, right);

            Assert.AreEqual(0, ExpressionShapeTestHelper.Inspect(combined).InvocationCount);
        }

        [TestMethod]
        public void AndAlso_DistinctParametersSingleParameter_Test()
        {
            Expression<Func<int, bool>> left = x => x > 1;
            Expression<Func<int, bool>> right = y => y < 5;

            var combined = ExpressionExtensions.AndAlso(left, right);
            var parameters = ExpressionShapeTestHelper.Inspect(combined).Parameters;

            Assert.AreEqual(1, combined.Parameters.Count);
            Assert.AreEqual(1, parameters.Count);
            Assert.AreSame(combined.Parameters[0], parameters.Single());
        }

        [TestMethod]
        public void AndAlso_SharedParameterNoInvocation_Test()
        {
            var parameter = Expression.Parameter(typeof(int), "x");
            var left = Expression.Lambda<Func<int, bool>>(
                Expression.GreaterThan(parameter, Expression.Constant(1)), parameter);
            var right = Expression.Lambda<Func<int, bool>>(
                Expression.LessThan(parameter, Expression.Constant(5)), parameter);

            var combined = ExpressionExtensions.AndAlso(left, right);
            var shape = ExpressionShapeTestHelper.Inspect(combined);

            Assert.AreEqual(0, shape.InvocationCount);
            Assert.AreSame(parameter, shape.Parameters.Single());
            CollectionAssert.AreEqual(new[] { 2, 3, 4 }, Source.Where(combined.Compile()).ToArray());
        }

        [TestMethod]
        public void AndAlso_AsQueryableWhere_Test()
        {
            Expression<Func<int, bool>> left = x => x > 1;
            Expression<Func<int, bool>> right = y => y < 5;

            var result = Source.AsQueryable().Where(ExpressionExtensions.AndAlso(left, right)).ToArray();

            CollectionAssert.AreEqual(new[] { 2, 3, 4 }, result);
        }

        [TestMethod]
        public void AndAlso_ShortCircuitsRightOperand_Test()
        {
            Expression<Func<int, bool>> left = x => x != 0;
            Expression<Func<int, bool>> right = y => 10 / y > 1;

            var compiled = ExpressionExtensions.AndAlso(left, right).Compile();

            Assert.IsFalse(compiled(0));
            Assert.IsTrue(compiled(2));
            Assert.IsFalse(compiled(20));
        }

        [TestMethod]
        public void And_DistinctParametersTruthTable_Test()
        {
            Expression<Func<int, bool>> left = x => x > 1;
            Expression<Func<int, bool>> right = y => y < 5;

            var compiled = PredicateBuilderExtensions.And(left, right).Compile();

            foreach (var value in TruthTableInputs)
                Assert.AreEqual(value > 1 && value < 5, compiled(value), $"value {value}");
        }

        [TestMethod]
        public void And_DistinctParametersNoInvocation_Test()
        {
            Expression<Func<int, bool>> left = x => x > 1;
            Expression<Func<int, bool>> right = y => y < 5;

            var combined = PredicateBuilderExtensions.And(left, right);

            Assert.AreEqual(0, ExpressionShapeTestHelper.Inspect(combined).InvocationCount);
        }

        [TestMethod]
        public void And_DistinctParametersSingleParameter_Test()
        {
            Expression<Func<int, bool>> left = x => x > 1;
            Expression<Func<int, bool>> right = y => y < 5;

            var combined = PredicateBuilderExtensions.And(left, right);
            var parameters = ExpressionShapeTestHelper.Inspect(combined).Parameters;

            Assert.AreEqual(1, combined.Parameters.Count);
            Assert.AreEqual(1, parameters.Count);
            Assert.AreSame(combined.Parameters[0], parameters.Single());
        }

        [TestMethod]
        public void And_AsQueryableWhere_Test()
        {
            Expression<Func<int, bool>> left = x => x > 1;
            Expression<Func<int, bool>> right = y => y < 5;

            var result = Source.AsQueryable().Where(PredicateBuilderExtensions.And(left, right)).ToArray();

            CollectionAssert.AreEqual(new[] { 2, 3, 4 }, result);
        }

        [TestMethod]
        public void And_ShortCircuitsRightOperand_Test()
        {
            Expression<Func<int, bool>> left = x => x != 0;
            Expression<Func<int, bool>> right = y => 10 / y > 1;

            var compiled = PredicateBuilderExtensions.And(left, right).Compile();

            Assert.IsFalse(compiled(0));
            Assert.IsTrue(compiled(2));
        }

        [TestMethod]
        public void And_CapturedVariablePredicate_Test()
        {
            var lowerBound = 2;
            var upperBound = 5;
            Expression<Func<int, bool>> left = x => x > lowerBound;
            Expression<Func<int, bool>> right = y => y < upperBound;

            var combined = PredicateBuilderExtensions.And(left, right);

            CollectionAssert.AreEqual(new[] { 3, 4 }, Source.AsQueryable().Where(combined).ToArray());
        }

        [TestMethod]
        public void Or_DistinctParametersTruthTable_Test()
        {
            Expression<Func<int, bool>> left = x => x < 2;
            Expression<Func<int, bool>> right = y => y > 4;

            var compiled = PredicateBuilderExtensions.Or(left, right).Compile();

            foreach (var value in TruthTableInputs)
                Assert.AreEqual(value < 2 || value > 4, compiled(value), $"value {value}");
        }

        [TestMethod]
        public void Or_DistinctParametersNoInvocation_Test()
        {
            Expression<Func<int, bool>> left = x => x < 2;
            Expression<Func<int, bool>> right = y => y > 4;

            var combined = PredicateBuilderExtensions.Or(left, right);

            Assert.AreEqual(0, ExpressionShapeTestHelper.Inspect(combined).InvocationCount);
        }

        [TestMethod]
        public void Or_DistinctParametersSingleParameter_Test()
        {
            Expression<Func<int, bool>> left = x => x < 2;
            Expression<Func<int, bool>> right = y => y > 4;

            var combined = PredicateBuilderExtensions.Or(left, right);
            var parameters = ExpressionShapeTestHelper.Inspect(combined).Parameters;

            Assert.AreEqual(1, combined.Parameters.Count);
            Assert.AreEqual(1, parameters.Count);
            Assert.AreSame(combined.Parameters[0], parameters.Single());
        }

        [TestMethod]
        public void Or_AsQueryableWhere_Test()
        {
            Expression<Func<int, bool>> left = x => x < 2;
            Expression<Func<int, bool>> right = y => y > 4;

            var result = Source.AsQueryable().Where(PredicateBuilderExtensions.Or(left, right)).ToArray();

            CollectionAssert.AreEqual(new[] { 1, 5, 6 }, result);
        }

        [TestMethod]
        public void Or_ShortCircuitsRightOperand_Test()
        {
            Expression<Func<int, bool>> left = x => x == 0;
            Expression<Func<int, bool>> right = y => 10 / y > 1;

            var compiled = PredicateBuilderExtensions.Or(left, right).Compile();

            Assert.IsTrue(compiled(0));
            Assert.IsTrue(compiled(2));
            Assert.IsFalse(compiled(20));
        }

        [TestMethod]
        public void Or_ChainedAfterAndNoInvocation_Test()
        {
            Expression<Func<int, bool>> first = x => x > 1;
            Expression<Func<int, bool>> second = y => y < 3;
            Expression<Func<int, bool>> third = z => z == 6;

            var combined = PredicateBuilderExtensions.Or(PredicateBuilderExtensions.And(first, second), third);
            var shape = ExpressionShapeTestHelper.Inspect(combined);

            Assert.AreEqual(0, shape.InvocationCount);
            Assert.AreSame(combined.Parameters[0], shape.Parameters.Single());
            CollectionAssert.AreEqual(new[] { 2, 6 }, Source.AsQueryable().Where(combined).ToArray());
        }

        [TestMethod]
        public void True_AnyInput_Test()
        {
            var compiled = PredicateBuilderExtensions.True<int>().Compile();

            foreach (var value in TruthTableInputs)
                Assert.IsTrue(compiled(value));
        }

        [TestMethod]
        public void False_AnyInput_Test()
        {
            var compiled = PredicateBuilderExtensions.False<int>().Compile();

            foreach (var value in TruthTableInputs)
                Assert.IsFalse(compiled(value));
        }

        [TestMethod]
        public void All_ThreePredicatesTruthTable_Test()
        {
            var compiled = ExpressionExtensions.All(ThreePredicates).Compile();

            foreach (var value in TruthTableInputs)
                Assert.AreEqual(value > 1 && value < 6 && value % 2 == 0, compiled(value), $"value {value}");
        }

        [TestMethod]
        public void All_ThreePredicatesNoInvocation_Test()
        {
            var combined = ExpressionExtensions.All(ThreePredicates);

            Assert.AreEqual(0, ExpressionShapeTestHelper.Inspect(combined).InvocationCount);
        }

        [TestMethod]
        public void All_ThreePredicatesSingleParameter_Test()
        {
            var combined = ExpressionExtensions.All(ThreePredicates);
            var parameters = ExpressionShapeTestHelper.Inspect(combined).Parameters;

            Assert.AreEqual(1, combined.Parameters.Count);
            Assert.AreEqual(1, parameters.Count);
            Assert.AreSame(combined.Parameters[0], parameters.Single());
        }

        [TestMethod]
        public void All_AsQueryableWhere_Test()
        {
            var result = Source.AsQueryable().Where(ExpressionExtensions.All(ThreePredicates)).ToArray();

            CollectionAssert.AreEqual(new[] { 2, 4 }, result);
        }

        [TestMethod]
        public void All_EmptySequenceAlwaysTrue_Test()
        {
            var combined = ExpressionExtensions.All(new Expression<Func<int, bool>>[0]);

            CollectionAssert.AreEqual(Source, Source.AsQueryable().Where(combined).ToArray());
        }

        [TestMethod]
        public void Any_ThreePredicatesTruthTable_Test()
        {
            var compiled = ExpressionExtensions.Any(ThreeAlternatives).Compile();

            foreach (var value in TruthTableInputs)
                Assert.AreEqual(value == 1 || value == 3 || value > 5, compiled(value), $"value {value}");
        }

        [TestMethod]
        public void Any_ThreePredicatesNoInvocation_Test()
        {
            var combined = ExpressionExtensions.Any(ThreeAlternatives);

            Assert.AreEqual(0, ExpressionShapeTestHelper.Inspect(combined).InvocationCount);
        }

        [TestMethod]
        public void Any_ThreePredicatesSingleParameter_Test()
        {
            var combined = ExpressionExtensions.Any(ThreeAlternatives);
            var parameters = ExpressionShapeTestHelper.Inspect(combined).Parameters;

            Assert.AreEqual(1, combined.Parameters.Count);
            Assert.AreEqual(1, parameters.Count);
            Assert.AreSame(combined.Parameters[0], parameters.Single());
        }

        [TestMethod]
        public void Any_AsQueryableWhere_Test()
        {
            var result = Source.AsQueryable().Where(ExpressionExtensions.Any(ThreeAlternatives)).ToArray();

            CollectionAssert.AreEqual(new[] { 1, 3, 6 }, result);
        }

        [TestMethod]
        public void Any_EmptySequenceAlwaysFalse_Test()
        {
            var combined = ExpressionExtensions.Any(new Expression<Func<int, bool>>[0]);

            Assert.AreEqual(0, Source.AsQueryable().Where(combined).Count());
        }

        [TestMethod]
        public void And_NestedLambdaCapturesParameter_Test()
        {
            Expression<Func<int, bool>> first = x => x > 1;
            Expression<Func<int, bool>> second = y => Source.Any(s => s == y);

            var combined = PredicateBuilderExtensions.And(first, second);
            var compiled = combined.Compile();

            Assert.AreEqual(0, ExpressionShapeTestHelper.Inspect(combined).InvocationCount);
            CollectionAssert.AreEqual(new[] { 2, 3, 4, 5, 6 }, TruthTableInputs.Where(compiled).ToArray());
        }

        [TestMethod]
        public void And_NullSecondExpression_Test()
        {
            Expression<Func<int, bool>> first = x => x > 1;

            var exception = Assert.ThrowsException<ArgumentNullException>(() => PredicateBuilderExtensions.And(first, null));

            Assert.AreEqual("expression", exception.ParamName);
        }

        [TestMethod]
        public void Or_NullSecondExpression_Test()
        {
            Expression<Func<int, bool>> first = x => x > 1;

            var exception = Assert.ThrowsException<ArgumentNullException>(() => PredicateBuilderExtensions.Or(first, null));

            Assert.AreEqual("expression", exception.ParamName);
        }
    }
}
