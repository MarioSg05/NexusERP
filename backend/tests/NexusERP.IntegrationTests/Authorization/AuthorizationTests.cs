using System.Net;
using System.Net.Http.Json;

using NexusERP.Application.Customers.RegisterCustomer;
using NexusERP.Domain.Customers.Enums;
using NexusERP.Domain.Identity.Enums;
using NexusERP.IntegrationTests.Infrastructure;

namespace NexusERP.IntegrationTests.Authorization;

[Collection(IntegrationTestCollection.Name)]
public sealed class AuthorizationTests
{
    private readonly SqlServerFixture _sqlServer;

    public AuthorizationTests(
        SqlServerFixture sqlServer)
    {
        _sqlServer = sqlServer;
    }

    [Fact]
    public async Task GetUsers_WithoutAuthentication_ShouldReturnUnauthorized()
    {
        var factory =
            _sqlServer.Factory;

        using var client =
            factory.CreateClient();

        using var response =
            await client.GetAsync(
                "/api/users");

        Assert.Equal(
            HttpStatusCode.Unauthorized,
            response.StatusCode);
    }

    [Fact]
    public async Task GetUsers_AsViewer_ShouldReturnForbidden()
    {
        var factory =
            _sqlServer.Factory;

        using var client =
            await TestAuthentication
                .CreateAuthenticatedClientAsync(
                    factory,
                    UserRole.Viewer);

        using var response =
            await client.GetAsync(
                "/api/users");

        Assert.Equal(
            HttpStatusCode.Forbidden,
            response.StatusCode);
    }

    [Fact]
    public async Task GetUsers_AsManager_ShouldReturnForbidden()
    {
        var factory =
            _sqlServer.Factory;

        using var client =
            await TestAuthentication
                .CreateAuthenticatedClientAsync(
                    factory,
                    UserRole.Manager);

        using var response =
            await client.GetAsync(
                "/api/users");

        Assert.Equal(
            HttpStatusCode.Forbidden,
            response.StatusCode);
    }

    [Fact]
    public async Task GetUsers_AsAdministrator_ShouldReturnOk()
    {
        var factory =
            _sqlServer.Factory;

        using var client =
            await TestAuthentication
                .CreateAuthenticatedClientAsync(
                    factory,
                    UserRole.Administrator);

        using var response =
            await client.GetAsync(
                "/api/users");

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);
    }

    [Fact]
    public async Task RegisterCustomer_AsViewer_ShouldReturnForbidden()
    {
        var factory =
            _sqlServer.Factory;

        using var client =
            await TestAuthentication
                .CreateAuthenticatedClientAsync(
                    factory,
                    UserRole.Viewer);

        var request =
            CreateCustomerRequest();

        using var response =
            await client.PostAsJsonAsync(
                "/api/customers",
                request);

        Assert.Equal(
            HttpStatusCode.Forbidden,
            response.StatusCode);
    }

    [Fact]
    public async Task RegisterCustomer_AsManager_ShouldReturnCreated()
    {
        var factory =
            _sqlServer.Factory;

        using var client =
            await TestAuthentication
                .CreateAuthenticatedClientAsync(
                    factory,
                    UserRole.Manager);

        var request =
            CreateCustomerRequest();

        using var response =
            await client.PostAsJsonAsync(
                "/api/customers",
                request);

        Assert.Equal(
            HttpStatusCode.Created,
            response.StatusCode);
    }

    [Fact]
    public async Task RegisterCustomer_AsAdministrator_ShouldReturnCreated()
    {
        var factory =
            _sqlServer.Factory;

        using var client =
            await TestAuthentication
                .CreateAuthenticatedClientAsync(
                    factory,
                    UserRole.Administrator);

        var request =
            CreateCustomerRequest();

        using var response =
            await client.PostAsJsonAsync(
                "/api/customers",
                request);

        Assert.Equal(
            HttpStatusCode.Created,
            response.StatusCode);
    }

    private static RegisterCustomerRequest
        CreateCustomerRequest()
    {
        var uniqueValue =
            Guid.NewGuid()
                .ToString("N");

        return new RegisterCustomerRequest(
            Name:
                $"Integration Customer {uniqueValue[..8]}",
            Email:
                $"customer-{uniqueValue}@nexuserp.test",
            Phone:
                "+50255550101",
            Type:
                CustomerType.Individual);
    }
}