using Board.Domain.Entities;
using Board.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Board.Infrastructure.Persistence.Repositories;

internal sealed class CommentRepository(BoardDbContext context)
    : Repository<Comment>(context), ICommentRepository
{
    public async Task<IReadOnlyList<Comment>> GetByCardIdAsync(
        Guid cardId,
        CancellationToken cancellationToken = default)
        => await _table
            .AsNoTracking()
            .Where(c => c.CardId == cardId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(cancellationToken);
}