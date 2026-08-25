using System.Text.Json;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using NexusERP.Application.Common.IntegrationEvents;
using NexusERP.Application.Sales.IntegrationEvents;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NexusERP.Infrastructure.Messaging.RabbitMq;

public sealed class RabbitMqIntegrationEventConsumer
    : IAsyncDisposable
{
    private readonly RabbitMqSettings _rabbitMqSettings;
    private readonly RabbitMqConsumerSettings _consumerSettings;
    private readonly IServiceScopeFactory _scopeFactory;

    private IConnection? _connection;
    private IChannel? _channel;

    public RabbitMqIntegrationEventConsumer(
        IOptions<RabbitMqSettings> rabbitMqOptions,
        IOptions<RabbitMqConsumerSettings> consumerOptions,
        IServiceScopeFactory scopeFactory)
    {
        _rabbitMqSettings =
            rabbitMqOptions.Value;

        _consumerSettings =
            consumerOptions.Value;

        _scopeFactory =
            scopeFactory;
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
                    cancellationToken: cancellationToken);

        await _channel.ExchangeDeclareAsync(
            exchange: _rabbitMqSettings.ExchangeName,
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false,
            arguments: null,
            cancellationToken: cancellationToken);

        await _channel.QueueDeclareAsync(
            queue: _consumerSettings.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: cancellationToken);

        await _channel.QueueBindAsync(
            queue: _consumerSettings.QueueName,
            exchange: _rabbitMqSettings.ExchangeName,
            routingKey: _consumerSettings.RoutingKey,
            arguments: null,
            cancellationToken: cancellationToken);

        var consumer =
            new AsyncEventingBasicConsumer(
                _channel);

        consumer.ReceivedAsync +=
            HandleMessageAsync;

        await _channel.BasicConsumeAsync(
            queue: _consumerSettings.QueueName,
            autoAck: false,
            consumer: consumer,
            cancellationToken: cancellationToken);
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
                    "sales-order-confirmed",
                    StringComparison.Ordinal))
            {
                await _channel.BasicNackAsync(
                    deliveryTag: args.DeliveryTag,
                    multiple: false,
                    requeue: false);

                return;
            }

            var integrationEvent =
                JsonSerializer.Deserialize<
                    SalesOrderConfirmedIntegrationEvent>(
                        args.Body.Span);

            if (integrationEvent is null)
            {
                await _channel.BasicNackAsync(
                    deliveryTag: args.DeliveryTag,
                    multiple: false,
                    requeue: false);

                return;
            }

            using var scope =
                _scopeFactory.CreateScope();

            var handler =
                scope.ServiceProvider
                    .GetRequiredService<
                        IIntegrationEventHandler<
                            SalesOrderConfirmedIntegrationEvent>>();

            await handler.HandleAsync(
                integrationEvent);

            await _channel.BasicAckAsync(
                deliveryTag: args.DeliveryTag,
                multiple: false);
        }
        catch
        {
            await _channel.BasicNackAsync(
                deliveryTag: args.DeliveryTag,
                multiple: false,
                requeue: false);
        }
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