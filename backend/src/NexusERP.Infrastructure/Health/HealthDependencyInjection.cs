using Microsoft.Extensions.DependencyInjection;

namespace NexusERP.Infrastructure.Health;

public static class HealthDependencyInjection
{
    public static IServiceCollection AddInfrastructureHealthChecks(
        this IServiceCollection services)
    {
        services
            .AddHealthChecks()
            .AddCheck<SqlServerHealthCheck>(
                "sql-server",
                tags:
                    ["ready"])
            .AddCheck<RabbitMqHealthCheck>(
                "rabbitmq",
                tags:
                    ["ready"]);

        return services;
    }
}