using NexusERP.Application.Products.GetProductById;
using NexusERP.Application.Products.GetProducts;

namespace NexusERP.Api.Endpoints.Products;

public static class GetProductByIdEndpoint
{
    public static IEndpointRouteBuilder MapGetProductById(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/api/products/{id:guid}",
                async (
                    Guid id,
                    GetProductByIdHandler handler,
                    CancellationToken cancellationToken) =>
                {
                    var product =
                        await handler.Handle(
                            id,
                            cancellationToken);

                    return product is null
                        ? Results.NotFound()
                        : Results.Ok(product);
                })
            .WithName("GetProductById")
            .WithSummary("Gets a product by identifier.")
            .WithDescription(
                "Returns the product matching the specified identifier.")
            .Produces<ProductListItem>(
                StatusCodes.Status200OK)
            .Produces(
                StatusCodes.Status404NotFound);

        return app;
    }
}