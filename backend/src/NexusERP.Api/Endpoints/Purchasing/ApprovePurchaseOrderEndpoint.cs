using NexusERP.Application.Purchasing.ApprovePurchaseOrder;
using NexusERP.Api.Authorization;

namespace NexusERP.Api.Endpoints.Purchasing;

public static class ApprovePurchaseOrderEndpoint
{
    public static IEndpointRouteBuilder MapApprovePurchaseOrder(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/api/purchase-orders/{id:guid}/approve",
                async (
                    Guid id,
                    ApprovePurchaseOrderHandler handler,
                    CancellationToken cancellationToken) =>
                {
                    var response =
                        await handler.Handle(
                            id,
                            cancellationToken);

                    return Results.Ok(response);
                })
                .RequireAuthorization(
    AuthorizationPolicies.ManageErp)
            .WithName("ApprovePurchaseOrder")
            .WithSummary("Approves a purchase order.")
            .WithDescription(
                "Approves a pending purchase order.")
            .Produces<ApprovePurchaseOrderResponse>(
                StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}