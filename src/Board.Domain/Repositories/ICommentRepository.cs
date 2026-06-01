using Board.Domain.Entities;

namespace Board.Domain.Repositories;

public interface ICommentRepository : IRepository<Comment>
{
    Task<IReadOnlyList<Comment>> GetByCardIdAsync(Guid cardId, CancellationToken cancellationToken = default);
}