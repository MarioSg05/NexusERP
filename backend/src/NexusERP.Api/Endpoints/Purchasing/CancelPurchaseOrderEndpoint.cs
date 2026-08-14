using NexusERP.Application.Purchasing.CancelPurchaseOrder;

namespace NexusERP.Api.Endpoints.Purchasing;

public static class CancelPurchaseOrderEndpoint
{
    public static IEndpointRouteBuilder MapCancelPurchaseOrder(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/api/purchase-orders/{id:guid}/cancel",
                async (
                    Guid id,
                    CancelPurchaseOrderHandler handler,
                    CancellationToken cancellationToken) =>
                {
                    var response =
                        await handler.Handle(
                            id,
                            cancellationToken);

                    return Results.Ok(response);
                })
            .WithName("CancelPurchaseOrder")
            .WithSummary("Cancels a purchase order.")
            .WithDescription(
                "Cancels a pending purchase order.")
            .Produces<CancelPurchaseOrderResponse>(
                StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}