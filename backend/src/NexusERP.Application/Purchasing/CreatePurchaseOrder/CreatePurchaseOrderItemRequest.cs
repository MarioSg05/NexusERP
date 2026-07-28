namespace NexusERP.Application.Purchasing.CreatePurchaseOrder;

public sealed record CreatePurchaseOrderItemRequest(
    Guid ProductId,
    int Quantity,
    decimal UnitPrice);