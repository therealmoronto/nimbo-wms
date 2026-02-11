using Nimbo.Wms.Tests.Common.Database;

namespace Nimbo.Wms.Infrastructure.Tests;

[CollectionDefinition(Name)]
public class PostgresCollection : ICollectionFixture<PostgresFixture>
{
    public const string Name = "PostgresCollection";
}
