using NexusERP.Application.Customers.UpdateCustomer;
using NexusERP.Api.Authorization;

namespace NexusERP.Api.Endpoints.Customers;

public static class UpdateCustomerEndpoint
{
    public static IEndpointRouteBuilder MapUpdateCustomer(
        this IEndpointRouteBuilder app)
    {
        app.MapPut(
                "/api/customers/{id:guid}",
                async (
                    Guid id,
                    UpdateCustomerRequest request,
                    UpdateCustomerHandler handler,
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
            .WithName("UpdateCustomer")
            .WithSummary("Updates an existing customer.")
            .WithDescription(
                "Updates the customer matching the specified identifier.")
            .Produces<UpdateCustomerResponse>(
                StatusCodes.Status200OK)
            .Produces(
                StatusCodes.Status400BadRequest);

        return app;
    }
}