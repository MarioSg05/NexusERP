using NexusERP.Application.Common.Interfaces;

namespace NexusERP.Application.Products.GetProducts;

public sealed class GetProductsHandler
{
    private readonly IProductQueries _productQueries;

    public GetProductsHandler(
        IProductQueries productQueries)
    {
        _productQueries = productQueries;
    }

    public async Task<IReadOnlyList<ProductListItem>> Handle(
        CancellationToken cancellationToken = default)
    {
        return await _productQueries.GetProductsAsync(
            cancellationToken);
    }
}