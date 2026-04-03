using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Infrastructure.DependencyInjection;
using Nimbo.Wms.Infrastructure.Persistence;
using Nimbo.Wms.OutboxProcessor;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.AddKafkaProducer<string, string>("kafka");
builder.AddNpgsqlDbContext<NimboWmsDbContext>("postgres");

builder.Services.AddInfrastructure();

builder.Services.AddHostedService<OutboxBackgroundService>();

var host = builder.Build();

host.Run();
