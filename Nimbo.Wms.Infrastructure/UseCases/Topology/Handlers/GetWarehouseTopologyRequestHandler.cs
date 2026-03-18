using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.Topology.Dtos;
using Nimbo.Wms.Contracts.Topology.Requests;
using Nimbo.Wms.Domain.Entities.Topology;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.Topology.Handlers;

[PublicAPI]
internal sealed class GetWarehouseTopologyRequestHandler : IRequestHandler<GetWarehouseTopologyRequest, WarehouseTopologyDto>
{
    private readonly NimboWmsDbContext _db;
    private readonly IMapper<Warehouse, WarehouseTopologyDto> _mapper;

    public GetWarehouseTopologyRequestHandler(NimboWmsDbContext db, IMapper<Warehouse, WarehouseTopologyDto> mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<WarehouseTopologyDto> Handle(GetWarehouseTopologyRequest request, CancellationToken ct = default)
    {
        var dbQuery = _db.Set<Warehouse>()
            .AsNoTracking()
            .Where(w => w.Id == request.WarehouseId);

        var warehouse = await _mapper.ProjectToDto(dbQuery).SingleOrDefaultAsync(ct);
        if (warehouse is null)
            throw new NotFoundException("Warehouse not found");

        return warehouse;
    }
}

