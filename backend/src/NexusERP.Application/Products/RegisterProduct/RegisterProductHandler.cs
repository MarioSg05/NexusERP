using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NexusERP.Application.Common.Interfaces;
using NexusERP.Domain.Exceptions;
using NexusERP.Domain.Products.Aggregates;
using NexusERP.Domain.Products.ValueObjects;


namespace NexusERP.Application.Products.RegisterProduct;

public sealed class RegisterProductHandler
{
    private readonly IApplicationDbContext _context;
    private readonly RegisterProductValidator _validator;

    public RegisterProductHandler(
        IApplicationDbContext context,
        RegisterProductValidator validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<RegisterProductResponse> Handle(
        RegisterProductRequest request,
        CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(
    request,
    cancellationToken);

        var sku = new ProductSku(request.Sku);

        var exists = await _context.Products
            .AnyAsync(
                x => x.Sku == sku,
                cancellationToken);

        if (exists)
            throw new DomainException(
                "A product with this SKU already exists.");

        var name = new ProductName(request.Name);

        var price = new ProductPrice(request.Price);

        var product = Product.Register(
            name,
            sku,
            price);

        await _context.Products.AddAsync(
            product,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return new RegisterProductResponse(
            product.Id,
            product.Sku.Value);
    }
}