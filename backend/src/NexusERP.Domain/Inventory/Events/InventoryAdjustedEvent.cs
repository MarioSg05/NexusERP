using NexusERP.Domain.Common;

namespace NexusERP.Domain.Inventory.Events;

public sealed record InventoryAdjustedEvent(
    Guid InventoryId,
    int Quantity)
    : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}