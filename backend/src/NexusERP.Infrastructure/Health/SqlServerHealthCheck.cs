using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

using NexusERP.Infrastructure.Persistence;

namespace NexusERP.Infrastructure.Health;

public sealed class SqlServerHealthCheck
    : IHealthCheck
{
    private readonly ApplicationDbContext
        _dbContext;

    public SqlServerHealthCheck(
        ApplicationDbContext dbContext)
    {
        _dbContext =
            dbContext;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var canConnect =
                await _dbContext.Database
                    .CanConnectAsync(
                        cancellationToken);

            return canConnect
                ? HealthCheckResult.Healthy(
                    "SQL Server is reachable.")
                : HealthCheckResult.Unhealthy(
                    "SQL Server is unreachable.");
        }
        catch (Exception exception)
        {
            return HealthCheckResult.Unhealthy(
                "SQL Server health check failed.",
                exception);
        }
    }
}
