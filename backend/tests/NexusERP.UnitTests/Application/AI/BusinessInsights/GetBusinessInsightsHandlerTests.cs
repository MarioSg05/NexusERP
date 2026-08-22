using NexusERP.Application.AI.BusinessInsights;
using NexusERP.Application.Common.Exceptions;
using NexusERP.Application.Common.Interfaces;
using NexusERP.Application.Dashboard.GetDashboard;
using NexusERP.Application.Dashboard.Widgets;

namespace NexusERP.UnitTests.Application.AI.BusinessInsights;

public sealed class GetBusinessInsightsHandlerTests
{
    [Fact]
    public async Task Handle_WhenAiProviderIsAvailable_ReturnsAnalysisAndAiSummary()
    {
        var dashboardQueries =
            new FakeDashboardQueries();

        var dashboardHandler =
            new GetDashboardHandler(
                dashboardQueries);

        var aiGenerator =
            new SuccessfulAiInsightsGenerator();

        var handler =
            new GetBusinessInsightsHandler(
                dashboardHandler,
                new BusinessInsightsAnalyzer(),
                aiGenerator);

        var result =
            await handler.Handle();

        Assert.Contains(
            "There are 3 registered products.",
            result.Inventory);

        Assert.Contains(
            "1 of 3 products is currently low in stock.",
            result.Inventory);

        Assert.Contains(
            "2 of 6 sales orders are pending.",
            result.Sales);

        Assert.Contains(
            "No purchase orders are currently pending.",
            result.Purchasing);

        Assert.Contains(
            "Review the 1 product currently classified as low stock.",
            result.AttentionAreas);

        Assert.Contains(
            "Review the 2 sales orders currently pending.",
            result.AttentionAreas);

        Assert.Equal(
            "Generated AI summary.",
            result.AiSummary);
    }

    [Fact]
    public async Task Handle_WhenAiProviderIsUnavailable_ReturnsAnalysisWithoutAiSummary()
    {
        var dashboardQueries =
            new FakeDashboardQueries();

        var dashboardHandler =
            new GetDashboardHandler(
                dashboardQueries);

        var aiGenerator =
            new UnavailableAiInsightsGenerator();

        var handler =
            new GetBusinessInsightsHandler(
                dashboardHandler,
                new BusinessInsightsAnalyzer(),
                aiGenerator);

        var result =
            await handler.Handle();

        Assert.Contains(
            "There are 3 registered products.",
            result.Inventory);

        Assert.Contains(
            "1 of 3 products is currently low in stock.",
            result.Inventory);

        Assert.Contains(
            "2 of 6 sales orders are pending.",
            result.Sales);

        Assert.Contains(
            "No purchase orders are currently pending.",
            result.Purchasing);

        Assert.Contains(
            "Review the 1 product currently classified as low stock.",
            result.AttentionAreas);

        Assert.Contains(
            "Review the 2 sales orders currently pending.",
            result.AttentionAreas);

        Assert.Null(
            result.AiSummary);
    }

    private sealed class FakeDashboardQueries
        : IDashboardQueries
    {
        public Task<DashboardInventoryWidget>
            GetInventoryWidgetAsync(
                CancellationToken cancellationToken = default)
        {
            return Task.FromResult(
                new DashboardInventoryWidget
                {
                    TotalProducts = 3,
                    ActiveProducts = 3,
                    LowStockProducts = 1
                });
        }

        public Task<DashboardSalesWidget>
            GetSalesWidgetAsync(
                CancellationToken cancellationToken = default)
        {
            return Task.FromResult(
                new DashboardSalesWidget
                {
                    TotalSalesOrders = 6,
                    PendingSalesOrders = 2,
                    TotalSalesAmount = 151098.00m
                });
        }

        public Task<DashboardPurchasingWidget>
            GetPurchasingWidgetAsync(
                CancellationToken cancellationToken = default)
        {
            return Task.FromResult(
                new DashboardPurchasingWidget
                {
                    TotalPurchaseOrders = 4,
                    PendingPurchaseOrders = 0,
                    TotalPurchasingAmount = 925.00m
                });
        }
    }

    private sealed class SuccessfulAiInsightsGenerator
        : IAiInsightsGenerator
    {
        public Task<string>
            GenerateBusinessInsightsAsync(
                IReadOnlyList<string> signals,
                CancellationToken cancellationToken = default)
        {
            return Task.FromResult(
                "Generated AI summary.");
        }
    }

    private sealed class UnavailableAiInsightsGenerator
        : IAiInsightsGenerator
    {
        public Task<string>
            GenerateBusinessInsightsAsync(
                IReadOnlyList<string> signals,
                CancellationToken cancellationToken = default)
        {
            throw new AiProviderUnavailableException(
                "AI provider is unavailable.");
        }
    }
}