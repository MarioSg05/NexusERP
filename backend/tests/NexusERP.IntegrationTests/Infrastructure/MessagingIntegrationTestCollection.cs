namespace NexusERP.IntegrationTests.Infrastructure;

[CollectionDefinition(Name)]
public sealed class MessagingIntegrationTestCollection
    : ICollectionFixture<MessagingIntegrationFixture>
{
    public const string Name =
        "NexusERP Messaging Integration Tests";
}