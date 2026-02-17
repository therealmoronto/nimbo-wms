using JetBrains.Annotations;
using Xunit.Sdk;

namespace Nimbo.Wms.Tests.Common.Attributes;

[PublicAPI]
[TraitDiscoverer("Nimbo.Wms.Tests.Common.Attributes.IntegrationTestDiscoverer", "Nimbo.Wms.Tests.Common")]
public class IntegrationTestAttribute : Attribute, ITraitAttribute
{
}
