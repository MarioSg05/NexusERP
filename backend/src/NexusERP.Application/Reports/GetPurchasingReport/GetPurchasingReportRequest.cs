namespace NexusERP.Application.Reports.GetPurchasingReport;

public sealed record GetPurchasingReportRequest(
    DateOnly? From = null,
    DateOnly? To = null);