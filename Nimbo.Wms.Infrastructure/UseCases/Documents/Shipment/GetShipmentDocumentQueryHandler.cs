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
public class GetShipmentDocumentQueryHandler : IRequestHandler<GetShipmentDocumentQuery, Result<ShipmentDocumentDto>>
{
    private readonly NimboWmsDbContext _dbContext;
    private readonly IMapper<ShipmentDocument, ShipmentDocumentBodyDto> _bodyMapper;
    private readonly IMapper<ShipmentDocumentLine, ShipmentDocumentLineDto> _lineMapper;
    private readonly IMapper<ShipmentPickLine, ShipmentPickLineDto> _pickLineMapper;

    public GetShipmentDocumentQueryHandler(
        NimboWmsDbContext dbContext,
        IMapper<ShipmentDocument, ShipmentDocumentBodyDto> bodyMapper,
        IMapper<ShipmentDocumentLine, ShipmentDocumentLineDto> lineMapper,
        IMapper<ShipmentPickLine, ShipmentPickLineDto> pickLineMapper)
    {
        _dbContext = dbContext;
        _bodyMapper = bodyMapper;
        _lineMapper = lineMapper;
        _pickLineMapper = pickLineMapper;
    }

    public async Task<Result<ShipmentDocumentDto>> Handle(GetShipmentDocumentQuery request, CancellationToken ct)
    {
        var dbQuery = _dbContext.Set<ShipmentDocument>()
            .AsNoTracking()
            .Where(d => d.Id == request.Id)
            .Include(d => d.Lines)
            .Include(d => d.PickLines);

        var document = await dbQuery.Select(d =>
                new ShipmentDocumentDto(
                    _bodyMapper.MapToDto(d),
                    _lineMapper.MapToDto(d.Lines).ToList(),
                    _pickLineMapper.MapToDto(d.PickLines).ToList()))
            .SingleOrDefaultAsync(ct);
        if (document is null)
            return Error.NotFound("document.notfound", $"Shipment document with ID {request.Id} not found");

        return document;
    }
}
