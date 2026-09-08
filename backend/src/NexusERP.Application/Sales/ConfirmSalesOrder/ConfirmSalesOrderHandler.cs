using Microsoft.EntityFrameworkCore;

using NexusERP.Application.Common.Exceptions;
using NexusERP.Application.Common.Interfaces;
using NexusERP.Domain.Exceptions;
using NexusERP.Domain.Inventory.ValueObjects;
using NexusERP.Domain.Sales.Enums;

namespace NexusERP.Application.Sales.ConfirmSalesOrder;

public sealed class ConfirmSalesOrderHandler
{
    private readonly IApplicationDbContext _context;

    public ConfirmSalesOrderHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ConfirmSalesOrderResponse> Handle(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var salesOrder = await _context.SalesOrders
            .Include(x => x.Items)
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);

        if (salesOrder is null)
        {
            throw new NotFoundException(
                "Sales order was not found.");
        }
        if (salesOrder.Status != SalesOrderStatus.Pending)
        {
            throw new DomainException(
                "Only pending sales orders can be modified.");
        }

        var productIds = salesOrder.Items
            .Select(x => x.ProductId)
            .Distinct()
            .ToList();

        var inventoryItems = await _context.Inventories
            .Where(x => productIds.Contains(x.ProductId))
            .ToListAsync(cancellationToken);

        var inventoryByProduct = inventoryItems
            .ToDictionary(
                x => x.ProductId,
                x => x);

        // Validate every line before mutating anything.
        foreach (var item in salesOrder.Items)
        {
            if (!inventoryByProduct.TryGetValue(
                    item.ProductId,
                    out var inventory))
            {
                throw new DomainException(
                    $"Inventory does not exist for product '{item.ProductId}'.");
            }

            if (inventory.Quantity.Value < item.Quantity.Value)
            {
                throw new DomainException(
                    $"Insufficient stock available for product '{item.ProductId}'.");
            }
        }

        // All lines are valid. Apply stock changes.
        foreach (var item in salesOrder.Items)
        {
            var inventory =
                inventoryByProduct[item.ProductId];

            inventory.DecreaseStock(
                new InventoryQuantity(
                    item.Quantity.Value));
        }

        salesOrder.Confirm();

        await _context.SaveChangesAsync(
            cancellationToken);

        return new ConfirmSalesOrderResponse(
            salesOrder.Id,
            salesOrder.Status.ToString());
    }
}