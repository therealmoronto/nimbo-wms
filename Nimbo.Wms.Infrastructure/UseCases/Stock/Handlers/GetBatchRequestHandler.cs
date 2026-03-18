using MediatR;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Stock.Dtos;
using Nimbo.Wms.Contracts.Stock.Requests;
using Nimbo.Wms.Domain.Entities.Stock;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.Stock.Handlers;

public sealed class GetBatchRequestHandler : IRequestHandler<GetBatchRequest, BatchDto>
{
    private readonly NimboWmsDbContext _dbContext;

    public GetBatchRequestHandler(NimboWmsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<BatchDto> Handle(GetBatchRequest request, CancellationToken ct = default)
    {
        var batch = await _dbContext.Set<Batch>()
            .AsNoTracking()
            .Where(b => b.Id == request.BatchId)
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
