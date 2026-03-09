using JetBrains.Annotations;
using Nimbo.Wms.Domain.Entities.Documents.Receiving;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;

[PublicAPI]
public interface IReceivingDocumentRepository : IDocumentRepository<ReceivingDocument, ReceivingDocumentId>
{

}
