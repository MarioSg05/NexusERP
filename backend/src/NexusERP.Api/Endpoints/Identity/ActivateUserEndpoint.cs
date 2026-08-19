using NexusERP.Api.Authorization;
using NexusERP.Application.Identity.ActivateUser;

namespace NexusERP.Api.Endpoints.Identity;

public static class ActivateUserEndpoint
{
    public static IEndpointRouteBuilder MapActivateUser(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/api/users/{id:guid}/activate",
                async (
                    Guid id,
                    ActivateUserHandler handler,
                    CancellationToken cancellationToken) =>
                {
                    var response =
                        await handler.Handle(
                            id,
                            cancellationToken);

                    return Results.Ok(response);
                })
            .RequireAuthorization(
                AuthorizationPolicies.ManageUsers)
            .WithName("ActivateUser")
            .WithSummary("Activates a user.")
            .WithDescription(
                "Activates the specified user account.")
            .Produces<ActivateUserResponse>(
                StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}