using Xunit.Abstractions;
using Xunit.Sdk;

namespace Nimbo.Wms.Tests.Common;

public class IntegrationTestDiscoverer : ITraitDiscoverer
{
    public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
    {
        yield return new("Category", "Integration");
    }
}
