using Microsoft.EntityFrameworkCore;

using NexusERP.Application.Common.DomainEvents;
using NexusERP.Domain.Common;
using NexusERP.Domain.Customers.Aggregates;
using NexusERP.Domain.Customers.Enums;
using NexusERP.Domain.Customers.Events;
using NexusERP.Domain.Customers.ValueObjects;
using NexusERP.Infrastructure.Persistence;
using NexusERP.IntegrationTests.Infrastructure;

namespace NexusERP.IntegrationTests.DomainEvents;

[Collection(IntegrationTestCollection.Name)]
public sealed class DomainEventPersistenceTests
{
    private readonly SqlServerFixture _sqlServer;

    public DomainEventPersistenceTests(
        SqlServerFixture sqlServer)
    {
        _sqlServer = sqlServer;
    }

    [Fact]
    public async Task SaveChangesAsync_WhenPersistenceSucceeds_ShouldDispatchAndClearDomainEvents()
    {
        var dispatcher =
            new CapturingDomainEventDispatcher();

        await using var dbContext =
            CreateDbContext(
                dispatcher);

        var customer =
            CreateCustomer();

        dbContext.Customers.Add(
            customer);

        Assert.Single(
            customer.DomainEvents);

        await dbContext.SaveChangesAsync();

        Assert.Empty(
            customer.DomainEvents);

        var domainEvent =
            Assert.Single(
                dispatcher.DispatchedEvents);

        var customerRegisteredEvent =
            Assert.IsType<CustomerRegisteredEvent>(
                domainEvent);

        Assert.Equal(
            customer.Id,
            customerRegisteredEvent.CustomerId);

        var customerExists =
            await dbContext.Customers
                .AsNoTracking()
                .AnyAsync(
                    x => x.Id == customer.Id);

        Assert.True(
            customerExists);
    }

    [Fact]
    public async Task SaveChangesAsync_WhenPersistenceFails_ShouldNotDispatchOrClearDomainEvents()
    {
        var duplicateEmail =
            $"duplicate-{Guid.NewGuid():N}@nexuserp.test";

        await SeedCustomerAsync(
            duplicateEmail);

        var dispatcher =
            new CapturingDomainEventDispatcher();

        await using var dbContext =
            CreateDbContext(
                dispatcher);

        var customer =
            CreateCustomer(
                duplicateEmail);

        dbContext.Customers.Add(
            customer);

        Assert.Single(
            customer.DomainEvents);

        await Assert.ThrowsAsync<
            DbUpdateException>(
                () =>
                    dbContext.SaveChangesAsync());

        Assert.Empty(
            dispatcher.DispatchedEvents);

        var pendingEvent =
            Assert.Single(
                customer.DomainEvents);

        var customerRegisteredEvent =
            Assert.IsType<CustomerRegisteredEvent>(
                pendingEvent);

        Assert.Equal(
            customer.Id,
            customerRegisteredEvent.CustomerId);
    }

    private ApplicationDbContext CreateDbContext(
        IDomainEventDispatcher dispatcher)
    {
        var options =
            new DbContextOptionsBuilder<
                ApplicationDbContext>()
                .UseSqlServer(
                    _sqlServer.ConnectionString)
                .Options;

        return new ApplicationDbContext(
            options,
            dispatcher);
    }

    private async Task SeedCustomerAsync(
        string email)
    {
        var dispatcher =
            new CapturingDomainEventDispatcher();

        await using var dbContext =
            CreateDbContext(
                dispatcher);

        var customer =
            CreateCustomer(
                email);

        dbContext.Customers.Add(
            customer);

        await dbContext.SaveChangesAsync();
    }

    private static Customer CreateCustomer(
        string? email = null)
    {
        var uniqueValue =
            Guid.NewGuid()
                .ToString("N");

        return Customer.Register(
            new CustomerName(
                $"Domain Event Customer {uniqueValue[..8]}"),
            new CustomerEmail(
                email ??
                $"domain-event-{uniqueValue}@nexuserp.test"),
            new CustomerPhone(
                "+50255550103"),
            CustomerType.Corporate);
    }

    private sealed class CapturingDomainEventDispatcher
        : IDomainEventDispatcher
    {
        private readonly List<IDomainEvent>
            _dispatchedEvents = [];

        public IReadOnlyCollection<IDomainEvent>
            DispatchedEvents =>
                _dispatchedEvents.AsReadOnly();

        public Task DispatchAsync(
            IReadOnlyCollection<IDomainEvent> domainEvents,
            CancellationToken cancellationToken = default)
        {
            _dispatchedEvents.AddRange(
                domainEvents);

            return Task.CompletedTask;
        }
    }
}