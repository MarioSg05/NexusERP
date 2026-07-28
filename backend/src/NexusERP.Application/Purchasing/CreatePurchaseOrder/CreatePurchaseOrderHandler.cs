using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NexusERP.Application.Common.Interfaces;
using NexusERP.Domain.Exceptions;
using NexusERP.Domain.Purchasing.Aggregates;
using NexusERP.Domain.Purchasing.Entities;
using NexusERP.Domain.Purchasing.ValueObjects;

namespace NexusERP.Application.Purchasing.CreatePurchaseOrder;

public sealed class CreatePurchaseOrderHandler
{
    private readonly IApplicationDbContext _context;
    private readonly CreatePurchaseOrderValidator _validator;

    public CreatePurchaseOrderHandler(
        IApplicationDbContext context,
        CreatePurchaseOrderValidator validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<CreatePurchaseOrderResponse> Handle(
        CreatePurchaseOrderRequest request,
        CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(
            request,
            cancellationToken);

        var supplierExists = await _context.Suppliers
            .AnyAsync(
                x => x.Id == request.SupplierId,
                cancellationToken);

        if (!supplierExists)
        {
            throw new DomainException(
                "The specified supplier does not exist.");
        }

        var productIds = request.Items
            .Select(x => x.ProductId)
            .Distinct()
            .ToList();

        var existingProductIds = await _context.Products
            .Where(x => productIds.Contains(x.Id))
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);

        var missingProductId = productIds
            .Except(existingProductIds)
            .FirstOrDefault();

        if (missingProductId != Guid.Empty)
        {
            throw new DomainException(
                $"The specified product '{missingProductId}' does not exist.");
        }

        var purchaseOrder = PurchaseOrder.Create(
            request.SupplierId);

        foreach (var item in request.Items)
        {
            var purchaseItem = PurchaseOrderItem.Create(
                item.ProductId,
                new PurchaseQuantity(item.Quantity),
                new PurchaseUnitPrice(item.UnitPrice));

            purchaseOrder.AddItem(purchaseItem);
        }

        await _context.PurchaseOrders.AddAsync(
            purchaseOrder,
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);

        return new CreatePurchaseOrderResponse(
            purchaseOrder.Id,
            purchaseOrder.SupplierId);
    }
}