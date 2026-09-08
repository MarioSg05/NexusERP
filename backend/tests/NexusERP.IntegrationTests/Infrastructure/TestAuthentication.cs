using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

using NexusERP.Application.Identity.LoginUser;
using NexusERP.Domain.Identity.Enums;

namespace NexusERP.IntegrationTests.Infrastructure;

public static class TestAuthentication
{
    public static async Task<HttpClient>
        CreateAuthenticatedClientAsync(
            IntegrationTestFactory factory,
            UserRole role)
    {
        var user =
            await TestUserSeeder.CreateAsync(
                factory,
                role);

        var client =
            factory.CreateClient();

        using var response =
            await client.PostAsJsonAsync(
                "/api/auth/login",
                new LoginUserRequest(
                    user.Email,
                    user.Password));

        if (response.StatusCode !=
            HttpStatusCode.OK)
        {
            client.Dispose();

            throw new InvalidOperationException(
                $"Test user login failed with status code {(int)response.StatusCode}.");
        }

        var login =
            await response.Content
                .ReadFromJsonAsync<LoginUserResponse>();

        if (login is null ||
            string.IsNullOrWhiteSpace(
                login.AccessToken))
        {
            client.Dispose();

            throw new InvalidOperationException(
                "Test user login did not return an access token.");
        }

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                login.AccessToken);

        return client;
    }
}