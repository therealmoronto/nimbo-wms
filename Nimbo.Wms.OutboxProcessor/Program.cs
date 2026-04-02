using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Infrastructure.DependencyInjection;
using Nimbo.Wms.Infrastructure.Persistence;
using Nimbo.Wms.OutboxProcessor;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.AddKafkaProducer<string, string>("kafka");

builder.Services.AddDbContext<NimboWmsDbContext>(options =>
{
    var cs = builder.Configuration.GetConnectionString("NimboWmsDb");
    options.UseNpgsql(cs, npgsql => npgsql.MigrationsAssembly("Nimbo.Wms.Infrastructure"));
});

builder.Services.AddInfrastructure();

builder.Services.AddHostedService<OutboxBackgroundService>();

var host = builder.Build();

host.Run();
