namespace NexusERP.Application.Reports.GetLowStockReport;

public sealed record GetLowStockReportRequest(
    int MinimumStock = 10);