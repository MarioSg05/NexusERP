namespace NexusERP.Application.Common.IntegrationEvents;

public interface IIntegrationEventPublisher
{
    Task PublishAsync(
        Guid messageId,
        string type,
        string payload,
        CancellationToken cancellationToken = default);
}