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

    private readonly RabbitMqRetryPolicy
    _retryPolicy;

    private IConnection? _connection;

    private IChannel? _channel;


    public RabbitMqIntegrationEventConsumer(
        IOptions<RabbitMqSettings> rabbitMqOptions,
        IOptions<RabbitMqConsumerSettings> consumerOptions,
        IServiceScopeFactory scopeFactory,
        ILogger<RabbitMqIntegrationEventConsumer> logger,
        RabbitMqRetryPolicy retryPolicy)
    {
        _rabbitMqSettings =
            rabbitMqOptions.Value;

        _consumerSettings =
            consumerOptions.Value;

        _scopeFactory =
            scopeFactory;

        _logger =
            logger;

        _retryPolicy =
            retryPolicy;
    }

    private async Task PublishForRetryAsync(
    BasicDeliverEventArgs args,
    int retryCount)
    {
        if (_channel is null)
        {
            throw new InvalidOperationException(
                "RabbitMQ channel is not available.");
        }

        var properties =
            CreateForwardProperties(
                args.BasicProperties);

        properties.Headers ??=
            new Dictionary<string, object?>();

        properties.Headers[
            RabbitMqRetryHeaders.RetryCount] =
                retryCount;

        await _channel.BasicPublishAsync(
            exchange:
                _consumerSettings.RetryExchangeName,
            routingKey:
                _consumerSettings.RoutingKey,
            mandatory:
                true,
            basicProperties:
                properties,
            body:
                args.Body);
    }

    private async Task PublishToDeadLetterAsync(
        BasicDeliverEventArgs args)
    {
        if (_channel is null)
        {
            throw new InvalidOperationException(
                "RabbitMQ channel is not available.");
        }

        var properties =
            CreateForwardProperties(
                args.BasicProperties);

        await _channel.BasicPublishAsync(
            exchange:
                _consumerSettings
                    .DeadLetterExchangeName,
            routingKey:
                _consumerSettings.RoutingKey,
            mandatory:
                true,
            basicProperties:
                properties,
            body:
                args.Body);
    }

    private static BasicProperties
        CreateForwardProperties(
            IReadOnlyBasicProperties source)
    {
        return new BasicProperties
        {
            MessageId =
                source.MessageId,

            ContentType =
                source.ContentType,

            Type =
                source.Type,

            DeliveryMode =
                DeliveryModes.Persistent,

            Headers =
                source.Headers is null
                    ? new Dictionary<
                        string,
                        object?>()
                    : new Dictionary<
                        string,
                        object?>(
                            source.Headers)
        };
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

        await _channel.ExchangeDeclareAsync(
            exchange:
                _consumerSettings.RetryExchangeName,
            type:
                ExchangeType.Direct,
            durable:
                true,
            autoDelete:
                false,
            arguments:
                null,
            cancellationToken:
                cancellationToken);

        await _channel.ExchangeDeclareAsync(
            exchange:
                _consumerSettings.DeadLetterExchangeName,
            type:
                ExchangeType.Direct,
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

        var retryQueueArguments =
            new Dictionary<string, object?>
            {
                ["x-message-ttl"] =
                    _consumerSettings.RetryDelaySeconds *
                    1000,

                ["x-dead-letter-exchange"] =
                    _rabbitMqSettings.ExchangeName,

                ["x-dead-letter-routing-key"] =
                    _consumerSettings.RoutingKey
            };

        await _channel.QueueDeclareAsync(
            queue:
                _consumerSettings.RetryQueueName,
            durable:
                true,
            exclusive:
                false,
            autoDelete:
                false,
            arguments:
                retryQueueArguments,
            cancellationToken:
                cancellationToken);

        await _channel.QueueBindAsync(
            queue:
                _consumerSettings.RetryQueueName,
            exchange:
                _consumerSettings.RetryExchangeName,
            routingKey:
                _consumerSettings.RoutingKey,
            arguments:
                null,
            cancellationToken:
                cancellationToken);

        await _channel.QueueDeclareAsync(
            queue:
                _consumerSettings.DeadLetterQueueName,
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
                _consumerSettings.DeadLetterQueueName,
            exchange:
                _consumerSettings.DeadLetterExchangeName,
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
            var retryCount =
                _retryPolicy.GetRetryCount(
                    args.BasicProperties);

            _logger.LogError(
                exception,
                "Failed to process RabbitMQ message {MessageId} of type {MessageType}. Retry count: {RetryCount}.",
                args.BasicProperties.MessageId,
                args.BasicProperties.Type,
                retryCount);

            if (_retryPolicy.ShouldRetry(
                    retryCount))
            {
                var nextRetryCount =
                    retryCount + 1;

                await PublishForRetryAsync(
                    args,
                    nextRetryCount);

                _logger.LogWarning(
                    "RabbitMQ message {MessageId} scheduled for retry {RetryCount}.",
                    args.BasicProperties.MessageId,
                    nextRetryCount);
            }
            else
            {
                await PublishToDeadLetterAsync(
                    args);

                _logger.LogError(
                    "RabbitMQ message {MessageId} moved to the dead-letter queue after {RetryCount} retries.",
                    args.BasicProperties.MessageId,
                    retryCount);
            }

            await _channel.BasicAckAsync(
                deliveryTag:
                    args.DeliveryTag,
                multiple:
                    false);
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

        if (string.IsNullOrWhiteSpace(
        _consumerSettings.RetryExchangeName))
        {
            throw new InvalidOperationException(
                "RabbitMQ retry exchange name is not configured.");
        }

        if (string.IsNullOrWhiteSpace(
                _consumerSettings.RetryQueueName))
        {
            throw new InvalidOperationException(
                "RabbitMQ retry queue name is not configured.");
        }

        if (string.IsNullOrWhiteSpace(
                _consumerSettings.DeadLetterExchangeName))
        {
            throw new InvalidOperationException(
                "RabbitMQ dead-letter exchange name is not configured.");
        }

        if (string.IsNullOrWhiteSpace(
                _consumerSettings.DeadLetterQueueName))
        {
            throw new InvalidOperationException(
                "RabbitMQ dead-letter queue name is not configured.");
        }

        if (_consumerSettings.MaxRetryAttempts <= 0)
        {
            throw new InvalidOperationException(
                "RabbitMQ maximum retry attempts must be greater than zero.");
        }

        if (_consumerSettings.RetryDelaySeconds <= 0)
        {
            throw new InvalidOperationException(
                "RabbitMQ retry delay must be greater than zero.");
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