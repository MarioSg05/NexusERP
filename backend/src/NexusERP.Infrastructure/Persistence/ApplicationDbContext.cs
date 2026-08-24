using Microsoft.EntityFrameworkCore;

using NexusERP.Application.Common.DomainEvents;
using NexusERP.Application.Common.Interfaces;
using NexusERP.Domain.Common;
using NexusERP.Domain.Customers.Aggregates;
using NexusERP.Domain.Identity.Aggregates;
using NexusERP.Domain.Inventory.Aggregates;
using NexusERP.Domain.Products.Aggregates;
using NexusERP.Domain.Purchasing.Aggregates;
using NexusERP.Domain.Sales.Aggregates;
using NexusERP.Domain.Suppliers.Aggregates;

namespace NexusERP.Infrastructure.Persistence;

public sealed class ApplicationDbContext
    : DbContext, IApplicationDbContext
{
    private readonly IDomainEventDispatcher
        _domainEventDispatcher;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IDomainEventDispatcher domainEventDispatcher)
        : base(options)
    {
        _domainEventDispatcher =
            domainEventDispatcher;
    }

    public DbSet<User> Users =>
        Set<User>();

    public DbSet<Customer> Customers =>
        Set<Customer>();

    public DbSet<PurchaseOrder> PurchaseOrders =>
        Set<PurchaseOrder>();

    public DbSet<Product> Products =>
        Set<Product>();

    public DbSet<InventoryItem> Inventories =>
        Set<InventoryItem>();

    public DbSet<Supplier> Suppliers =>
        Set<Supplier>();

    public DbSet<SalesOrder> SalesOrders =>
        Set<SalesOrder>();

    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        var aggregates =
            ChangeTracker
                .Entries<AggregateRoot>()
                .Where(
                    entry =>
                        entry.Entity.DomainEvents.Count > 0)
                .Select(
                    entry => entry.Entity)
                .ToList();

        var domainEvents =
            aggregates
                .SelectMany(
                    aggregate =>
                        aggregate.DomainEvents)
                .ToList();

        var result =
            await base.SaveChangesAsync(
                cancellationToken);

        foreach (var aggregate in aggregates)
        {
            aggregate.ClearDomainEvents();
        }

        if (domainEvents.Count > 0)
        {
            await _domainEventDispatcher
                .DispatchAsync(
                    domainEvents,
                    cancellationToken);
        }

        return result;
    }

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(
            modelBuilder);
    }
}
