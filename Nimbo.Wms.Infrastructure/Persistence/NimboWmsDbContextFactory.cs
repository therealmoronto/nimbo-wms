using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Nimbo.Wms.Infrastructure.Persistence;

[PublicAPI]
public sealed class NimboWmsDbContextFactory : IDesignTimeDbContextFactory<NimboWmsDbContext>
{
    public NimboWmsDbContext CreateDbContext(string[] args)
    {
        // 1) CI / explicit override
        var envConn = Environment.GetEnvironmentVariable("NIMBO_WMS_CONNECTION_STRING");
        if (!string.IsNullOrWhiteSpace(envConn))
            return Create(envConn);
        
        // 2) Local dev from solution root: read Nimbo.Wms/appsettings*.json
        var solutionRoot = FindSolutionRoot(Directory.GetCurrentDirectory());
        var apiProjectPath = Path.Combine(solutionRoot, "Nimbo.Wms"); // API project folder

        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        var configuration = new ConfigurationBuilder()
            .SetBasePath(apiProjectPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: false)
            .AddEnvironmentVariables()
            .Build();
        
        var conn = configuration.GetConnectionString("NimboWmsDb");
        if (string.IsNullOrWhiteSpace(conn))
            throw new InvalidOperationException(
                "Connection string 'ConnectionStrings:NimboWmsDb' was not found. " +
                "Check Nimbo.Wms/appsettings*.json or set NIMBO_WMS_CONNECTION_STRING.");

        return Create(conn);
    }
    
    private static NimboWmsDbContext Create(string connectionString)
    {
        var options = new DbContextOptionsBuilder<NimboWmsDbContext>()
            .UseNpgsql(connectionString, npgsql => npgsql.MigrationsAssembly(typeof(NimboWmsDbContext).Assembly.FullName))
            .Options;

        return new NimboWmsDbContext(options);
    }
    
    private static string FindSolutionRoot(string startDir)
    {
        var dir = new DirectoryInfo(startDir);

        while (dir != null)
        {
            if (dir.GetFiles("*.sln").Length > 0)
                return dir.FullName;

            dir = dir.Parent;
        }

        // Fallback: current dir
        return startDir;
    }
}
