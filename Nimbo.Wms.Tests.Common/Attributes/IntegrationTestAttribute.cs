using JetBrains.Annotations;
using Xunit.Sdk;

namespace Nimbo.Wms.Tests.Common.Attributes;

[PublicAPI]
[TraitDiscoverer("Nimbo.Wms.Tests.Common.IntegrationTestDiscoverer", "Nimbo.Wms.Tests")]
public class IntegrationTestAttribute : Attribute, ITraitAttribute
{
}
