using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.Topology.Dtos;
using Nimbo.Wms.Contracts.Topology.Queries;
using Nimbo.Wms.Domain.Entities.Topology;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.Topology.Handlers;

[PublicAPI]
internal sealed class GetWarehouseTopologyQueryHandler : IRequestHandler<GetWarehouseTopologyQuery, WarehouseTopologyDto>
{
    private readonly NimboWmsDbContext _db;
    private readonly IMapper<Warehouse, WarehouseTopologyDto> _mapper;

    public GetWarehouseTopologyQueryHandler(NimboWmsDbContext db, IMapper<Warehouse, WarehouseTopologyDto> mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<WarehouseTopologyDto> Handle(GetWarehouseTopologyQuery query, CancellationToken ct = default)
    {
        var dbQuery = _db.Set<Warehouse>()
            .AsNoTracking()
            .Where(w => w.Id == query.WarehouseId);

        var warehouse = await _mapper.ProjectToDto(dbQuery).SingleOrDefaultAsync(ct);
        if (warehouse is null)
            throw new NotFoundException("Warehouse not found");

        return warehouse;
    }
}

