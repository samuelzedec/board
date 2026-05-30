using Board.Domain.Abstractions;

namespace Board.Domain.Entities;

public sealed class Column : Entity
{
    public Guid ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Order { get; set; }

    public Project Project { get; set; } = null!;
    public ICollection<Card> Cards { get; set; } = [];
}
