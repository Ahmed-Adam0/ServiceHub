using System;

namespace ServiceHub.Domain.Common.Interfaces;

/// <summary>
/// Defines a contract for domain events dispatched by aggregate roots.
/// </summary>
public interface IDomainEvent
{
    /// <summary>
    /// Gets the date and time when the event occurred.
    /// </summary>
    DateTime OccurredOn { get; }
}
