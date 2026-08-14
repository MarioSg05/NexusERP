using NexusERP.Application.Sales.ConfirmSalesOrder;

namespace NexusERP.Api.Endpoints.Sales;

public static class ConfirmSalesOrderEndpoint
{
    public static IEndpointRouteBuilder MapConfirmSalesOrder(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/api/sales-orders/{id:guid}/confirm",
                async (
                    Guid id,
                    ConfirmSalesOrderHandler handler,
                    CancellationToken cancellationToken) =>
                {
                    var response =
                        await handler.Handle(
                            id,
                            cancellationToken);

                    return Results.Ok(response);
                })
            .WithName("ConfirmSalesOrder")
            .WithSummary("Confirms a sales order.")
            .WithDescription(
                "Confirms a pending sales order and decreases the corresponding inventory stock.")
            .Produces<ConfirmSalesOrderResponse>(
                StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}