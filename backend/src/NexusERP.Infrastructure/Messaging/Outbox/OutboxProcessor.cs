using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using NexusERP.Application.Common.IntegrationEvents;
using NexusERP.Infrastructure.Persistence;

namespace NexusERP.Infrastructure.Messaging.Outbox;

public sealed class OutboxProcessor
{
    private readonly ApplicationDbContext
        _dbContext;

    private readonly IIntegrationEventPublisher
        _publisher;

    private readonly ILogger<OutboxProcessor>
        _logger;

    public OutboxProcessor(
        ApplicationDbContext dbContext,
        IIntegrationEventPublisher publisher,
        ILogger<OutboxProcessor> logger)
    {
        _dbContext =
            dbContext;

        _publisher =
            publisher;

        _logger =
            logger;
    }

    public async Task<int> ProcessAsync(
        int batchSize,
        CancellationToken cancellationToken = default)
    {
        if (batchSize <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(batchSize),
                batchSize,
                "Batch size must be greater than zero.");
        }

        var messages =
            await _dbContext.OutboxMessages
                .Where(
                    message =>
                        message.ProcessedOnUtc == null)
                .OrderBy(
                    message =>
                        message.OccurredOnUtc)
                .ThenBy(
                    message =>
                        message.Id)
                .Take(
                    batchSize)
                .ToListAsync(
                    cancellationToken);

        var processedCount =
            0;

        foreach (var message in messages)
        {
            cancellationToken
                .ThrowIfCancellationRequested();

            try
            {
                await _publisher.PublishAsync(
                    message.Id,
                    message.Type,
                    message.Payload,
                    cancellationToken);

                message.MarkAsProcessed(
                    DateTime.UtcNow);

                _logger.LogInformation(
                    "Published Outbox message {MessageId} of type {MessageType}.",
                    message.Id,
                    message.Type);

                processedCount++;
            }
            catch (OperationCanceledException)
                when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {
                message.SetError(
                    exception.Message);

                _logger.LogError(
                    exception,
                    "Failed to publish Outbox message {MessageId} of type {MessageType}.",
                    message.Id,
                    message.Type);
            }

            await _dbContext.SaveChangesAsync(
                cancellationToken);
        }

        return processedCount;
    }
}