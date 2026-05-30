using Board.Domain.Abstractions;

namespace Board.Domain.Entities;

public sealed class Project : Entity
{
    public Guid OwnerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Color { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public User Owner { get; set; } = null!;
    public ICollection<Column> Columns { get; set; } = [];
}
