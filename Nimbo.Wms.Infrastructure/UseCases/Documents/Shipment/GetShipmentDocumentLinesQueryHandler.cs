using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Contracts;
using Nimbo.Wms.Contracts.Documents.Shipment.Dtos;
using Nimbo.Wms.Contracts.Documents.Shipment.Queries;
using Nimbo.Wms.Domain.Entities.Documents.Shipment;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.Shipment;

[PublicAPI]
public class GetShipmentDocumentLinesQueryHandler : IRequestHandler<GetShipmentDocumentLinesQuery, IReadOnlyList<ShipmentDocumentLineDto>>
{
    private readonly NimboWmsDbContext _dbContext;
    private readonly IMapper<ShipmentDocumentLine, ShipmentDocumentLineDto> _mapper;

    public GetShipmentDocumentLinesQueryHandler(NimboWmsDbContext dbContext, IMapper<ShipmentDocumentLine, ShipmentDocumentLineDto> mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<ShipmentDocumentLineDto>> Handle(GetShipmentDocumentLinesQuery request, CancellationToken ct)
    {
        var lines = await _dbContext.Set<ShipmentDocumentLine>()
            .AsNoTracking()
            .Where(l => l.DocumentId == request.DocumentId)
            .ToListAsync(ct);

        return _mapper.MapToDto(lines).ToList();
    }
}
