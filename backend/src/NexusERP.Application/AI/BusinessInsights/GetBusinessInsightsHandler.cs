using NexusERP.Application.Common.Exceptions;
using NexusERP.Application.Common.Interfaces;
using NexusERP.Application.Dashboard.GetDashboard;

namespace NexusERP.Application.AI.BusinessInsights;

public sealed class GetBusinessInsightsHandler
{
    private readonly GetDashboardHandler _dashboardHandler;
    private readonly BusinessInsightsAnalyzer _analyzer;
    private readonly IAiInsightsGenerator _insightsGenerator;

    public GetBusinessInsightsHandler(
        GetDashboardHandler dashboardHandler,
        BusinessInsightsAnalyzer analyzer,
        IAiInsightsGenerator insightsGenerator)
    {
        _dashboardHandler = dashboardHandler;
        _analyzer = analyzer;
        _insightsGenerator = insightsGenerator;
    }

    public async Task<GetBusinessInsightsResponse> Handle(
        CancellationToken cancellationToken = default)
    {
        var dashboard =
            await _dashboardHandler.Handle(
                new GetDashboardRequest(),
                cancellationToken);

        var context =
            new BusinessInsightsContext(
                dashboard.Inventory.TotalProducts,
                dashboard.Inventory.ActiveProducts,
                dashboard.Inventory.LowStockProducts,
                dashboard.Sales.TotalSalesOrders,
                dashboard.Sales.PendingSalesOrders,
                dashboard.Sales.TotalSalesAmount,
                dashboard.Purchasing.TotalPurchaseOrders,
                dashboard.Purchasing.PendingPurchaseOrders,
                dashboard.Purchasing.TotalPurchasingAmount);

        var analysis =
            _analyzer.Analyze(context);

        string? aiSummary = null;

        try
        {
            aiSummary =
                await _insightsGenerator
                    .GenerateBusinessInsightsAsync(
                        analysis.AiSignals,
                        cancellationToken);
        }
        catch (AiProviderUnavailableException)
        {
            // Deterministic business insights remain available
            // when the optional AI provider is unavailable.
        }

        return new GetBusinessInsightsResponse(
            analysis.InventoryFacts,
            analysis.SalesFacts,
            analysis.PurchasingFacts,
            analysis.AttentionAreas,
            aiSummary);
    }
}