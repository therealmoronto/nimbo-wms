using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Nimbo.Wms.Domain.Common;

namespace Nimbo.Wms.Infrastructure.Persistence.Converters;

internal static class UtcDateTime
{
    private const string UnspecifiedKindMessage =
        "DateTime.Kind is Unspecified. All DateTime values must be UTC (use ISO-8601 with 'Z').";

    internal static readonly ValueConverter<DateTime, DateTime> Converter =
        new(
            value => EnsureUtcOrThrow(value),
            value => SpecifyUtcKind(value));

    internal static readonly ValueConverter<DateTime?, DateTime?> NullableConverter =
        new(
            value => EnsureUtcOrNull(value),
            value => value.HasValue ? SpecifyUtcKind(value.Value) : null);

    internal static readonly ValueComparer<DateTime> Comparer =
        new(
            (a, b) => UniversalTimesEqual(a, b),
            value => UniversalTimeHash(value),
            value => SpecifyUtcKind(value.ToUniversalTime()));

    internal static readonly ValueComparer<DateTime?> NullableComparer =
        new(
            (a, b) => NullableUniversalTimesEqual(a, b),
            value => value.HasValue ? UniversalTimeHash(value.Value) : 0,
            value => value.HasValue ? SpecifyUtcKind(value.Value.ToUniversalTime()) : null);

    private static DateTime SpecifyUtcKind(DateTime value) => DateTime.SpecifyKind(value, DateTimeKind.Utc);

    private static bool UniversalTimesEqual(DateTime a, DateTime b) => a.ToUniversalTime() == b.ToUniversalTime();

    private static int UniversalTimeHash(DateTime value) => value.ToUniversalTime().GetHashCode();

    private static bool NullableUniversalTimesEqual(DateTime? a, DateTime? b)
    {
        return a.HasValue
            ? b.HasValue && UniversalTimesEqual(a.Value, b.Value)
            : !b.HasValue;
    }

    private static DateTime? EnsureUtcOrNull(DateTime? value) => value.HasValue ? EnsureUtcOrThrow(value.Value) : null;

    private static DateTime EnsureUtcOrThrow(DateTime v)
    {
        if (v.Kind == DateTimeKind.Utc)
            return v;

        if (v.Kind == DateTimeKind.Local)
            return v.ToUniversalTime();

        throw new InvalidOperationException(UnspecifiedKindMessage);
    }
}
