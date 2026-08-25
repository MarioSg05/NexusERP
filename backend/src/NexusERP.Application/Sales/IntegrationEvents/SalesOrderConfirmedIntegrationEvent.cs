using NexusERP.Application.Common.IntegrationEvents;

namespace NexusERP.Application.Sales.IntegrationEvents;

public sealed record SalesOrderConfirmedIntegrationEvent(
    Guid Id,
    DateTime OccurredOnUtc,
    Guid SalesOrderId)
    : IIntegrationEvent
{
    public string Type =>
        "sales-order-confirmed";
}