using Microsoft.AspNetCore.Mvc;
using NexusERP.Application.Dashboard.GetDashboard;

namespace NexusERP.Api.Endpoints.Dashboard;

public static class GetDashboardEndpoint
{
    public static IEndpointRouteBuilder MapGetDashboard(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/api/dashboard",
            async (
                [FromServices] GetDashboardHandler handler,
                CancellationToken cancellationToken) =>
            {
                var request = new GetDashboardRequest();

                var response =
                    await handler.Handle(
                        request,
                        cancellationToken);

                return Results.Ok(response);
            })

        .WithName("GetDashboard")
        .WithSummary("Gets the ERP dashboard.")
        .WithDescription("Returns the dashboard with inventory, sales and purchasing KPIs.")
        .Produces<GetDashboardResponse>(
            StatusCodes.Status200OK);

        return app;
    }
}