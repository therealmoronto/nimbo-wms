using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;

namespace Nimbo.Wms.Domain.Entities.MasterData;

public class Item : BaseEntity<ItemId>
{
    // ReSharper disable once UnusedMember.Global
    public Item()
    {
        // Required by EF Core
    }

    /// <exception cref="ArgumentException">Thrown when the provided strings of name, internalSku or barcode are empty or whitespace or when weight or volume are negative</exception>
    public Item(ItemId id,
        string name,
        string internalSku,
        string barcode,
        UnitOfMeasure baseUomCode,
        string? manufacturer = null,
        decimal? weightKg = null,
        decimal? volumeM3 = null)
    {
        Id = id;

        Name = RequireNonEmpty(name, nameof(name));
        InternalSku = RequireNonEmpty(internalSku, nameof(internalSku));

        BaseUomCode = baseUomCode;

        Barcode = TrimOrNull(barcode);
        Manufacturer = TrimOrNull(manufacturer);

        WeightKg = RequirePositiveOrNull(weightKg, nameof(weightKg));
        VolumeM3 = RequirePositiveOrNull(volumeM3, nameof(volumeM3));
    }

    public string Name { get; private set; }
    
    public string InternalSku { get; private set; }
    
    public string? Barcode { get; private set; }
    
    public UnitOfMeasure BaseUomCode { get; private set; }
    
    public string? Manufacturer { get; private set; }
    
    public decimal? WeightKg { get; private set; }
    
    public decimal? VolumeM3 { get; private set; }
    
    public void Rename(string name) => Name = RequireNonEmpty(name, nameof(name));

    public void ChangeInternalSku(string internalSku)
        => InternalSku = RequireNonEmpty(internalSku, nameof(internalSku));

    public void ChangeBaseUom(UnitOfMeasure baseUomCode)
        => BaseUomCode = baseUomCode;

    public void SetBarcode(string? barcode) => Barcode = TrimOrNull(barcode);

    public void SetManufacturer(string? manufacturer) => Manufacturer = TrimOrNull(manufacturer);

    public void SetPhysical(decimal? weightKg, decimal? volumeM3)
    {
        WeightKg = RequirePositiveOrNull(weightKg, nameof(weightKg));
        VolumeM3 = RequirePositiveOrNull(volumeM3, nameof(volumeM3));
    }

    private static string RequireNonEmpty(string value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Value cannot be empty.", paramName);

        return value.Trim();
    }

    private static string? TrimOrNull(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static decimal? RequirePositiveOrNull(decimal? value, string paramName)
    {
        if (value is not null && value.Value <= 0m)
            throw new ArgumentOutOfRangeException(paramName, "Value must be greater than zero.");
        return value;
    }
}
