using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Npgsql;

var builder = DistributedApplication.CreateBuilder(args);

var kafka = builder.AddKafka("kafka")
    .WithKafkaUI();

var username = builder.AddParameter("postgres-username", "nimbo_admin");
var password = builder.AddParameter("postgres-password", "nimbo_password");

// Create temporary database for development
// Read from environment variables ConnectionString__nimboDb
var postgres = builder.AddPostgres("nimboDb", username, password, 5432);

var healthcheck = postgres.Resource.Annotations.OfType<HealthCheckAnnotation>().SingleOrDefault();
if (healthcheck != null)
    postgres.Resource.Annotations.Remove(healthcheck);

var nimboDb = postgres.AddDatabase("nimbo");

// Make Aspire to check the health of the database for current user instead of postgres
builder.Services.AddHealthChecks()
    .AddAsyncCheck(
        "custom_postgres_check",
        async ct =>
        {
            try
            {
                var connectionString = await postgres.Resource.ConnectionStringExpression.GetValueAsync(ct);
                await using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync(ct);
                return HealthCheckResult.Healthy();
            }
            catch (Exception e)
            {
                return HealthCheckResult.Unhealthy("Postgres not ready yet", e);
            }
        });

postgres.WithHealthCheck("custom_postgres_check");

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
