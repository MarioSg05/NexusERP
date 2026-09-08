using NexusERP.Domain.Common;

namespace NexusERP.Domain.Sales.Events;

public sealed record SalesOrderConfirmedEvent(Guid SalesOrderId)
    : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}