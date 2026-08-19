using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using NexusERP.Api.Authorization;
using NexusERP.Application.Identity.ChangeUserRole;

namespace NexusERP.Api.Endpoints.Identity;

public static class ChangeUserRoleEndpoint
{
    public static IEndpointRouteBuilder MapChangeUserRole(
        this IEndpointRouteBuilder app)
    {
        app.MapPut(
                "/api/users/{id:guid}/role",
                async (
                    Guid id,
                    ChangeUserRoleRequest request,
                    ClaimsPrincipal principal,
                    ChangeUserRoleHandler handler,
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
                            request,
                            cancellationToken);

                    return Results.Ok(response);
                })
            .RequireAuthorization(
                AuthorizationPolicies.ManageUsers)
            .WithName("ChangeUserRole")
            .WithSummary("Changes a user's role.")
            .WithDescription(
                "Changes the role assigned to the specified user.")
            .Produces<ChangeUserRoleResponse>(
                StatusCodes.Status200OK)
            .Produces(
                StatusCodes.Status400BadRequest)
            .Produces(
                StatusCodes.Status401Unauthorized)
            .Produces(
                StatusCodes.Status403Forbidden)
            .Produces(
                StatusCodes.Status404NotFound);

        return app;
    }
}