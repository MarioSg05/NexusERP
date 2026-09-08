using NexusERP.Application.Sales.CreateSalesOrder;
using NexusERP.Api.Authorization;

namespace NexusERP.Api.Endpoints.Sales;

public static class CreateSalesOrderEndpoint
{
    public static IEndpointRouteBuilder MapCreateSalesOrder(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/sales-orders",
            async (
                CreateSalesOrderRequest request,
                CreateSalesOrderHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response =
                    await handler.Handle(
                        request,
                        cancellationToken);

                return Results.Created(
                    $"/api/sales-orders/{response.Id}",
                    response);
            })
.RequireAuthorization(
    AuthorizationPolicies.ManageErp)
        .WithName("CreateSalesOrder")
        .WithSummary("Creates a new sales order.")
        .WithDescription("Creates a new sales order in the system.")
        .Produces<CreateSalesOrderResponse>(
            StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest);

        return app;
    }
}