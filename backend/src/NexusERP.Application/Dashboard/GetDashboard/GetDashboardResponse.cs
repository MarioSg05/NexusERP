using NexusERP.Application.Dashboard.Widgets;

namespace NexusERP.Application.Dashboard.GetDashboard;

public sealed class GetDashboardResponse
{
    public DashboardInventoryWidget Inventory { get; init; } = new();

    public DashboardSalesWidget Sales { get; init; } = new();

    public DashboardPurchasingWidget Purchasing { get; init; } = new();
}