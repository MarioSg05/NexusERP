using NexusERP.Domain.Common;

namespace NexusERP.Application.Common.DomainEvents;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(
        IReadOnlyCollection<IDomainEvent> domainEvents,
        CancellationToken cancellationToken = default);
}