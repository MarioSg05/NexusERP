using NexusERP.Application.Customers.RegisterCustomer;

namespace NexusERP.Api.Endpoints.Customers;

public static class RegisterCustomerEndpoint
{
    public static IEndpointRouteBuilder MapRegisterCustomer(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/customers",
            async (
                RegisterCustomerRequest request,
                RegisterCustomerHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response =
                    await handler.Handle(
                        request,
                        cancellationToken);

                return Results.Created(
                    $"/api/customers/{response.Id}",
                    response);
            })

        .WithName("RegisterCustomer")
        .WithSummary("Registers a new customer.")
        .WithDescription("Creates a new customer in the system.")
        .Produces<RegisterCustomerResponse>(
            StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest);

        return app;
    }
}