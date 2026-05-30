using Board.Domain.Abstractions;
using Board.Domain.Enums;

namespace Board.Domain.Entities;

public sealed class Card : Entity
{
    public Guid ColumnId { get; set; }
    public Guid? AssigneeId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Order { get; set; }
    public DateOnly? DueDate { get; set; }
    public Priority Priority { get; set; } = Priority.None;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    public Column Column { get; set; } = null!;
    public User? Assignee { get; set; }
    public ICollection<Comment> Comments { get; set; } = [];
}
