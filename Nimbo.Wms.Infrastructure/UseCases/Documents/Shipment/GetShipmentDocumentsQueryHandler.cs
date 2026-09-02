using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.Documents.Shipment.Dtos;
using Nimbo.Wms.Contracts.Documents.Shipment.Queries;
using Nimbo.Wms.Domain.Entities.Documents.Shipment;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.Shipment;

[PublicAPI]
public class GetShipmentDocumentsQueryHandler : IRequestHandler<GetShipmentDocumentsQuery, IReadOnlyList<ShipmentDocumentBodyDto>>
{
    private readonly NimboWmsDbContext _dbContext;
    private readonly IMapper<ShipmentDocument, ShipmentDocumentBodyDto> _mapper;

    public GetShipmentDocumentsQueryHandler(NimboWmsDbContext dbContext, IMapper<ShipmentDocument, ShipmentDocumentBodyDto> mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<ShipmentDocumentBodyDto>> Handle(GetShipmentDocumentsQuery request, CancellationToken ct)
    {
        var documents = await _dbContext.Set<ShipmentDocument>()
            .AsNoTracking()
            .ToListAsync(ct);

        return _mapper.MapToDto(documents).ToList();
    }
}
