using Microsoft.EntityFrameworkCore;
using NexusERP.Application.Common.Interfaces;
using NexusERP.Domain.Customers.Aggregates;
using NexusERP.Domain.Identity.Aggregates;
using NexusERP.Domain.Products.Aggregates;
using NexusERP.Domain.Inventory.Aggregates;
using NexusERP.Domain.Suppliers.Aggregates;
using NexusERP.Domain.Purchasing.Aggregates;
using NexusERP.Domain.Sales.Aggregates;

namespace NexusERP.Infrastructure.Persistence;

public sealed class ApplicationDbContext
    : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<User> Users => Set<User>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<InventoryItem> Inventories => Set<InventoryItem>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<SalesOrder> SalesOrders => Set<SalesOrder>();
    public override Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}