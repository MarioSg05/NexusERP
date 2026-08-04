namespace NexusERP.Application.Dashboard.Widgets;

public sealed class DashboardPurchasingWidget
{
    public int TotalPurchaseOrders { get; init; }

    public int PendingPurchaseOrders { get; init; }

    public decimal TotalPurchasingAmount { get; init; }
}