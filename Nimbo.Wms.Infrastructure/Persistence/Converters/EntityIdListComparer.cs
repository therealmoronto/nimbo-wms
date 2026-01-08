using Microsoft.EntityFrameworkCore.ChangeTracking;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.Persistence.Converters;

public sealed class EntityIdListComparer<TId> : ValueComparer<List<TId>>
    where TId : struct, IEntityId
{
    public EntityIdListComparer()
        : base(
            (a, b) => SequenceEqual(a, b),
            v => Hash(v),
            v => v == null ? new List<TId>() : new List<TId>(v)) { }

    private static bool SequenceEqual(List<TId>? a, List<TId>? b)
    {
        a ??= new();
        b ??= new();
        if (a.Count != b.Count) return false;

        for (var i = 0; i < a.Count; i++)
            if (a[i].Value != b[i].Value) return false;

        return true;
    }

    private static int Hash(List<TId>? v)
    {
        v ??= new();
        unchecked
        {
            var hash = 17;
            foreach (var x in v)
                hash = hash * 31 + x.Value.GetHashCode();
            return hash;
        }
    }
}
