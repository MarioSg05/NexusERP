using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace NexusERP.IntegrationTests.Infrastructure;

public sealed class IntegrationTestFactory
    : WebApplicationFactory<Program>
{
    private readonly string
        _connectionString;

    private readonly string?
        _rabbitMqConnectionString;

    public IntegrationTestFactory(
        string connectionString,
        string? rabbitMqConnectionString = null)
    {
        _connectionString =
            connectionString;

        _rabbitMqConnectionString =
            rabbitMqConnectionString;
    }

    protected override void ConfigureWebHost(
        IWebHostBuilder builder)
    {
        builder.UseEnvironment(
            "IntegrationTesting");

        builder.ConfigureAppConfiguration(
            (_, configurationBuilder) =>
            {
                var configuration =
                    new Dictionary<string, string?>
                    {
                        ["ConnectionStrings:DefaultConnection"] =
                            _connectionString,

                        ["Jwt:Issuer"] =
                            "NexusERP.IntegrationTests",

                        ["Jwt:Audience"] =
                            "NexusERP.IntegrationTests.Client",

                        ["Jwt:Key"] =
                            "NexusERP.IntegrationTests.Jwt.Signing.Key.2026.MinimumLength",

                        ["Jwt:ExpirationMinutes"] =
                            "60"
                    };

                if (!string.IsNullOrWhiteSpace(
                        _rabbitMqConnectionString))
                {
                    configuration[
                        "RabbitMq:ConnectionString"] =
                            _rabbitMqConnectionString;
                }

                configurationBuilder
                    .AddInMemoryCollection(
                        configuration);
            });
    }
}