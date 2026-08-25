using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NexusERP.Application.Common.IntegrationEvents;
using NexusERP.Infrastructure.Messaging.Outbox;
using NexusERP.Infrastructure.Messaging.RabbitMq;

namespace NexusERP.Infrastructure.Messaging;

public static class MessagingDependencyInjection
{
    public static IServiceCollection AddMessaging(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<RabbitMqSettings>(
            configuration.GetSection(
                RabbitMqSettings.SectionName));

        services.AddScoped<
            IIntegrationEventPublisher,
            RabbitMqIntegrationEventPublisher>();

        services.Configure<RabbitMqConsumerSettings>(
            configuration.GetSection(
                RabbitMqConsumerSettings.SectionName));

        services.AddSingleton<
            RabbitMqIntegrationEventConsumer>();

        services.AddScoped<OutboxMessageFactory>();

        services.AddScoped<OutboxProcessor>();

        return services;
    }
}