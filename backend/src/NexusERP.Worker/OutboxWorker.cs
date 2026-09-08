using Microsoft.Extensions.Options;

using NexusERP.Infrastructure.Messaging.Outbox;

namespace NexusERP.Worker;

public sealed class OutboxWorker
    : BackgroundService
{
    private readonly IServiceScopeFactory
        _scopeFactory;

    private readonly OutboxWorkerSettings
        _settings;

    private readonly ILogger<OutboxWorker>
        _logger;

    public OutboxWorker(
        IServiceScopeFactory scopeFactory,
        IOptions<OutboxWorkerSettings> options,
        ILogger<OutboxWorker> logger)
    {
        _scopeFactory =
            scopeFactory;

        _settings =
            options.Value;

        _logger =
            logger;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        if (_settings.BatchSize <= 0)
        {
            throw new InvalidOperationException(
                "Outbox batch size must be greater than zero.");
        }

        if (_settings.PollingIntervalSeconds <= 0)
        {
            throw new InvalidOperationException(
                "Outbox polling interval must be greater than zero.");
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope =
                    _scopeFactory.CreateScope();

                var processor =
                    scope.ServiceProvider
                        .GetRequiredService<
                            OutboxProcessor>();

                var processedCount =
                    await processor.ProcessAsync(
                        _settings.BatchSize,
                        stoppingToken);

                if (processedCount > 0)
                {
                    _logger.LogInformation(
                        "Processed {MessageCount} Outbox messages.",
                        processedCount);
                }
            }
            catch (OperationCanceledException)
                when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    exception,
                    "An error occurred while processing Outbox messages.");
            }

            try
            {
                await Task.Delay(
                    TimeSpan.FromSeconds(
                        _settings.PollingIntervalSeconds),
                    stoppingToken);
            }
            catch (OperationCanceledException)
                when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
        }
    }
}