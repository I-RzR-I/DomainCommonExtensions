using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataTypeTests.TestHelpers
{
    internal sealed class ExpressionShapeTestHelper : ExpressionVisitor
    {
        private readonly HashSet<ParameterExpression> _parameters = new HashSet<ParameterExpression>();

        private ExpressionShapeTestHelper()
        {
        }

        internal int InvocationCount { get; private set; }

        internal IReadOnlyCollection<ParameterExpression> Parameters => _parameters;

        internal static ExpressionShapeTestHelper Inspect(Expression expression)
        {
            var helper = new ExpressionShapeTestHelper();
            helper.Visit(expression);

            return helper;
        }

        protected override Expression VisitInvocation(InvocationExpression node)
        {
            InvocationCount++;

            return base.VisitInvocation(node);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            _parameters.Add(node);

            return base.VisitParameter(node);
        }
    }
}
