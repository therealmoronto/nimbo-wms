using Xunit;

namespace Nimbo.Wms.Tests.Common.Database;

[CollectionDefinition(Name)]
public class PostgresCollection : ICollectionFixture<PostgresFixture>
{
    public const string Name = "PostgresCollection";
}
