using NexusERP.Application.Common.IntegrationEvents;
using NexusERP.Domain.Sales.Events;

namespace NexusERP.Application.Sales.IntegrationEvents;

public sealed class SalesOrderConfirmedIntegrationEventMapper
    : IIntegrationEventMapper<SalesOrderConfirmedEvent>
{
    public IIntegrationEvent Map(
        SalesOrderConfirmedEvent domainEvent)
    {
        return new SalesOrderConfirmedIntegrationEvent(
            Guid.NewGuid(),
            domainEvent.OccurredOn,
            domainEvent.SalesOrderId);
    }
}