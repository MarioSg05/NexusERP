using NexusERP.Application.Common.Interfaces;

namespace NexusERP.Application.Reports.GetPurchasingReport;

public sealed class GetPurchasingReportHandler
{
    private readonly IReportQueries _reportQueries;

    public GetPurchasingReportHandler(
        IReportQueries reportQueries)
    {
        _reportQueries = reportQueries;
    }

    public async Task<IReadOnlyCollection<GetPurchasingReportResponse>> Handle(
        GetPurchasingReportRequest request,
        CancellationToken cancellationToken = default)
    {
        return await _reportQueries.GetPurchasingReportAsync(
            request.From,
            request.To,
            cancellationToken);
    }
}