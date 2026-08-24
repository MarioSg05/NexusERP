using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace NexusERP.IntegrationTests.Infrastructure;

public sealed class IntegrationTestFactory
    : WebApplicationFactory<Program>
{
    private readonly string _connectionString;

    public IntegrationTestFactory(
        string connectionString)
    {
        _connectionString = connectionString;
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

                configurationBuilder
                    .AddInMemoryCollection(
                        configuration);
            });
    }
}