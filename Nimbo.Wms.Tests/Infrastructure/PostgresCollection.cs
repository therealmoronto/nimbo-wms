namespace Nimbo.Wms.Tests.Infrastructure;

[CollectionDefinition(Name)]
public class PostgresCollection : ICollectionFixture<PostgresFixture>
{
    public const string Name = "PostgresCollection";
}
