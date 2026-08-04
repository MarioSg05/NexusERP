using NexusERP.Application.Dashboard.Widgets;

namespace NexusERP.Application.Common.Interfaces;

public interface IDashboardQueries
{
    Task<DashboardInventoryWidget>
        GetInventoryWidgetAsync(
            CancellationToken cancellationToken = default);

    Task<DashboardSalesWidget>
        GetSalesWidgetAsync(
            CancellationToken cancellationToken = default);

    Task<DashboardPurchasingWidget>
        GetPurchasingWidgetAsync(
            CancellationToken cancellationToken = default);
}