namespace NexusERP.Infrastructure.Messaging.RabbitMq;

public sealed class RabbitMqConsumerSettings
{
    public const string SectionName =
        "RabbitMqConsumer";

    public string QueueName { get; init; } =
        "nexuserp.sales-order-confirmed";

    public string RoutingKey { get; init; } =
        "sales-order-confirmed";
}