using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using NexusERP.Api.Authorization;
using NexusERP.Application.Identity.DeactivateUser;

namespace NexusERP.Api.Endpoints.Identity;

public static class DeactivateUserEndpoint
{
    public static IEndpointRouteBuilder MapDeactivateUser(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/api/users/{id:guid}/deactivate",
                async (
                    Guid id,
                    ClaimsPrincipal principal,
                    DeactivateUserHandler handler,
                    CancellationToken cancellationToken) =>
                {
                    var userIdValue =
                        principal.FindFirstValue(
                            ClaimTypes.NameIdentifier)
                        ?? principal.FindFirstValue(
                            JwtRegisteredClaimNames.Sub);

                    if (
                        !Guid.TryParse(
                            userIdValue,
                            out var currentUserId))
                    {
                        return Results.Unauthorized();
                    }

                    var response =
                        await handler.Handle(
                            id,
                            currentUserId,
                            cancellationToken);

                    return Results.Ok(response);
                })
            .RequireAuthorization(
                AuthorizationPolicies.ManageUsers)
            .WithName("DeactivateUser")
            .WithSummary("Deactivates a user.")
            .WithDescription(
                "Deactivates the specified user account.")
            .Produces<DeactivateUserResponse>(
                StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}