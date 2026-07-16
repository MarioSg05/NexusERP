using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace NexusERP.Api.Endpoints.Identity;

public static class MeEndpoint
{
    public static IEndpointRouteBuilder MapMeEndpoint(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/api/auth/me",
            (ClaimsPrincipal user) =>
            {
                var userId =
                    user.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? user.FindFirstValue(JwtRegisteredClaimNames.Sub);

                var email =
                    user.FindFirstValue(ClaimTypes.Email)
                    ?? user.FindFirstValue(JwtRegisteredClaimNames.Email);

                return Results.Ok(new
                {
                    UserId = userId,
                    Email = email
                });
            })
        .RequireAuthorization()
        .WithName("Me")
        .WithSummary("Returns the authenticated user.")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);

        return app;
    }
}