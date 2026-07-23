namespace NexusERP.Application.Products.RegisterProduct;

public sealed record RegisterProductRequest(
    string Name,
    string Sku,
    decimal Price);