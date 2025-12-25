using Microsoft.EntityFrameworkCore;

namespace Nimbo.Wms.Infrastructure.Persistences;

public sealed class NimboWmsDbContext : DbContext
{
    public NimboWmsDbContext(DbContextOptions<NimboWmsDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
