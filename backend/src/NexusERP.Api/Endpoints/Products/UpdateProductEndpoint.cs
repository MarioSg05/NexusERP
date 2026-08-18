using NexusERP.Application.Products.UpdateProduct;
using NexusERP.Api.Authorization;

namespace NexusERP.Api.Endpoints.Products;

public static class UpdateProductEndpoint
{
    public static IEndpointRouteBuilder MapUpdateProduct(
        this IEndpointRouteBuilder app)
    {
        app.MapPut(
                "/api/products/{id:guid}",
                async (
                    Guid id,
                    UpdateProductRequest request,
                    UpdateProductHandler handler,
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
    AuthorizationPolicies.ManageErp)
            .WithName("UpdateProduct")
            .WithSummary("Updates an existing product.")
            .WithDescription(
                "Updates the name and price of the product matching the specified identifier.")
            .Produces<UpdateProductResponse>(
                StatusCodes.Status200OK)
            .Produces(
                StatusCodes.Status400BadRequest)
            .Produces(
                StatusCodes.Status404NotFound);

        return app;
    }
}