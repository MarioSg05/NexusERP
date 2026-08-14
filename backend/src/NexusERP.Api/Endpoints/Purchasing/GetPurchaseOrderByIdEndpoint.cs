using NexusERP.Application.Purchasing.GetPurchaseOrderById;

namespace NexusERP.Api.Endpoints.Purchasing;

public static class GetPurchaseOrderByIdEndpoint
{
    public static IEndpointRouteBuilder MapGetPurchaseOrderById(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/api/purchase-orders/{id:guid}",
                async (
                    Guid id,
                    GetPurchaseOrderByIdHandler handler,
                    CancellationToken cancellationToken) =>
                {
                    var purchaseOrder =
                        await handler.Handle(
                            id,
                            cancellationToken);

                    return purchaseOrder is null
                        ? Results.NotFound()
                        : Results.Ok(purchaseOrder);
                })
            .WithName("GetPurchaseOrderById")
            .WithSummary("Gets a purchase order by identifier.")
            .WithDescription(
                "Returns the purchase order and its items matching the specified identifier.")
            .Produces<PurchaseOrderDetail>(
                StatusCodes.Status200OK)
            .Produces(
                StatusCodes.Status404NotFound);

        return app;
    }
}