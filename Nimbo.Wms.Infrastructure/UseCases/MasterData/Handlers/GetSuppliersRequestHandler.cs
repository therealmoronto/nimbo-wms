using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Contracts.MasterData.Dtos;
using Nimbo.Wms.Contracts.MasterData.Requests;
using Nimbo.Wms.Domain.Entities.MasterData;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.MasterData.Handlers;

[PublicAPI]
internal class GetSuppliersRequestHandler : IRequestHandler<GetSuppliersRequest, IReadOnlyList<SupplierDto>>
{
    private readonly NimboWmsDbContext _dbContext;

    public GetSuppliersRequestHandler(NimboWmsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<SupplierDto>> Handle(GetSuppliersRequest request, CancellationToken ct = default)
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
