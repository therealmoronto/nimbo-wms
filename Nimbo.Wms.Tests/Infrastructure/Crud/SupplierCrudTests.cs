using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Domain.Entities.MasterData;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Tests.Common;

namespace Nimbo.Wms.Tests.Infrastructure.Crud;

[IntegrationTest]
[Collection(PostgresCollection.Name)]
public class SupplierCrudTests
{
    private readonly PostgresFixture _fixture;

    public SupplierCrudTests(PostgresFixture fixture) => _fixture = fixture;
    
    [Fact]
    public async Task Supplier_crud_should_work_successfully_test()
    {
        TestSkip.If(!_fixture.IsStarted, "Docker is not available. Start Docker Engine to run integration tests locally.");

        await _fixture.EnsureMigratedAsync();
        
        var guid = Guid.NewGuid();
        var id = SupplierId.From(guid);
        var code = $"SUP-{guid:N}";
        var supplier = new Supplier(
            id,
            code,
            name: "Test Supplier",
            email: "test@supplier.local");
        
        // Create
        await using (var db = DbContextFactory.Create(_fixture.ConnectionString))
        {
            db.Set<Supplier>().Add(supplier);
            await db.SaveChangesAsync();
        }

        // Read
        await using (var db = DbContextFactory.Create(_fixture.ConnectionString))
        {
            db.ChangeTracker.Clear();

            var loaded = await db.Set<Supplier>()
                .SingleAsync(x => x.Id.Equals(id));

            loaded.Code.Should().Be(code);
            loaded.Name.Should().Be("Test Supplier");
            loaded.Email.Should().Be("test@supplier.local");
        }

        // Update
        await using (var db = DbContextFactory.Create(_fixture.ConnectionString))
        {
            var loaded = await db.Set<Supplier>()
                .SingleAsync(x => x.Equals(id));

            loaded.Rename("Renamed Supplier");
            loaded.Deactivate();

            await db.SaveChangesAsync();
        }

        await using (var db = DbContextFactory.Create(_fixture.ConnectionString))
        {
            db.ChangeTracker.Clear();

            var loaded = await db.Set<Supplier>()
                .SingleAsync(x => x.Equals(id));

            loaded.Name.Should().Be("Renamed Supplier");
            loaded.IsActive.Should().BeFalse();
        }
        
        // Delete
        await using (var db = DbContextFactory.Create(_fixture.ConnectionString))
        {
            var loaded = await db.Set<Supplier>()
                .SingleAsync(x => x.Equals(id));

            db.Remove(loaded);
            await db.SaveChangesAsync();
        }

        await using (var db = DbContextFactory.Create(_fixture.ConnectionString))
        {
            var exists = await db.Set<Supplier>().AnyAsync(x => x.Equals(id));
            exists.Should().BeFalse();
        }
    }
}
