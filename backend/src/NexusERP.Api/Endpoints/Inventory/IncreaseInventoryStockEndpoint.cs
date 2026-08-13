using NexusERP.Application.Inventory.IncreaseInventoryStock;

namespace NexusERP.Api.Endpoints.Inventory;

public static class IncreaseInventoryStockEndpoint
{
    public static IEndpointRouteBuilder MapIncreaseInventoryStock(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/api/inventory/{id:guid}/increase",
                async (
                    Guid id,
                    IncreaseInventoryStockRequest request,
                    IncreaseInventoryStockHandler handler,
                    CancellationToken cancellationToken) =>
                {
                    var response =
                        await handler.Handle(
                            id,
                            request,
                            cancellationToken);

                    return Results.Ok(response);
                })
            .WithName("IncreaseInventoryStock")
            .WithSummary("Increases inventory stock.")
            .WithDescription(
                "Increases the stock quantity of an inventory item.")
            .Produces<IncreaseInventoryStockResponse>(
                StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}