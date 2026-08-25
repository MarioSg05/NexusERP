using NexusERP.Application.Sales.IntegrationEvents;

namespace NexusERP.UnitTests.Application.Sales.IntegrationEvents;

public sealed class SalesOrderConfirmedIntegrationEventHandlerTests
{
    [Fact]
    public async Task HandleAsync_WithValidEvent_ShouldComplete()
    {
        var integrationEvent =
            new SalesOrderConfirmedIntegrationEvent(
                Guid.NewGuid(),
                DateTime.UtcNow,
                Guid.NewGuid());

        var handler =
            new SalesOrderConfirmedIntegrationEventHandler();

        await handler.HandleAsync(
            integrationEvent);
    }

    [Fact]
    public async Task HandleAsync_WithNullEvent_ShouldThrow()
    {
        var handler =
            new SalesOrderConfirmedIntegrationEventHandler();

        await Assert.ThrowsAsync<
            ArgumentNullException>(
                () =>
                    handler.HandleAsync(
                        null!));
    }
}