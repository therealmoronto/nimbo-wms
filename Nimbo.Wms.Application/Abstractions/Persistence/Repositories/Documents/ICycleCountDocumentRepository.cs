using JetBrains.Annotations;
using Nimbo.Wms.Domain.Entities.Documents.CycleCount;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;

[PublicAPI]
public interface ICycleCountDocumentRepository : IDocumentRepository<CycleCountDocument, CycleCountDocumentId>
{

}
