using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;

namespace Nimbo.Wms.Domain.Entities.WarehouseData;

public sealed class Zone : IEntity<ZoneId>
{
    
    /// <exception cref="ArgumentException">Thrown when the provided decimal params is negative or when the provided strings code and name are empty or whitespace</exception>
    public Zone(
        ZoneId id,
        WarehouseId warehouseId,
        string code,
        string name,
        ZoneType type,
        decimal? maxWeightKg = null,
        decimal? maxVolumeM3 = null,
        bool isQuarantine = false,
        bool isDamagedArea = false)
    {
        Id = id;
        WarehouseId = warehouseId;

        Code = RequireNonEmpty(code, nameof(code));
        Name = RequireNonEmpty(name, nameof(name));

        Type = type;

        MaxWeightKg = RequireNonNegativeOrNull(maxWeightKg, nameof(maxWeightKg));
        MaxVolumeM3 = RequireNonNegativeOrNull(maxVolumeM3, nameof(maxVolumeM3));

        IsQuarantine = isQuarantine;
        IsDamagedArea = isDamagedArea;
    }
    
    public ZoneId Id { get; }

    public WarehouseId WarehouseId { get; }

    public string Code { get; private set; }

    public string Name { get; private set; }

    public ZoneType Type { get; private set; }

    public decimal? MaxWeightKg { get; private set; }

    public decimal? MaxVolumeM3 { get; private set; }

    public bool IsQuarantine { get; private set; }

    public bool IsDamagedArea { get; private set; }
    
    public void Rename(string name) => Name = RequireNonEmpty(name, nameof(name));

    public void ChangeCode(string code) => Code = RequireNonEmpty(code, nameof(code));

    public void ChangeType(ZoneType type) => Type = type;

    public void SetCapacity(decimal? maxWeightKg, decimal? maxVolumeM3)
    {
        MaxWeightKg = RequireNonNegativeOrNull(maxWeightKg, nameof(maxWeightKg));
        MaxVolumeM3 = RequireNonNegativeOrNull(maxVolumeM3, nameof(maxVolumeM3));
    }

    public void SetQuarantine(bool value) => IsQuarantine = value;

    public void SetDamagedArea(bool value) => IsDamagedArea = value;

    /// <summary>
    /// Checks if the provided string is non-empty and trims it.
    /// </summary>
    /// <param name="value">String to check</param>
    /// <param name="paramName">Setted parameter name</param>
    /// <returns>Trimmed string</returns>
    /// <exception cref="ArgumentException">Thrown when the provided string is empty or whitespace</exception>
    private static string RequireNonEmpty(string value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Value cannot be empty.", paramName);
        return value.Trim();
    }

    /// <summary>
    /// Checks if the provided decimal is non-negative.
    /// </summary>
    /// <param name="value">Decimal to check</param>
    /// <param name="paramName">Setted parameter name</param>
    /// <returns>Verified value</returns>
    /// <exception cref="ArgumentException">Thrown when the provided decimal is negative</exception>
    private static decimal? RequireNonNegativeOrNull(decimal? value, string paramName)
    {
        if (value < 0m)
            throw new ArgumentOutOfRangeException(paramName, "Value cannot be negative.");
        return value;
    }
}
