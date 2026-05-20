using MediatR;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.Documents.Receiving.Dtos;
using Nimbo.Wms.Contracts.Documents.Receiving.Queries;
using Nimbo.Wms.Domain.Entities.Documents.Receiving;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.Receiving;

public class GetReceivingDocumentQueryHandler : IRequestHandler<GetReceivingDocumentQuery, ReceivingDocumentDto>
{
    private readonly NimboWmsDbContext _dbContext;
    private readonly IMapper<ReceivingDocument, ReceivingDocumentDto> _mapper;

    public GetReceivingDocumentQueryHandler(NimboWmsDbContext dbContext, IMapper<ReceivingDocument, ReceivingDocumentDto> mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ReceivingDocumentDto> Handle(GetReceivingDocumentQuery request, CancellationToken ct)
    {
        var dbQuery = _dbContext.Set<ReceivingDocument>()
            .AsNoTracking()
            .Where(d => d.Id == request.Id)
            .Include(d => d.Lines);

        var document = await _mapper.ProjectToDto(dbQuery).SingleOrDefaultAsync(ct);
        if (document is null)
            throw new NotFoundException($"Receiving document with ID {request.Id} not found");

        return document;
    }
}
