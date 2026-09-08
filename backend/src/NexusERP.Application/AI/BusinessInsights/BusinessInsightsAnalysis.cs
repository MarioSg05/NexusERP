namespace NexusERP.Application.AI.BusinessInsights;

public sealed record BusinessInsightsAnalysis(
    IReadOnlyList<string> InventoryFacts,
    IReadOnlyList<string> SalesFacts,
    IReadOnlyList<string> PurchasingFacts,
    IReadOnlyList<string> AttentionAreas,
    IReadOnlyList<string> AiSignals);