using NexusERP.Application.Common.Interfaces;

namespace NexusERP.Application.Reports.GetSalesReport;

public sealed class GetSalesReportHandler
{
    private readonly IReportQueries _reportQueries;

    public GetSalesReportHandler(
        IReportQueries reportQueries)
    {
        _reportQueries = reportQueries;
    }

    public async Task<IReadOnlyCollection<GetSalesReportResponse>> Handle(
        GetSalesReportRequest request,
        CancellationToken cancellationToken = default)
    {
        return await _reportQueries.GetSalesReportAsync(
            request.From,
            request.To,
            cancellationToken);
    }
}