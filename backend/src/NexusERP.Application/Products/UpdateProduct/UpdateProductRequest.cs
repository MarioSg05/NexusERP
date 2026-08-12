namespace NexusERP.Application.Products.UpdateProduct;

public sealed record UpdateProductRequest(
    string Name,
    decimal Price);