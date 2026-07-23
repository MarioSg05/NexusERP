using NexusERP.Domain.Common;

namespace NexusERP.Domain.Products.Events;

public sealed record ProductRegisteredEvent(Guid ProductId)
    : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}