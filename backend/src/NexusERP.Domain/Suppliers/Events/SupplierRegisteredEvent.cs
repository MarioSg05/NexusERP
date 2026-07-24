using NexusERP.Domain.Common;

namespace NexusERP.Domain.Suppliers.Events;

public sealed record SupplierRegisteredEvent(Guid SupplierId)
    : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}