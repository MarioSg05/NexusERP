namespace NexusERP.Application.Reports.GetPurchasingReport;

public sealed class GetPurchasingReportResponse
{
    public Guid PurchaseOrderId { get; init; }

    public Guid SupplierId { get; init; }

    public DateTime OrderDate { get; init; }

    public string Status { get; init; } = string.Empty;

    public decimal Total { get; init; }
}