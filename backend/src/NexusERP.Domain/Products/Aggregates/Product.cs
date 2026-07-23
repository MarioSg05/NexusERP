using NexusERP.Domain.Common;
using NexusERP.Domain.Products.Events;
using NexusERP.Domain.Products.ValueObjects;

namespace NexusERP.Domain.Products.Aggregates;

public sealed class Product : AggregateRoot
{
    public ProductName Name { get; private set; }

    public ProductSku Sku { get; private set; }

    public ProductPrice Price { get; private set; }

    public bool IsActive { get; private set; }

    private Product(
        ProductName name,
        ProductSku sku,
        ProductPrice price)
    {
        Name = name;
        Sku = sku;
        Price = price;

        IsActive = true;
    }

    public static Product Register(
        ProductName name,
        ProductSku sku,
        ProductPrice price)
    {
        var product = new Product(
            name,
            sku,
            price);

        product.AddDomainEvent(
            new ProductRegisteredEvent(product.Id));

        return product;
    }

    public void ChangePrice(ProductPrice price)
    {
        if (Price == price)
            return;

        Price = price;

        UpdateAudit();
    }

    public void Activate()
    {
        if (IsActive)
            return;

        IsActive = true;

        UpdateAudit();
    }

    public void Deactivate()
    {
        if (!IsActive)
            return;

        IsActive = false;

        UpdateAudit();
    }
}