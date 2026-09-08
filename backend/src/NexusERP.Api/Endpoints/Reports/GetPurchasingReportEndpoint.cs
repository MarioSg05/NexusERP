using Microsoft.AspNetCore.Mvc;
using NexusERP.Application.Reports.GetPurchasingReport;

namespace NexusERP.Api.Endpoints.Reports;

public static class GetPurchasingReportEndpoint
{
    public static IEndpointRouteBuilder MapGetPurchasingReport(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/api/reports/purchasing",
            async (
                DateOnly? from,
                DateOnly? to,
                [FromServices] GetPurchasingReportHandler handler,
                CancellationToken cancellationToken) =>
            {
                var request =
                    new GetPurchasingReportRequest(
                        from,
                        to);

                var response =
                    await handler.Handle(
                        request,
                        cancellationToken);

                return Results.Ok(response);
            })

        .WithName("GetPurchasingReport")
        .WithSummary("Gets the purchasing report.")
        .WithDescription("Returns purchase orders filtered by an optional date range.")
        .Produces<IReadOnlyCollection<GetPurchasingReportResponse>>(
            StatusCodes.Status200OK);

        return app;
    }
}