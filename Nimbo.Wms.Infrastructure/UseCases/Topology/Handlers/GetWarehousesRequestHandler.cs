using MediatR;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Abstractions.UseCases.Topology.Queries;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.Topology.Dtos;
using Nimbo.Wms.Domain.Entities.Topology;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.Topology.Queries;

internal sealed class GetWarehousesRequestHandler : IRequestHandler<GetWarehousesRequest, IReadOnlyList<WarehouseListItemDto>>
{
    private readonly NimboWmsDbContext _db;
    private readonly IMapper<Warehouse, WarehouseListItemDto> _mapper;

    public GetWarehousesRequestHandler(NimboWmsDbContext db, IMapper<Warehouse, WarehouseListItemDto> mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<WarehouseListItemDto>> Handle(GetWarehousesRequest request, CancellationToken ct = default)
    {
        var dbQuery = _db.Set<Warehouse>().AsNoTracking();
        return await _mapper.ProjectToDto(dbQuery).ToListAsync(ct);
    }
}
