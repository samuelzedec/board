using Board.Domain.Entities;
using Board.Domain.Enums;
using Board.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Board.Infrastructure.Persistence.Repositories;

internal sealed class CardRepository(BoardDbContext context)
    : Repository<Card>(context), ICardRepository
{
    public async Task<IReadOnlyList<Card>> GetByColumnIdAsync(
        Guid columnId,
        Guid? assigneeId = null,
        Priority? priority = null,
        CancellationToken cancellationToken = default)
    {
        var query = _table
            .AsNoTracking()
            .Where(c => c.ColumnId == columnId);

        if (assigneeId is not null)
            query = query.Where(c => c.AssigneeId == assigneeId);

        if (priority is not null)
            query = query.Where(c => c.Priority == priority);

        return await query
            .OrderBy(c => c.Order)
            .ToListAsync(cancellationToken);
    }
}