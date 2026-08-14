using NexusERP.Application.Common.Interfaces;

namespace NexusERP.Application.Purchasing.GetPurchaseOrders;

public sealed class GetPurchaseOrdersHandler
{
    private readonly IPurchasingQueries _purchasingQueries;

    public GetPurchaseOrdersHandler(
        IPurchasingQueries purchasingQueries)
    {
        _purchasingQueries = purchasingQueries;
    }

    public async Task<IReadOnlyList<PurchaseOrderListItem>> Handle(
        CancellationToken cancellationToken = default)
    {
        return await _purchasingQueries.GetPurchaseOrdersAsync(
            cancellationToken);
    }
}