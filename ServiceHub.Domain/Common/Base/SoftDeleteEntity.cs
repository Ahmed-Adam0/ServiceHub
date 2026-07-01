using System;
using ServiceHub.Domain.Common.Interfaces;

namespace ServiceHub.Domain.Common.Base;

/// <summary>
/// Base class representing a Domain Entity with generic audit trail parameters and soft delete behavior.
/// </summary>
/// <typeparam name="TId">The generic type of the identifier.</typeparam>
public abstract class SoftDeleteEntity<TId> : AuditableEntity<TId>, ISoftDelete
{
    /// <inheritdoc />
    public bool IsDeleted { get; set; }

    /// <inheritdoc />
    public DateTime? DeletedAt { get; set; }

    /// <inheritdoc />
    public string? DeletedBy { get; set; }
}

/// <summary>
/// A soft deletable base class that defaults the identifier type to Guid for standard entity models.
/// </summary>
public abstract class SoftDeleteEntity : SoftDeleteEntity<Guid>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SoftDeleteEntity"/> class and auto-generates its unique identifier.
    /// </summary>
    protected SoftDeleteEntity()
    {
        Id = Guid.NewGuid();
    }
}
