using FluentValidation;
using Microsoft.EntityFrameworkCore;

using NexusERP.Application.Common.Exceptions;
using NexusERP.Application.Common.Interfaces;
using NexusERP.Domain.Products.ValueObjects;

namespace NexusERP.Application.Products.UpdateProduct;

public sealed class UpdateProductHandler
{
    private readonly IApplicationDbContext _context;
    private readonly UpdateProductValidator _validator;

    public UpdateProductHandler(
        IApplicationDbContext context,
        UpdateProductValidator validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<UpdateProductResponse> Handle(
        Guid id,
        UpdateProductRequest request,
        CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(
            request,
            cancellationToken);

        var product = await _context.Products
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);

        if (product is null)
        {
            throw new NotFoundException(
                "Product was not found.");
        }

        var name =
            new ProductName(request.Name);

        var price =
            new ProductPrice(request.Price);

        product.ChangeName(name);
        product.ChangePrice(price);

        await _context.SaveChangesAsync(
            cancellationToken);

        return new UpdateProductResponse(
            product.Id,
            product.Sku.Value);
    }
}