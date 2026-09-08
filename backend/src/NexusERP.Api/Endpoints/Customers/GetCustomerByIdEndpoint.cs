using NexusERP.Application.Customers.GetCustomerById;
using NexusERP.Application.Customers.GetCustomers;

namespace NexusERP.Api.Endpoints.Customers;

public static class GetCustomerByIdEndpoint
{
    public static IEndpointRouteBuilder MapGetCustomerById(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/api/customers/{id:guid}",
                async (
                    Guid id,
                    GetCustomerByIdHandler handler,
                    CancellationToken cancellationToken) =>
                {
                    var customer =
                        await handler.Handle(
                            id,
                            cancellationToken);

                    return customer is null
                        ? Results.NotFound()
                        : Results.Ok(customer);
                })
            .WithName("GetCustomerById")
            .WithSummary("Gets a customer by identifier.")
            .WithDescription(
                "Returns the customer matching the specified identifier.")
            .Produces<CustomerListItem>(
                StatusCodes.Status200OK)
            .Produces(
                StatusCodes.Status404NotFound);

        return app;
    }
}