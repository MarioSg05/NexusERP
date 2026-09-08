using Testcontainers.RabbitMq;

namespace NexusERP.IntegrationTests.Infrastructure;

public sealed class RabbitMqFixture
    : IAsyncLifetime
{
    private readonly RabbitMqContainer _container =
        new RabbitMqBuilder(
            "rabbitmq:4.1-management")
            .Build();

    public string ConnectionString =>
        _container.GetConnectionString();

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _container.DisposeAsync();
    }
}