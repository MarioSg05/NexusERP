using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using NexusERP.Application;
using NexusERP.Application.Common.DomainEvents;
using NexusERP.Domain.Customers.Aggregates;
using NexusERP.Domain.Customers.Enums;
using NexusERP.Domain.Customers.Events;
using NexusERP.Domain.Customers.ValueObjects;
using NexusERP.Infrastructure.Persistence;
using NexusERP.IntegrationTests.Infrastructure;
using NexusERP.Infrastructure.Messaging.Outbox;

namespace NexusERP.IntegrationTests.DomainEvents;

[Collection(IntegrationTestCollection.Name)]
public sealed class DomainEventDependencyInjectionTests
{
    private readonly SqlServerFixture _sqlServer;

    public DomainEventDependencyInjectionTests(
        SqlServerFixture sqlServer)
    {
        _sqlServer = sqlServer;
    }

    [Fact]
    public async Task SaveChangesAsync_WithRealDispatcher_ShouldResolveAndInvokeRegisteredHandler()
    {
        var services =
            new ServiceCollection();

        services.AddApplication();

        services.AddScoped<OutboxMessageFactory>();

        services.AddDbContext<ApplicationDbContext>(
            options =>
                options.UseSqlServer(
                    _sqlServer.ConnectionString));

        services.AddScoped<
            CapturingCustomerRegisteredEventHandler>();

        services.AddScoped<
            IDomainEventHandler<CustomerRegisteredEvent>>(
                provider =>
                    provider.GetRequiredService<
                        CapturingCustomerRegisteredEventHandler>());

        await using var serviceProvider =
            services.BuildServiceProvider();

        await using var scope =
            serviceProvider.CreateAsyncScope();

        var dbContext =
            scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

        var handler =
            scope.ServiceProvider
                .GetRequiredService<
                    CapturingCustomerRegisteredEventHandler>();

        var uniqueValue =
            Guid.NewGuid()
                .ToString("N");

        var customer =
            Customer.Register(
                new CustomerName(
                    $"DI Customer {uniqueValue[..8]}"),
                new CustomerEmail(
                    $"di-domain-event-{uniqueValue}@nexuserp.test"),
                new CustomerPhone(
                    "+50255550104"),
                CustomerType.Corporate);

        dbContext.Customers.Add(
            customer);

        Assert.Single(
            customer.DomainEvents);

        await dbContext.SaveChangesAsync();

        Assert.Empty(
            customer.DomainEvents);

        Assert.Equal(
            1,
            handler.InvocationCount);

        Assert.NotNull(
            handler.LastEvent);

        Assert.Equal(
            customer.Id,
            handler.LastEvent.CustomerId);

        var persisted =
            await dbContext.Customers
                .AsNoTracking()
                .AnyAsync(
                    x => x.Id == customer.Id);

        Assert.True(
            persisted);
    }

    private sealed class
        CapturingCustomerRegisteredEventHandler
        : IDomainEventHandler<CustomerRegisteredEvent>
    {
        public int InvocationCount
        {
            get;
            private set;
        }

        public CustomerRegisteredEvent? LastEvent
        {
            get;
            private set;
        }

        public Task Handle(
            CustomerRegisteredEvent domainEvent,
            CancellationToken cancellationToken = default)
        {
            InvocationCount++;

            LastEvent =
                domainEvent;

            return Task.CompletedTask;
        }
    }
}