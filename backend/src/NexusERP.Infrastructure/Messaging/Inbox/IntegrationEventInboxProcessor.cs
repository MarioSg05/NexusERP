using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using NexusERP.Application.Common.IntegrationEvents;

using NexusERP.Infrastructure.Persistence;

namespace NexusERP.Infrastructure.Messaging.Inbox;

public sealed class IntegrationEventInboxProcessor
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IServiceProvider _serviceProvider;

    public IntegrationEventInboxProcessor(
        ApplicationDbContext dbContext,
        IServiceProvider serviceProvider)
    {
        _dbContext =
            dbContext;

        _serviceProvider =
            serviceProvider;
    }

    public async Task<bool> ProcessAsync<
        TIntegrationEvent>(
        TIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
        where TIntegrationEvent : IIntegrationEvent
    {
        ArgumentNullException.ThrowIfNull(
            integrationEvent);

        var alreadyProcessed =
            await _dbContext.InboxMessages
                .AnyAsync(
                    x =>
                        x.Id ==
                        integrationEvent.Id,
                    cancellationToken);

        if (alreadyProcessed)
        {
            return false;
        }

        await using var transaction =
            await _dbContext.Database
                .BeginTransactionAsync(
                    cancellationToken);

        try
        {
            // Check again inside the transaction.
            alreadyProcessed =
                await _dbContext.InboxMessages
                    .AnyAsync(
                        x =>
                            x.Id ==
                            integrationEvent.Id,
                        cancellationToken);

            if (alreadyProcessed)
            {
                await transaction.CommitAsync(
                    cancellationToken);

                return false;
            }

            var handler =
                _serviceProvider
                    .GetRequiredService<
                        IIntegrationEventHandler<
                            TIntegrationEvent>>();

            await handler.HandleAsync(
                integrationEvent,
                cancellationToken);

            var inboxMessage =
                InboxMessage.Create(
                    integrationEvent.Id,
                    DateTime.UtcNow,
                    integrationEvent.Type);

            inboxMessage.MarkAsProcessed(
                DateTime.UtcNow);

            _dbContext.InboxMessages.Add(
                inboxMessage);

            await _dbContext.SaveChangesAsync(
                cancellationToken);

            await transaction.CommitAsync(
                cancellationToken);

            return true;
        }
        catch
        {
            await transaction.RollbackAsync(
                cancellationToken);

            throw;
        }
    }
}