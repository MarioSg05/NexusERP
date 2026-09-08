namespace NexusERP.Application.Reports.GetSalesReport;

public sealed record GetSalesReportRequest(
    DateOnly? From = null,
    DateOnly? To = null);