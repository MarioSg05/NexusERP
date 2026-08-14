using NexusERP.Application.Common.Interfaces;

namespace NexusERP.Application.Purchasing.GetPurchaseOrderById;

public sealed class GetPurchaseOrderByIdHandler
{
    private readonly IPurchasingQueries _purchasingQueries;

    public GetPurchaseOrderByIdHandler(
        IPurchasingQueries purchasingQueries)
    {
        _purchasingQueries = purchasingQueries;
    }

    public async Task<PurchaseOrderDetail?> Handle(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _purchasingQueries
            .GetPurchaseOrderByIdAsync(
                id,
                cancellationToken);
    }
}