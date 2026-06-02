using Board.Domain.Entities;
using Board.Domain.Enums;

namespace Board.Domain.Repositories;

public interface ICardRepository : IRepository<Card>
{
    Task<IReadOnlyList<Card>> GetByColumnIdAsync(
        Guid columnId,
        Guid? assigneeId = null,
        Priority? priority = null,
        CancellationToken cancellationToken = default);
}