using NexusERP.Domain.Customers.Enums;

namespace NexusERP.Application.Customers.UpdateCustomer;

public sealed record UpdateCustomerRequest(
    string Name,
    string Email,
    string? Phone,
    CustomerType Type);