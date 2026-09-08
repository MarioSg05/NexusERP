using NexusERP.Application.Inventory.AdjustInventoryStock;
using NexusERP.Api.Authorization;

namespace NexusERP.Api.Endpoints.Inventory;

public static class AdjustInventoryStockEndpoint
{
    public static IEndpointRouteBuilder MapAdjustInventoryStock(
        this IEndpointRouteBuilder app)
    {
        app.MapPut(
                "/api/inventory/{id:guid}/adjust",
                async (
                    Guid id,
                    AdjustInventoryStockRequest request,
                    AdjustInventoryStockHandler handler,
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
            .WithName("AdjustInventoryStock")
            .WithSummary("Adjusts inventory stock.")
            .WithDescription(
                "Sets the stock quantity of an inventory item to the specified value.")
            .Produces<AdjustInventoryStockResponse>(
                StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}