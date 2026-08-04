using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NexusERP.Application.Common.Interfaces;
using NexusERP.Infrastructure.Persistence;
using NexusERP.Infrastructure.Identity.Services;
using Microsoft.Extensions.Options;
using NexusERP.Infrastructure.Identity.Jwt;
using NexusERP.Infrastructure.Persistence.Queries;

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

        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        services.AddScoped<IApplicationDbContext>(
            provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IReportQueries, ReportQueries>();

        services.AddScoped<IDashboardQueries, DashboardQueries>();

        services.AddScoped<IPasswordHasher, PasswordHasher>();

        return services;
    }
}