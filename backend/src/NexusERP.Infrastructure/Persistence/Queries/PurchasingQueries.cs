using Microsoft.EntityFrameworkCore;

using NexusERP.Application.Common.Interfaces;
using NexusERP.Application.Purchasing.GetPurchaseOrders;
using NexusERP.Application.Purchasing.GetPurchaseOrderById;

namespace NexusERP.Infrastructure.Persistence.Queries;

public sealed class PurchasingQueries
    : IPurchasingQueries
{
    private readonly ApplicationDbContext _context;

    public PurchasingQueries(
        ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<PurchaseOrderListItem>>
        GetPurchaseOrdersAsync(
            CancellationToken cancellationToken = default)
    {
        var rows = await (
            from purchaseOrder in _context.PurchaseOrders
                .AsNoTracking()
            join supplier in _context.Suppliers
                .AsNoTracking()
                on purchaseOrder.SupplierId equals supplier.Id
            orderby purchaseOrder.OrderDate descending
            select new
            {
                PurchaseOrder = purchaseOrder,
                Supplier = supplier
            })
            .ToListAsync(cancellationToken);

        return rows
            .Select(row => new PurchaseOrderListItem
            {
                Id = row.PurchaseOrder.Id,
                SupplierId = row.PurchaseOrder.SupplierId,
                SupplierName = row.Supplier.Name.Value,
                OrderDate = row.PurchaseOrder.OrderDate,
                Status = row.PurchaseOrder.Status.ToString(),
                Total = row.PurchaseOrder.Total.Value
            })
            .ToList();
    }

    public async Task<PurchaseOrderDetail?>
    GetPurchaseOrderByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var purchaseOrder = await _context.PurchaseOrders
            .AsNoTracking()
            .Include(x => x.Items)
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);

        if (purchaseOrder is null)
            return null;

        var supplier = await _context.Suppliers
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == purchaseOrder.SupplierId,
                cancellationToken);

        if (supplier is null)
            return null;

        var productIds = purchaseOrder.Items
            .Select(x => x.ProductId)
            .Distinct()
            .ToList();

        var products = await _context.Products
            .AsNoTracking()
            .Where(x => productIds.Contains(x.Id))
            .ToListAsync(cancellationToken);

        var productLookup = products
            .ToDictionary(
                x => x.Id,
                x => x);

        var items = purchaseOrder.Items
            .Select(item =>
            {
                productLookup.TryGetValue(
                    item.ProductId,
                    out var product);

                return new PurchaseOrderItemDetail
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProductName =
                        product?.Name.Value ??
                        "Unknown Product",
                    Sku =
                        product?.Sku.Value ??
                        string.Empty,
                    Quantity = item.Quantity.Value,
                    UnitPrice = item.UnitPrice.Value,
                    LineTotal = item.LineTotal.Value
                };
            })
            .ToList();

        return new PurchaseOrderDetail
        {
            Id = purchaseOrder.Id,
            SupplierId = purchaseOrder.SupplierId,
            SupplierName = supplier.Name.Value,
            OrderDate = purchaseOrder.OrderDate,
            Status = purchaseOrder.Status.ToString(),
            Total = purchaseOrder.Total.Value,
            Items = items
        };
    }
}
