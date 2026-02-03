namespace Nimbo.Wms.Infrastructure.Tests.Infrastructure;

[CollectionDefinition(Name)]
public class PostgresCollection : ICollectionFixture<PostgresFixture>
{
    public const string Name = "PostgresCollection";
}
