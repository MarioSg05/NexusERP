using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using NexusERP.Infrastructure.Persistence;

using Testcontainers.MsSql;

namespace NexusERP.IntegrationTests.Infrastructure;

public sealed class SqlServerFixture
    : IAsyncLifetime
{
    private const string SqlServerImage =
        "mcr.microsoft.com/mssql/server:2022-CU14-ubuntu-22.04";

    private readonly MsSqlContainer _container =
        new MsSqlBuilder(SqlServerImage)
            .Build();

    public string ConnectionString =>
        _container.GetConnectionString();

    public IntegrationTestFactory Factory
    {
        get;
        private set;
    } = null!;

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        Factory =
            new IntegrationTestFactory(
                ConnectionString);

        using var scope =
            Factory.Services.CreateScope();

        var dbContext =
            scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

        await dbContext.Database
            .MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        await Factory.DisposeAsync();

        await _container.DisposeAsync();
    }
}