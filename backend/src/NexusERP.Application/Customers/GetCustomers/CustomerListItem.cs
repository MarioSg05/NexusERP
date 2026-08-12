namespace NexusERP.Application.Customers.GetCustomers;

public sealed class CustomerListItem
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public string? Phone { get; init; }

    public string Type { get; init; } = string.Empty;

    public bool IsActive { get; init; }
}