namespace NexusERP.Application.AI.BusinessInsights;

public sealed record GetBusinessInsightsResponse(
    IReadOnlyList<string> Inventory,
    IReadOnlyList<string> Sales,
    IReadOnlyList<string> Purchasing,
    IReadOnlyList<string> AttentionAreas,
    string? AiSummary);