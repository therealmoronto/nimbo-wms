using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Nimbo.Wms.Infrastructure.Persistences;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<NimboWmsDbContext>(options =>
{
    var cs = builder.Configuration.GetConnectionString("NimboWmsDb");
    options.UseNpgsql(cs);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "Nimbo.Wms",
            Version = "v1"
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Nimbo WMS API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
