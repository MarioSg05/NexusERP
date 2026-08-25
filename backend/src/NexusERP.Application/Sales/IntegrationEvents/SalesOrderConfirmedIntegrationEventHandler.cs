using NexusERP.Application.Common.IntegrationEvents;

namespace NexusERP.Application.Sales.IntegrationEvents;

public sealed class SalesOrderConfirmedIntegrationEventHandler
    : IIntegrationEventHandler<
        SalesOrderConfirmedIntegrationEvent>
{
    public Task HandleAsync(
        SalesOrderConfirmedIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(
            integrationEvent);

        return Task.CompletedTask;
    }
}