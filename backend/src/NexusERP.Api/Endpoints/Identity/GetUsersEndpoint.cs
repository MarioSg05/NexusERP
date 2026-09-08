using NexusERP.Api.Authorization;
using NexusERP.Application.Identity.GetUsers;

namespace NexusERP.Api.Endpoints.Identity;

public static class GetUsersEndpoint
{
    public static IEndpointRouteBuilder MapGetUsers(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/api/users",
                async (
                    GetUsersHandler handler,
                    CancellationToken cancellationToken) =>
                {
                    var users =
                        await handler.Handle(
                            cancellationToken);

                    return Results.Ok(users);
                })
            .RequireAuthorization(
                AuthorizationPolicies.ManageUsers)
            .WithName("GetUsers")
            .WithSummary("Gets all users.")
            .WithDescription(
                "Returns the users registered in the system.")
            .Produces<IReadOnlyList<UserListItem>>(
                StatusCodes.Status200OK)
            .Produces(
                StatusCodes.Status401Unauthorized)
            .Produces(
                StatusCodes.Status403Forbidden);

        return app;
    }
}