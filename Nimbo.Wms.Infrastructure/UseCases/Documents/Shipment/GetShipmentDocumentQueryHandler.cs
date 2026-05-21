using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Application.Mappings.Documents.Shipment;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.Documents.Shipment.Dtos;
using Nimbo.Wms.Contracts.Documents.Shipment.Queries;
using Nimbo.Wms.Domain.Entities.Documents.Shipment;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.Shipment;

[PublicAPI]
public class GetShipmentDocumentQueryHandler : IRequestHandler<GetShipmentDocumentQuery, ShipmentDocumentDto>
{
    private readonly NimboWmsDbContext _dbContext;
    private readonly ShipmentDocumentBodyMapper _bodyMapper;
    private readonly ShipmentDocumentLineMapper _lineMapper;
    private readonly ShipmentPickLineMapper _pickLineMapper;

    public GetShipmentDocumentQueryHandler(
        NimboWmsDbContext dbContext,
        ShipmentDocumentBodyMapper bodyMapper,
        ShipmentDocumentLineMapper lineMapper,
        ShipmentPickLineMapper pickLineMapper)
    {
        _dbContext = dbContext;
        _bodyMapper = bodyMapper;
        _lineMapper = lineMapper;
        _pickLineMapper = pickLineMapper;
    }

    public async Task<ShipmentDocumentDto> Handle(GetShipmentDocumentQuery request, CancellationToken ct)
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
            throw new NotFoundException($"Shipment document with ID {request.Id} not found");

        return document;
    }
}