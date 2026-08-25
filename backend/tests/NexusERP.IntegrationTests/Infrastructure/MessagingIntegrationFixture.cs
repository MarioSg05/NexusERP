namespace NexusERP.IntegrationTests.Infrastructure;

public sealed class MessagingIntegrationFixture
    : IAsyncLifetime
{
    public SqlServerFixture SqlServer { get; } =
        new();

    public RabbitMqFixture RabbitMq { get; } =
        new();

    public async Task InitializeAsync()
    {
        await SqlServer.InitializeAsync();

        try
        {
            await RabbitMq.InitializeAsync();
        }
        catch
        {
            await SqlServer.DisposeAsync();

            throw;
        }
    }

    public async Task DisposeAsync()
    {
        await RabbitMq.DisposeAsync();

        await SqlServer.DisposeAsync();
    }
}