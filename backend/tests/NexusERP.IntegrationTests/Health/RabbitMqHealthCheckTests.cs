using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

using NexusERP.Infrastructure.Health;
using NexusERP.Infrastructure.Messaging.RabbitMq;

namespace NexusERP.IntegrationTests.Health;

public sealed class RabbitMqHealthCheckTests
{
    [Fact]
    public async Task CheckHealthAsync_WithUnavailableRabbitMq_ShouldReturnUnhealthy()
    {
        var settings =
            new RabbitMqSettings
            {
                ConnectionString =
                    "amqp://guest:guest@127.0.0.1:1/",

                ExchangeName =
                    "nexuserp.test.events"
            };

        var healthCheck =
            new RabbitMqHealthCheck(
                Options.Create(
                    settings));

        var context =
            new HealthCheckContext();

        var result =
            await healthCheck.CheckHealthAsync(
                context);

        Assert.Equal(
            HealthStatus.Unhealthy,
            result.Status);

        Assert.Equal(
            "RabbitMQ health check failed.",
            result.Description);

        Assert.NotNull(
            result.Exception);
    }
}