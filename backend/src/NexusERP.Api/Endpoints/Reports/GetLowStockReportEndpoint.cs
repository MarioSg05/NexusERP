using Microsoft.AspNetCore.Mvc;
using NexusERP.Application.Reports.GetLowStockReport;

namespace NexusERP.Api.Endpoints.Reports;

public static class GetLowStockReportEndpoint
{
    public static IEndpointRouteBuilder MapGetLowStockReport(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/api/reports/low-stock",
            async (
                int minimumStock,
                [FromServices] GetLowStockReportHandler handler,
                CancellationToken cancellationToken) =>
            {
                var request =
                    new GetLowStockReportRequest(minimumStock);

                var response =
                    await handler.Handle(
                        request,
                        cancellationToken);

                return Results.Ok(response);
            })

        .WithName("GetLowStockReport")
        .WithSummary("Gets products with low stock.")
        .WithDescription("Returns products whose inventory is less than or equal to the specified minimum stock.")
        .Produces<IReadOnlyCollection<GetLowStockReportResponse>>(
            StatusCodes.Status200OK);

        return app;
    }
}