using Microsoft.EntityFrameworkCore;

namespace NexusERP.Application.Dashboard.Widgets;

public sealed class DashboardSalesWidget
{
    public int TotalSalesOrders { get; init; }

    public int PendingSalesOrders { get; init; }

    [Precision(18, 2)]
    public decimal TotalSalesAmount { get; init; }
}