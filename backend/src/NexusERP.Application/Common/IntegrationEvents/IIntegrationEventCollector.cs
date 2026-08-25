using NexusERP.Domain.Common;

namespace NexusERP.Application.Common.IntegrationEvents;

public interface IIntegrationEventCollector
{
    IReadOnlyCollection<IIntegrationEvent> Collect(
        IReadOnlyCollection<IDomainEvent> domainEvents);
}