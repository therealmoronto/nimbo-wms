using Nimbo.Wms.Infrastructure.DependencyInjection;
using Nimbo.Wms.Infrastructure.Persistence;
using Nimbo.Wms.OutboxProcessor;
using Nimbo.Wms.ServiceDefaults;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.AddKafkaProducer<string, string>("kafka", settings =>
{
    settings.Config.MessageSendMaxRetries = 3;
    settings.Config.MessageTimeoutMs = 500;
});
builder.AddNpgsqlDbContext<NimboWmsDbContext>("nimboDb");

builder.Services.AddInfrastructure();

builder.Services.AddHostedService<OutboxBackgroundService>();

var host = builder.Build();

host.Run();
