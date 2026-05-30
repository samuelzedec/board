using Board.Domain.Abstractions;

namespace Board.Domain.Entities;

public sealed class User : Entity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTimeOffset? LastAccessAt { get; set; }

    public ICollection<Project> Projects { get; set; } = [];
    public ICollection<Card> AssignedCards { get; set; } = [];
    public ICollection<Comment> Comments { get; set; } = [];
}
