using NexusERP.Domain.Common;

namespace NexusERP.Domain.Customers.Events;

public sealed record CustomerRegisteredEvent(Guid CustomerId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}