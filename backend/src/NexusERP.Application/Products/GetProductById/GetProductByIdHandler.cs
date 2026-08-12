using NexusERP.Application.Common.Interfaces;
using NexusERP.Application.Products.GetProducts;

namespace NexusERP.Application.Products.GetProductById;

public sealed class GetProductByIdHandler
{
    private readonly IProductQueries _productQueries;

    public GetProductByIdHandler(
        IProductQueries productQueries)
    {
        _productQueries = productQueries;
    }

    public async Task<ProductListItem?> Handle(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _productQueries.GetProductByIdAsync(
            id,
            cancellationToken);
    }
}