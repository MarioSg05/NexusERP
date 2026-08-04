namespace NexusERP.Application.Reports.GetInventoryReport;

public sealed record GetInventoryReportResponse(
    Guid ProductId,
    string Sku,
    string ProductName,
    int Quantity,
    bool IsActive);