using Nimbo.Wms.Domain.Entities.Documents.Relocation;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;

public interface IRelocationDocumentRepository : IDocumentRepository<RelocationDocument, RelocationDocumentId>
{

}
