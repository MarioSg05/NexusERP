using System.Net;

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
}