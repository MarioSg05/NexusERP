using Microsoft.EntityFrameworkCore;
using NexusERP.Application.Common.Interfaces;

namespace NexusERP.Application.Reports.GetLowStockReport;

public sealed class GetLowStockReportHandler
{
    private readonly IReportQueries _reportQueries;

    public GetLowStockReportHandler(IReportQueries reportQueries)
    {
        _reportQueries = reportQueries;
    }

    public async Task<IReadOnlyCollection<GetLowStockReportResponse>> Handle(
        GetLowStockReportRequest request,
        CancellationToken cancellationToken)
    {
        return await _reportQueries.GetLowStockReportAsync(
            request.MinimumStock,
            cancellationToken);
    }
}