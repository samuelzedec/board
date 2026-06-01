using Board.Domain.Entities;
using Board.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Board.Infrastructure.Persistence.Repositories;

internal sealed class CommentRepository(BoardDbContext context)
    : Repository<Comment>(context), ICommentRepository
{
    public async Task<IReadOnlyList<Comment>> GetByCardIdAsync(
        Guid cardId,
        Guid? authorId = null,
        string? search = null,
        CancellationToken cancellationToken = default)
    {
        var query = _table
            .AsNoTracking()
            .Where(c => c.CardId == cardId);

        if (authorId is not null)
            query = query.Where(c => c.AuthorId == authorId);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(c => EF.Functions.ILike(c.Content, $"%{search}%"));

        return await query
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}