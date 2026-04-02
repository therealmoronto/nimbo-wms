var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Nimbo_Wms>("api")
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.Nimbo_Wms_OutboxProcessor>("outbox-processor");

builder.Build().Run();
