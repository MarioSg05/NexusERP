using NexusERP.Application.Inventory.GetInventory;

namespace NexusERP.Api.Endpoints.Inventory;

public static class GetInventoryEndpoint
{
    public static IEndpointRouteBuilder MapGetInventory(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/api/inventory",
                async (
                    GetInventoryHandler handler,
                    CancellationToken cancellationToken) =>
                {
                    var inventory =
                        await handler.Handle(
                            cancellationToken);

                    return Results.Ok(inventory);
                })
            .WithName("GetInventory")
            .WithSummary("Gets inventory.")
            .WithDescription(
                "Returns inventory information for registered products.")
            .Produces<IReadOnlyList<InventoryListItem>>(
                StatusCodes.Status200OK);

        return app;
    }
}