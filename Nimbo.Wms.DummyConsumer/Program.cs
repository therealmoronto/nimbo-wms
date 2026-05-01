using Confluent.Kafka;
using MassTransit;
using Nimbo.Wms.Domain.Entities.Documents;
using Nimbo.Wms.DummyConsumer;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddMassTransit(config =>
{
    config.AddRider(rider =>
    {
        rider.AddConsumer<DummyConsumer>();

        rider.UsingKafka((context, cfg) =>
        {
            cfg.Host(builder.Configuration.GetConnectionString("kafka"));

            cfg.TopicEndpoint<IDocumentPostedEvent>(
                "document-events",
                "dummy-consuming",
                e =>
                {
                    e.ConfigureConsumer<DummyConsumer>(context);
                    e.AutoOffsetReset = AutoOffsetReset.Earliest;
                });
        });
    });
});

var host = builder.Build();
host.Run();
