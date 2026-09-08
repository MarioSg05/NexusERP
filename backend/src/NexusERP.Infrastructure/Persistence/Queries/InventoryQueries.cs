using Microsoft.EntityFrameworkCore;

using NexusERP.Application.Common.Interfaces;
using NexusERP.Application.Inventory.GetInventory;

namespace NexusERP.Infrastructure.Persistence.Queries;

public sealed class InventoryQueries
    : IInventoryQueries
{
    private readonly ApplicationDbContext _context;

    public InventoryQueries(
        ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<InventoryListItem>>
        GetInventoryAsync(
            CancellationToken cancellationToken = default)
    {
        var rows = await (
            from inventory in _context.Inventories
                .AsNoTracking()
            join product in _context.Products
                .AsNoTracking()
                on inventory.ProductId equals product.Id
            orderby product.Name
            select new
            {
                Inventory = inventory,
                Product = product
            })
            .ToListAsync(cancellationToken);

        return rows
            .Select(row => new InventoryListItem
            {
                Id = row.Inventory.Id,
                ProductId = row.Inventory.ProductId,
                ProductName = row.Product.Name.Value,
                Sku = row.Product.Sku.Value,
                Quantity = row.Inventory.Quantity.Value,
                IsActive = row.Inventory.IsActive
            })
            .ToList();
    }
}