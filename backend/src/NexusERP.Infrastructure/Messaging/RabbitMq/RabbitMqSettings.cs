namespace NexusERP.Infrastructure.Messaging.RabbitMq;

public sealed class RabbitMqSettings
{
    public const string SectionName =
        "RabbitMq";

    public string ConnectionString { get; init; } =
        string.Empty;

    public string ExchangeName { get; init; } =
        "nexuserp.events";
}