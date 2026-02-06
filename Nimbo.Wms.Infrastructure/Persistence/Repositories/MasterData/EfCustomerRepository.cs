using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Domain.Entities.MasterData;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.Persistence.Repositories.MasterData;

internal sealed class EfCustomerRepository : EfRepository<Customer, CustomerId>, ICustomerRepository
{
    public EfCustomerRepository(NimboWmsDbContext dbContext)
        : base(dbContext) { }
}
