using NexusERP.Domain.Common;

namespace NexusERP.Domain.Inventory.Events;

public sealed record InventoryCreatedEvent(Guid InventoryId)
    : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}