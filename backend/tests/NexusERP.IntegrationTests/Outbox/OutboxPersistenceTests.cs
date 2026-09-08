using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using NexusERP.Domain.Customers.Aggregates;
using NexusERP.Domain.Customers.Enums;
using NexusERP.Domain.Customers.ValueObjects;
using NexusERP.Infrastructure.Messaging.Outbox;
using NexusERP.Infrastructure.Persistence;
using NexusERP.IntegrationTests.Infrastructure;

namespace NexusERP.IntegrationTests.Outbox;

[Collection(IntegrationTestCollection.Name)]
public sealed class OutboxPersistenceTests
{
    private readonly SqlServerFixture _sqlServer;

    public OutboxPersistenceTests(
        SqlServerFixture sqlServer)
    {
        _sqlServer = sqlServer;
    }

    [Fact]
    public async Task OutboxMessage_ShouldPersist()
    {
        var factory =
            _sqlServer.Factory;

        var message =
            OutboxMessage.Create(
                Guid.NewGuid(),
                DateTime.UtcNow,
                "TestIntegrationEvent",
                """
                {"value":"test"}
                """);

        using (var scope =
            factory.Services.CreateScope())
        {
            var dbContext =
                scope.ServiceProvider
                    .GetRequiredService<ApplicationDbContext>();

            dbContext.OutboxMessages.Add(
                message);

            await dbContext.SaveChangesAsync();
        }

        using var assertScope =
            factory.Services.CreateScope();

        var assertDbContext =
            assertScope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

        var persistedMessage =
            await assertDbContext.OutboxMessages
                .AsNoTracking()
                .SingleAsync(
                    x => x.Id == message.Id);

        Assert.Equal(
            message.Id,
            persistedMessage.Id);

        Assert.Equal(
            "TestIntegrationEvent",
            persistedMessage.Type);

        Assert.Equal(
            """{"value":"test"}""",
            persistedMessage.Payload);

        Assert.Null(
            persistedMessage.ProcessedOnUtc);

        Assert.Null(
            persistedMessage.Error);
    }

    [Fact]
    public async Task BusinessDataAndOutboxMessage_ShouldPersistTogether()
    {
        var factory =
            _sqlServer.Factory;

        var customer =
            CreateCustomer();

        var message =
            OutboxMessage.Create(
                Guid.NewGuid(),
                DateTime.UtcNow,
                "CustomerRegisteredIntegrationEvent",
                $$"""
                {"customerId":"{{customer.Id}}"}
                """);

        using (var scope =
            factory.Services.CreateScope())
        {
            var dbContext =
                scope.ServiceProvider
                    .GetRequiredService<ApplicationDbContext>();

            dbContext.Customers.Add(
                customer);

            dbContext.OutboxMessages.Add(
                message);

            await dbContext.SaveChangesAsync();
        }

        using var assertScope =
            factory.Services.CreateScope();

        var assertDbContext =
            assertScope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

        var customerExists =
            await assertDbContext.Customers
                .AsNoTracking()
                .AnyAsync(
                    x => x.Id == customer.Id);

        var messageExists =
            await assertDbContext.OutboxMessages
                .AsNoTracking()
                .AnyAsync(
                    x => x.Id == message.Id);

        Assert.True(
            customerExists);

        Assert.True(
            messageExists);
    }

    [Fact]
    public async Task WhenBusinessPersistenceFails_OutboxMessage_ShouldRollback()
    {
        var factory =
            _sqlServer.Factory;

        var duplicateEmail =
            $"outbox-duplicate-{Guid.NewGuid():N}@nexuserp.test";

        var existingCustomer =
            CreateCustomer(
                duplicateEmail);

        using (var seedScope =
            factory.Services.CreateScope())
        {
            var seedDbContext =
                seedScope.ServiceProvider
                    .GetRequiredService<ApplicationDbContext>();

            seedDbContext.Customers.Add(
                existingCustomer);

            await seedDbContext.SaveChangesAsync();
        }

        var duplicateCustomer =
            CreateCustomer(
                duplicateEmail);

        var message =
            OutboxMessage.Create(
                Guid.NewGuid(),
                DateTime.UtcNow,
                "CustomerRegisteredIntegrationEvent",
                $$"""
                {"customerId":"{{duplicateCustomer.Id}}"}
                """);

        using (var actScope =
            factory.Services.CreateScope())
        {
            var dbContext =
                actScope.ServiceProvider
                    .GetRequiredService<ApplicationDbContext>();

            dbContext.Customers.Add(
                duplicateCustomer);

            dbContext.OutboxMessages.Add(
                message);

            await Assert.ThrowsAsync<DbUpdateException>(
                () =>
                    dbContext.SaveChangesAsync());
        }

        using var assertScope =
            factory.Services.CreateScope();

        var assertDbContext =
            assertScope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

        var duplicateCustomerExists =
            await assertDbContext.Customers
                .AsNoTracking()
                .AnyAsync(
                    x => x.Id ==
                        duplicateCustomer.Id);

        var messageExists =
            await assertDbContext.OutboxMessages
                .AsNoTracking()
                .AnyAsync(
                    x => x.Id == message.Id);

        Assert.False(
            duplicateCustomerExists);

        Assert.False(
            messageExists);
    }

    private static Customer CreateCustomer(
        string? email = null)
    {
        var uniqueValue =
            Guid.NewGuid()
                .ToString("N");

        return Customer.Register(
            new CustomerName(
                $"Outbox Customer {uniqueValue[..8]}"),
            new CustomerEmail(
                email ??
                $"outbox-{uniqueValue}@nexuserp.test"),
            new CustomerPhone(
                "+50255550105"),
            CustomerType.Corporate);
    }
}