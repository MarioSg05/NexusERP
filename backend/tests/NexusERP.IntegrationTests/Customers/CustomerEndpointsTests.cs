using System.Net;
using System.Net.Http.Json;

using NexusERP.Application.Customers.GetCustomers;
using NexusERP.Application.Customers.RegisterCustomer;
using NexusERP.Domain.Customers.Enums;
using NexusERP.Domain.Identity.Enums;
using NexusERP.IntegrationTests.Infrastructure;

namespace NexusERP.IntegrationTests.Customers;

[Collection(IntegrationTestCollection.Name)]
public sealed class CustomerEndpointsTests
{
    private readonly SqlServerFixture _sqlServer;

    public CustomerEndpointsTests(
        SqlServerFixture sqlServer)
    {
        _sqlServer = sqlServer;
    }

    [Fact]
    public async Task RegisterCustomer_ThenGetById_ShouldReturnPersistedCustomer()
    {
        var factory =
            _sqlServer.Factory;

        using var client =
            await TestAuthentication
                .CreateAuthenticatedClientAsync(
                    factory,
                    UserRole.Manager);

        var uniqueValue =
            Guid.NewGuid()
                .ToString("N");

        var request =
            new RegisterCustomerRequest(
                Name:
                    $"Integration Customer {uniqueValue[..8]}",
                Email:
                    $"customer-{uniqueValue}@nexuserp.test",
                Phone:
                    "+50255550101",
                Type:
                    CustomerType.Corporate);

        using var createResponse =
            await client.PostAsJsonAsync(
                "/api/customers",
                request);

        Assert.Equal(
            HttpStatusCode.Created,
            createResponse.StatusCode);

        var createdCustomer =
            await createResponse.Content
                .ReadFromJsonAsync<RegisterCustomerResponse>();

        Assert.NotNull(createdCustomer);

        Assert.NotEqual(
            Guid.Empty,
            createdCustomer.Id);

        Assert.Equal(
            request.Email,
            createdCustomer.Email);

        using var getResponse =
            await client.GetAsync(
                $"/api/customers/{createdCustomer.Id}");

        Assert.Equal(
            HttpStatusCode.OK,
            getResponse.StatusCode);

        var persistedCustomer =
            await getResponse.Content
                .ReadFromJsonAsync<CustomerListItem>();

        Assert.NotNull(persistedCustomer);

        Assert.Equal(
            createdCustomer.Id,
            persistedCustomer.Id);

        Assert.Equal(
            request.Name,
            persistedCustomer.Name);

        Assert.Equal(
            request.Email,
            persistedCustomer.Email);

        Assert.Equal(
            request.Phone,
            persistedCustomer.Phone);

        Assert.Equal(
            request.Type.ToString(),
            persistedCustomer.Type);

        Assert.True(
            persistedCustomer.IsActive);
    }

    [Fact]
    public async Task GetCustomerById_WhenCustomerDoesNotExist_ShouldReturnNotFound()
    {
        var factory =
            _sqlServer.Factory;

        using var client =
            await TestAuthentication
                .CreateAuthenticatedClientAsync(
                    factory,
                    UserRole.Manager);

        var customerId =
            Guid.NewGuid();

        using var response =
            await client.GetAsync(
                $"/api/customers/{customerId}");

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }
}