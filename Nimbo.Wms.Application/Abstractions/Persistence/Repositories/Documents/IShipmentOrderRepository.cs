using Nimbo.Wms.Domain.Documents.Outbound;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;

public interface IShipmentOrderRepository : IRepository<ShipmentOrder, ShipmentOrderId>
{
    
}
