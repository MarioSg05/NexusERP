using NexusERP.Api.Authorization;
using NexusERP.Application.Suppliers.ActivateSupplier;

namespace NexusERP.Api.Endpoints.Suppliers;

public static class ActivateSupplierEndpoint
{
    public static IEndpointRouteBuilder MapActivateSupplier(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/api/suppliers/{id:guid}/activate",
                async (
                    Guid id,
                    ActivateSupplierHandler handler,
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
            .WithName("ActivateSupplier")
            .WithSummary("Activates a supplier.")
            .WithDescription(
                "Activates the specified supplier.")
            .Produces<ActivateSupplierResponse>(
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