namespace NexusERP.Application.Purchasing.CreatePurchaseOrder;

public sealed record CreatePurchaseOrderResponse(
    Guid Id,
    Guid SupplierId);