using System;
using System.Collections.Generic;
using ServiceHub.Domain.Common.Interfaces;

namespace ServiceHub.Domain.Common.Base;

/// <summary>
/// Base class representing a Domain Entity with a generic identifier, equality comparison, and domain event dispatch patterns.
/// </summary>
/// <typeparam name="TId">The generic type of the identifier.</typeparam>
public abstract class BaseEntity<TId>
{
    private readonly List<IDomainEvent> _domainEvents = new();

    /// <summary>
    /// Gets the unique identifier for the entity.
    /// </summary>
    public TId Id { get; protected set; } = default!;

    /// <summary>
    /// Gets a read-only list of domain events dispatched by this entity.
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Adds a domain event to the collection.
    /// </summary>
    /// <param name="domainEvent">The domain event.</param>
    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Removes a domain event from the collection.
    /// </summary>
    /// <param name="domainEvent">The domain event.</param>
    public void RemoveDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    /// <summary>
    /// Clears all registered domain events.
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is not BaseEntity<TId> other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (EqualityComparer<TId>.Default.Equals(Id, default!) || EqualityComparer<TId>.Default.Equals(other.Id, default!))
            return false;

        return EqualityComparer<TId>.Default.Equals(Id, other.Id);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return EqualityComparer<TId>.Default.Equals(Id, default!)
            ? base.GetHashCode()
            : Id!.GetHashCode();
    }

    /// <summary>
    /// Compares two entity instances for equality.
    /// </summary>
    public static bool operator ==(BaseEntity<TId>? left, BaseEntity<TId>? right)
    {
        if (left is null && right is null)
            return true;

        if (left is null || right is null)
            return false;

        return left.Equals(right);
    }

    /// <summary>
    /// Compares two entity instances for inequality.
    /// </summary>
    public static bool operator !=(BaseEntity<TId>? left, BaseEntity<TId>? right)
    {
        return !(left == right);
    }
}

/// <summary>
/// A non-generic base class that defaults the identifier type to Guid for standard entity models.
/// </summary>
public abstract class BaseEntity : BaseEntity<Guid>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseEntity"/> class and auto-generates its unique identifier.
    /// </summary>
    protected BaseEntity()
    {
        Id = Guid.NewGuid();
    }
}
