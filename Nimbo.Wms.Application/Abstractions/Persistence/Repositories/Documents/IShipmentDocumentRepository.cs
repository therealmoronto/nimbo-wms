using JetBrains.Annotations;
using Nimbo.Wms.Domain.Entities.Documents.Shipment;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;

[PublicAPI]
public interface IShipmentDocumentRepository : IDocumentRepository<ShipmentDocument, ShipmentDocumentId>
{

}
