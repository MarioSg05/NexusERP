using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

using NexusERP.Application.Identity.LoginUser;
using NexusERP.IntegrationTests.Infrastructure;

namespace NexusERP.IntegrationTests.Authentication;

[Collection(IntegrationTestCollection.Name)]
public sealed class AuthenticationTests
{
    private readonly SqlServerFixture _sqlServer;

    public AuthenticationTests(
        SqlServerFixture sqlServer)
    {
        _sqlServer = sqlServer;
    }

    [Fact]
    public async Task Login_WithValidCredentials_ShouldReturnAccessToken()
    {
        var factory =
            _sqlServer.Factory;

        var user =
            await TestUserSeeder.CreateAsync(
                factory);

        using var client =
            factory.CreateClient();

        using var response =
            await client.PostAsJsonAsync(
                "/api/auth/login",
                new LoginUserRequest(
                    user.Email,
                    user.Password));

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var result =
            await response.Content
                .ReadFromJsonAsync<LoginUserResponse>();

        Assert.NotNull(result);

        Assert.Equal(
            user.Id,
            result.UserId);

        Assert.Equal(
            user.Email,
            result.Email);

        Assert.False(
            string.IsNullOrWhiteSpace(
                result.AccessToken));

        Assert.True(
            result.ExpiresAt >
            DateTime.UtcNow);
    }

    [Fact]
    public async Task Login_WithInvalidPassword_ShouldReturnUnauthorized()
    {
        var factory =
            _sqlServer.Factory;

        var user =
            await TestUserSeeder.CreateAsync(
                factory);

        using var client =
            factory.CreateClient();

        using var response =
            await client.PostAsJsonAsync(
                "/api/auth/login",
                new LoginUserRequest(
                    user.Email,
                    "WrongPassword123!"));

        Assert.Equal(
            HttpStatusCode.Unauthorized,
            response.StatusCode);
    }

    [Fact]
    public async Task Login_WithInactiveUser_ShouldReturnUnauthorized()
    {
        var factory =
            _sqlServer.Factory;

        var user =
            await TestUserSeeder.CreateAsync(
                factory,
                isActive: false);

        using var client =
            factory.CreateClient();

        using var response =
            await client.PostAsJsonAsync(
                "/api/auth/login",
                new LoginUserRequest(
                    user.Email,
                    user.Password));

        Assert.Equal(
            HttpStatusCode.Unauthorized,
            response.StatusCode);
    }

    [Fact]
    public async Task AccessToken_ShouldAuthenticateProtectedRequest()
    {
        var factory =
            _sqlServer.Factory;

        var user =
            await TestUserSeeder.CreateAsync(
                factory);

        using var client =
            factory.CreateClient();

        using var loginResponse =
            await client.PostAsJsonAsync(
                "/api/auth/login",
                new LoginUserRequest(
                    user.Email,
                    user.Password));

        Assert.Equal(
            HttpStatusCode.OK,
            loginResponse.StatusCode);

        var login =
            await loginResponse.Content
                .ReadFromJsonAsync<LoginUserResponse>();

        Assert.NotNull(login);

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                login.AccessToken);

        using var response =
            await client.GetAsync(
                "/api/dashboard");

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);
    }
}