using Microsoft.AspNetCore.Mvc;
using NexusERP.Application.Reports.GetSalesReport;

namespace NexusERP.Api.Endpoints.Reports;

public static class GetSalesReportEndpoint
{
    public static IEndpointRouteBuilder MapGetSalesReport(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/api/reports/sales",
            async (
                DateOnly? from,
                DateOnly? to,
                [FromServices] GetSalesReportHandler handler,
                CancellationToken cancellationToken) =>
            {
                var request =
                    new GetSalesReportRequest(
                        from,
                        to);

                var response =
                    await handler.Handle(
                        request,
                        cancellationToken);

                return Results.Ok(response);
            })

        .WithName("GetSalesReport")
        .WithSummary("Gets the sales report.")
        .WithDescription("Returns sales orders filtered by an optional date range.")
        .Produces<IReadOnlyCollection<GetSalesReportResponse>>(
            StatusCodes.Status200OK);

        return app;
    }
}