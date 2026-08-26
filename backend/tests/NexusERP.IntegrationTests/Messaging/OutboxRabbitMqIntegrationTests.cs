using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

using NexusERP.Infrastructure.Messaging.Outbox;
using NexusERP.Infrastructure.Messaging.RabbitMq;
using NexusERP.Infrastructure.Persistence;
using NexusERP.IntegrationTests.Infrastructure;

using RabbitMQ.Client;

namespace NexusERP.IntegrationTests.Messaging;

[Collection(MessagingIntegrationTestCollection.Name)]
public sealed class OutboxRabbitMqIntegrationTests
{
    private readonly MessagingIntegrationFixture
        _fixture;

    public OutboxRabbitMqIntegrationTests(
        MessagingIntegrationFixture fixture)
    {
        _fixture =
            fixture;
    }

    [Fact]
    public async Task ProcessAsync_WithPendingOutboxMessage_ShouldPublishToRabbitMqAndMarkAsProcessed()
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
                        _fixture.RabbitMq
                            .ConnectionString)
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

        var message =
            OutboxMessage.Create(
                messageId,
                DateTime.UtcNow,
                messageType,
                payload);

        using (var seedScope =
            _fixture.SqlServer.Factory.Services
                .CreateScope())
        {
            var seedDbContext =
                seedScope.ServiceProvider
                    .GetRequiredService<
                        ApplicationDbContext>();

            seedDbContext.OutboxMessages.Add(
                message);

            await seedDbContext.SaveChangesAsync();
        }

        var settings =
            new RabbitMqSettings
            {
                ConnectionString =
                    _fixture.RabbitMq
                        .ConnectionString,

                ExchangeName =
                    exchangeName
            };

        var publisher =
            new RabbitMqIntegrationEventPublisher(
                Options.Create(
                    settings));

        using (var processScope =
            _fixture.SqlServer.Factory.Services
                .CreateScope())
        {
            var dbContext =
                processScope.ServiceProvider
                    .GetRequiredService<
                        ApplicationDbContext>();

            var processor =
                new OutboxProcessor(
                    dbContext,
                    publisher,
                    NullLogger<
                        OutboxProcessor>.Instance);

            var processedCount =
                await processor.ProcessAsync(
                    batchSize: 1);

            Assert.Equal(
                1,
                processedCount);
        }

        var result =
            await channel.BasicGetAsync(
                queueName,
                autoAck: true);

        Assert.NotNull(
            result);

        Assert.Equal(
            messageId.ToString(),
            result.BasicProperties.MessageId);

        Assert.Equal(
            messageType,
            result.BasicProperties.Type);

        Assert.Equal(
            messageType,
            result.RoutingKey);

        var receivedPayload =
            Encoding.UTF8.GetString(
                result.Body.Span);

        Assert.Equal(
            payload,
            receivedPayload);

        using var assertScope =
            _fixture.SqlServer.Factory.Services
                .CreateScope();

        var assertDbContext =
            assertScope.ServiceProvider
                .GetRequiredService<
                    ApplicationDbContext>();

        var persistedMessage =
            await assertDbContext.OutboxMessages
                .AsNoTracking()
                .SingleAsync(
                    x => x.Id == messageId);

        Assert.NotNull(
            persistedMessage.ProcessedOnUtc);

        Assert.Null(
            persistedMessage.Error);
    }
}