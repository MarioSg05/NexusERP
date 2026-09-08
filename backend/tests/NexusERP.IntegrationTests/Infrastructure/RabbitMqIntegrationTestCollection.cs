namespace NexusERP.IntegrationTests.Infrastructure;

[CollectionDefinition(Name)]
public sealed class RabbitMqIntegrationTestCollection
    : ICollectionFixture<RabbitMqFixture>
{
    public const string Name =
        "NexusERP RabbitMQ Integration Tests";
}