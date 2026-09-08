using NexusERP.Api.Authorization;
using NexusERP.Application.Identity.UpdateUser;

namespace NexusERP.Api.Endpoints.Identity;

public static class UpdateUserEndpoint
{
    public static IEndpointRouteBuilder MapUpdateUser(
        this IEndpointRouteBuilder app)
    {
        app.MapPut(
                "/api/users/{id:guid}",
                async (
                    Guid id,
                    UpdateUserRequest request,
                    UpdateUserHandler handler,
                    CancellationToken cancellationToken) =>
                {
                    var response =
                        await handler.Handle(
                            id,
                            request,
                            cancellationToken);

                    return Results.Ok(response);
                })
            .RequireAuthorization(
                AuthorizationPolicies.ManageUsers)
            .WithName("UpdateUser")
            .WithSummary("Updates a user.")
            .WithDescription(
                "Updates the name and email of the specified user.")
            .Produces<UpdateUserResponse>(
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