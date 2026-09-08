namespace NexusERP.IntegrationTests.Infrastructure;

[CollectionDefinition(Name)]
public sealed class IntegrationTestCollection
    : ICollectionFixture<SqlServerFixture>
{
    public const string Name =
        "NexusERP Integration Tests";
}