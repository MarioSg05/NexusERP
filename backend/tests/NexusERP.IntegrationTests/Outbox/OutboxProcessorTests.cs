using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;

using NexusERP.Application.Common.IntegrationEvents;
using NexusERP.Infrastructure.Messaging.Outbox;
using NexusERP.Infrastructure.Persistence;
using NexusERP.IntegrationTests.Infrastructure;

namespace NexusERP.IntegrationTests.Outbox;

[Collection(IntegrationTestCollection.Name)]
public sealed class OutboxProcessorTests
{
    private readonly SqlServerFixture
        _sqlServer;

    public OutboxProcessorTests(
        SqlServerFixture sqlServer)
    {
        _sqlServer =
            sqlServer;
    }

    private async Task ClearOutboxMessagesAsync()
    {
        using var scope =
            _sqlServer.Factory.Services
                .CreateScope();

        var dbContext =
            scope.ServiceProvider
                .GetRequiredService<
                    ApplicationDbContext>();

        await dbContext.OutboxMessages
            .ExecuteDeleteAsync();
    }

    [Fact]
    public async Task ProcessAsync_WithPendingMessage_ShouldPublishAndMarkAsProcessed()
    {
        await ClearOutboxMessagesAsync();

        var message =
            await CreateOutboxMessageAsync();

        var publisher =
            new CapturingIntegrationEventPublisher();

        await ProcessAsync(
            publisher,
            batchSize: 100);

        var persistedMessage =
            await GetOutboxMessageAsync(
                message.Id);

        Assert.NotNull(
            persistedMessage.ProcessedOnUtc);

        Assert.Null(
            persistedMessage.Error);

        var publishedMessage =
            Assert.Single(
                publisher.PublishedMessages);

        Assert.Equal(
            message.Id,
            publishedMessage.MessageId);

        Assert.Equal(
            message.Type,
            publishedMessage.Type);

        Assert.Equal(
            message.Payload,
            publishedMessage.Payload);
    }

    [Fact]
    public async Task ProcessAsync_WhenPublisherFails_ShouldKeepMessagePendingAndStoreError()
    {
        await ClearOutboxMessagesAsync();

        var message =
            await CreateOutboxMessageAsync();

        var publisher =
            new ThrowingIntegrationEventPublisher(
                "Publisher unavailable.");

        await ProcessAsync(
            publisher,
            batchSize: 100);

        var persistedMessage =
            await GetOutboxMessageAsync(
                message.Id);

        Assert.Null(
            persistedMessage.ProcessedOnUtc);

        Assert.Equal(
            "Publisher unavailable.",
            persistedMessage.Error);
    }

    [Fact]
    public async Task ProcessAsync_WithProcessedMessage_ShouldIgnoreMessage()
    {
        await ClearOutboxMessagesAsync();

        var message =
            await CreateOutboxMessageAsync(
                markAsProcessed: true);

        var publisher =
            new CapturingIntegrationEventPublisher();

        var processedCount =
            await ProcessAsync(
                publisher,
                batchSize: 100);

        Assert.Equal(
            0,
            processedCount);

        Assert.Empty(
            publisher.PublishedMessages);

        var persistedMessage =
            await GetOutboxMessageAsync(
                message.Id);

        Assert.NotNull(
            persistedMessage.ProcessedOnUtc);
    }

    [Fact]
    public async Task ProcessAsync_ShouldRespectBatchSize()
    {
        await ClearOutboxMessagesAsync();

        var firstMessage =
            await CreateOutboxMessageAsync();

        var secondMessage =
            await CreateOutboxMessageAsync();

        var thirdMessage =
            await CreateOutboxMessageAsync();

        var publisher =
            new CapturingIntegrationEventPublisher();

        var processedCount =
            await ProcessAsync(
                publisher,
                batchSize: 2);

        Assert.Equal(
            2,
            processedCount);

        Assert.Equal(
            2,
            publisher.PublishedMessages.Count);

        var messageIds =
            new[]
            {
                firstMessage.Id,
                secondMessage.Id,
                thirdMessage.Id
            };

        var persistedMessages =
            await GetOutboxMessagesAsync(
                messageIds);

        Assert.Equal(
            2,
            persistedMessages.Count(
                x => x.ProcessedOnUtc != null));

        Assert.Single(
            persistedMessages,
            x => x.ProcessedOnUtc == null);
    }

    [Fact]
    public async Task ProcessAsync_WhenFailedMessageSucceedsLater_ShouldMarkAsProcessedAndClearError()
    {
        await ClearOutboxMessagesAsync();

        var message =
            await CreateOutboxMessageAsync();

        var failingPublisher =
            new ThrowingIntegrationEventPublisher(
                "Temporary failure.");

        await ProcessAsync(
            failingPublisher,
            batchSize: 100);

        var failedMessage =
            await GetOutboxMessageAsync(
                message.Id);

        Assert.Null(
            failedMessage.ProcessedOnUtc);

        Assert.Equal(
            "Temporary failure.",
            failedMessage.Error);

        var successfulPublisher =
            new CapturingIntegrationEventPublisher();

        await ProcessAsync(
            successfulPublisher,
            batchSize: 100);

        var processedMessage =
            await GetOutboxMessageAsync(
                message.Id);

        Assert.NotNull(
            processedMessage.ProcessedOnUtc);

        Assert.Null(
            processedMessage.Error);

        var publishedMessage =
            Assert.Single(
                successfulPublisher.PublishedMessages);

        Assert.Equal(
            message.Id,
            publishedMessage.MessageId);
    }

    [Fact]
    public async Task ProcessAsync_WithInvalidBatchSize_ShouldThrow()
    {
        await ClearOutboxMessagesAsync();

        var publisher =
            new CapturingIntegrationEventPublisher();

        await Assert.ThrowsAsync<
            ArgumentOutOfRangeException>(
                () =>
                    ProcessAsync(
                        publisher,
                        batchSize: 0));
    }

    private async Task<OutboxMessage>
        CreateOutboxMessageAsync(
            bool markAsProcessed = false)
    {
        var uniqueValue =
            Guid.NewGuid();

        var message =
            OutboxMessage.Create(
                uniqueValue,
                DateTime.UtcNow,
                "test-integration-event",
                $$"""
                {"id":"{{uniqueValue}}"}
                """);

        if (markAsProcessed)
        {
            message.MarkAsProcessed(
                DateTime.UtcNow);
        }

        using var scope =
            _sqlServer.Factory.Services
                .CreateScope();

        var dbContext =
            scope.ServiceProvider
                .GetRequiredService<
                    ApplicationDbContext>();

        dbContext.OutboxMessages.Add(
            message);

        await dbContext.SaveChangesAsync();

        return message;
    }

    private async Task<int> ProcessAsync(
        IIntegrationEventPublisher publisher,
        int batchSize)
    {
        using var scope =
            _sqlServer.Factory.Services
                .CreateScope();

        var dbContext =
            scope.ServiceProvider
                .GetRequiredService<
                    ApplicationDbContext>();

        var processor =
            new OutboxProcessor(
                dbContext,
                publisher,
                NullLogger<
                    OutboxProcessor>.Instance);

        return await processor.ProcessAsync(
            batchSize);
    }

    private async Task<OutboxMessage>
        GetOutboxMessageAsync(
            Guid id)
    {
        using var scope =
            _sqlServer.Factory.Services
                .CreateScope();

        var dbContext =
            scope.ServiceProvider
                .GetRequiredService<
                    ApplicationDbContext>();

        return await dbContext.OutboxMessages
            .AsNoTracking()
            .SingleAsync(
                x => x.Id == id);
    }

    private async Task<IReadOnlyList<OutboxMessage>>
        GetOutboxMessagesAsync(
            IReadOnlyCollection<Guid> ids)
    {
        using var scope =
            _sqlServer.Factory.Services
                .CreateScope();

        var dbContext =
            scope.ServiceProvider
                .GetRequiredService<
                    ApplicationDbContext>();

        return await dbContext.OutboxMessages
            .AsNoTracking()
            .Where(
                x => ids.Contains(x.Id))
            .ToListAsync();
    }

    private sealed class
        CapturingIntegrationEventPublisher
        : IIntegrationEventPublisher
    {
        private readonly List<PublishedMessage>
            _publishedMessages = [];

        public IReadOnlyCollection<PublishedMessage>
            PublishedMessages =>
                _publishedMessages.AsReadOnly();

        public Task PublishAsync(
            Guid messageId,
            string type,
            string payload,
            CancellationToken cancellationToken = default)
        {
            _publishedMessages.Add(
                new PublishedMessage(
                    messageId,
                    type,
                    payload));

            return Task.CompletedTask;
        }
    }

    private sealed class
        ThrowingIntegrationEventPublisher
        : IIntegrationEventPublisher
    {
        private readonly string
            _message;

        public ThrowingIntegrationEventPublisher(
            string message)
        {
            _message =
                message;
        }

        public Task PublishAsync(
            Guid messageId,
            string type,
            string payload,
            CancellationToken cancellationToken = default)
        {
            throw new InvalidOperationException(
                _message);
        }
    }

    private sealed record PublishedMessage(
        Guid MessageId,
        string Type,
        string Payload);
}