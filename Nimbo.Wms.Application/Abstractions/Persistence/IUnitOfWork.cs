namespace Nimbo.Wms.Application.Abstractions.Persistence;

public interface IUnitOfWork
{
    Task CommitAsync(CancellationToken ct = default);
}
