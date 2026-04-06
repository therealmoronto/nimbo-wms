using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Filters;
using Nimbo.Wms.Infrastructure.Persistence;
using Nimbo.Wms.Middlewares;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<UtcDateTimeValidationFilter>();
});

builder.AddNpgsqlDbContext<NimboWmsDbContext>("nimboDb");

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenApi();

var app = builder.Build();


if (app.Environment.IsDevelopment() || app.Configuration.GetValue<bool>("RunMigrationsOnStartup"))
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<NimboWmsDbContext>();

    try
    {
        app.Logger.LogInformation("Applying database migrations...");
        await context.Database.MigrateAsync();
        app.Logger.LogInformation("Database migrations applied successfully");
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "Error applying database migrations");
        throw; // Приложение не должно запускаться без БД
    }
}

app.UseMiddleware<ProblemDetailsExceptionMiddleware>();
app.UseHttpsRedirection();
app.MapControllers();
app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Nimbo API");
        options.WithTheme(ScalarTheme.Laserwave); // Можно поиграться с темами
    });
}

app.Run();
