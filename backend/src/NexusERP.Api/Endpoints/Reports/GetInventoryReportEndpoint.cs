using Microsoft.AspNetCore.Mvc;
using NexusERP.Application.Reports.GetInventoryReport;

namespace NexusERP.Api.Endpoints.Reports;

public static class GetInventoryReportEndpoint
{
    public static IEndpointRouteBuilder MapGetInventoryReport(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/api/reports/inventory",
            async (
                [FromServices] GetInventoryReportHandler handler,
                CancellationToken cancellationToken) =>
            {
                var request =
                    new GetInventoryReportRequest();

                var response =
                    await handler.Handle(
                        request,
                        cancellationToken);

                return Results.Ok(response);
            })

        .WithName("GetInventoryReport")
        .WithSummary("Gets the current inventory report.")
        .WithDescription("Returns the current inventory with product information.")
        .Produces<IReadOnlyCollection<GetInventoryReportResponse>>(
            StatusCodes.Status200OK);

        return app;
    }
}