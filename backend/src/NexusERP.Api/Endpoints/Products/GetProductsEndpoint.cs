using NexusERP.Application.Products.GetProducts;

namespace NexusERP.Api.Endpoints.Products;

public static class GetProductsEndpoint
{
    public static IEndpointRouteBuilder MapGetProducts(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/api/products",
                async (
                    GetProductsHandler handler,
                    CancellationToken cancellationToken) =>
                {
                    var products =
                        await handler.Handle(
                            cancellationToken);

                    return Results.Ok(products);
                })
            .WithName("GetProducts")
            .WithSummary("Gets all products.")
            .WithDescription(
                "Returns the products registered in the system.")
            .Produces<IReadOnlyList<ProductListItem>>(
                StatusCodes.Status200OK);

        return app;
    }
}