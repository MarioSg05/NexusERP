using NexusERP.Application.Purchasing.GetPurchaseOrders;
using NexusERP.Application.Purchasing.GetPurchaseOrderById;

namespace NexusERP.Application.Common.Interfaces;

public interface IPurchasingQueries
{
    Task<IReadOnlyList<PurchaseOrderListItem>>
        GetPurchaseOrdersAsync(
            CancellationToken cancellationToken = default);

    Task<PurchaseOrderDetail?>
        GetPurchaseOrderByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default);
}