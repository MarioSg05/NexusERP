namespace NexusERP.Infrastructure.Messaging.Outbox;

public sealed class OutboxMessage
{
    public Guid Id { get; private set; }

    public DateTime OccurredOnUtc { get; private set; }

    public string Type { get; private set; } =
        string.Empty;

    public string Payload { get; private set; } =
        string.Empty;

    public DateTime? ProcessedOnUtc { get; private set; }

    public string? Error { get; private set; }

    private OutboxMessage()
    {
    }

    public static OutboxMessage Create(
        Guid id,
        DateTime occurredOnUtc,
        string type,
        string payload)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Outbox message identifier is required.",
                nameof(id));
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(
            type);

        ArgumentException.ThrowIfNullOrWhiteSpace(
            payload);

        return new OutboxMessage
        {
            Id = id,
            OccurredOnUtc = occurredOnUtc,
            Type = type.Trim(),
            Payload = payload
        };
    }

    public void MarkAsProcessed(
        DateTime processedOnUtc)
    {
        ProcessedOnUtc =
            processedOnUtc;

        Error =
            null;
    }

    public void SetError(
        string error)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(
            error);

        Error =
            error.Trim();
    }
}