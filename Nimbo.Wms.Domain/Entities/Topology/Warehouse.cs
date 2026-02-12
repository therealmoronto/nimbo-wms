using Nimbo.Wms.Domain.Common;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;

namespace Nimbo.Wms.Domain.Entities.Topology;

public sealed class Warehouse : BaseEntity<WarehouseId>
{
    private readonly List<Zone> _zones = new();
    private readonly List<Location> _locations = new();

    // ReSharper disable once UnusedMember.Global
    public Warehouse()
    {
        // Required by EF Core
    }

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

    public string Code { get; private set; }
    
    public string Name { get; private set; }
    
    public string? Address { get; private set; }
    
    public string? Description { get; private set; }
    
    public bool IsActive { get; private set; }

    public IReadOnlyCollection<Zone> Zones => _zones.AsReadOnly();

    public IReadOnlyCollection<Location> Locations => _locations.AsReadOnly();

    public void Rename(string name) => Name = RequireNonEmpty(name, nameof(name));

    public void ChangeCode(string code) => Code = RequireNonEmpty(code, nameof(code));

    public void SetAddress(string? address) => Address = address;

    public void SetDescription(string? description) => Description = description;

    public void Deactivate() => IsActive = false;

    public void Activate() => IsActive = true;

    public Zone AddZone(ZoneId zoneId, string code, string name, ZoneType type)
    {
        if (_zones.Any(z => z.Code == code))
            throw new DomainException("Zone code must be unique within warehouse");

        var zone = new Zone(zoneId, Id, code, name, type);
        _zones.Add(zone);

        return zone;
    }

    public Location AddLocation(LocationId locationId, ZoneId zoneId, string code, LocationType type)
    {
        if (_locations.Any(l => l.Code == code))
            throw new DomainException("Location code must be unique within warehouse");

        if (_zones.All(z => !z.Id.Equals(zoneId)))
            throw new DomainException("Zone does not belong to warehouse");

        var location = new Location(locationId, Id, zoneId, code, type);
        _locations.Add(location);

        return location;
    }

    public Zone GetZone(ZoneId zoneId)
    {
        var zone = _zones.SingleOrDefault(z => z.Id.Equals(zoneId));
        return zone ?? throw new DomainException("Zone does not belong to warehouse");
    }

    public Location GetLocation(LocationId locationId)
    {
        var location = _locations.SingleOrDefault(l => l.Id.Equals(locationId));
        return location ?? throw new DomainException("Location does not belong to warehouse");
    }

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
