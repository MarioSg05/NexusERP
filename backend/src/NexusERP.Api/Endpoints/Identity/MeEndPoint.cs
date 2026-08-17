using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using NexusERP.Application.Identity.GetCurrentUser;

namespace NexusERP.Api.Endpoints.Identity;

public static class MeEndpoint
{
    public static IEndpointRouteBuilder MapMeEndpoint(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/api/auth/me",
            async (
                ClaimsPrincipal user,
                GetCurrentUserHandler handler,
                CancellationToken cancellationToken) =>
            {
                var userIdValue =
                    user.FindFirstValue(
                        ClaimTypes.NameIdentifier)
                    ?? user.FindFirstValue(
                        JwtRegisteredClaimNames.Sub);

                if (!Guid.TryParse(
                    userIdValue,
                    out var userId))
                {
                    return Results.Unauthorized();
                }

                var response =
                    await handler.Handle(
                        userId,
                        cancellationToken);

                return Results.Ok(response);
            })
        .RequireAuthorization()
        .WithName("Me")
        .WithSummary(
            "Returns the authenticated user.")
        .WithDescription(
            "Returns the current active user represented by the authenticated JWT.")
        .Produces<GetCurrentUserResponse>(
            StatusCodes.Status200OK)
        .Produces(
            StatusCodes.Status401Unauthorized)
        .Produces(
            StatusCodes.Status404NotFound);

        return app;
    }
}