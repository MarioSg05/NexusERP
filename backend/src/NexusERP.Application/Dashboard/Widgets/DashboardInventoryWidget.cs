namespace NexusERP.Application.Dashboard.Widgets;

public sealed class DashboardInventoryWidget
{
    public int TotalProducts { get; init; }

    public int ActiveProducts { get; init; }

    public int LowStockProducts { get; init; }
}