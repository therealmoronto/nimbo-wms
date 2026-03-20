using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.Stock.Dtos;
using Nimbo.Wms.Contracts.Stock.Requests;
using Nimbo.Wms.Domain.Entities.Stock;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.Stock.Handlers;

[PublicAPI]
internal sealed class GetBatchRequestHandler : IRequestHandler<GetBatchRequest, BatchDto>
{
    private readonly NimboWmsDbContext _dbContext;
    private readonly IMapper<Batch, BatchDto> _mapper;

    public GetBatchRequestHandler(NimboWmsDbContext dbContext, IMapper<Batch, BatchDto> mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<BatchDto> Handle(GetBatchRequest request, CancellationToken ct = default)
    {
        var dbQuery = _dbContext.Set<Batch>()
            .AsNoTracking()
            .Where(b => b.Id == request.BatchId);

        var batch = await _mapper.ProjectToDto(dbQuery).SingleOrDefaultAsync(ct);
        if (batch is null)
            throw new NotFoundException("Batch not found");

        return batch;
    }
}
