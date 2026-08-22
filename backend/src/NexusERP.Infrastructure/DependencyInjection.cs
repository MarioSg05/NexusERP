using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NexusERP.Application.Common.Interfaces;
using NexusERP.Infrastructure.Persistence;
using NexusERP.Infrastructure.Identity.Services;
using Microsoft.Extensions.Options;
using NexusERP.Infrastructure.Identity.Jwt;
using NexusERP.Infrastructure.Persistence.Queries;
using NexusERP.Infrastructure.AI;

namespace NexusERP.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"));
        });

        services.Configure<JwtSettings>(
          configuration.GetSection(JwtSettings.SectionName));

        services.Configure<OllamaSettings>(
            configuration.GetSection(
                OllamaSettings.SectionName));

        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        services.AddHttpClient<
            IAiInsightsGenerator,
            OllamaBusinessInsightsGenerator>(
                (serviceProvider, client) =>
                {
                    var settings =
                        serviceProvider
                            .GetRequiredService<
                                IOptions<OllamaSettings>>()
                            .Value;

                    client.Timeout =
                        TimeSpan.FromSeconds(
                            settings.TimeoutSeconds);
                });
        services.AddScoped<IApplicationDbContext>(
            provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IReportQueries, ReportQueries>();

        services.AddScoped<IDashboardQueries, DashboardQueries>();

        services.AddScoped<IPasswordHasher, PasswordHasher>();

        services.AddScoped<ICustomerQueries, CustomerQueries>();

        services.AddScoped<IProductQueries, ProductQueries>();

        services.AddScoped<IInventoryQueries, InventoryQueries>();

        services.AddScoped<ISupplierQueries, SupplierQueries>();

        services.AddScoped<IPurchasingQueries, PurchasingQueries>();

        services.AddScoped<ISalesQueries, SalesQueries>();

        return services;
    }
}