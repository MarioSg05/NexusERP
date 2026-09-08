using NexusERP.Application.Sales.GetSalesOrders;

namespace NexusERP.Api.Endpoints.Sales;

public static class GetSalesOrdersEndpoint
{
    public static IEndpointRouteBuilder MapGetSalesOrders(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/api/sales-orders",
                async (
                    GetSalesOrdersHandler handler,
                    CancellationToken cancellationToken) =>
                {
                    var salesOrders =
                        await handler.Handle(
                            cancellationToken);

                    return Results.Ok(salesOrders);
                })
            .WithName("GetSalesOrders")
            .WithSummary("Gets all sales orders.")
            .WithDescription(
                "Returns the sales orders registered in the system.")
            .Produces<IReadOnlyList<SalesOrderListItem>>(
                StatusCodes.Status200OK);

        return app;
    }
}