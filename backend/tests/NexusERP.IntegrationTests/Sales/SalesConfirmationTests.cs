using System.Net;
using System.Net.Http.Json;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using NexusERP.Application.Sales.ConfirmSalesOrder;
using NexusERP.Domain.Customers.Aggregates;
using NexusERP.Domain.Customers.Enums;
using NexusERP.Domain.Customers.ValueObjects;
using NexusERP.Domain.Identity.Enums;
using NexusERP.Domain.Inventory.Aggregates;
using NexusERP.Domain.Inventory.ValueObjects;
using NexusERP.Domain.Products.Aggregates;
using NexusERP.Domain.Products.ValueObjects;
using NexusERP.Domain.Sales.Aggregates;
using NexusERP.Domain.Sales.Entities;
using NexusERP.Domain.Sales.Enums;
using NexusERP.Domain.Sales.ValueObjects;
using NexusERP.Infrastructure.Persistence;
using NexusERP.IntegrationTests.Infrastructure;
using System.Text.Json;

namespace NexusERP.IntegrationTests.Sales;

[Collection(IntegrationTestCollection.Name)]
public sealed class SalesConfirmationTests
{
    private readonly SqlServerFixture _sqlServer;

    public SalesConfirmationTests(
        SqlServerFixture sqlServer)
    {
        _sqlServer = sqlServer;
    }

    [Fact]
    public async Task ConfirmSalesOrder_WithSufficientStock_ShouldConfirmOrderAndDecreaseInventory()
    {
        var factory =
            _sqlServer.Factory;

        var scenario =
            await CreateSalesScenarioAsync(
                factory,
                inventoryQuantity: 10,
                salesQuantity: 3);

        using var client =
            await TestAuthentication
                .CreateAuthenticatedClientAsync(
                    factory,
                    UserRole.Manager);

        using var response =
            await client.PostAsync(
                $"/api/sales-orders/{scenario.SalesOrderId}/confirm",
                content: null);

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var result =
            await response.Content
                .ReadFromJsonAsync<ConfirmSalesOrderResponse>();

        Assert.NotNull(result);

        Assert.Equal(
            scenario.SalesOrderId,
            result.Id);

        Assert.Equal(
            SalesOrderStatus.Confirmed.ToString(),
            result.Status);

        using var assertScope =
            factory.Services.CreateScope();

        var dbContext =
            assertScope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

        var persistedOrder =
            await dbContext.SalesOrders
                .AsNoTracking()
                .SingleAsync(
                    x => x.Id ==
                        scenario.SalesOrderId);

        var persistedInventory =
            await dbContext.Inventories
                .AsNoTracking()
                .SingleAsync(
                    x => x.ProductId ==
                        scenario.ProductId);

        var outboxMessage =
            await dbContext.OutboxMessages
                .AsNoTracking()
                .SingleAsync(
                    x =>
                        x.Type ==
                            "sales-order-confirmed" &&
                        x.Payload.Contains(
                            scenario.SalesOrderId.ToString()));

        Assert.Equal(
            SalesOrderStatus.Confirmed,
            persistedOrder.Status);

        Assert.Equal(
            7,
            persistedInventory.Quantity.Value);

        Assert.NotEqual(
    Guid.Empty,
    outboxMessage.Id);

        Assert.Equal(
            "sales-order-confirmed",
            outboxMessage.Type);

        Assert.Null(
            outboxMessage.ProcessedOnUtc);

        Assert.Null(
            outboxMessage.Error);

        using var payload =
            JsonDocument.Parse(
                outboxMessage.Payload);

        var payloadRoot =
            payload.RootElement;

        Assert.Equal(
            outboxMessage.Id,
            payloadRoot
                .GetProperty("id")
                .GetGuid());

        Assert.Equal(
            scenario.SalesOrderId,
            payloadRoot
                .GetProperty("salesOrderId")
                .GetGuid());

        Assert.True(
            payloadRoot.TryGetProperty(
                "occurredOnUtc",
                out _));
    }

    [Fact]
    public async Task ConfirmSalesOrder_WithInsufficientStock_ShouldLeaveOrderAndInventoryUnchanged()
    {
        var factory =
            _sqlServer.Factory;

        var scenario =
            await CreateSalesScenarioAsync(
                factory,
                inventoryQuantity: 2,
                salesQuantity: 3);

        using var client =
            await TestAuthentication
                .CreateAuthenticatedClientAsync(
                    factory,
                    UserRole.Manager);

        using var response =
            await client.PostAsync(
                $"/api/sales-orders/{scenario.SalesOrderId}/confirm",
                content: null);

        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);

        using var assertScope =
            factory.Services.CreateScope();

        var dbContext =
            assertScope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

        var persistedOrder =
            await dbContext.SalesOrders
                .AsNoTracking()
                .SingleAsync(
                    x => x.Id ==
                        scenario.SalesOrderId);

        var persistedInventory =
            await dbContext.Inventories
                .AsNoTracking()
                .SingleAsync(
                    x => x.ProductId ==
                        scenario.ProductId);

        var outboxMessageExists =
            await dbContext.OutboxMessages
                .AsNoTracking()
                .AnyAsync(
                    x =>
                        x.Type ==
                            "sales-order-confirmed" &&
                        x.Payload.Contains(
                            scenario.SalesOrderId.ToString()));

        Assert.False(
            outboxMessageExists);

        Assert.Equal(
            SalesOrderStatus.Pending,
            persistedOrder.Status);

        Assert.Equal(
            2,
            persistedInventory.Quantity.Value);
    }

    [Fact]
    public async Task ConfirmSalesOrder_WhenOneLineHasInsufficientStock_ShouldNotApplyPartialChanges()
    {
        var factory =
            _sqlServer.Factory;

        var scenario =
            await CreateMultiItemSalesScenarioAsync(
                factory);

        using var client =
            await TestAuthentication
                .CreateAuthenticatedClientAsync(
                    factory,
                    UserRole.Manager);

        using var response =
            await client.PostAsync(
                $"/api/sales-orders/{scenario.SalesOrderId}/confirm",
                content: null);

        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);

        using var assertScope =
            factory.Services.CreateScope();

        var dbContext =
            assertScope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

        var persistedOrder =
            await dbContext.SalesOrders
                .AsNoTracking()
                .SingleAsync(
                    x => x.Id ==
                        scenario.SalesOrderId);

        var firstInventory =
            await dbContext.Inventories
                .AsNoTracking()
                .SingleAsync(
                    x => x.ProductId ==
                        scenario.FirstProductId);

        var secondInventory =
            await dbContext.Inventories
                .AsNoTracking()
                .SingleAsync(
                    x => x.ProductId ==
                        scenario.SecondProductId);

        var outboxMessageExists =
            await dbContext.OutboxMessages
                .AsNoTracking()
                .AnyAsync(
                    x =>
                        x.Type ==
                            "sales-order-confirmed" &&
                        x.Payload.Contains(
                            scenario.SalesOrderId.ToString()));

        Assert.False(
            outboxMessageExists);

        Assert.Equal(
            SalesOrderStatus.Pending,
            persistedOrder.Status);

        Assert.Equal(
            10,
            firstInventory.Quantity.Value);

        Assert.Equal(
            2,
            secondInventory.Quantity.Value);
    }

    private static async Task<SalesScenario>
        CreateSalesScenarioAsync(
            IntegrationTestFactory factory,
            int inventoryQuantity,
            int salesQuantity)
    {
        using var scope =
            factory.Services.CreateScope();

        var dbContext =
            scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

        var uniqueValue =
            Guid.NewGuid()
                .ToString("N");

        var customer =
            Customer.Register(
                new CustomerName(
                    $"Sales Customer {uniqueValue[..8]}"),
                new CustomerEmail(
                    $"sales-{uniqueValue}@nexuserp.test"),
                new CustomerPhone(
                    "+50255550101"),
                CustomerType.Corporate);

        var product =
            Product.Register(
                new ProductName(
                    $"Sales Product {uniqueValue[..8]}"),
                new ProductSku(
                    $"SALES-{uniqueValue[..12]}"),
                new ProductPrice(
                    100.00m));

        var inventory =
            InventoryItem.Create(
                product.Id,
                new InventoryQuantity(
                    inventoryQuantity));

        var salesOrder =
            SalesOrder.Create(
                customer.Id);

        salesOrder.AddItem(
            SalesOrderItem.Create(
                product.Id,
                new SalesQuantity(
                    salesQuantity),
                new SalesUnitPrice(
                    100.00m)));

        dbContext.Customers.Add(
            customer);

        dbContext.Products.Add(
            product);

        dbContext.Inventories.Add(
            inventory);

        dbContext.SalesOrders.Add(
            salesOrder);

        await dbContext.SaveChangesAsync();

        return new SalesScenario(
            salesOrder.Id,
            product.Id);
    }

    private static async Task<MultiItemSalesScenario>
        CreateMultiItemSalesScenarioAsync(
            IntegrationTestFactory factory)
    {
        using var scope =
            factory.Services.CreateScope();

        var dbContext =
            scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

        var uniqueValue =
            Guid.NewGuid()
                .ToString("N");

        var customer =
            Customer.Register(
                new CustomerName(
                    $"Multi Sales Customer {uniqueValue[..8]}"),
                new CustomerEmail(
                    $"multi-sales-{uniqueValue}@nexuserp.test"),
                new CustomerPhone(
                    "+50255550102"),
                CustomerType.Corporate);

        var firstProduct =
            Product.Register(
                new ProductName(
                    $"First Sales Product {uniqueValue[..8]}"),
                new ProductSku(
                    $"FIRST-{uniqueValue[..12]}"),
                new ProductPrice(
                    100.00m));

        var secondProduct =
            Product.Register(
                new ProductName(
                    $"Second Sales Product {uniqueValue[..8]}"),
                new ProductSku(
                    $"SECOND-{uniqueValue[..12]}"),
                new ProductPrice(
                    200.00m));

        var firstInventory =
            InventoryItem.Create(
                firstProduct.Id,
                new InventoryQuantity(
                    10));

        var secondInventory =
            InventoryItem.Create(
                secondProduct.Id,
                new InventoryQuantity(
                    2));

        var salesOrder =
            SalesOrder.Create(
                customer.Id);

        salesOrder.AddItem(
            SalesOrderItem.Create(
                firstProduct.Id,
                new SalesQuantity(
                    3),
                new SalesUnitPrice(
                    100.00m)));

        salesOrder.AddItem(
            SalesOrderItem.Create(
                secondProduct.Id,
                new SalesQuantity(
                    5),
                new SalesUnitPrice(
                    200.00m)));

        dbContext.Customers.Add(
            customer);

        dbContext.Products.AddRange(
            firstProduct,
            secondProduct);

        dbContext.Inventories.AddRange(
            firstInventory,
            secondInventory);

        dbContext.SalesOrders.Add(
            salesOrder);

        await dbContext.SaveChangesAsync();

        return new MultiItemSalesScenario(
            salesOrder.Id,
            firstProduct.Id,
            secondProduct.Id);
    }

    private sealed record SalesScenario(
        Guid SalesOrderId,
        Guid ProductId);

    private sealed record MultiItemSalesScenario(
        Guid SalesOrderId,
        Guid FirstProductId,
        Guid SecondProductId);
}