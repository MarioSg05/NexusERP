namespace NexusERP.Application.Suppliers.ActivateSupplier;

public sealed record ActivateSupplierResponse(
    Guid Id,
    bool IsActive);