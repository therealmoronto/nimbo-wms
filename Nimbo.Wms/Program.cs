using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Filters;
using Nimbo.Wms.Http;
using Nimbo.Wms.Infrastructure.DependencyInjection;
using Nimbo.Wms.Infrastructure.Persistence;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddKafkaProducer<string, string>("kafka");

builder.Services.AddControllers(options =>
{
    options.Filters.Add<UtcDateTimeValidationFilter>();
});

builder.Services.AddDbContext<NimboWmsDbContext>(
    options =>
{
    var cs = builder.Configuration.GetConnectionString("NimboWmsDb");
    options.UseNpgsql(cs, npgsql => npgsql.MigrationsAssembly("Nimbo.Wms.Infrastructure"));
});

builder.Services.AddInfrastructure();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Nimbo WMS API");
        options.WithTheme(ScalarTheme.Laserwave); // Можно поиграться с темами
    });
}

app.UseMiddleware<ProblemDetailsExceptionMiddleware>();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
