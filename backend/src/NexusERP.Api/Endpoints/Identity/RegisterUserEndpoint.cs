using NexusERP.Application.Identity.RegisterUser;
using NexusERP.Api.Authorization;

namespace NexusERP.Api.Endpoints.Identity;

public static class RegisterUserEndpoint
{
    public static IEndpointRouteBuilder MapRegisterUser(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/users",
            async (
                RegisterUserRequest request,
                RegisterUserHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response =
                    await handler.Handle(
                        request,
                        cancellationToken);

                return Results.Created(
                    $"/api/users/{response.Id}",
                    response);
            })
.RequireAuthorization(
    AuthorizationPolicies.ManageUsers)
.WithName("RegisterUser")
.WithSummary("Registers a new user.")
.WithDescription("Creates a new user in the system.")
.Produces<RegisterUserResponse>(StatusCodes.Status201Created)
.Produces(StatusCodes.Status400BadRequest);

        return app;
    }
}