using NexusERP.Application.Sales.IntegrationEvents;
using NexusERP.Domain.Sales.Events;

namespace NexusERP.UnitTests.Application.Sales.IntegrationEvents;

public sealed class SalesOrderConfirmedIntegrationEventMapperTests
{
    [Fact]
    public void Map_ShouldCreateSalesOrderConfirmedIntegrationEvent()
    {
        var salesOrderId =
            Guid.NewGuid();

        var domainEvent =
            new SalesOrderConfirmedEvent(
                salesOrderId);

        var mapper =
            new SalesOrderConfirmedIntegrationEventMapper();

        var result =
            mapper.Map(
                domainEvent);

        var integrationEvent =
            Assert.IsType<
                SalesOrderConfirmedIntegrationEvent>(
                    result);

        Assert.NotEqual(
            Guid.Empty,
            integrationEvent.Id);

        Assert.Equal(
            domainEvent.OccurredOn,
            integrationEvent.OccurredOnUtc);

        Assert.Equal(
            salesOrderId,
            integrationEvent.SalesOrderId);

        Assert.Equal(
            "sales-order-confirmed",
            integrationEvent.Type);
    }
}