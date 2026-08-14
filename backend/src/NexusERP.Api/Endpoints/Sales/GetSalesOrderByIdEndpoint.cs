using NexusERP.Application.Sales.GetSalesOrderById;

namespace NexusERP.Api.Endpoints.Sales;

public static class GetSalesOrderByIdEndpoint
{
    public static IEndpointRouteBuilder MapGetSalesOrderById(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/api/sales-orders/{id:guid}",
                async (
                    Guid id,
                    GetSalesOrderByIdHandler handler,
                    CancellationToken cancellationToken) =>
                {
                    var salesOrder =
                        await handler.Handle(
                            id,
                            cancellationToken);

                    return salesOrder is null
                        ? Results.NotFound()
                        : Results.Ok(salesOrder);
                })
            .WithName("GetSalesOrderById")
            .WithSummary(
                "Gets a sales order by identifier.")
            .WithDescription(
                "Returns the sales order and its items matching the specified identifier.")
            .Produces<SalesOrderDetail>(
                StatusCodes.Status200OK)
            .Produces(
                StatusCodes.Status404NotFound);

        return app;
    }
}