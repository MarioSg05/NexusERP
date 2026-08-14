using NexusERP.Application.Sales.CancelSalesOrder;

namespace NexusERP.Api.Endpoints.Sales;

public static class CancelSalesOrderEndpoint
{
    public static IEndpointRouteBuilder MapCancelSalesOrder(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/api/sales-orders/{id:guid}/cancel",
                async (
                    Guid id,
                    CancelSalesOrderHandler handler,
                    CancellationToken cancellationToken) =>
                {
                    var response =
                        await handler.Handle(
                            id,
                            cancellationToken);

                    return Results.Ok(response);
                })
            .WithName("CancelSalesOrder")
            .WithSummary("Cancels a sales order.")
            .WithDescription(
                "Cancels a pending sales order.")
            .Produces<CancelSalesOrderResponse>(
                StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}