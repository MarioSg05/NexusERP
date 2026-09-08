using NexusERP.Application.Common.Interfaces;

namespace NexusERP.Application.Dashboard.GetDashboard;

public sealed class GetDashboardHandler
{
    private readonly IDashboardQueries _dashboardQueries;

    public GetDashboardHandler(
        IDashboardQueries dashboardQueries)
    {
        _dashboardQueries = dashboardQueries;
    }

    public async Task<GetDashboardResponse> Handle(
        GetDashboardRequest request,
        CancellationToken cancellationToken = default)
    {
        var inventory =
            await _dashboardQueries.GetInventoryWidgetAsync(
                cancellationToken);

        var sales =
            await _dashboardQueries.GetSalesWidgetAsync(
                cancellationToken);

        var purchasing =
            await _dashboardQueries.GetPurchasingWidgetAsync(
                cancellationToken);

        return new GetDashboardResponse
        {
            Inventory = inventory,
            Sales = sales,
            Purchasing = purchasing
        };
    }
}