using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;
using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Domain.Documents.Audit;

public class InventoryCount : Document<InventoryCountId, InventoryCountStatus>
{
    private readonly List<InventoryCountLine> _lines = new();
    private readonly List<LocationId> _locationScope;

    private InventoryCount()
    {
        // Required by EF Core
    }

    public InventoryCount(
        InventoryCountId id,
        WarehouseId warehouseId,
        DateTime createdAt,
        string? externalReference = null,
        ZoneId? zoneId = null,
        IEnumerable<LocationId>? locationScope = null)
        : base(id, InventoryCountStatus.Draft, createdAt, externalReference)
    {
        WarehouseId = warehouseId;
        ZoneId = zoneId;

        _locationScope = locationScope?.ToList() ?? new List<LocationId>();
    }
    
    public WarehouseId WarehouseId { get; }

    public ZoneId? ZoneId { get; }

    /// <summary>
    /// Optional explicit scope selection (cycle counting): specific locations.
    /// </summary>
    public IReadOnlyCollection<LocationId> LocationScope => _locationScope;

    public IReadOnlyCollection<InventoryCountLine> Lines => _lines.AsReadOnly();

    public DateTime? StartedAt { get; private set; }

    public DateTime? ClosedAt { get; private set; }
    
    public static InventoryCount Create(
        WarehouseId warehouseId,
        DateTime createdAt,
        string? externalReference = null,
        ZoneId? zoneId = null,
        IEnumerable<LocationId>? locationScope = null)
        => new(InventoryCountId.New(), warehouseId, createdAt, externalReference, zoneId, locationScope);

    /// <summary>
    /// Start counting. After this point stock is expected to be locked (InventoryItem.Status -> Audit) by process layer.
    /// </summary>
    public void Start(DateTime utcNow)
    {
        EnsureStatus(InventoryCountStatus.Draft);
        Status = InventoryCountStatus.InProgress;
        StartedAt = utcNow;
    }

    public InventoryCountLine AddLine(ItemId itemId, LocationId locationId, Quantity systemQuantity)
    {
        EnsureStatus(InventoryCountStatus.InProgress);

        var line = new InventoryCountLine(Id, itemId, locationId, systemQuantity);
        _lines.Add(line);
        return line;
    }

    public void RecordCount(Guid lineId, Quantity countedQuantity)
    {
        EnsureStatus(InventoryCountStatus.InProgress);

        var line = _lines.SingleOrDefault(x => x.Id == lineId)
                   ?? throw new KeyNotFoundException("InventoryCountLine not found.");

        line.SetCountedQuantity(countedQuantity);
    }

    public void MoveToReconciliation()
    {
        EnsureStatus(InventoryCountStatus.InProgress);

        if (_lines.Count == 0)
            throw new InvalidOperationException("InventoryCount must have at least one line.");

        if (_lines.Any(l => !l.IsCounted))
            throw new InvalidOperationException("All lines must be counted before reconciliation.");

        Status = InventoryCountStatus.Reconciliation;
    }

    /// <summary>
    /// Close the inventory count after adjustments are applied by process layer.
    /// </summary>
    public void Close(DateTime utcNow)
    {
        EnsureStatus(InventoryCountStatus.Reconciliation);

        Status = InventoryCountStatus.Closed;
        ClosedAt = utcNow;
    }

    private void EnsureStatus(InventoryCountStatus expected)
    {
        if (Status != expected)
            throw new InvalidOperationException($"Invalid InventoryCount status. Expected {expected}, actual {Status}.");
    }
}
