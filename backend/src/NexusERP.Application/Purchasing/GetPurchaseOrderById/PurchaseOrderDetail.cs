namespace NexusERP.Application.Purchasing.GetPurchaseOrderById;

public sealed class PurchaseOrderDetail
{
    public Guid Id { get; init; }

    public Guid SupplierId { get; init; }

    public string SupplierName { get; init; } = string.Empty;

    public DateTime OrderDate { get; init; }

    public string Status { get; init; } = string.Empty;

    public decimal Total { get; init; }

    public IReadOnlyList<PurchaseOrderItemDetail> Items { get; init; }
        = [];
}