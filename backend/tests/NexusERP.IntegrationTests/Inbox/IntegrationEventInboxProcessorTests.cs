using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using NexusERP.Application.Common.IntegrationEvents;
using NexusERP.Application.Sales.IntegrationEvents;
using NexusERP.Infrastructure.Messaging.Inbox;
using NexusERP.Infrastructure.Persistence;
using NexusERP.IntegrationTests.Infrastructure;

namespace NexusERP.IntegrationTests.Inbox;

[Collection(IntegrationTestCollection.Name)]
public sealed class IntegrationEventInboxProcessorTests
{
    private readonly SqlServerFixture _sqlServer;

    public IntegrationEventInboxProcessorTests(
        SqlServerFixture sqlServer)
    {
        _sqlServer =
            sqlServer;
    }

    [Fact]
    public async Task ProcessAsync_WithNewEvent_ShouldInvokeHandlerAndPersistInboxMessage()
    {
        var integrationEvent =
            CreateIntegrationEvent();

        var handler =
            new CountingHandler();

        var services =
            new ServiceCollection();

        services.AddSingleton<
            IIntegrationEventHandler<
                SalesOrderConfirmedIntegrationEvent>>(
                    handler);

        await using var serviceProvider =
            services.BuildServiceProvider();

        using var scope =
            _sqlServer.Factory.Services
                .CreateScope();

        var dbContext =
            scope.ServiceProvider
                .GetRequiredService<
                    ApplicationDbContext>();

        var processor =
            new IntegrationEventInboxProcessor(
                dbContext,
                serviceProvider);

        var processed =
            await processor.ProcessAsync(
                integrationEvent);

        Assert.True(
            processed);

        Assert.Equal(
            1,
            handler.InvocationCount);

        var inboxMessage =
            await dbContext.InboxMessages
                .AsNoTracking()
                .SingleAsync(
                    x =>
                        x.Id ==
                        integrationEvent.Id);

        Assert.Equal(
            integrationEvent.Id,
            inboxMessage.Id);

        Assert.Equal(
            integrationEvent.Type,
            inboxMessage.Type);

        Assert.NotNull(
            inboxMessage.ProcessedOnUtc);
    }

    [Fact]
    public async Task ProcessAsync_WithDuplicateEvent_ShouldNotInvokeHandlerAgain()
    {
        var integrationEvent =
            CreateIntegrationEvent();

        var handler =
            new CountingHandler();

        var services =
            new ServiceCollection();

        services.AddSingleton<
            IIntegrationEventHandler<
                SalesOrderConfirmedIntegrationEvent>>(
                    handler);

        await using var serviceProvider =
            services.BuildServiceProvider();

        using var scope =
            _sqlServer.Factory.Services
                .CreateScope();

        var dbContext =
            scope.ServiceProvider
                .GetRequiredService<
                    ApplicationDbContext>();

        var processor =
            new IntegrationEventInboxProcessor(
                dbContext,
                serviceProvider);

        var firstResult =
            await processor.ProcessAsync(
                integrationEvent);

        var secondResult =
            await processor.ProcessAsync(
                integrationEvent);

        Assert.True(
            firstResult);

        Assert.False(
            secondResult);

        Assert.Equal(
            1,
            handler.InvocationCount);

        var inboxCount =
            await dbContext.InboxMessages
                .AsNoTracking()
                .CountAsync(
                    x =>
                        x.Id ==
                        integrationEvent.Id);

        Assert.Equal(
            1,
            inboxCount);
    }

    [Fact]
    public async Task ProcessAsync_WhenHandlerFails_ShouldRollbackInboxMessage()
    {
        var integrationEvent =
            CreateIntegrationEvent();

        var handler =
            new FailingHandler();

        var services =
            new ServiceCollection();

        services.AddSingleton<
            IIntegrationEventHandler<
                SalesOrderConfirmedIntegrationEvent>>(
                    handler);

        await using var serviceProvider =
            services.BuildServiceProvider();

        using var scope =
            _sqlServer.Factory.Services
                .CreateScope();

        var dbContext =
            scope.ServiceProvider
                .GetRequiredService<
                    ApplicationDbContext>();

        var processor =
            new IntegrationEventInboxProcessor(
                dbContext,
                serviceProvider);

        await Assert.ThrowsAsync<
            InvalidOperationException>(
                () =>
                    processor.ProcessAsync(
                        integrationEvent));

        var inboxExists =
            await dbContext.InboxMessages
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

    private sealed class CountingHandler
        : IIntegrationEventHandler<
            SalesOrderConfirmedIntegrationEvent>
    {
        public int InvocationCount { get; private set; }

        public Task HandleAsync(
            SalesOrderConfirmedIntegrationEvent integrationEvent,
            CancellationToken cancellationToken = default)
        {
            InvocationCount++;

            return Task.CompletedTask;
        }
    }

    private sealed class FailingHandler
        : IIntegrationEventHandler<
            SalesOrderConfirmedIntegrationEvent>
    {
        public int InvocationCount { get; private set; }

        public Task HandleAsync(
            SalesOrderConfirmedIntegrationEvent integrationEvent,
            CancellationToken cancellationToken = default)
        {
            InvocationCount++;

            throw new InvalidOperationException(
                "Simulated Integration Event handler failure.");
        }
    }
}