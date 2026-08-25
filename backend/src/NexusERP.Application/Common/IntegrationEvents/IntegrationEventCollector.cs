using Microsoft.Extensions.DependencyInjection;

using NexusERP.Domain.Common;

namespace NexusERP.Application.Common.IntegrationEvents;

public sealed class IntegrationEventCollector
    : IIntegrationEventCollector
{
    private readonly IServiceProvider _serviceProvider;

    public IntegrationEventCollector(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IReadOnlyCollection<IIntegrationEvent> Collect(
        IReadOnlyCollection<IDomainEvent> domainEvents)
    {
        var integrationEvents =
            new List<IIntegrationEvent>();

        foreach (var domainEvent in domainEvents)
        {
            var wrapperType =
                typeof(IntegrationEventMapperWrapper<>)
                    .MakeGenericType(
                        domainEvent.GetType());

            var wrapper =
                (IIntegrationEventMapperWrapper)
                Activator.CreateInstance(
                    wrapperType)!;

            integrationEvents.AddRange(
                wrapper.Map(
                    domainEvent,
                    _serviceProvider));
        }

        return integrationEvents;
    }

    private interface IIntegrationEventMapperWrapper
    {
        IReadOnlyCollection<IIntegrationEvent> Map(
            IDomainEvent domainEvent,
            IServiceProvider serviceProvider);
    }

    private sealed class IntegrationEventMapperWrapper<TDomainEvent>
        : IIntegrationEventMapperWrapper
        where TDomainEvent : IDomainEvent
    {
        public IReadOnlyCollection<IIntegrationEvent> Map(
            IDomainEvent domainEvent,
            IServiceProvider serviceProvider)
        {
            var mappers =
                serviceProvider
                    .GetServices<
                        IIntegrationEventMapper<TDomainEvent>>();

            return mappers
                .Select(
                    mapper =>
                        mapper.Map(
                            (TDomainEvent)domainEvent))
                .ToList();
        }
    }
}