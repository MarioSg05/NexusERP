using FluentValidation;
using Microsoft.EntityFrameworkCore;

using NexusERP.Application.Common.Exceptions;
using NexusERP.Application.Common.Interfaces;
using NexusERP.Domain.Inventory.ValueObjects;

namespace NexusERP.Application.Inventory.DecreaseInventoryStock;

public sealed class DecreaseInventoryStockHandler
{
    private readonly IApplicationDbContext _context;
    private readonly DecreaseInventoryStockValidator _validator;

    public DecreaseInventoryStockHandler(
        IApplicationDbContext context,
        DecreaseInventoryStockValidator validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<DecreaseInventoryStockResponse> Handle(
        Guid id,
        DecreaseInventoryStockRequest request,
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

        inventory.DecreaseStock(quantity);

        await _context.SaveChangesAsync(
            cancellationToken);

        return new DecreaseInventoryStockResponse(
            inventory.Id,
            inventory.Quantity.Value);
    }
}