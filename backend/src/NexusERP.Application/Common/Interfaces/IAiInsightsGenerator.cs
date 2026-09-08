namespace NexusERP.Application.Common.Interfaces;

public interface IAiInsightsGenerator
{
    Task<string> GenerateBusinessInsightsAsync(
        IReadOnlyList<string> signals,
        CancellationToken cancellationToken = default);
}