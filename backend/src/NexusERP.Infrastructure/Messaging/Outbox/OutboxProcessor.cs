using Microsoft.EntityFrameworkCore;

using NexusERP.Application.Common.IntegrationEvents;
using NexusERP.Infrastructure.Persistence;

namespace NexusERP.Infrastructure.Messaging.Outbox;

public sealed class OutboxProcessor
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IIntegrationEventPublisher _publisher;

    public OutboxProcessor(
        ApplicationDbContext dbContext,
        IIntegrationEventPublisher publisher)
    {
        _dbContext = dbContext;
        _publisher = publisher;
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
                .Take(batchSize)
                .ToListAsync(
                    cancellationToken);

        var processedCount = 0;

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
            }

            await _dbContext.SaveChangesAsync(
                cancellationToken);
        }

        return processedCount;
    }
}