namespace NexusERP.Application.Suppliers.RegisterSupplier;

public sealed record RegisterSupplierRequest(
    string Name,
    string TaxIdentifier,
    string? Email,
    string? Phone);