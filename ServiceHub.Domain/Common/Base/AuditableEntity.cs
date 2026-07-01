using System;
using ServiceHub.Domain.Common.Interfaces;

namespace ServiceHub.Domain.Common.Base;

/// <summary>
/// Base class representing a Domain Entity with generic audit trail parameters.
/// </summary>
/// <typeparam name="TId">The generic type of the identifier.</typeparam>
public abstract class AuditableEntity<TId> : BaseEntity<TId>, IAuditable
{
    /// <inheritdoc />
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <inheritdoc />
    public string? CreatedBy { get; set; }

    /// <inheritdoc />
    public DateTime? LastModifiedAt { get; set; }

    /// <inheritdoc />
    public string? LastModifiedBy { get; set; }
}

/// <summary>
/// An auditable base class that defaults the identifier type to Guid for standard entity models.
/// </summary>
public abstract class AuditableEntity : AuditableEntity<Guid>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AuditableEntity"/> class and auto-generates its unique identifier.
    /// </summary>
    protected AuditableEntity()
    {
        Id = Guid.NewGuid();
    }
}
