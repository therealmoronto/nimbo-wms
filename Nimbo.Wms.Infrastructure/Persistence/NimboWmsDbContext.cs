using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Infrastructure.Persistence.Converters;

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
        
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTime))
                {
                    property.SetValueConverter(UtcDateTime.Converter);
                    property.SetValueComparer(UtcDateTime.Comparer);
                }
                else if (property.ClrType == typeof(DateTime?))
                {
                    property.SetValueConverter(UtcDateTime.NullableConverter);
                    property.SetValueComparer(UtcDateTime.NullableComparer);
                }
            }
        }
    }
}
