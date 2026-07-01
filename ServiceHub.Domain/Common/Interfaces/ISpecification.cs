using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ServiceHub.Domain.Common.Interfaces;

/// <summary>
/// Defines specification query patterns for domain entity criteria filters and eager loading links.
/// </summary>
/// <typeparam name="T">The type of the query target.</typeparam>
public interface ISpecification<T>
{
    /// <summary>
    /// Gets the criteria filter expression.
    /// </summary>
    Expression<Func<T, bool>> Criteria { get; }

    /// <summary>
    /// Gets the list of navigation property inclusion expressions.
    /// </summary>
    List<Expression<Func<T, object>>> Includes { get; }

    /// <summary>
    /// Gets the ascending sorting expression.
    /// </summary>
    Expression<Func<T, object>> OrderBy { get; }

    /// <summary>
    /// Gets the descending sorting expression.
    /// </summary>
    Expression<Func<T, object>> OrderByDescending { get; }
}
