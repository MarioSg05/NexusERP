using NexusERP.Api.Authorization;
using NexusERP.Application.Suppliers.DeactivateSupplier;

namespace NexusERP.Api.Endpoints.Suppliers;

public static class DeactivateSupplierEndpoint
{
    public static IEndpointRouteBuilder MapDeactivateSupplier(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/api/suppliers/{id:guid}/deactivate",
                async (
                    Guid id,
                    DeactivateSupplierHandler handler,
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
            .WithName("DeactivateSupplier")
            .WithSummary("Deactivates a supplier.")
            .WithDescription(
                "Deactivates the specified supplier.")
            .Produces<DeactivateSupplierResponse>(
                StatusCodes.Status200OK)
            .Produces(
                StatusCodes.Status401Unauthorized)
            .Produces(
                StatusCodes.Status403Forbidden)
            .Produces(
                StatusCodes.Status404NotFound);

        return app;
    }
}