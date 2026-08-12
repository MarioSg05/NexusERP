using NexusERP.Application.Products.GetProducts;

namespace NexusERP.Application.Common.Interfaces;

public interface IProductQueries
{
    Task<IReadOnlyList<ProductListItem>>
        GetProductsAsync(
            CancellationToken cancellationToken = default);

    Task<ProductListItem?>
        GetProductByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default);
}