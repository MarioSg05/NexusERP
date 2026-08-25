using NexusERP.Application.Common;
using NexusERP.Infrastructure.Messaging;
using NexusERP.Infrastructure.Persistence;
using NexusERP.Worker;

var builder =
    Host.CreateApplicationBuilder(args);

builder.Services.AddEventProcessing();

builder.Services.AddPersistence(
    builder.Configuration);

builder.Services.AddMessaging(
    builder.Configuration);

builder.Services.Configure<OutboxWorkerSettings>(
    builder.Configuration.GetSection(
        OutboxWorkerSettings.SectionName));

builder.Services.AddHostedService<OutboxWorker>();

builder.Services.AddHostedService<
    IntegrationEventConsumerWorker>();

var host =
    builder.Build();

await host.RunAsync();