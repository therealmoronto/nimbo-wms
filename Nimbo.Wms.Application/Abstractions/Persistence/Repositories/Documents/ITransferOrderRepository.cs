using Nimbo.Wms.Domain.Documents.Transfer;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;

public interface ITransferOrderRepository : IRepository<TransferOrder, TransferOrderId>
{
    
}
