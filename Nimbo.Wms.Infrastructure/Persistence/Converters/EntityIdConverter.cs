using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.Persistence.Converters;

public sealed class EntityIdConverter<TId> : ValueConverter<TId, Guid>
    where TId : struct, IEntityId
{
    
    private static readonly Func<Guid, TId> Factory = CreateFactory();

    /// <exception cref="InvalidOperationException">Thrown when the provided type TId does not implement IEntityId</exception>
    public EntityIdConverter()
        : base(v => v.Value, v => Factory(v)) { }

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
