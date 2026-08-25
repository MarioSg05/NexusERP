namespace NexusERP.Worker;

public sealed class OutboxWorkerSettings
{
    public const string SectionName =
        "Outbox";

    public int BatchSize { get; init; } =
        20;

    public int PollingIntervalSeconds { get; init; } =
        5;
}