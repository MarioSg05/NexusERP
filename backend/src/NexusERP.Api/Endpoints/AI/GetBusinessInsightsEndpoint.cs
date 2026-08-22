using NexusERP.Application.AI.BusinessInsights;

namespace NexusERP.Api.Endpoints.AI;

public static class GetBusinessInsightsEndpoint
{
    public static IEndpointRouteBuilder MapGetBusinessInsights(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/api/ai/business-insights",
                async (
                    GetBusinessInsightsHandler handler,
                    CancellationToken cancellationToken) =>
                {
                    var response =
                        await handler.Handle(
                            cancellationToken);

                    return Results.Ok(response);
                })
            .WithName("GetBusinessInsights")
            .WithSummary(
                "Generates AI business insights.")
            .WithDescription(
                "Generates a read-only AI analysis using aggregated NexusERP business metrics.")
            .Produces<GetBusinessInsightsResponse>(
                StatusCodes.Status200OK)
            .Produces(
                StatusCodes.Status401Unauthorized)
            .Produces(
                StatusCodes.Status500InternalServerError);

        return app;
    }
}