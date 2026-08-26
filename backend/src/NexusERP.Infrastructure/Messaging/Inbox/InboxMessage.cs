namespace NexusERP.Infrastructure.Messaging.Inbox;

public sealed class InboxMessage
{
    public Guid Id { get; private set; }

    public DateTime ReceivedOnUtc { get; private set; }

    public string Type { get; private set; } =
        string.Empty;

    public DateTime? ProcessedOnUtc { get; private set; }

    private InboxMessage()
    {
    }

    public static InboxMessage Create(
        Guid id,
        DateTime receivedOnUtc,
        string type)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Inbox message identifier is required.",
                nameof(id));
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(
            type);

        return new InboxMessage
        {
            Id = id,
            ReceivedOnUtc = receivedOnUtc,
            Type = type.Trim()
        };
    }

    public void MarkAsProcessed(
        DateTime processedOnUtc)
    {
        ProcessedOnUtc =
            processedOnUtc;
    }
}