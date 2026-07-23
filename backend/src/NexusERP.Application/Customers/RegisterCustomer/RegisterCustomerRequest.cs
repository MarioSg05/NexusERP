using NexusERP.Domain.Customers.Enums;

namespace NexusERP.Application.Customers.RegisterCustomer;

public sealed record RegisterCustomerRequest(
    string Name,
    string Email,
    string? Phone,
    CustomerType Type);