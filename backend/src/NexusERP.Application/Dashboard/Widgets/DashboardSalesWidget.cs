namespace NexusERP.Application.Dashboard.Widgets;

public sealed class DashboardSalesWidget
{
    public int TotalSalesOrders { get; init; }

    public int PendingSalesOrders { get; init; }

    public decimal TotalSalesAmount { get; init; }
}