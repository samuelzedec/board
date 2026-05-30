using Board.Domain.Abstractions;

namespace Board.Domain.Entities;

public sealed class Comment : Entity
{
    public Guid CardId { get; set; }
    public Guid AuthorId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTimeOffset? EditedAt { get; set; }

    public Card Card { get; set; } = null!;
    public User Author { get; set; } = null!;
}
