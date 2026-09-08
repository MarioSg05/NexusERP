namespace NexusERP.Application.Suppliers.GetSuppliers;

public sealed class SupplierListItem
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string TaxIdentifier { get; init; } = string.Empty;

    public bool IsActive { get; init; }
}