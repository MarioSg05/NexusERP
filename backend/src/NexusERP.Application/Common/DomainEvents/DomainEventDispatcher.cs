using Microsoft.Extensions.DependencyInjection;

using NexusERP.Domain.Common;

namespace NexusERP.Application.Common.DomainEvents;

public sealed class DomainEventDispatcher
    : IDomainEventDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public DomainEventDispatcher(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task DispatchAsync(
        IReadOnlyCollection<IDomainEvent> domainEvents,
        CancellationToken cancellationToken = default)
    {
        foreach (var domainEvent in domainEvents)
        {
            cancellationToken
                .ThrowIfCancellationRequested();

            var wrapperType =
                typeof(DomainEventHandlerWrapper<>)
                    .MakeGenericType(
                        domainEvent.GetType());

            var wrapper =
                (IDomainEventHandlerWrapper)
                Activator.CreateInstance(
                    wrapperType)!;

            await wrapper.Handle(
                domainEvent,
                _serviceProvider,
                cancellationToken);
        }
    }

    private interface IDomainEventHandlerWrapper
    {
        Task Handle(
            IDomainEvent domainEvent,
            IServiceProvider serviceProvider,
            CancellationToken cancellationToken);
    }

    private sealed class DomainEventHandlerWrapper<TDomainEvent>
        : IDomainEventHandlerWrapper
        where TDomainEvent : IDomainEvent
    {
        public async Task Handle(
            IDomainEvent domainEvent,
            IServiceProvider serviceProvider,
            CancellationToken cancellationToken)
        {
            var handlers =
                serviceProvider
                    .GetServices<
                        IDomainEventHandler<TDomainEvent>>();

            foreach (var handler in handlers)
            {
                await handler.Handle(
                    (TDomainEvent)domainEvent,
                    cancellationToken);
            }
        }
    }
}