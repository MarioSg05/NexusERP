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
public sealed class RabbitMqIntegrationEventConsumerTests
{
    private readonly MessagingIntegrationFixture
        _fixture;

    public RabbitMqIntegrationEventConsumerTests(
        MessagingIntegrationFixture fixture)
    {
        _fixture =
            fixture;
    }

    [Fact]
    public async Task Consumer_ShouldConsumeAcknowledgeAndPersistInboxMessage()
    {
        var queueName =
            $"nexuserp.test.consumer.{Guid.NewGuid():N}";

        var exchangeName =
            $"nexuserp.test.events.{Guid.NewGuid():N}";

        const string routingKey =
            "sales-order-confirmed";

        var integrationEvent =
            new SalesOrderConfirmedIntegrationEvent(
                Guid.NewGuid(),
                DateTime.UtcNow,
                Guid.NewGuid());

        var handler =
            new CountingHandler();

        using var consumerScope =
            CreateConsumerScope(
                handler);

        await using var consumer =
            CreateConsumer(
                queueName,
                exchangeName,
                consumerScope.ServiceProvider);

        await consumer.StartAsync();

        await PublishAsync(
            exchangeName,
            routingKey,
            integrationEvent);

        await handler.WaitForInvocationAsync(
            TimeSpan.FromSeconds(10));

        await WaitUntilAsync(
            async () =>
            {
                using var assertScope =
                    _fixture.SqlServer.Factory.Services
                        .CreateScope();

                var dbContext =
                    assertScope.ServiceProvider
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
            TimeSpan.FromSeconds(10));

        Assert.Equal(
            1,
            handler.InvocationCount);

        using var finalScope =
            _fixture.SqlServer.Factory.Services
                .CreateScope();

        var finalDbContext =
            finalScope.ServiceProvider
                .GetRequiredService<
                    ApplicationDbContext>();

        var inboxMessage =
            await finalDbContext.InboxMessages
                .AsNoTracking()
                .SingleAsync(
                    x =>
                        x.Id ==
                        integrationEvent.Id);

        Assert.Equal(
            integrationEvent.Type,
            inboxMessage.Type);

        Assert.NotNull(
            inboxMessage.ProcessedOnUtc);

        await AssertQueueIsEmptyAsync(
            queueName);
    }

    [Fact]
    public async Task Consumer_WithDuplicateDelivery_ShouldInvokeHandlerOnlyOnce()
    {
        var queueName =
            $"nexuserp.test.consumer.duplicate.{Guid.NewGuid():N}";

        var exchangeName =
            $"nexuserp.test.events.{Guid.NewGuid():N}";

        const string routingKey =
            "sales-order-confirmed";

        var integrationEvent =
            new SalesOrderConfirmedIntegrationEvent(
                Guid.NewGuid(),
                DateTime.UtcNow,
                Guid.NewGuid());

        var handler =
            new CountingHandler();

        using var consumerScope =
            CreateConsumerScope(
                handler);

        await using var consumer =
            CreateConsumer(
                queueName,
                exchangeName,
                consumerScope.ServiceProvider);

        await consumer.StartAsync();

        await PublishAsync(
            exchangeName,
            routingKey,
            integrationEvent);

        await handler.WaitForInvocationAsync(
            TimeSpan.FromSeconds(10));

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
                            integrationEvent.Id);
            },
            TimeSpan.FromSeconds(10));

        await PublishAsync(
            exchangeName,
            routingKey,
            integrationEvent);

        await Task.Delay(
            1000);

        Assert.Equal(
            1,
            handler.InvocationCount);

        using var assertScope =
            _fixture.SqlServer.Factory.Services
                .CreateScope();

        var assertDbContext =
            assertScope.ServiceProvider
                .GetRequiredService<
                    ApplicationDbContext>();

        var inboxCount =
            await assertDbContext.InboxMessages
                .AsNoTracking()
                .CountAsync(
                    x =>
                        x.Id ==
                        integrationEvent.Id);

        Assert.Equal(
            1,
            inboxCount);

        await AssertQueueIsEmptyAsync(
            queueName);
    }

    [Fact]
    public async Task Consumer_WhenHandlerFails_ShouldNotPersistInboxMessage()
    {
        var queueName =
            $"nexuserp.test.consumer.failure.{Guid.NewGuid():N}";

        var exchangeName =
            $"nexuserp.test.events.{Guid.NewGuid():N}";

        const string routingKey =
            "sales-order-confirmed";

        var integrationEvent =
            new SalesOrderConfirmedIntegrationEvent(
                Guid.NewGuid(),
                DateTime.UtcNow,
                Guid.NewGuid());

        var handler =
            new FailingHandler();

        using var consumerScope =
            CreateConsumerScope(
                handler);

        await using var consumer =
            CreateConsumer(
                queueName,
                exchangeName,
                consumerScope.ServiceProvider);

        await consumer.StartAsync();

        await PublishAsync(
            exchangeName,
            routingKey,
            integrationEvent);

        await handler.WaitForInvocationAsync(
            TimeSpan.FromSeconds(10));

        await Task.Delay(
            500);

        using var assertScope =
            _fixture.SqlServer.Factory.Services
                .CreateScope();

        var assertDbContext =
            assertScope.ServiceProvider
                .GetRequiredService<
                    ApplicationDbContext>();

        var inboxExists =
            await assertDbContext.InboxMessages
                .AsNoTracking()
                .AnyAsync(
                    x =>
                        x.Id ==
                        integrationEvent.Id);

        Assert.False(
            inboxExists);

        Assert.Equal(
            1,
            handler.InvocationCount);

        await AssertQueueIsEmptyAsync(
            queueName);
    }

    [Fact]
    public async Task Consumer_ShouldUseRabbitMqMessageIdForInboxIdentity()
    {
        var queueName =
            $"nexuserp.test.consumer.message-id.{Guid.NewGuid():N}";

        var exchangeName =
            $"nexuserp.test.events.{Guid.NewGuid():N}";

        const string routingKey =
            "sales-order-confirmed";

        var rabbitMqMessageId =
            Guid.NewGuid();

        var integrationEvent =
            new SalesOrderConfirmedIntegrationEvent(
                Guid.Empty,
                DateTime.UtcNow,
                Guid.NewGuid());

        var handler =
            new CountingHandler();

        using var consumerScope =
            CreateConsumerScope(
                handler);

        await using var consumer =
            CreateConsumer(
                queueName,
                exchangeName,
                consumerScope.ServiceProvider);

        await consumer.StartAsync();

        await PublishAsync(
            exchangeName,
            routingKey,
            integrationEvent,
            rabbitMqMessageId);

        await handler.WaitForInvocationAsync(
            TimeSpan.FromSeconds(10));

        await WaitUntilAsync(
            async () =>
            {
                using var assertScope =
                    _fixture.SqlServer.Factory.Services
                        .CreateScope();

                var dbContext =
                    assertScope.ServiceProvider
                        .GetRequiredService<
                            ApplicationDbContext>();

                return await dbContext.InboxMessages
                    .AsNoTracking()
                    .AnyAsync(
                        x =>
                            x.Id ==
                            rabbitMqMessageId &&
                            x.ProcessedOnUtc != null);
            },
            TimeSpan.FromSeconds(10));

        Assert.Equal(
            1,
            handler.InvocationCount);

        using var finalScope =
            _fixture.SqlServer.Factory.Services
                .CreateScope();

        var finalDbContext =
            finalScope.ServiceProvider
                .GetRequiredService<
                    ApplicationDbContext>();

        var inboxMessage =
            await finalDbContext.InboxMessages
                .AsNoTracking()
                .SingleAsync(
                    x =>
                        x.Id ==
                        rabbitMqMessageId);

        Assert.Equal(
            rabbitMqMessageId,
            inboxMessage.Id);

        Assert.Equal(
            integrationEvent.Type,
            inboxMessage.Type);

        Assert.NotNull(
            inboxMessage.ProcessedOnUtc);

        await AssertQueueIsEmptyAsync(
            queueName);
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
            string queueName,
            string exchangeName,
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
                        exchangeName
                });

        var consumerSettings =
            Options.Create(
                new RabbitMqConsumerSettings
                {
                    QueueName =
                        queueName,

                    RoutingKey =
                        "sales-order-confirmed",

                    RetryExchangeName =
                        $"{exchangeName}.retry",

                    RetryQueueName =
                        $"{queueName}.retry",

                    DeadLetterExchangeName =
                        $"{exchangeName}.dead-letter",

                    DeadLetterQueueName =
                        $"{queueName}.dlq",

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
        string routingKey,
        SalesOrderConfirmedIntegrationEvent
            integrationEvent,
        Guid? messageId = null)
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
                    (messageId ??
                        integrationEvent.Id)
                        .ToString(),

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
                Encoding.UTF8.GetBytes(
                    payload));
    }

    private async Task AssertQueueIsEmptyAsync(
        string queueName)
    {
        await WaitUntilAsync(
            async () =>
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
            "The expected condition was not reached.");
    }

    private sealed class CountingHandler
        : IIntegrationEventHandler<
            SalesOrderConfirmedIntegrationEvent>
    {
        private readonly TaskCompletionSource
            _invoked =
                new(
                    TaskCreationOptions
                        .RunContinuationsAsynchronously);

        public int InvocationCount { get; private set; }

        public Task HandleAsync(
            SalesOrderConfirmedIntegrationEvent
                integrationEvent,
            CancellationToken cancellationToken = default)
        {
            InvocationCount++;

            _invoked.TrySetResult();

            return Task.CompletedTask;
        }

        public Task WaitForInvocationAsync(
            TimeSpan timeout)
        {
            return _invoked.Task.WaitAsync(
                timeout);
        }
    }

    private sealed class FailingHandler
        : IIntegrationEventHandler<
            SalesOrderConfirmedIntegrationEvent>
    {
        private readonly TaskCompletionSource
            _invoked =
                new(
                    TaskCreationOptions
                        .RunContinuationsAsynchronously);

        public int InvocationCount { get; private set; }

        public Task HandleAsync(
            SalesOrderConfirmedIntegrationEvent
                integrationEvent,
            CancellationToken cancellationToken = default)
        {
            InvocationCount++;

            _invoked.TrySetResult();

            throw new InvalidOperationException(
                "Simulated Integration Event handler failure.");
        }

        public Task WaitForInvocationAsync(
            TimeSpan timeout)
        {
            return _invoked.Task.WaitAsync(
                timeout);
        }
    }
}