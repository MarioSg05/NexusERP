using System.Text;

using Microsoft.Extensions.Options;

using NexusERP.Infrastructure.Messaging.RabbitMq;
using NexusERP.IntegrationTests.Infrastructure;

using RabbitMQ.Client;

namespace NexusERP.IntegrationTests.Messaging;

[Collection(RabbitMqIntegrationTestCollection.Name)]
public sealed class RabbitMqIntegrationEventPublisherTests
{
    private readonly RabbitMqFixture _rabbitMq;

    public RabbitMqIntegrationEventPublisherTests(
        RabbitMqFixture rabbitMq)
    {
        _rabbitMq =
            rabbitMq;
    }

    [Fact]
    public async Task PublishAsync_ShouldPublishMessageToRabbitMq()
    {
        var exchangeName =
            $"nexuserp.test.{Guid.NewGuid():N}";

        var queueName =
            $"nexuserp.test.{Guid.NewGuid():N}";

        const string messageType =
            "sales-order-confirmed";

        var messageId =
            Guid.NewGuid();

        var payload =
            $$"""
            {"id":"{{messageId}}"}
            """;

        var connectionFactory =
            new ConnectionFactory
            {
                Uri =
                    new Uri(
                        _rabbitMq.ConnectionString)
            };

        await using var connection =
            await connectionFactory
                .CreateConnectionAsync();

        await using var channel =
            await connection
                .CreateChannelAsync();

        await channel.ExchangeDeclareAsync(
            exchange: exchangeName,
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false);

        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: false,
            exclusive: false,
            autoDelete: true);

        await channel.QueueBindAsync(
            queue: queueName,
            exchange: exchangeName,
            routingKey: messageType);

        var settings =
            new RabbitMqSettings
            {
                ConnectionString =
                    _rabbitMq.ConnectionString,

                ExchangeName =
                    exchangeName
            };

        var publisher =
            new RabbitMqIntegrationEventPublisher(
                Options.Create(
                    settings));

        await publisher.PublishAsync(
            messageId,
            messageType,
            payload);

        var result =
            await channel.BasicGetAsync(
                queueName,
                autoAck: true);

        Assert.NotNull(
            result);

        Assert.Equal(
            messageType,
            result.RoutingKey);

        Assert.Equal(
            messageId.ToString(),
            result.BasicProperties.MessageId);

        Assert.Equal(
            "application/json",
            result.BasicProperties.ContentType);

        Assert.Equal(
            messageType,
            result.BasicProperties.Type);

        var receivedPayload =
            Encoding.UTF8.GetString(
                result.Body.Span);

        Assert.Equal(
            payload,
            receivedPayload);
    }
}