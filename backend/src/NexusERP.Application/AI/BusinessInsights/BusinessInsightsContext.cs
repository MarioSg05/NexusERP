namespace NexusERP.Application.AI.BusinessInsights;

public sealed record BusinessInsightsContext(
    int TotalProducts,
    int ActiveProducts,
    int LowStockProducts,
    int TotalSalesOrders,
    int PendingSalesOrders,
    decimal TotalSalesAmount,
    int TotalPurchaseOrders,
    int PendingPurchaseOrders,
    decimal TotalPurchasingAmount);