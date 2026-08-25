using System.Text.Json;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using NexusERP.Application.Common.IntegrationEvents;
using NexusERP.Application.Sales.IntegrationEvents;
using NexusERP.Infrastructure.Messaging.RabbitMq;
using NexusERP.IntegrationTests.Infrastructure;

using RabbitMQ.Client;

namespace NexusERP.IntegrationTests.Messaging;

[Collection(RabbitMqIntegrationTestCollection.Name)]
public sealed class RabbitMqIntegrationEventConsumerTests
{
    private readonly RabbitMqFixture _rabbitMq;

    public RabbitMqIntegrationEventConsumerTests(
        RabbitMqFixture rabbitMq)
    {
        _rabbitMq =
            rabbitMq;
    }

    [Fact]
    public async Task Consumer_ShouldConsumeAndAcknowledgeSalesOrderConfirmedEvent()
    {
        var queueName =
            $"nexuserp.test.consumer.{Guid.NewGuid():N}";

        const string exchangeName =
            "nexuserp.test.events";

        const string routingKey =
            "sales-order-confirmed";

        var integrationEvent =
            new SalesOrderConfirmedIntegrationEvent(
                Guid.NewGuid(),
                DateTime.UtcNow,
                Guid.NewGuid());

        var services =
            new ServiceCollection();

        var testHandler =
            new TestSalesOrderConfirmedHandler();

        services.AddSingleton<
            IIntegrationEventHandler<
                SalesOrderConfirmedIntegrationEvent>>(
                    testHandler);

        await using var serviceProvider =
            services.BuildServiceProvider();

        var rabbitMqSettings =
            Options.Create(
                new RabbitMqSettings
                {
                    ConnectionString =
                        _rabbitMq.ConnectionString,

                    ExchangeName =
                        exchangeName
                });

        var consumerSettings =
            Options.Create(
                new RabbitMqConsumerSettings
                {
                    QueueName =
                        queueName,

                    RoutingKey =
                        routingKey
                });

        await using var consumer =
            new RabbitMqIntegrationEventConsumer(
                rabbitMqSettings,
                consumerSettings,
                serviceProvider.GetRequiredService<
                    IServiceScopeFactory>());

        await consumer.StartAsync();

        var connectionFactory =
            new ConnectionFactory
            {
                Uri =
                    new Uri(
                        _rabbitMq.ConnectionString)
            };

        await using var connection =
            await connectionFactory
                .CreateConnectionAsync();

        await using var channel =
            await connection
                .CreateChannelAsync();

        var payload =
            JsonSerializer.Serialize(
                integrationEvent);

        var properties =
            new BasicProperties
            {
                MessageId =
                    integrationEvent.Id.ToString(),

                ContentType =
                    "application/json",

                Type =
                    integrationEvent.Type,

                DeliveryMode =
                    DeliveryModes.Persistent
            };

        await channel.BasicPublishAsync(
            exchange: exchangeName,
            routingKey: routingKey,
            mandatory: true,
            basicProperties: properties,
            body:
                System.Text.Encoding.UTF8.GetBytes(
                    payload));

        var handledEvent =
            await testHandler.WaitForEventAsync(
                TimeSpan.FromSeconds(10));

        Assert.Equal(
            integrationEvent.Id,
            handledEvent.Id);

        Assert.Equal(
            integrationEvent.SalesOrderId,
            handledEvent.SalesOrderId);

        await WaitUntilAsync(
            async () =>
            {
                var result =
                    await channel.BasicGetAsync(
                        queueName,
                        autoAck: false);

                if (result is null)
                {
                    return true;
                }

                await channel.BasicNackAsync(
                    result.DeliveryTag,
                    multiple: false,
                    requeue: true);

                return false;
            },
            TimeSpan.FromSeconds(10));
    }

    [Fact]
    public async Task Consumer_WithInvalidMessage_ShouldNotLeaveMessageReady()
    {
        var queueName =
            $"nexuserp.test.consumer.invalid.{Guid.NewGuid():N}";

        const string exchangeName =
            "nexuserp.test.events";

        const string routingKey =
            "sales-order-confirmed";

        var services =
            new ServiceCollection();

        services.AddSingleton<
            IIntegrationEventHandler<
                SalesOrderConfirmedIntegrationEvent>>(
                    new TestSalesOrderConfirmedHandler());

        await using var serviceProvider =
            services.BuildServiceProvider();

        var rabbitMqSettings =
            Options.Create(
                new RabbitMqSettings
                {
                    ConnectionString =
                        _rabbitMq.ConnectionString,

                    ExchangeName =
                        exchangeName
                });

        var consumerSettings =
            Options.Create(
                new RabbitMqConsumerSettings
                {
                    QueueName =
                        queueName,

                    RoutingKey =
                        routingKey
                });

        await using var consumer =
            new RabbitMqIntegrationEventConsumer(
                rabbitMqSettings,
                consumerSettings,
                serviceProvider.GetRequiredService<
                    IServiceScopeFactory>());

        await consumer.StartAsync();

        var connectionFactory =
            new ConnectionFactory
            {
                Uri =
                    new Uri(
                        _rabbitMq.ConnectionString)
            };

        await using var connection =
            await connectionFactory
                .CreateConnectionAsync();

        await using var channel =
            await connection
                .CreateChannelAsync();

        var properties =
            new BasicProperties
            {
                Type =
                    routingKey,

                ContentType =
                    "application/json"
            };

        await channel.BasicPublishAsync(
            exchange: exchangeName,
            routingKey: routingKey,
            mandatory: true,
            basicProperties: properties,
            body:
                System.Text.Encoding.UTF8.GetBytes(
                    "{ invalid-json }"));

        await WaitUntilAsync(
            async () =>
            {
                var result =
                    await channel.BasicGetAsync(
                        queueName,
                        autoAck: false);

                if (result is null)
                {
                    return true;
                }

                await channel.BasicNackAsync(
                    result.DeliveryTag,
                    multiple: false,
                    requeue: true);

                return false;
            },
            TimeSpan.FromSeconds(10));
    }

    private static async Task WaitUntilAsync(
        Func<Task<bool>> condition,
        TimeSpan timeout)
    {
        var startedAt =
            DateTime.UtcNow;

        while (DateTime.UtcNow - startedAt < timeout)
        {
            if (await condition())
            {
                return;
            }

            await Task.Delay(
                100);
        }

        throw new TimeoutException(
            "The expected RabbitMQ condition was not reached.");
    }

    private sealed class TestSalesOrderConfirmedHandler
        : IIntegrationEventHandler<
            SalesOrderConfirmedIntegrationEvent>
    {
        private readonly TaskCompletionSource<
            SalesOrderConfirmedIntegrationEvent>
            _completionSource =
                new(
                    TaskCreationOptions
                        .RunContinuationsAsynchronously);

        public Task HandleAsync(
            SalesOrderConfirmedIntegrationEvent integrationEvent,
            CancellationToken cancellationToken = default)
        {
            _completionSource.TrySetResult(
                integrationEvent);

            return Task.CompletedTask;
        }

        public async Task<
            SalesOrderConfirmedIntegrationEvent>
            WaitForEventAsync(
                TimeSpan timeout)
        {
            return await _completionSource.Task
                .WaitAsync(
                    timeout);
        }
    }
}