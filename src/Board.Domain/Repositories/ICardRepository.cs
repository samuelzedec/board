using Board.Domain.Entities;

namespace Board.Domain.Repositories;

public interface ICardRepository : IRepository<Card>
{
    Task<IReadOnlyList<Card>> GetByColumnIdAsync(Guid columnId, CancellationToken cancellationToken = default);
}