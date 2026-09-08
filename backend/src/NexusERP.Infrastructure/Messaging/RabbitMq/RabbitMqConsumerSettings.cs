namespace NexusERP.Infrastructure.Messaging.RabbitMq;

public sealed class RabbitMqConsumerSettings
{
    public const string SectionName =
        "RabbitMqConsumer";

    public string QueueName { get; init; } =
        "nexuserp.sales-order-confirmed";

    public string RoutingKey { get; init; } =
        "sales-order-confirmed";

    public string RetryExchangeName { get; init; } =
        "nexuserp.retry";

    public string RetryQueueName { get; init; } =
        "nexuserp.sales-order-confirmed.retry";

    public string DeadLetterExchangeName { get; init; } =
        "nexuserp.dead-letter";

    public string DeadLetterQueueName { get; init; } =
        "nexuserp.sales-order-confirmed.dlq";

    public int MaxRetryAttempts { get; init; } =
        3;

    public int RetryDelaySeconds { get; init; } =
        5;
}
