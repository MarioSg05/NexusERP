using RabbitMQ.Client;

namespace NexusERP.Infrastructure.Messaging.RabbitMq;

public sealed class RabbitMqRetryPolicy
{
    private readonly RabbitMqConsumerSettings
        _settings;

    public RabbitMqRetryPolicy(
        RabbitMqConsumerSettings settings)
    {
        _settings =
            settings;
    }

    public int GetRetryCount(
        IReadOnlyBasicProperties properties)
    {
        if (properties.Headers is null ||
            !properties.Headers.TryGetValue(
                RabbitMqRetryHeaders.RetryCount,
                out var value) ||
            value is null)
        {
            return 0;
        }

        return value switch
        {
            int intValue =>
                intValue,

            long longValue
                when longValue <= int.MaxValue =>
                    (int)longValue,

            byte[] bytes
                when int.TryParse(
                    System.Text.Encoding.UTF8
                        .GetString(bytes),
                    out var parsedValue) =>
                    parsedValue,

            _ =>
                0
        };
    }

    public bool ShouldRetry(
        int retryCount)
    {
        return retryCount <
            _settings.MaxRetryAttempts;
    }
}