var builder = DistributedApplication.CreateBuilder(args);

var kafka = builder.AddKafka("kafka")
    .WithKafkaUI();

var username = builder.AddParameter("postgres-username", "nimbo_admin");
var password = builder.AddParameter("postgres-password", "nimbo_password");

var postgres = builder.AddPostgres("postgres", username, password, 5432);
var nimboDb = postgres.AddDatabase("nimbo");

builder.AddProject<Projects.Nimbo_Wms>("api")
    .WithHttpHealthCheck("/api/health")
    .WithReference(nimboDb)
    .WaitFor(postgres);

builder.AddProject<Projects.Nimbo_Wms_OutboxProcessor>("outbox-processor")
    .WithReference(kafka)
    .WithReference(nimboDb)
    .WaitFor(kafka)
    .WaitFor(postgres);

builder.Build().Run();
