using System;
using System.Collections.Generic;
using System.Linq;

namespace ServiceHub.Domain.Common.Base;

/// <summary>
/// Abstract base class for Domain Value Objects enforcing structural value equality comparisons.
/// </summary>
public abstract class ValueObject
{
    /// <summary>
    /// Gets the list of member properties used for value equality sequence matching.
    /// </summary>
    /// <returns>An enumerable collection of component objects.</returns>
    protected abstract IEnumerable<object?> GetEqualityComponents();

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType())
        {
            return false;
        }

        var other = (ValueObject)obj;

        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x != null ? x.GetHashCode() : 0)
            .Aggregate((x, y) => x ^ y);
    }

    /// <summary>
    /// Compares two value object instances for equality.
    /// </summary>
    public static bool operator ==(ValueObject? left, ValueObject? right)
    {
        if (left is null && right is null)
        {
            return true;
        }

        if (left is null || right is null)
        {
            return false;
        }

        return left.Equals(right);
    }

    /// <summary>
    /// Compares two value object instances for inequality.
    /// </summary>
    public static bool operator !=(ValueObject? left, ValueObject? right)
    {
        return !(left == right);
    }
}
