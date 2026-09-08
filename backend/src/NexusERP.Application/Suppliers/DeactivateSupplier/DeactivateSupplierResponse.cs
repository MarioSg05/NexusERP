namespace NexusERP.Application.Suppliers.DeactivateSupplier;

public sealed record DeactivateSupplierResponse(
    Guid Id,
    bool IsActive);