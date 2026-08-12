namespace NexusERP.Application.Products.GetProducts;

public sealed class ProductListItem
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Sku { get; init; } = string.Empty;

    public decimal Price { get; init; }

    public bool IsActive { get; init; }
}