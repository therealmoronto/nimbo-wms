using MassTransit;
using Nimbo.Wms.Domain.Entities.Documents;
using Nimbo.Wms.Infrastructure.DependencyInjection;
using Nimbo.Wms.Infrastructure.Persistence;
using Nimbo.Wms.OutboxProcessor;
using Nimbo.Wms.ServiceDefaults;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddMassTransit(config =>
{
    config.UsingInMemory();

    config.AddRider(rider =>
    {
        rider.AddProducer<string, IDocumentPostedEvent>("document-events");

        rider.UsingKafka((_, cfg) =>
        {
            cfg.Host(builder.Configuration.GetConnectionString("kafka"));
        });
    });
});

builder.AddNpgsqlDbContext<NimboWmsDbContext>("nimboDb");

builder.Services.AddInfrastructure();

builder.Services.AddHostedService<OutboxBackgroundService>();

var host = builder.Build();

host.Run();
