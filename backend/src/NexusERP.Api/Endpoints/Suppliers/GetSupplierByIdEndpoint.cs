using NexusERP.Application.Suppliers.GetSupplierById;

namespace NexusERP.Api.Endpoints.Suppliers;

public static class GetSupplierByIdEndpoint
{
    public static IEndpointRouteBuilder MapGetSupplierById(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/api/suppliers/{id:guid}",
                async (
                    Guid id,
                    GetSupplierByIdHandler handler,
                    CancellationToken cancellationToken) =>
                {
                    var supplier =
                        await handler.Handle(
                            id,
                            cancellationToken);

                    return Results.Ok(supplier);
                })
            .WithName("GetSupplierById")
            .WithSummary("Gets a supplier by identifier.")
            .WithDescription(
                "Returns the specified supplier.")
            .Produces<SupplierDetail>(
                StatusCodes.Status200OK)
            .Produces(
                StatusCodes.Status401Unauthorized)
            .Produces(
                StatusCodes.Status404NotFound);

        return app;
    }
}