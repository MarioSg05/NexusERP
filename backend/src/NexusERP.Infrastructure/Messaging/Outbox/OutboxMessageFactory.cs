using System.Text.Json;

using NexusERP.Application.Common.IntegrationEvents;

namespace NexusERP.Infrastructure.Messaging.Outbox;

public sealed class OutboxMessageFactory
{
    private static readonly JsonSerializerOptions
        SerializerOptions =
            new()
            {
                PropertyNamingPolicy =
                    JsonNamingPolicy.CamelCase
            };

    public OutboxMessage Create(
        IIntegrationEvent integrationEvent)
    {
        ArgumentNullException.ThrowIfNull(
            integrationEvent);

        var payload =
            JsonSerializer.Serialize(
                integrationEvent,
                integrationEvent.GetType(),
                SerializerOptions);

        return OutboxMessage.Create(
            integrationEvent.Id,
            integrationEvent.OccurredOnUtc,
            integrationEvent.Type,
            payload);
    }
}