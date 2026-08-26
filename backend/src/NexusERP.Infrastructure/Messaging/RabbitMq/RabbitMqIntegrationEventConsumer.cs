using System.Text.Json;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using NexusERP.Application.Sales.IntegrationEvents;
using NexusERP.Infrastructure.Messaging.Inbox;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NexusERP.Infrastructure.Messaging.RabbitMq;

public sealed class RabbitMqIntegrationEventConsumer
    : IAsyncDisposable
{
    private const string SalesOrderConfirmedType =
        "sales-order-confirmed";

    private readonly RabbitMqSettings
        _rabbitMqSettings;

    private readonly RabbitMqConsumerSettings
        _consumerSettings;

    private readonly IServiceScopeFactory
        _scopeFactory;

    private readonly ILogger<
        RabbitMqIntegrationEventConsumer>
        _logger;

    private IConnection? _connection;

    private IChannel? _channel;

    public RabbitMqIntegrationEventConsumer(
        IOptions<RabbitMqSettings> rabbitMqOptions,
        IOptions<RabbitMqConsumerSettings> consumerOptions,
        IServiceScopeFactory scopeFactory,
        ILogger<RabbitMqIntegrationEventConsumer> logger)
    {
        _rabbitMqSettings =
            rabbitMqOptions.Value;

        _consumerSettings =
            consumerOptions.Value;

        _scopeFactory =
            scopeFactory;

        _logger =
            logger;
    }

    public async Task StartAsync(
        CancellationToken cancellationToken = default)
    {
        ValidateSettings();

        var connectionFactory =
            new ConnectionFactory
            {
                Uri =
                    new Uri(
                        _rabbitMqSettings.ConnectionString)
            };

        _connection =
            await connectionFactory
                .CreateConnectionAsync(
                    cancellationToken);

        _channel =
            await _connection
                .CreateChannelAsync(
                    cancellationToken:
                        cancellationToken);

        await _channel.ExchangeDeclareAsync(
            exchange:
                _rabbitMqSettings.ExchangeName,
            type:
                ExchangeType.Topic,
            durable:
                true,
            autoDelete:
                false,
            arguments:
                null,
            cancellationToken:
                cancellationToken);

        await _channel.QueueDeclareAsync(
            queue:
                _consumerSettings.QueueName,
            durable:
                true,
            exclusive:
                false,
            autoDelete:
                false,
            arguments:
                null,
            cancellationToken:
                cancellationToken);

        await _channel.QueueBindAsync(
            queue:
                _consumerSettings.QueueName,
            exchange:
                _rabbitMqSettings.ExchangeName,
            routingKey:
                _consumerSettings.RoutingKey,
            arguments:
                null,
            cancellationToken:
                cancellationToken);

        var consumer =
            new AsyncEventingBasicConsumer(
                _channel);

        consumer.ReceivedAsync +=
            HandleMessageAsync;

        await _channel.BasicConsumeAsync(
            queue:
                _consumerSettings.QueueName,
            autoAck:
                false,
            consumer:
                consumer,
            cancellationToken:
                cancellationToken);
    }

    private async Task HandleMessageAsync(
        object sender,
        BasicDeliverEventArgs args)
    {
        if (_channel is null)
        {
            return;
        }

        try
        {
            if (!string.Equals(
                    args.BasicProperties.Type,
                    SalesOrderConfirmedType,
                    StringComparison.Ordinal))
            {
                await RejectAsync(
                    args.DeliveryTag);

                return;
            }

            if (!Guid.TryParse(
                    args.BasicProperties.MessageId,
                    out var messageId))
            {
                _logger.LogWarning(
                    "RabbitMQ message has an invalid MessageId. Type: {MessageType}.",
                    args.BasicProperties.Type);

                await RejectAsync(
                    args.DeliveryTag);

                return;
            }

            var payloadEvent =
                JsonSerializer.Deserialize<
                    SalesOrderConfirmedIntegrationEvent>(
                        args.Body.Span);

            if (payloadEvent is null)
            {
                await RejectAsync(
                    args.DeliveryTag);

                return;
            }

            var integrationEvent =
                new SalesOrderConfirmedIntegrationEvent(
                    messageId,
                    payloadEvent.OccurredOnUtc,
                    payloadEvent.SalesOrderId);

            using var scope =
                _scopeFactory.CreateScope();

            var inboxProcessor =
                scope.ServiceProvider
                    .GetRequiredService<
                        IntegrationEventInboxProcessor>();

            await inboxProcessor.ProcessAsync(
                integrationEvent);

            await _channel.BasicAckAsync(
                deliveryTag:
                    args.DeliveryTag,
                multiple:
                    false);
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Failed to process RabbitMQ message {MessageId} of type {MessageType}.",
                args.BasicProperties.MessageId,
                args.BasicProperties.Type);

            await RejectAsync(
                args.DeliveryTag);
        }
    }

    private async Task RejectAsync(
        ulong deliveryTag)
    {
        if (_channel is null)
        {
            return;
        }

        await _channel.BasicNackAsync(
            deliveryTag:
                deliveryTag,
            multiple:
                false,
            requeue:
                false);
    }

    private void ValidateSettings()
    {
        if (string.IsNullOrWhiteSpace(
                _rabbitMqSettings.ConnectionString))
        {
            throw new InvalidOperationException(
                "RabbitMQ connection string is not configured.");
        }

        if (string.IsNullOrWhiteSpace(
                _rabbitMqSettings.ExchangeName))
        {
            throw new InvalidOperationException(
                "RabbitMQ exchange name is not configured.");
        }

        if (string.IsNullOrWhiteSpace(
                _consumerSettings.QueueName))
        {
            throw new InvalidOperationException(
                "RabbitMQ consumer queue name is not configured.");
        }

        if (string.IsNullOrWhiteSpace(
                _consumerSettings.RoutingKey))
        {
            throw new InvalidOperationException(
                "RabbitMQ consumer routing key is not configured.");
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null)
        {
            await _channel.DisposeAsync();
        }

        if (_connection is not null)
        {
            await _connection.DisposeAsync();
        }
    }
}