namespace NexusERP.Application.Suppliers.UpdateSupplier;

public sealed record UpdateSupplierResponse(
    Guid Id,
    string Name,
    string TaxIdentifier,
    string? Email,
    string? Phone,
    bool IsActive);