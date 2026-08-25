using System.Text;

using Microsoft.Extensions.Options;

using NexusERP.Application.Common.IntegrationEvents;

using RabbitMQ.Client;

namespace NexusERP.Infrastructure.Messaging.RabbitMq;

public sealed class RabbitMqIntegrationEventPublisher
    : IIntegrationEventPublisher
{
    private readonly RabbitMqSettings _settings;

    public RabbitMqIntegrationEventPublisher(
        IOptions<RabbitMqSettings> options)
    {
        _settings =
            options.Value;
    }

    public async Task PublishAsync(
        Guid messageId,
        string type,
        string payload,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(
            type);

        ArgumentException.ThrowIfNullOrWhiteSpace(
            payload);

        if (string.IsNullOrWhiteSpace(
                _settings.ConnectionString))
        {
            throw new InvalidOperationException(
                "RabbitMQ connection string is not configured.");
        }

        if (string.IsNullOrWhiteSpace(
                _settings.ExchangeName))
        {
            throw new InvalidOperationException(
                "RabbitMQ exchange name is not configured.");
        }

        var connectionFactory =
            new ConnectionFactory
            {
                Uri =
                    new Uri(
                        _settings.ConnectionString)
            };

        await using var connection =
            await connectionFactory
                .CreateConnectionAsync(
                    cancellationToken);

        await using var channel =
            await connection
                .CreateChannelAsync(
                    cancellationToken: cancellationToken);

        await channel.ExchangeDeclareAsync(
            exchange: _settings.ExchangeName,
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false,
            arguments: null,
            cancellationToken: cancellationToken);

        var properties =
            new BasicProperties
            {
                MessageId =
                    messageId.ToString(),

                ContentType =
                    "application/json",

                Type =
                    type,

                DeliveryMode =
                    DeliveryModes.Persistent
            };

        var body =
            Encoding.UTF8.GetBytes(
                payload);

        await channel.BasicPublishAsync(
            exchange: _settings.ExchangeName,
            routingKey: type,
            mandatory: true,
            basicProperties: properties,
            body: body,
            cancellationToken: cancellationToken);
    }
}