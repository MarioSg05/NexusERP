using NexusERP.Application.Suppliers.GetSuppliers;

namespace NexusERP.Api.Endpoints.Suppliers;

public static class GetSuppliersEndpoint
{
    public static IEndpointRouteBuilder MapGetSuppliers(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/api/suppliers",
                async (
                    GetSuppliersHandler handler,
                    CancellationToken cancellationToken) =>
                {
                    var suppliers =
                        await handler.Handle(
                            cancellationToken);

                    return Results.Ok(suppliers);
                })
            .WithName("GetSuppliers")
            .WithSummary("Gets all suppliers.")
            .WithDescription(
                "Returns the suppliers registered in the system.")
            .Produces<IReadOnlyList<SupplierListItem>>(
                StatusCodes.Status200OK);

        return app;
    }
}