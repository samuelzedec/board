using Board.Domain.Entities;
using Board.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Board.Infrastructure.Persistence.Repositories;

internal sealed class CardRepository(BoardDbContext context)
    : Repository<Card>(context), ICardRepository
{
    public async Task<IReadOnlyList<Card>> GetByColumnIdAsync(
        Guid columnId,
        CancellationToken cancellationToken = default)
        => await _table
            .AsNoTracking()
            .Where(c => c.ColumnId == columnId)
            .OrderBy(c => c.Order)
            .ToListAsync(cancellationToken);
}