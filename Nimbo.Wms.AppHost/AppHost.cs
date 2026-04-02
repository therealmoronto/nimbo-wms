var builder = DistributedApplication.CreateBuilder(args);

var kafka = builder.AddKafka("kafka").WithKafkaUI();

builder.AddProject<Projects.Nimbo_Wms>("api")
    .WithHttpHealthCheck("/api/health")
    .WithReference(kafka);

builder.AddProject<Projects.Nimbo_Wms_OutboxProcessor>("outbox-processor")
    .WithReference(kafka);

builder.Build().Run();
