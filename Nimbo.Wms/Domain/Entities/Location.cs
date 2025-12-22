using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;

namespace Nimbo.Wms.Domain.Entities;

public sealed class Location : IEntity<LocationId>
{
    /// <exception cref="ArgumentException">Thrown when the provided strings of code or name are empty or whitespace</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the provided decimal params is negative</exception>
    public Location(
        LocationId id,
        WarehouseId warehouseId,
        ZoneId zoneId,
        string code,
        LocationType type,
        decimal? maxWeightKg = null,
        decimal? maxVolumeM3 = null,
        bool isSingleItemOnly = false,
        bool isPickingLocation = false,
        bool isReceivingLocation = false,
        bool isShippingLocation = false,
        bool isActive = true,
        bool isBlocked = false,
        string? aisle = null,
        string? rack = null,
        string? level = null,
        string? position = null)
    {
        Id = id;

        WarehouseId = warehouseId;
        ZoneId = zoneId;

        Code = RequireNonEmpty(code, nameof(code));
        Type = type;

        MaxWeightKg = RequireNonNegativeOrNull(maxWeightKg, nameof(maxWeightKg));
        MaxVolumeM3 = RequireNonNegativeOrNull(maxVolumeM3, nameof(maxVolumeM3));

        IsSingleItemOnly = isSingleItemOnly;

        IsPickingLocation = isPickingLocation;
        IsReceivingLocation = isReceivingLocation;
        IsShippingLocation = isShippingLocation;

        IsActive = isActive;
        IsBlocked = isBlocked;

        Aisle = TrimOrNull(aisle);
        Rack = TrimOrNull(rack);
        Level = TrimOrNull(level);
        Position = TrimOrNull(position);
    }

    public LocationId Id { get; }

    public WarehouseId WarehouseId { get; }

    public ZoneId ZoneId { get; }

    public string Code { get; private set; }

    public string? Aisle { get; private set; }
    
    public string? Rack { get; private set; }
    
    public string? Level { get; private set; }
    
    public string? Position { get; private set; }

    public LocationType Type { get; private set; }

    public decimal? MaxWeightKg { get; private set; }

    public decimal? MaxVolumeM3 { get; private set; }

    public bool IsSingleItemOnly { get; private set; }

    public bool IsPickingLocation { get; private set; }

    public bool IsReceivingLocation { get; private set; }

    public bool IsShippingLocation { get; private set; }

    public bool IsActive { get; private set; } = true;
    public bool IsBlocked { get; private set; }

    public void ChangeCode(string code) => Code = RequireNonEmpty(code, nameof(code));

    public void ChangeType(LocationType type) => Type = type;

    public void SetAddressParts(string? aisle, string? rack, string? level, string? position)
    {
        Aisle = TrimOrNull(aisle);
        Rack = TrimOrNull(rack);
        Level = TrimOrNull(level);
        Position = TrimOrNull(position);
    }

    public void SetCapacity(decimal? maxWeightKg, decimal? maxVolumeM3)
    {
        MaxWeightKg = RequireNonNegativeOrNull(maxWeightKg, nameof(maxWeightKg));
        MaxVolumeM3 = RequireNonNegativeOrNull(maxVolumeM3, nameof(maxVolumeM3));
    }

    public void SetUsageFlags(bool isSingleItemOnly, bool isPickingLocation, bool isReceivingLocation, bool isShippingLocation)
    {
        IsSingleItemOnly = isSingleItemOnly;
        IsPickingLocation = isPickingLocation;
        IsReceivingLocation = isReceivingLocation;
        IsShippingLocation = isShippingLocation;
    }

    public void Block() => IsBlocked = true;

    public void Unblock() => IsBlocked = false;

    public void Deactivate() => IsActive = false;

    public void Activate() => IsActive = true;

    /// <summary>
    /// Checks if the provided string is non-empty and trims it.
    /// </summary>
    /// <param name="value">String to be checked and trimmed</param>
    /// <param name="paramName">Name of the parameter for exception message</param>
    /// <returns>Trimmed string or throws ArgumentException if empty</returns>
    /// <exception cref="ArgumentException">Thrown when value is empty</exception>
    private static string RequireNonEmpty(string value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Value cannot be empty.", paramName);
        return value.Trim();
    }

    private static string? TrimOrNull(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    /// <summary>
    /// Checks if the provided decimal is non-negative.
    /// </summary>
    /// <param name="value">Decimal value to be checked</param>
    /// <param name="paramName">Name of the parameter for exception message</param>
    /// <returns>Original value if non-negative, otherwise throws ArgumentOutOfRangeException</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when value is negative</exception>
    private static decimal? RequireNonNegativeOrNull(decimal? value, string paramName)
    {
        if (value is not null && value.Value < 0m)
            throw new ArgumentOutOfRangeException(paramName, "Value cannot be negative.");
        return value;
    }
}
