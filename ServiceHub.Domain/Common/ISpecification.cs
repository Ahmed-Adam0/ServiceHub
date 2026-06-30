using System;
using System.Linq.Expressions;

namespace ServiceHub.Domain.Common;

public interface ISpecification<T>
{
    Expression<Func<T, bool>> Criteria { get; }
    bool IsSatisfiedBy(T entity);
}

