using NexusERP.Application.Common.Interfaces;

namespace NexusERP.Application.Reports.GetInventoryReport;

public sealed class GetInventoryReportHandler
{
    private readonly IReportQueries _reportQueries;

    public GetInventoryReportHandler(IReportQueries reportQueries)
    {
        _reportQueries = reportQueries;
    }

    public async Task<IReadOnlyCollection<GetInventoryReportResponse>> Handle(
        GetInventoryReportRequest request,
        CancellationToken cancellationToken)
    {
        return await _reportQueries.GetInventoryReportAsync(cancellationToken);
    }
}