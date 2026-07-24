using Microsoft.EntityFrameworkCore;
using NexusERP.Domain.Customers.Aggregates;
using NexusERP.Domain.Identity.Aggregates;
using NexusERP.Domain.Products.Aggregates;
using NexusERP.Domain.Inventory.Aggregates;
using NexusERP.Domain.Suppliers.Aggregates;

namespace NexusERP.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Customer> Customers { get; }
    DbSet<Product> Products { get; }
    DbSet<InventoryItem> Inventories { get; }
    DbSet<Supplier> Suppliers { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}