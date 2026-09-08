using NexusERP.Application.Products.RegisterProduct;
using NexusERP.Api.Authorization;

namespace NexusERP.Api.Endpoints.Products;

public static class RegisterProductEndpoint
{
    public static IEndpointRouteBuilder MapRegisterProduct(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/products",
            async (
                RegisterProductRequest request,
                RegisterProductHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response =
                    await handler.Handle(
                        request,
                        cancellationToken);

                return Results.Created(
                    $"/api/products/{response.Id}",
                    response);
            })
.RequireAuthorization(
    AuthorizationPolicies.ManageErp)
        .WithName("RegisterProduct")
        .WithSummary("Registers a new product.")
        .WithDescription("Creates a new product in the system.")
        .Produces<RegisterProductResponse>(
            StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest);

        return app;
    }
}