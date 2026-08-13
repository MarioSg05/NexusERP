using FluentValidation;
using Microsoft.EntityFrameworkCore;

using NexusERP.Application.Common.Exceptions;
using NexusERP.Application.Common.Interfaces;
using NexusERP.Domain.Inventory.ValueObjects;

namespace NexusERP.Application.Inventory.AdjustInventoryStock;

public sealed class AdjustInventoryStockHandler
{
    private readonly IApplicationDbContext _context;
    private readonly AdjustInventoryStockValidator _validator;

    public AdjustInventoryStockHandler(
        IApplicationDbContext context,
        AdjustInventoryStockValidator validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<AdjustInventoryStockResponse> Handle(
        Guid id,
        AdjustInventoryStockRequest request,
        CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(
            request,
            cancellationToken);

        var inventory = await _context.Inventories
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);

        if (inventory is null)
        {
            throw new NotFoundException(
                "Inventory item was not found.");
        }

        var quantity =
            new InventoryQuantity(request.Quantity);

        inventory.AdjustStock(quantity);

        await _context.SaveChangesAsync(
            cancellationToken);

        return new AdjustInventoryStockResponse(
            inventory.Id,
            inventory.Quantity.Value);
    }
}