namespace NexusERP.IntegrationTests.Infrastructure;

public sealed class MessagingIntegrationFixture
    : IAsyncLifetime
{
    public SqlServerFixture SqlServer { get; } =
        new();

    public RabbitMqFixture RabbitMq { get; } =
        new();

    public IntegrationTestFactory Factory
    {
        get;
        private set;
    } = null!;

    public async Task InitializeAsync()
    {
        await SqlServer.InitializeAsync();

        try
        {
            await RabbitMq.InitializeAsync();

            Factory =
                new IntegrationTestFactory(
                    SqlServer.ConnectionString,
                    RabbitMq.ConnectionString);
        }
        catch
        {
            await SqlServer.DisposeAsync();

            throw;
        }
    }

    public async Task DisposeAsync()
    {
        if (Factory is not null)
        {
            await Factory.DisposeAsync();
        }

        await RabbitMq.DisposeAsync();

        await SqlServer.DisposeAsync();
    }
}