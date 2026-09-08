using System.Net;

using Microsoft.Extensions.Options;

using NexusERP.Infrastructure.Identity.Jwt;

namespace NexusERP.IntegrationTests.Infrastructure;

[Collection(IntegrationTestCollection.Name)]
public sealed class ApiStartupTests
{
    private readonly SqlServerFixture _sqlServer;

    public ApiStartupTests(
        SqlServerFixture sqlServer)
    {
        _sqlServer = sqlServer;
    }

    [Fact]
    public async Task ProtectedEndpoint_ShouldReturnUnauthorized()
    {
        var factory =
            _sqlServer.Factory;

        using var client =
            factory.CreateClient(
                new()
                {
                    AllowAutoRedirect = false
                });

        using var response =
            await client.GetAsync(
                "/api/dashboard");

        Assert.Equal(
            HttpStatusCode.Unauthorized,
            response.StatusCode);
    }

    [Theory]
    [InlineData(
        "Jwt:Key",
        "",
        "JWT signing key is required.")]
    [InlineData(
        "Jwt:Issuer",
        "",
        "JWT issuer is required.")]
    [InlineData(
        "Jwt:Audience",
        "",
        "JWT audience is required.")]
    [InlineData(
        "Jwt:ExpirationMinutes",
        "0",
        "JWT expiration must be greater than zero.")]
    public void InvalidJwtConfiguration_ShouldFailStartup(
        string configurationKey,
        string configurationValue,
        string expectedFailure)
    {
        using var factory =
            new IntegrationTestFactory(
                _sqlServer.ConnectionString,
                configurationOverrides:
                    new Dictionary<string, string?>
                    {
                        [configurationKey] =
                            configurationValue
                    });

        var exception =
            Assert.Throws<OptionsValidationException>(
                () =>
                    factory.CreateClient());

        Assert.Contains(
            expectedFailure,
            exception.Failures);
    }
}