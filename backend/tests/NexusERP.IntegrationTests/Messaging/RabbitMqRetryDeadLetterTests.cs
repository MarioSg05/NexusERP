using System.Text;
using System.Text.Json;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

using NexusERP.Application.Common.IntegrationEvents;
using NexusERP.Application.Sales.IntegrationEvents;
using NexusERP.Infrastructure.Messaging.Inbox;
using NexusERP.Infrastructure.Messaging.RabbitMq;
using NexusERP.Infrastructure.Persistence;
using NexusERP.IntegrationTests.Infrastructure;

using RabbitMQ.Client;

namespace NexusERP.IntegrationTests.Messaging;

[Collection(MessagingIntegrationTestCollection.Name)]
public sealed class RabbitMqRetryDeadLetterTests
{
    private readonly MessagingIntegrationFixture
        _fixture;

    public RabbitMqRetryDeadLetterTests(
        MessagingIntegrationFixture fixture)
    {
        _fixture =
            fixture;
    }

    [Fact]
    public async Task Consumer_WithTransientFailure_ShouldRetryAndEventuallyProcess()
    {
        var names =
            CreateTopologyNames();

        var integrationEvent =
            CreateIntegrationEvent();

        var handler =
            new TransientFailureHandler();

        using var consumerScope =
            CreateConsumerScope(
                handler);

        await using var consumer =
            CreateConsumer(
                names,
                consumerScope.ServiceProvider);

        await consumer.StartAsync();

        await PublishAsync(
            names.ExchangeName,
            integrationEvent);

        await WaitUntilAsync(
            async () =>
            {
                using var scope =
                    _fixture.SqlServer.Factory.Services
                        .CreateScope();

                var dbContext =
                    scope.ServiceProvider
                        .GetRequiredService<
                            ApplicationDbContext>();

                return await dbContext.InboxMessages
                    .AsNoTracking()
                    .AnyAsync(
                        x =>
                            x.Id ==
                            integrationEvent.Id &&
                            x.ProcessedOnUtc != null);
            },
            TimeSpan.FromSeconds(15));

        Assert.Equal(
            2,
            handler.InvocationCount);

        var deadLetterMessage =
            await GetMessageAsync(
                names.DeadLetterQueueName);

        Assert.Null(
            deadLetterMessage);
    }

    [Fact]
    public async Task Consumer_WithPermanentFailure_ShouldMoveMessageToDeadLetterQueue()
    {
        var names =
            CreateTopologyNames();

        var integrationEvent =
            CreateIntegrationEvent();

        var handler =
            new PermanentFailureHandler();

        using var consumerScope =
            CreateConsumerScope(
                handler);

        await using var consumer =
            CreateConsumer(
                names,
                consumerScope.ServiceProvider);

        await consumer.StartAsync();

        await PublishAsync(
            names.ExchangeName,
            integrationEvent);

        var deadLetterMessage =
            await WaitForMessageAsync(
                names.DeadLetterQueueName,
                TimeSpan.FromSeconds(20));

        Assert.NotNull(
            deadLetterMessage);

        Assert.Equal(
            integrationEvent.Id.ToString(),
            deadLetterMessage.BasicProperties.MessageId);

        Assert.Equal(
            integrationEvent.Type,
            deadLetterMessage.BasicProperties.Type);

        Assert.Equal(
            4,
            handler.InvocationCount);

        using var assertScope =
            _fixture.SqlServer.Factory.Services
                .CreateScope();

        var dbContext =
            assertScope.ServiceProvider
                .GetRequiredService<
                    ApplicationDbContext>();

        var inboxExists =
            await dbContext.InboxMessages
                .AsNoTracking()
                .AnyAsync(
                    x =>
                        x.Id ==
                        integrationEvent.Id);

        Assert.False(
            inboxExists);
    }

    private IServiceScope CreateConsumerScope(
        IIntegrationEventHandler<
            SalesOrderConfirmedIntegrationEvent>
            handler)
    {
        var services =
            new ServiceCollection();

        services.AddScoped(
            _ =>
                _fixture.SqlServer.Factory.Services
                    .GetRequiredService<
                        IServiceScopeFactory>()
                    .CreateScope()
                    .ServiceProvider
                    .GetRequiredService<
                        ApplicationDbContext>());

        services.AddScoped<
            IntegrationEventInboxProcessor>();

        services.AddSingleton<
            IIntegrationEventHandler<
                SalesOrderConfirmedIntegrationEvent>>(
                    handler);

        var serviceProvider =
            services.BuildServiceProvider();

        return serviceProvider.CreateScope();
    }

    private RabbitMqIntegrationEventConsumer
        CreateConsumer(
            TopologyNames names,
            IServiceProvider serviceProvider)
    {
        var rabbitMqSettings =
            Options.Create(
                new RabbitMqSettings
                {
                    ConnectionString =
                        _fixture.RabbitMq
                            .ConnectionString,

                    ExchangeName =
                        names.ExchangeName
                });

        var consumerSettings =
            Options.Create(
                new RabbitMqConsumerSettings
                {
                    QueueName =
                        names.QueueName,

                    RoutingKey =
                        "sales-order-confirmed",

                    RetryExchangeName =
                        names.RetryExchangeName,

                    RetryQueueName =
                        names.RetryQueueName,

                    DeadLetterExchangeName =
                        names.DeadLetterExchangeName,

                    DeadLetterQueueName =
                        names.DeadLetterQueueName,

                    MaxRetryAttempts =
                        3,

                    RetryDelaySeconds =
                        1
                });

        var retryPolicy =
            new RabbitMqRetryPolicy(
                consumerSettings.Value);

        return new RabbitMqIntegrationEventConsumer(
            rabbitMqSettings,
            consumerSettings,
            serviceProvider.GetRequiredService<
                IServiceScopeFactory>(),
            NullLogger<
                RabbitMqIntegrationEventConsumer>.Instance,
            retryPolicy);
    }

    private async Task PublishAsync(
        string exchangeName,
        SalesOrderConfirmedIntegrationEvent
            integrationEvent)
    {
        var connectionFactory =
            new ConnectionFactory
            {
                Uri =
                    new Uri(
                        _fixture.RabbitMq
                            .ConnectionString)
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
            exchange:
                exchangeName,
            routingKey:
                integrationEvent.Type,
            mandatory:
                true,
            basicProperties:
                properties,
            body:
                Encoding.UTF8.GetBytes(
                    payload));
    }

    private async Task<BasicGetResult?>
        GetMessageAsync(
            string queueName)
    {
        var connectionFactory =
            new ConnectionFactory
            {
                Uri =
                    new Uri(
                        _fixture.RabbitMq
                            .ConnectionString)
            };

        await using var connection =
            await connectionFactory
                .CreateConnectionAsync();

        await using var channel =
            await connection
                .CreateChannelAsync();

        return await channel.BasicGetAsync(
            queueName,
            autoAck: true);
    }

    private async Task<BasicGetResult>
        WaitForMessageAsync(
            string queueName,
            TimeSpan timeout)
    {
        BasicGetResult? result =
            null;

        await WaitUntilAsync(
            async () =>
            {
                result =
                    await GetMessageAsync(
                        queueName);

                return result is not null;
            },
            timeout);

        return result!;
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

    private static
        SalesOrderConfirmedIntegrationEvent
        CreateIntegrationEvent()
    {
        return new SalesOrderConfirmedIntegrationEvent(
            Guid.NewGuid(),
            DateTime.UtcNow,
            Guid.NewGuid());
    }

    private static TopologyNames
        CreateTopologyNames()
    {
        var suffix =
            Guid.NewGuid().ToString("N");

        return new TopologyNames(
            $"nexuserp.test.events.{suffix}",
            $"nexuserp.test.queue.{suffix}",
            $"nexuserp.test.retry.{suffix}",
            $"nexuserp.test.queue.{suffix}.retry",
            $"nexuserp.test.dead-letter.{suffix}",
            $"nexuserp.test.queue.{suffix}.dlq");
    }

    private sealed record TopologyNames(
        string ExchangeName,
        string QueueName,
        string RetryExchangeName,
        string RetryQueueName,
        string DeadLetterExchangeName,
        string DeadLetterQueueName);

    private sealed class TransientFailureHandler
        : IIntegrationEventHandler<
            SalesOrderConfirmedIntegrationEvent>
    {
        public int InvocationCount { get; private set; }

        public Task HandleAsync(
            SalesOrderConfirmedIntegrationEvent
                integrationEvent,
            CancellationToken cancellationToken = default)
        {
            InvocationCount++;

            if (InvocationCount == 1)
            {
                throw new InvalidOperationException(
                    "Simulated transient failure.");
            }

            return Task.CompletedTask;
        }
    }

    private sealed class PermanentFailureHandler
        : IIntegrationEventHandler<
            SalesOrderConfirmedIntegrationEvent>
    {
        public int InvocationCount { get; private set; }

        public Task HandleAsync(
            SalesOrderConfirmedIntegrationEvent
                integrationEvent,
            CancellationToken cancellationToken = default)
        {
            InvocationCount++;

            throw new InvalidOperationException(
                "Simulated permanent failure.");
        }
    }
}