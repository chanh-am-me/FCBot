using System.Linq.Expressions;

namespace Infrastructure.Facades.Common.Extensions;

public static class ExpressionExtension
{
    public static Expression<Func<T, TProperty>> Join<T, TProperty>(Expression<Func<T, TProperty>> baseExpression, Expression<Func<T, TProperty>>? joinExpression, ExpressionOperator exOperator = ExpressionOperator.And)
    {
        if (joinExpression != null)
        {
            ParameterExpression parammeter = Expression.Parameter(typeof(T), "x");
            Expression bodyExpr = exOperator switch
            {
                ExpressionOperator.Or => Expression.OrElse(Expression.Invoke(baseExpression, parammeter), Expression.Invoke(joinExpression, parammeter)),
                ExpressionOperator.And => Expression.AndAlso(Expression.Invoke(baseExpression, parammeter), Expression.Invoke(joinExpression, parammeter)),
                _ => throw new NotSupportedException($"{nameof(ExpressionOperator)} not supported"),
            };
            baseExpression = Expression.Lambda<Func<T, TProperty>>(bodyExpr, parammeter);
        }

        return baseExpression;
    }
}

public enum ExpressionOperator
{
    /// <summary>
    /// Và
    /// </summary>
    And = 1,

    /// <summary>
    /// Hoặc
    /// </summary>
    Or = 2,
}
