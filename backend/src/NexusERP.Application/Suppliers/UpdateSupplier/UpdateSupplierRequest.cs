namespace NexusERP.Application.Suppliers.UpdateSupplier;

public sealed record UpdateSupplierRequest(
    string? Email,
    string? Phone);