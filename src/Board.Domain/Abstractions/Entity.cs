namespace Board.Domain.Abstractions;

public abstract class Entity
{
    public Guid Id { get; } = Guid.CreateVersion7();
    public DateTimeOffset CreatedAt { get; } = DateTimeOffset.UtcNow;
}