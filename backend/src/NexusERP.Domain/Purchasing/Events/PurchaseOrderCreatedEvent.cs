using NexusERP.Domain.Common;

namespace NexusERP.Domain.Purchasing.Events;

public sealed record PurchaseOrderCreatedEvent(Guid PurchaseOrderId)
    : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}