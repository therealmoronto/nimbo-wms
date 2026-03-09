using Nimbo.Wms.Domain.Entities.Documents.Adjustment;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;

public interface IAdjustmentDocumentRepository : IDocumentRepository<AdjustmentDocument, AdjustmentDocumentId>
{

}
