using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ServiceHub.Domain.Common.Interfaces;

namespace ServiceHub.Domain.Common.Base;

/// <summary>
/// Abstract base class representing a Specification design pattern for generic entity queries.
/// </summary>
/// <typeparam name="T">The type of the query target.</typeparam>
public abstract class Specification<T> : ISpecification<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Specification{T}"/> class with a filter expression criteria.
    /// </summary>
    /// <param name="criteria">The filter criteria.</param>
    protected Specification(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }

    /// <inheritdoc />
    public Expression<Func<T, bool>> Criteria { get; }

    /// <inheritdoc />
    public List<Expression<Func<T, object>>> Includes { get; } = new();

    /// <inheritdoc />
    public Expression<Func<T, object>> OrderBy { get; private set; } = null!;

    /// <inheritdoc />
    public Expression<Func<T, object>> OrderByDescending { get; private set; } = null!;

    /// <summary>
    /// Adds a navigation property inclusion to the eager loading list.
    /// </summary>
    /// <param name="includeExpression">The property inclusion expression.</param>
    protected void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    /// <summary>
    /// Applies an ascending order sorting parameter.
    /// </summary>
    /// <param name="orderByExpression">The sorting expression.</param>
    protected void ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }

    /// <summary>
    /// Applies a descending order sorting parameter.
    /// </summary>
    /// <param name="orderByDescendingExpression">The sorting expression.</param>
    protected void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
    {
        OrderByDescending = orderByDescendingExpression;
    }
}
