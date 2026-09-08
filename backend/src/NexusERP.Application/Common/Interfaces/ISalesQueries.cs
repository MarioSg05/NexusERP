using NexusERP.Application.Sales.GetSalesOrderById;
using NexusERP.Application.Sales.GetSalesOrders;

namespace NexusERP.Application.Common.Interfaces;

public interface ISalesQueries
{
    Task<IReadOnlyList<SalesOrderListItem>>
        GetSalesOrdersAsync(
            CancellationToken cancellationToken = default);

    Task<SalesOrderDetail?>
        GetSalesOrderByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default);
}