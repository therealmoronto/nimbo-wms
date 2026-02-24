using JetBrains.Annotations;
using Nimbo.Wms.Domain.Common;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Domain.Entities.Documents.Common;

[PublicAPI]
public abstract class DocumentBase<TId, TStatus, TLine>
    where TId : struct, IEntityId
    where TStatus : struct, Enum
    where TLine : DocumentLineBase<TId>
{
    private readonly List<TLine> _lines = new();

    protected DocumentBase()
    {
        // Required by EF Core  
    }

    protected DocumentBase(TId id, string code, string title, DateTime createdAt)
    {
        Id = id;
        Code = code;
        Title = title;
        CreatedAt = createdAt;
        Status = Enum.Parse<TStatus>("Draft", ignoreCase: false);
        Touch();
    }

    public TId Id { get; }

    public string Code { get; private set; }

    public string Title { get; private set; }

    public TStatus Status { get; private set; }

    public DateTime CreatedAt { get; }

    public DateTime UpdatedAt { get; private set; }

    public DateTime? PostedAt { get; private set; }

    public long Version { get; private set; }

    public string? Notes { get; private set; }

    public IReadOnlyCollection<TLine> Lines => _lines.AsReadOnly();

    public virtual bool IsEditable() => Status.ToString() == "Draft";

    public void EnsureCanBeEdited()
    {
        if (!IsEditable())
            throw new InvalidOperationException("Cannot edit posted or cancelled documents.");
    }

    public void AddLine(TLine line, DateTime utcNow)
    {
        EnsureCanBeEdited();
        if (_lines.Any(x => Equals(x.Id, line.Id)))
            throw new DomainException($"Line with id '{line.Id}' already exists.");

        _lines.Add(line);
        Touch();
    }

    protected void RemoveLine(Guid lineId)
    {
        EnsureCanBeEdited();

        var index = _lines.FindIndex(x => Equals(x.Id, lineId));
        if (index < 0)
            throw new DomainException($"Line with id '{lineId}' not found.");

        _lines.RemoveAt(index);
        Touch();
    }

    protected TLine GetLine(Guid lineId)
    {
        var line = _lines.FirstOrDefault(x => Equals(x.Id, lineId));
        return line ?? throw new DomainException($"Line with id '{lineId}' not found.");
    }

    protected void ChangeLineQuantity(Guid lineId, Quantity quantity)
    {
        EnsureCanBeEdited();
        var line = GetLine(lineId);
        line.ChangeQuantity(quantity);
        Touch();
    }

    protected void ChangeLineNotes(Guid lineId, string? notes)
    {
        EnsureCanBeEdited();
        var line = GetLine(lineId);
        line.ChangeNotes(notes);
        Touch();
    }

    public virtual void ChangeCode(string newCode)
    {
        EnsureCanBeEdited();
        if (string.IsNullOrWhiteSpace(newCode))
            throw new DomainException("Code cannot be empty.");

        Code = newCode.Trim();
        Touch();
    }

    public virtual void ChangeTitle(string newTitle)
    {
        EnsureCanBeEdited();
        if (string.IsNullOrWhiteSpace(newTitle))
            throw new DomainException("Title cannot be empty.");

        Title = newTitle;
    }

    public virtual void ChangeNotes(string? newNotes)
    {
        EnsureCanBeEdited();
        Notes = newNotes?.Trim();
        Touch();
    }

    public virtual void TransitionTo(TStatus newStatus)
    {
        ValidateTransition(Status, newStatus);
        Status = newStatus;
        Touch();
    }

    public virtual void MarkPosted()
    {
        if (PostedAt is not null)
            throw new DomainException("Document has already been posted.");
        
        if (Status.ToString() != "Posted")
            throw new DomainException("Document must be in 'Posted' status before marking as posted.");

        Touch();
        PostedAt = DateTime.UtcNow;
    }

    protected void Touch()
    {
        UpdatedAt = DateTime.UtcNow;
        Version++;
    }

    protected virtual void ValidateTransition(TStatus currentStatus, TStatus newStatus) { }

    protected virtual void EnsureLinesAreValid()
    {
        if (_lines.Count == 0)
            throw new DomainException("Document must have at least one line.");
    }
}
