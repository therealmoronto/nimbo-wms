using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.Persistences.Converters;

public sealed class EntityIdListConverter<TId> : ValueConverter<List<TId>, Guid[]>
    where TId : struct, IEntityId
{
    private static readonly Func<Guid, TId> Factory = CreateFactory();

    public EntityIdListConverter()
        : base(
            ids => (ids ?? new()).Select(x => x.Value).ToArray(),
            guids => (guids ?? Array.Empty<Guid>()).Select(Factory).ToList())
    {
    }

    private static Func<Guid, TId> CreateFactory()
    {
        var ctor = typeof(TId).GetConstructor([typeof(Guid)])
                   ?? throw new InvalidOperationException(
                       $"Type '{typeof(TId).Name}' must have a public constructor .ctor(Guid).");

        var g = Expression.Parameter(typeof(Guid), "g");
        var body = Expression.New(ctor, g);
        return Expression.Lambda<Func<Guid, TId>>(body, g).Compile();
    }
}
