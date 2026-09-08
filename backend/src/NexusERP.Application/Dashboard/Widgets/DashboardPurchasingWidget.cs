using Microsoft.EntityFrameworkCore;

namespace NexusERP.Application.Dashboard.Widgets;

public sealed class DashboardPurchasingWidget
{
    public int TotalPurchaseOrders { get; init; }

    public int PendingPurchaseOrders { get; init; }

    [Precision(18, 2)]
    public decimal TotalPurchasingAmount { get; init; }
}