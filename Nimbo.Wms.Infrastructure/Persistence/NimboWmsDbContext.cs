using Microsoft.EntityFrameworkCore;

namespace Nimbo.Wms.Infrastructure.Persistence;

public sealed class NimboWmsDbContext : DbContext
{
    public NimboWmsDbContext(DbContextOptions<NimboWmsDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("nimbo");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NimboWmsDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
