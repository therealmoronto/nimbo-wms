using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Application.Abstractions.UseCases.MasterData.Queries;
using Nimbo.Wms.Contracts.Topology.Dtos;
using Nimbo.Wms.Domain.Entities.MasterData;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.MasterData.Queries;

public class GetSuppliersHandler : IQueryHandler<GetSuppliersQuery, IReadOnlyList<SupplierDto>>
{
    private readonly NimboWmsDbContext _dbContext;

    public GetSuppliersHandler(NimboWmsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<SupplierDto>> HandleAsync(GetSuppliersQuery query, CancellationToken ct = default)
    {
        var suppliers = await _dbContext.Set<Supplier>()
            .AsNoTracking()
            .Select(s => new SupplierDto(
                s.Id.Value,
                s.Code,
                s.Name,
                s.TaxId,
                s.Address,
                s.ContactName,
                s.Phone,
                s.Email,
                s.IsActive,
                new()
            ))
            .ToListAsync(ct);

        return suppliers;
    }
}
