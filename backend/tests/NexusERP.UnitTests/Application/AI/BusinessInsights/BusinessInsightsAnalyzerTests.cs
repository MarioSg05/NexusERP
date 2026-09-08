using NexusERP.Application.AI.BusinessInsights;

namespace NexusERP.UnitTests.Application.AI.BusinessInsights;

public sealed class BusinessInsightsAnalyzerTests
{
    private readonly BusinessInsightsAnalyzer _analyzer =
        new();

    [Fact]
    public void Analyze_WithCurrentBusinessSnapshot_ReturnsExpectedFacts()
    {
        var context =
            new BusinessInsightsContext(
                TotalProducts: 3,
                ActiveProducts: 3,
                LowStockProducts: 1,
                TotalSalesOrders: 6,
                PendingSalesOrders: 2,
                TotalSalesAmount: 151098.00m,
                TotalPurchaseOrders: 4,
                PendingPurchaseOrders: 0,
                TotalPurchasingAmount: 925.00m);

        var result =
            _analyzer.Analyze(context);

        Assert.Contains(
            "There are 3 registered products.",
            result.InventoryFacts);

        Assert.Contains(
            "3 of 3 products are active.",
            result.InventoryFacts);

        Assert.Contains(
            "1 of 3 products is currently low in stock.",
            result.InventoryFacts);

        Assert.Contains(
            "There are 6 sales orders.",
            result.SalesFacts);

        Assert.Contains(
            "2 of 6 sales orders are pending.",
            result.SalesFacts);

        Assert.Contains(
            "The total sales amount is 151098.00.",
            result.SalesFacts);

        Assert.Contains(
            "There are 4 purchase orders.",
            result.PurchasingFacts);

        Assert.Contains(
            "No purchase orders are currently pending.",
            result.PurchasingFacts);

        Assert.Contains(
            "The total purchasing amount is 925.00.",
            result.PurchasingFacts);

        Assert.Contains(
            "Review the 1 product currently classified as low stock.",
            result.AttentionAreas);

        Assert.Contains(
            "Review the 2 sales orders currently pending.",
            result.AttentionAreas);
    }

    [Fact]
    public void Analyze_WithNoProducts_ReturnsNoInventoryAssessment()
    {
        var context =
            CreateContext(
                totalProducts: 0,
                activeProducts: 0,
                lowStockProducts: 0);

        var result =
            _analyzer.Analyze(context);

        Assert.Contains(
            "There are no registered products.",
            result.InventoryFacts);

        Assert.Contains(
            "No inventory stock assessment can be made because there are no registered products.",
            result.InventoryFacts);

        Assert.Contains(
            "No inventory products are currently registered.",
            result.AiSignals);
    }

    [Fact]
    public void Analyze_WithNoSalesOrders_ReturnsNoSalesOrdersFact()
    {
        var context =
            CreateContext(
                totalSalesOrders: 0,
                pendingSalesOrders: 0,
                totalSalesAmount: 0m);

        var result =
            _analyzer.Analyze(context);

        Assert.Contains(
            "There are no sales orders.",
            result.SalesFacts);

        Assert.Contains(
            "The total sales amount is 0.00.",
            result.SalesFacts);

        Assert.Contains(
            "No sales orders are currently registered.",
            result.AiSignals);
    }

    [Fact]
    public void Analyze_WithNoPurchaseOrders_ReturnsNoPurchaseOrdersFact()
    {
        var context =
            CreateContext(
                totalPurchaseOrders: 0,
                pendingPurchaseOrders: 0,
                totalPurchasingAmount: 0m);

        var result =
            _analyzer.Analyze(context);

        Assert.Contains(
            "There are no purchase orders.",
            result.PurchasingFacts);

        Assert.Contains(
            "The total purchasing amount is 0.00.",
            result.PurchasingFacts);

        Assert.Contains(
            "No purchase orders are currently registered.",
            result.AiSignals);
    }

    [Fact]
    public void Analyze_WithNothingRequiringAttention_ReturnsCleanSnapshotMessage()
    {
        var context =
            CreateContext(
                totalProducts: 3,
                activeProducts: 3,
                lowStockProducts: 0,
                totalSalesOrders: 4,
                pendingSalesOrders: 0,
                totalPurchaseOrders: 2,
                pendingPurchaseOrders: 0);

        var result =
            _analyzer.Analyze(context);

        Assert.Single(
            result.AttentionAreas);

        Assert.Equal(
            "No low-stock products, pending sales orders, or pending purchase orders require attention in the current snapshot.",
            result.AttentionAreas[0]);

        Assert.Contains(
            "No low-stock inventory currently requires review.",
            result.AiSignals);

        Assert.Contains(
            "No sales orders are currently pending.",
            result.AiSignals);

        Assert.Contains(
            "No purchase orders are currently pending.",
            result.AiSignals);
    }

    [Fact]
    public void Analyze_WithSinglePendingOrders_UsesSingularWording()
    {
        var context =
            CreateContext(
                totalProducts: 5,
                activeProducts: 5,
                lowStockProducts: 1,
                totalSalesOrders: 5,
                pendingSalesOrders: 1,
                totalPurchaseOrders: 5,
                pendingPurchaseOrders: 1);

        var result =
            _analyzer.Analyze(context);

        Assert.Contains(
            "Review the 1 product currently classified as low stock.",
            result.AttentionAreas);

        Assert.Contains(
            "Review the 1 sales order currently pending.",
            result.AttentionAreas);

        Assert.Contains(
            "Review the 1 purchase order currently pending.",
            result.AttentionAreas);
    }

    [Fact]
    public void Analyze_WithInactiveProducts_ReturnsInactiveProductSignal()
    {
        var context =
            CreateContext(
                totalProducts: 5,
                activeProducts: 3);

        var result =
            _analyzer.Analyze(context);

        Assert.Contains(
            "Some registered products are inactive.",
            result.AiSignals);

        Assert.DoesNotContain(
            "All registered products are active.",
            result.AiSignals);
    }

    private static BusinessInsightsContext CreateContext(
        int totalProducts = 0,
        int activeProducts = 0,
        int lowStockProducts = 0,
        int totalSalesOrders = 0,
        int pendingSalesOrders = 0,
        decimal totalSalesAmount = 0m,
        int totalPurchaseOrders = 0,
        int pendingPurchaseOrders = 0,
        decimal totalPurchasingAmount = 0m)
    {
        return new BusinessInsightsContext(
            totalProducts,
            activeProducts,
            lowStockProducts,
            totalSalesOrders,
            pendingSalesOrders,
            totalSalesAmount,
            totalPurchaseOrders,
            pendingPurchaseOrders,
            totalPurchasingAmount);
    }
}