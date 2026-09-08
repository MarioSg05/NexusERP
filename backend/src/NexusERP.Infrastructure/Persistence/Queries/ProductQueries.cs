using Microsoft.EntityFrameworkCore;

using NexusERP.Application.Common.Interfaces;
using NexusERP.Application.Products.GetProducts;

namespace NexusERP.Infrastructure.Persistence.Queries;

public sealed class ProductQueries
    : IProductQueries
{
    private readonly ApplicationDbContext _context;

    public ProductQueries(
        ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<ProductListItem>>
        GetProductsAsync(
            CancellationToken cancellationToken = default)
    {
        var products = await _context.Products
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);

        return products
            .Select(product => new ProductListItem
            {
                Id = product.Id,
                Name = product.Name.Value,
                Sku = product.Sku.Value,
                Price = product.Price.Value,
                IsActive = product.IsActive
            })
            .ToList();
    }
    public async Task<ProductListItem?>
    GetProductByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var product = await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);

        if (product is null)
            return null;

        return new ProductListItem
        {
            Id = product.Id,
            Name = product.Name.Value,
            Sku = product.Sku.Value,
            Price = product.Price.Value,
            IsActive = product.IsActive
        };
    }
}