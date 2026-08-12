using NexusERP.Application.Customers.GetCustomers;

namespace NexusERP.Api.Endpoints.Customers;

public static class GetCustomersEndpoint
{
    public static IEndpointRouteBuilder MapGetCustomers(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/api/customers",
                async (
                    GetCustomersHandler handler,
                    CancellationToken cancellationToken) =>
                {
                    var customers =
                        await handler.Handle(cancellationToken);

                    return Results.Ok(customers);
                })
            .WithName("GetCustomers")
            .WithSummary("Gets all customers.")
            .WithDescription(
                "Returns the customers registered in the system.")
            .Produces<IReadOnlyList<CustomerListItem>>(
                StatusCodes.Status200OK);

        return app;
    }
}