using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.MasterData.Dtos;
using Nimbo.Wms.Contracts.MasterData.Queries;
using Nimbo.Wms.Domain.Entities.MasterData;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.MasterData.Handlers;

[PublicAPI]
internal sealed class GetSupplierQueryHandler : IRequestHandler<GetSupplierQuery, SupplierDto>
{
    private readonly NimboWmsDbContext _dbContext;
    private readonly IMapper<Supplier, SupplierDto> _mapper;

    public GetSupplierQueryHandler(NimboWmsDbContext dbContext, IMapper<Supplier, SupplierDto> mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<SupplierDto> Handle(GetSupplierQuery query, CancellationToken ct = default)
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
