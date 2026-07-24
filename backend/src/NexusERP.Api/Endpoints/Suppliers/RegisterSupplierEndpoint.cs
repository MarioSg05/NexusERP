using NexusERP.Application.Suppliers.RegisterSupplier;

namespace NexusERP.Api.Endpoints.Suppliers;

public static class RegisterSupplierEndpoint
{
    public static IEndpointRouteBuilder MapRegisterSupplier(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/suppliers",
            async (
                RegisterSupplierRequest request,
                RegisterSupplierHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response =
                    await handler.Handle(
                        request,
                        cancellationToken);

                return Results.Created(
                    $"/api/suppliers/{response.Id}",
                    response);
            })

        .WithName("RegisterSupplier")
        .WithSummary("Registers a new supplier.")
        .WithDescription("Creates a new supplier in the system.")
        .Produces<RegisterSupplierResponse>(
            StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest);

        return app;
    }
}