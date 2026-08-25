using Microsoft.Extensions.DependencyInjection;

using NexusERP.Application.Common.DomainEvents;
using NexusERP.Application.Common.IntegrationEvents;
using NexusERP.Application.Sales.IntegrationEvents;
using NexusERP.Domain.Sales.Events;

namespace NexusERP.Application.Common;

public static class EventProcessingDependencyInjection
{
    public static IServiceCollection AddEventProcessing(
        this IServiceCollection services)
    {
        services.AddScoped<
            IDomainEventDispatcher,
            DomainEventDispatcher>();

        services.AddScoped<
            IIntegrationEventCollector,
            IntegrationEventCollector>();

        services.AddScoped<
            IIntegrationEventMapper<SalesOrderConfirmedEvent>,
            SalesOrderConfirmedIntegrationEventMapper>();

        services.AddScoped<
            IIntegrationEventHandler<
                SalesOrderConfirmedIntegrationEvent>,
            SalesOrderConfirmedIntegrationEventHandler>();

        return services;
    }
}