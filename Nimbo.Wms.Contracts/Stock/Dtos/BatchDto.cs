namespace Nimbo.Wms.Contracts.Stock.Dtos;

public sealed class BatchDto
{
    public Guid Id { get; set; }

    public Guid ItemId { get; set; }

    public string BatchNumber { get; set; }

    public Guid? SupplierId { get; set; }

    public DateTime? ManufacturedAt { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public DateTime? ReceivedAt { get; set; }

    public string? Notes { get; set; }
}
