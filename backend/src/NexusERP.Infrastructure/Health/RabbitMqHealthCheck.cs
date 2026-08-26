using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

using NexusERP.Infrastructure.Messaging.RabbitMq;

using RabbitMQ.Client;

namespace NexusERP.Infrastructure.Health;

public sealed class RabbitMqHealthCheck
    : IHealthCheck
{
    private readonly RabbitMqSettings
        _settings;

    public RabbitMqHealthCheck(
        IOptions<RabbitMqSettings> options)
    {
        _settings =
            options.Value;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(
                _settings.ConnectionString))
        {
            return HealthCheckResult.Unhealthy(
                "RabbitMQ connection string is not configured.");
        }

        try
        {
            var connectionFactory =
                new ConnectionFactory
                {
                    Uri =
                        new Uri(
                            _settings.ConnectionString)
                };

            await using var connection =
                await connectionFactory
                    .CreateConnectionAsync(
                        cancellationToken);

            return connection.IsOpen
                ? HealthCheckResult.Healthy(
                    "RabbitMQ is reachable.")
                : HealthCheckResult.Unhealthy(
                    "RabbitMQ connection is not open.");
        }
        catch (Exception exception)
        {
            return HealthCheckResult.Unhealthy(
                "RabbitMQ health check failed.",
                exception);
        }
    }
}