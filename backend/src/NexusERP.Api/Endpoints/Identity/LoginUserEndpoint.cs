using NexusERP.Application.Identity.LoginUser;

namespace NexusERP.Api.Endpoints.Identity;

public static class LoginUserEndpoint
{
    public static IEndpointRouteBuilder MapLoginUser(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/auth/login",
            async (
                LoginUserRequest request,
                LoginUserHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response =
                    await handler.Handle(
                        request,
                        cancellationToken);

                return Results.Ok(response);
            })
        .AllowAnonymous()
        .WithName("LoginUser")
        .WithSummary("Authenticates a user.")
        .WithDescription(
            "Authenticates a registered user and returns a JWT.")
        .Produces<LoginUserResponse>(
            StatusCodes.Status200OK)
        .Produces(
            StatusCodes.Status401Unauthorized);

        return app;
    }
}