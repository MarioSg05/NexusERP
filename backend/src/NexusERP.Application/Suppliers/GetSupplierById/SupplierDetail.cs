namespace NexusERP.Application.Suppliers.GetSupplierById;

public sealed record SupplierDetail(
    Guid Id,
    string Name,
    string TaxIdentifier,
    string? Email,
    string? Phone,
    bool IsActive);