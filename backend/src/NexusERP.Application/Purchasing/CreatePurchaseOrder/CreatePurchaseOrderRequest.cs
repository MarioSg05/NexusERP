namespace NexusERP.Application.Purchasing.CreatePurchaseOrder;

public sealed record CreatePurchaseOrderRequest(
    Guid SupplierId,
    IReadOnlyCollection<CreatePurchaseOrderItemRequest> Items);