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

    private readonly IReadOnlyDictionary<
        string,
        string?>?
        _configurationOverrides;

    public IntegrationTestFactory(
        string connectionString,
        string? rabbitMqConnectionString = null,
        IReadOnlyDictionary<string, string?>?
            configurationOverrides = null)
    {
        _connectionString =
            connectionString;

        _rabbitMqConnectionString =
            rabbitMqConnectionString;

        _configurationOverrides =
            configurationOverrides;
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

                if (_configurationOverrides is not null)
                {
                    foreach (var configurationOverride
                        in _configurationOverrides)
                    {
                        configuration[
                            configurationOverride.Key] =
                                configurationOverride.Value;
                    }
                }

                configurationBuilder
                    .AddInMemoryCollection(
                        configuration);
            });
    }
}