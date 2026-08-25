using NexusERP.Domain.Common;

namespace NexusERP.Application.Common.IntegrationEvents;

public interface IIntegrationEventMapper<in TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    IIntegrationEvent Map(
        TDomainEvent domainEvent);
}