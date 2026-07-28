using NexusERP.Application.Purchasing.CreatePurchaseOrder;

namespace NexusERP.Api.Endpoints.Purchasing;

public static class CreatePurchaseOrderEndpoint
{
    public static IEndpointRouteBuilder MapCreatePurchaseOrder(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/purchase-orders",
            async (
                CreatePurchaseOrderRequest request,
                CreatePurchaseOrderHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response =
                    await handler.Handle(
                        request,
                        cancellationToken);

                return Results.Created(
                    $"/api/purchase-orders/{response.Id}",
                    response);
            })

        .WithName("CreatePurchaseOrder")
        .WithSummary("Creates a new purchase order.")
        .WithDescription("Creates a new purchase order in the system.")
        .Produces<CreatePurchaseOrderResponse>(
            StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest);

        return app;
    }
}