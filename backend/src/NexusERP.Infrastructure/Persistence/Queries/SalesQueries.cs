using Microsoft.EntityFrameworkCore;

using NexusERP.Application.Common.Interfaces;
using NexusERP.Application.Sales.GetSalesOrderById;
using NexusERP.Application.Sales.GetSalesOrders;

namespace NexusERP.Infrastructure.Persistence.Queries;

public sealed class SalesQueries
    : ISalesQueries
{
    private readonly ApplicationDbContext _context;

    public SalesQueries(
        ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<SalesOrderListItem>>
        GetSalesOrdersAsync(
            CancellationToken cancellationToken = default)
    {
        var rows = await (
            from salesOrder in _context.SalesOrders
                .AsNoTracking()
            join customer in _context.Customers
                .AsNoTracking()
                on salesOrder.CustomerId equals customer.Id
            orderby salesOrder.OrderDate descending
            select new
            {
                SalesOrder = salesOrder,
                Customer = customer
            })
            .ToListAsync(cancellationToken);

        return rows
            .Select(row => new SalesOrderListItem
            {
                Id = row.SalesOrder.Id,
                CustomerId = row.SalesOrder.CustomerId,
                CustomerName = row.Customer.Name.Value,
                OrderDate = row.SalesOrder.OrderDate,
                Status = row.SalesOrder.Status.ToString(),
                Total = row.SalesOrder.Total.Value
            })
            .ToList();
    }

    public async Task<SalesOrderDetail?>
        GetSalesOrderByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
    {
        var salesOrder = await _context.SalesOrders
            .AsNoTracking()
            .Include(x => x.Items)
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);

        if (salesOrder is null)
            return null;

        var customer = await _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == salesOrder.CustomerId,
                cancellationToken);

        if (customer is null)
            return null;

        var productIds = salesOrder.Items
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

        var items = salesOrder.Items
            .Select(item =>
            {
                productLookup.TryGetValue(
                    item.ProductId,
                    out var product);

                return new SalesOrderItemDetail
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

        return new SalesOrderDetail
        {
            Id = salesOrder.Id,
            CustomerId = salesOrder.CustomerId,
            CustomerName = customer.Name.Value,
            OrderDate = salesOrder.OrderDate,
            Status = salesOrder.Status.ToString(),
            Total = salesOrder.Total.Value,
            Items = items
        };
    }
}