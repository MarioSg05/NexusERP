using FluentValidation;
using Microsoft.EntityFrameworkCore;

using NexusERP.Application.Common.Exceptions;
using NexusERP.Application.Common.Interfaces;
using NexusERP.Domain.Inventory.ValueObjects;

namespace NexusERP.Application.Inventory.IncreaseInventoryStock;

public sealed class IncreaseInventoryStockHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IncreaseInventoryStockValidator _validator;

    public IncreaseInventoryStockHandler(
        IApplicationDbContext context,
        IncreaseInventoryStockValidator validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<IncreaseInventoryStockResponse> Handle(
        Guid id,
        IncreaseInventoryStockRequest request,
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

        inventory.IncreaseStock(quantity);

        await _context.SaveChangesAsync(
            cancellationToken);

        return new IncreaseInventoryStockResponse(
            inventory.Id,
            inventory.Quantity.Value);
    }
}