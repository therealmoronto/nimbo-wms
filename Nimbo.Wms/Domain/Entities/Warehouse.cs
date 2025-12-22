using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Domain.Entities;

public sealed class Warehouse : IEntity<WarehouseId>
{
    /// <exception cref="ArgumentException">Thrown when the provided strings of code or name are empty or whitespace</exception>
    public Warehouse(
        WarehouseId id,
        string code,
        string name,
        string? address = null,
        string? description = null,
        bool isActive = true)
    {
        Id = id;
        Code = RequireNonEmpty(code, nameof(Code));
        Name = RequireNonEmpty(name, nameof(Name));
        Address = address;
        Description = description;
        IsActive = isActive;
    }
    
    public WarehouseId Id { get; }
    
    public string Code { get; private set; }
    
    public string Name { get; private set; }
    
    public string? Address { get; private set; }
    
    public string? Description { get; private set; }
    
    public bool IsActive { get; private set; }
    
    public void Rename(string name) => Name = RequireNonEmpty(name, nameof(name));

    public void ChangeCode(string code) => Code = RequireNonEmpty(code, nameof(code));

    public void SetAddress(string? address) => Address = address;

    public void SetDescription(string? description) => Description = description;

    public void Deactivate() => IsActive = false;

    public void Activate() => IsActive = true;

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
}
