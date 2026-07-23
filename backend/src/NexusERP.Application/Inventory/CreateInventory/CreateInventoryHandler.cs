using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NexusERP.Application.Common.Interfaces;
using NexusERP.Domain.Exceptions;
using NexusERP.Domain.Inventory.Aggregates;
using NexusERP.Domain.Inventory.ValueObjects;

namespace NexusERP.Application.Inventory.CreateInventory;

public sealed class CreateInventoryHandler
{
    private readonly IApplicationDbContext _context;
    private readonly CreateInventoryValidator _validator;

    public CreateInventoryHandler(
        IApplicationDbContext context,
        CreateInventoryValidator validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<CreateInventoryResponse> Handle(
    CreateInventoryRequest request,
    CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(
            request,
            cancellationToken);

        var productExists = await _context.Products
            .AnyAsync(
                x => x.Id == request.ProductId,
                cancellationToken);

        if (!productExists)
        {
            throw new DomainException(
                "The specified product does not exist.");
        }

        var inventoryExists = await _context.Inventories
            .AnyAsync(
                x => x.ProductId == request.ProductId,
                cancellationToken);

        if (inventoryExists)
        {
            throw new DomainException(
                "Inventory already exists for the specified product.");
        }

        var quantity = new InventoryQuantity(request.Quantity);

        var inventory = InventoryItem.Create(
            request.ProductId,
            quantity);

        await _context.Inventories.AddAsync(
            inventory,
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);

        return new CreateInventoryResponse(
            inventory.Id,
            inventory.ProductId,
            inventory.Quantity.Value);
    }
}