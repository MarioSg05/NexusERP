using NexusERP.Application.Reports.GetInventoryReport;
using NexusERP.Application.Reports.GetSalesReport;
using NexusERP.Application.Reports.GetPurchasingReport;

namespace NexusERP.Application.Common.Interfaces;

public interface IReportQueries
{
    Task<IReadOnlyCollection<GetInventoryReportResponse>>
        GetInventoryReportAsync(
            CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<GetLowStockReportResponse>>
        GetLowStockReportAsync(
            int minimumStock,
            CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<GetSalesReportResponse>>
        GetSalesReportAsync(
        DateOnly? from,
        DateOnly? to,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<GetPurchasingReportResponse>>
        GetPurchasingReportAsync(
        DateOnly? from,
        DateOnly? to,
        CancellationToken cancellationToken = default);
}