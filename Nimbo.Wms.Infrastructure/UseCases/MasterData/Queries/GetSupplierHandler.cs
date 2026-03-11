using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Application.Abstractions.UseCases.MasterData.Queries;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.MasterData.Dtos;
using Nimbo.Wms.Domain.Entities.MasterData;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.MasterData.Queries;

public sealed class GetSupplierHandler : IQueryHandler<GetSupplierQuery, SupplierDto>
{
    private readonly NimboWmsDbContext _dbContext;
    private readonly IMapper<Supplier, SupplierDto> _mapper;

    public GetSupplierHandler(NimboWmsDbContext dbContext, IMapper<Supplier, SupplierDto> mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<SupplierDto> HandleAsync(GetSupplierQuery query, CancellationToken ct = default)
    {
        var dbQuery = _dbContext.Set<Supplier>()
            .AsNoTracking()
            .Where(s => s.Id == query.SupplierId);

        var supplier = await _mapper.ProjectToDto(dbQuery).SingleOrDefaultAsync(ct);
        if (supplier == null)
            throw new NotFoundException($"Supplier not found.");

        return supplier;
    }
}
