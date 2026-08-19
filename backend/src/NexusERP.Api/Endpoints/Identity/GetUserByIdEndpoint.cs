using NexusERP.Api.Authorization;
using NexusERP.Application.Identity.GetUserById;

namespace NexusERP.Api.Endpoints.Identity;

public static class GetUserByIdEndpoint
{
    public static IEndpointRouteBuilder MapGetUserById(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/api/users/{id:guid}",
                async (
                    Guid id,
                    GetUserByIdHandler handler,
                    CancellationToken cancellationToken) =>
                {
                    var user =
                        await handler.Handle(
                            id,
                            cancellationToken);

                    return Results.Ok(user);
                })
            .RequireAuthorization(
                AuthorizationPolicies.ManageUsers)
            .WithName("GetUserById")
            .WithSummary("Gets a user by identifier.")
            .WithDescription(
                "Returns the specified user.")
            .Produces<UserDetail>(
                StatusCodes.Status200OK)
            .Produces(
                StatusCodes.Status401Unauthorized)
            .Produces(
                StatusCodes.Status403Forbidden)
            .Produces(
                StatusCodes.Status404NotFound);

        return app;
    }
}