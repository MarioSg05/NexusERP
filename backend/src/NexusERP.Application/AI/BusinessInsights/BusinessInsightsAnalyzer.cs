namespace NexusERP.Application.AI.BusinessInsights;

public sealed class BusinessInsightsAnalyzer
{
    public BusinessInsightsAnalysis Analyze(
        BusinessInsightsContext context)
    {
        return new BusinessInsightsAnalysis(
            BuildInventoryFacts(context),
            BuildSalesFacts(context),
            BuildPurchasingFacts(context),
            BuildAttentionAreas(context),
            BuildAiSignals(context));
    }

    private static IReadOnlyList<string>
        BuildInventoryFacts(
            BusinessInsightsContext context)
    {
        if (context.TotalProducts == 0)
        {
            return new List<string>
            {
                "There are no registered products.",
                "No inventory stock assessment can be made because there are no registered products."
            };
        }

        var facts = new List<string>
        {
            $"There are {context.TotalProducts} registered products.",
            $"{context.ActiveProducts} of {context.TotalProducts} products are active.",
            context.LowStockProducts == 1
                ? $"1 of {context.TotalProducts} products is currently low in stock."
                : $"{context.LowStockProducts} of {context.TotalProducts} products are currently low in stock."
        };

        if (context.LowStockProducts == 0)
        {
            facts.Add(
                "No products are currently classified as low stock.");
        }

        return facts;
    }

    private static IReadOnlyList<string>
        BuildSalesFacts(
            BusinessInsightsContext context)
    {
        if (context.TotalSalesOrders == 0)
        {
            return new List<string>
            {
                "There are no sales orders.",
                $"The total sales amount is {context.TotalSalesAmount:F2}."
            };
        }

        var facts = new List<string>
        {
            $"There are {context.TotalSalesOrders} sales orders.",
            $"{context.PendingSalesOrders} of {context.TotalSalesOrders} sales orders are pending.",
            $"The total sales amount is {context.TotalSalesAmount:F2}."
        };

        if (context.PendingSalesOrders == 0)
        {
            facts.Add(
                "No sales orders are currently pending.");
        }

        return facts;
    }

    private static IReadOnlyList<string>
        BuildPurchasingFacts(
            BusinessInsightsContext context)
    {
        if (context.TotalPurchaseOrders == 0)
        {
            return new List<string>
            {
                "There are no purchase orders.",
                $"The total purchasing amount is {context.TotalPurchasingAmount:F2}."
            };
        }

        var facts = new List<string>
        {
            $"There are {context.TotalPurchaseOrders} purchase orders.",
            $"{context.PendingPurchaseOrders} of {context.TotalPurchaseOrders} purchase orders are pending.",
            $"The total purchasing amount is {context.TotalPurchasingAmount:F2}."
        };

        if (context.PendingPurchaseOrders == 0)
        {
            facts.Add(
                "No purchase orders are currently pending.");
        }

        return facts;
    }

    private static IReadOnlyList<string>
        BuildAttentionAreas(
            BusinessInsightsContext context)
    {
        var areas = new List<string>();

        if (context.LowStockProducts > 0)
        {
            var productLabel =
                context.LowStockProducts == 1
                    ? "product"
                    : "products";

            areas.Add(
                $"Review the {context.LowStockProducts} {productLabel} currently classified as low stock.");
        }

        if (context.PendingSalesOrders > 0)
        {
            var orderLabel =
                context.PendingSalesOrders == 1
                    ? "sales order"
                    : "sales orders";

            areas.Add(
                $"Review the {context.PendingSalesOrders} {orderLabel} currently pending.");
        }

        if (context.PendingPurchaseOrders > 0)
        {
            var orderLabel =
                context.PendingPurchaseOrders == 1
                    ? "purchase order"
                    : "purchase orders";

            areas.Add(
                $"Review the {context.PendingPurchaseOrders} {orderLabel} currently pending.");
        }

        if (areas.Count == 0)
        {
            areas.Add(
                "No low-stock products, pending sales orders, or pending purchase orders require attention in the current snapshot.");
        }

        return areas;
    }

    private static IReadOnlyList<string>
        BuildAiSignals(
            BusinessInsightsContext context)
    {
        var signals = new List<string>();

        if (context.TotalProducts == 0)
        {
            signals.Add(
                "No inventory products are currently registered.");
        }
        else
        {
            if (context.ActiveProducts ==
                context.TotalProducts)
            {
                signals.Add(
                    "All registered products are active.");
            }
            else
            {
                signals.Add(
                    "Some registered products are inactive.");
            }

            if (context.LowStockProducts > 0)
            {
                signals.Add(
                    "Low-stock inventory requires review.");
            }
            else
            {
                signals.Add(
                    "No low-stock inventory currently requires review.");
            }
        }

        if (context.TotalSalesOrders == 0)
        {
            signals.Add(
                "No sales orders are currently registered.");
        }
        else if (context.PendingSalesOrders > 0)
        {
            signals.Add(
                "Pending sales orders require review.");
        }
        else
        {
            signals.Add(
                "No sales orders are currently pending.");
        }

        if (context.TotalPurchaseOrders == 0)
        {
            signals.Add(
                "No purchase orders are currently registered.");
        }
        else if (context.PendingPurchaseOrders > 0)
        {
            signals.Add(
                "Pending purchase orders require review.");
        }
        else
        {
            signals.Add(
                "No purchase orders are currently pending.");
        }

        return signals;
    }
}