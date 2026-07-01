using System;

namespace ServiceHub.Domain.Common.Interfaces;

/// <summary>
/// Defines audit tracking properties for tracing creation and modification operations.
/// </summary>
public interface IAuditable
{
    /// <summary>
    /// Gets or sets the date and time when the entity was created.
    /// </summary>
    DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the identity of the user who created the entity.
    /// </summary>
    string? CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the entity was last modified.
    /// </summary>
    DateTime? LastModifiedAt { get; set; }

    /// <summary>
    /// Gets or sets the identity of the user who last modified the entity.
    /// </summary>
    string? LastModifiedBy { get; set; }
}
