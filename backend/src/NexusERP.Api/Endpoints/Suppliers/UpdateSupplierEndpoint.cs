using NexusERP.Api.Authorization;
using NexusERP.Application.Suppliers.UpdateSupplier;

namespace NexusERP.Api.Endpoints.Suppliers;

public static class UpdateSupplierEndpoint
{
    public static IEndpointRouteBuilder MapUpdateSupplier(
        this IEndpointRouteBuilder app)
    {
        app.MapPut(
                "/api/suppliers/{id:guid}",
                async (
                    Guid id,
                    UpdateSupplierRequest request,
                    UpdateSupplierHandler handler,
                    CancellationToken cancellationToken) =>
                {
                    var response =
                        await handler.Handle(
                            id,
                            request,
                            cancellationToken);

                    return Results.Ok(response);
                })
            .RequireAuthorization(
                AuthorizationPolicies.ManageErp)
            .WithName("UpdateSupplier")
            .WithSummary("Updates a supplier.")
            .WithDescription(
                "Updates the contact information of the specified supplier.")
            .Produces<UpdateSupplierResponse>(
                StatusCodes.Status200OK)
            .Produces(
                StatusCodes.Status400BadRequest)
            .Produces(
                StatusCodes.Status401Unauthorized)
            .Produces(
                StatusCodes.Status403Forbidden)
            .Produces(
                StatusCodes.Status404NotFound);

        return app;
    }
}