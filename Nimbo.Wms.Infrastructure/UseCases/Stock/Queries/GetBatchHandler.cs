using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Application.Abstractions.UseCases.Stock.Queries;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.Stock.Dtos;
using Nimbo.Wms.Domain.Entities.Stock;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.Stock.Queries;

public sealed class GetBatchHandler : IQueryHandler<GetBatchQuery, BatchDto>
{
    private readonly NimboWmsDbContext _dbContext;
    private readonly IMapper<Batch, BatchDto> _mapper;

    public GetBatchHandler(NimboWmsDbContext dbContext, IMapper<Batch, BatchDto> mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<BatchDto> HandleAsync(GetBatchQuery query, CancellationToken ct = default)
    {
        var dbQuery = _dbContext.Set<Batch>()
            .AsNoTracking()
            .Where(b => b.Id == query.BatchId);

        var batch = await _mapper.ProjectToDto(dbQuery).SingleOrDefaultAsync(ct);
        if (batch is null)
            throw new NotFoundException("Batch not found");

        return batch;
    }
}
