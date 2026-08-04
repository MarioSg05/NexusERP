namespace NexusERP.Application.Reports.GetSalesReport;

public sealed class GetSalesReportResponse
{
    public Guid SalesOrderId { get; init; }

    public Guid CustomerId { get; init; }

    public DateTime OrderDate { get; init; }

    public string Status { get; init; } = string.Empty;

    public decimal Total { get; init; }
}