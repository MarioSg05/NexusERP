using NexusERP.Application.Purchasing.GetPurchaseOrders;

namespace NexusERP.Api.Endpoints.Purchasing;

public static class GetPurchaseOrdersEndpoint
{
    public static IEndpointRouteBuilder MapGetPurchaseOrders(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/api/purchase-orders",
                async (
                    GetPurchaseOrdersHandler handler,
                    CancellationToken cancellationToken) =>
                {
                    var purchaseOrders =
                        await handler.Handle(
                            cancellationToken);

                    return Results.Ok(purchaseOrders);
                })
            .WithName("GetPurchaseOrders")
            .WithSummary("Gets all purchase orders.")
            .WithDescription(
                "Returns the purchase orders registered in the system.")
            .Produces<IReadOnlyList<PurchaseOrderListItem>>(
                StatusCodes.Status200OK);

        return app;
    }
}