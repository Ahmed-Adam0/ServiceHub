using System;

namespace ServiceHub.Domain.Common;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}

