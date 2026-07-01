using System;

namespace ServiceHub.Domain.Common.Interfaces;

/// <summary>
/// Defines soft delete properties for logical record suppression and audits.
/// </summary>
public interface ISoftDelete
{
    /// <summary>
    /// Gets or sets a value indicating whether this entity is logically deleted.
    /// </summary>
    bool IsDeleted { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the entity was logically deleted.
    /// </summary>
    DateTime? DeletedAt { get; set; }

    /// <summary>
    /// Gets or sets the identity of the user who logically deleted the entity.
    /// </summary>
    string? DeletedBy { get; set; }
}
