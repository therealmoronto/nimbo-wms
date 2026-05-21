using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Mappings.Documents.Shipment;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.Documents.Shipment.Dtos;
using Nimbo.Wms.Contracts.Documents.Shipment.Queries;
using Nimbo.Wms.Domain.Entities.Documents.Shipment;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.Shipment;

[PublicAPI]
public class GetShipmentPickLinesQueryHandler : IRequestHandler<GetShipmentPickLinesQuery, IReadOnlyList<ShipmentPickLineDto>>
{
    private readonly NimboWmsDbContext _dbContext;
    private readonly IMapper<ShipmentPickLine, ShipmentPickLineDto> _mapper;

    public GetShipmentPickLinesQueryHandler(NimboWmsDbContext dbContext, IMapper<ShipmentPickLine, ShipmentPickLineDto> mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<ShipmentPickLineDto>> Handle(GetShipmentPickLinesQuery request, CancellationToken ct)
    {
        var pickLines = await _dbContext.Set<ShipmentPickLine>()
            .AsNoTracking()
            .Where(pl => pl.DocumentId == request.DocumentId)
            .ToListAsync(ct);

        return _mapper.MapToDto(pickLines).ToList();
    }
}
