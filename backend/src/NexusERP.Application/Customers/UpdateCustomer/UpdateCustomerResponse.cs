namespace NexusERP.Application.Customers.UpdateCustomer;

public sealed record UpdateCustomerResponse(
    Guid Id,
    string Email);