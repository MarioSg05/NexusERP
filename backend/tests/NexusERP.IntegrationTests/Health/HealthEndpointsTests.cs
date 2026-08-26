using System.Net;

using NexusERP.IntegrationTests.Infrastructure;

namespace NexusERP.IntegrationTests.Health;

[Collection(MessagingIntegrationTestCollection.Name)]
public sealed class HealthEndpointsTests
{
    private readonly MessagingIntegrationFixture
        _fixture;

    public HealthEndpointsTests(
        MessagingIntegrationFixture fixture)
    {
        _fixture =
            fixture;
    }

    [Fact]
    public async Task Live_ShouldReturnOkWithoutAuthentication()
    {
        using var client =
            _fixture.Factory.CreateClient();

        using var response =
            await client.GetAsync(
                "/health/live");

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var content =
            await response.Content
                .ReadAsStringAsync();

        Assert.Equal(
            "Healthy",
            content);
    }

    [Fact]
    public async Task Ready_WithHealthyDependencies_ShouldReturnOk()
    {
        using var client =
            _fixture.Factory.CreateClient();

        using var response =
            await client.GetAsync(
                "/health/ready");

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var content =
            await response.Content
                .ReadAsStringAsync();

        Assert.Equal(
            "Healthy",
            content);
    }
}