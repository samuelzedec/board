using Board.Domain.Entities;
using Board.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Board.Infrastructure.Persistence.Repositories;

internal sealed class ColumnRepository(BoardDbContext context)
    : Repository<Column>(context), IColumnRepository
{
    public async Task<IReadOnlyList<Column>> GetByProjectIdAsync(
        Guid projectId,
        CancellationToken cancellationToken = default)
        => await _table
            .AsNoTracking()
            .Where(c => c.ProjectId == projectId)
            .OrderBy(c => c.Order)
            .ToListAsync(cancellationToken);
}