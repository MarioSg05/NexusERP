namespace NexusERP.Application.Purchasing.CancelPurchaseOrder;

public sealed record CancelPurchaseOrderResponse(
    Guid Id,
    string Status);