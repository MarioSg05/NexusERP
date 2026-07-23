using NexusERP.Application.Inventory.CreateInventory;

namespace NexusERP.Api.Endpoints.Inventory;

public static class CreateInventoryEndpoint
{
    public static IEndpointRouteBuilder MapCreateInventory(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/inventory",
            async (
                CreateInventoryRequest request,
                CreateInventoryHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response = await handler.Handle(
                    request,
                    cancellationToken);

                return Results.Created(
                    $"/api/inventory/{response.Id}",
                    response);
            })

        .WithName("CreateInventory")
        .WithSummary("Creates a new inventory record.")
        .WithDescription("Creates an inventory record for an existing product.")
        .Produces<CreateInventoryResponse>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest);

        return app;
    }
}