using NexusERP.Infrastructure.Messaging.RabbitMq;

namespace NexusERP.Worker;

public sealed class IntegrationEventConsumerWorker
    : BackgroundService
{
    private readonly RabbitMqIntegrationEventConsumer
        _consumer;

    private readonly ILogger<
        IntegrationEventConsumerWorker>
        _logger;

    public IntegrationEventConsumerWorker(
        RabbitMqIntegrationEventConsumer consumer,
        ILogger<IntegrationEventConsumerWorker> logger)
    {
        _consumer =
            consumer;

        _logger =
            logger;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        try
        {
            await _consumer.StartAsync(
                stoppingToken);

            _logger.LogInformation(
                "RabbitMQ Integration Event consumer started.");

            await Task.Delay(
                Timeout.InfiniteTimeSpan,
                stoppingToken);
        }
        catch (OperationCanceledException)
            when (stoppingToken.IsCancellationRequested)
        {
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "RabbitMQ Integration Event consumer stopped unexpectedly.");

            throw;
        }
    }
}