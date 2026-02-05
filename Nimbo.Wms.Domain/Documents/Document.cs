using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Domain.Documents;

public abstract class Document<TId, TStatus> : BaseEntity<TId>
    where TId : struct, IEntityId
    where TStatus : struct, Enum
{
    protected Document()
    {
        // Required by EF Core
    }
    
    protected Document(TId id, string code, string name, TStatus status,  DateTime createdAt, string? externalReference)
    {
        Id = id;
        CreatedAt = createdAt;
        Status = status;
        ExternalReference = !string.IsNullOrWhiteSpace(externalReference) ? externalReference.Trim() : null;
        UpdateCode(code);
        UpdateName(name);
    }

    public new TId Id { get; }
    
    public string Code { get; private set; }
    
    public string Name { get; private set; }

    public DateTime CreatedAt { get; }

    public TStatus Status { get; protected set; }

    public string? ExternalReference { get; }

    public void UpdateCode(string newCode)
    {
        if (string.IsNullOrWhiteSpace(newCode))
            throw new ArgumentException("Code cannot be null or whitespace", nameof(newCode));

        Code = newCode.Trim();
    }
    
    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Name cannot be null or whitespace", nameof(newName));
        
        Name = newName.Trim();
    }
}
