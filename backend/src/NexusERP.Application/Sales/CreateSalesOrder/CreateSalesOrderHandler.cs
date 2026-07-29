using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NexusERP.Application.Common.Interfaces;
using NexusERP.Domain.Exceptions;
using NexusERP.Domain.Sales.Aggregates;
using NexusERP.Domain.Sales.Entities;
using NexusERP.Domain.Sales.ValueObjects;

namespace NexusERP.Application.Sales.CreateSalesOrder;

public sealed class CreateSalesOrderHandler
{
    private readonly IApplicationDbContext _context;
    private readonly CreateSalesOrderValidator _validator;

    public CreateSalesOrderHandler(
        IApplicationDbContext context,
        CreateSalesOrderValidator validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<CreateSalesOrderResponse> Handle(
        CreateSalesOrderRequest request,
        CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(
            request,
            cancellationToken);

        var customerExists = await _context.Customers
            .AnyAsync(
                x => x.Id == request.CustomerId,
                cancellationToken);

        if (!customerExists)
        {
            throw new DomainException(
                "The specified customer does not exist.");
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

        var salesOrder = SalesOrder.Create(
            request.CustomerId);

        foreach (var item in request.Items)
        {
            var salesOrderItem = SalesOrderItem.Create(
                item.ProductId,
                new SalesQuantity(item.Quantity),
                new SalesUnitPrice(item.UnitPrice));

            salesOrder.AddItem(salesOrderItem);
        }

        await _context.SalesOrders.AddAsync(
            salesOrder,
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);

        return new CreateSalesOrderResponse(
            salesOrder.Id,
            salesOrder.CustomerId);
    }
}