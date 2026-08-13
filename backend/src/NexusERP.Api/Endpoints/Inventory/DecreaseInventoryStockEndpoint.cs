using NexusERP.Application.Inventory.DecreaseInventoryStock;

namespace NexusERP.Api.Endpoints.Inventory;

public static class DecreaseInventoryStockEndpoint
{
    public static IEndpointRouteBuilder MapDecreaseInventoryStock(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/api/inventory/{id:guid}/decrease",
                async (
                    Guid id,
                    DecreaseInventoryStockRequest request,
                    DecreaseInventoryStockHandler handler,
                    CancellationToken cancellationToken) =>
                {
                    var response =
                        await handler.Handle(
                            id,
                            request,
                            cancellationToken);

                    return Results.Ok(response);
                })
            .WithName("DecreaseInventoryStock")
            .WithSummary("Decreases inventory stock.")
            .WithDescription(
                "Decreases the stock quantity of an inventory item.")
            .Produces<DecreaseInventoryStockResponse>(
                StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}