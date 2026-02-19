using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Application.Abstractions.UseCases.Stock.Queries;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Stock.Dtos;
using Nimbo.Wms.Domain.Entities.Stock;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.Stock.Queries;

public sealed class GetBatchHandler : IQueryHandler<GetBatchQuery, BatchDto>
{
    private readonly NimboWmsDbContext _dbContext;

    public GetBatchHandler(NimboWmsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<BatchDto> HandleAsync(GetBatchQuery query, CancellationToken ct = default)
    {
        var batch = await _dbContext.Set<Batch>()
            .AsNoTracking()
            .Where(b => b.Id == query.BatchId)
            .Select(b => new BatchDto(
                b.Id,
                b.ItemId,
                b.BatchNumber,
                b.SupplierId,
                b.ManufacturedAt,
                b.ExpiryDate,
                b.ReceivedAt,
                b.Notes))
            .SingleOrDefaultAsync(ct);

        if (batch is null)
            throw new NotFoundException("Batch not found");

        return batch;
    }
}
