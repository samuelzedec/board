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
            .Where(column => column.ProjectId == projectId)
            .OrderBy(column => column.Order)
            .ToListAsync(cancellationToken);

    public async Task<Column?> GetByProjectIdAndOrderAsync(
        Guid projectId,
        int order,
        CancellationToken cancellationToken = default)
        => await _table
            .AsNoTracking()
            .SingleOrDefaultAsync(
                column => column.ProjectId == projectId && column.Order == order,
                cancellationToken);
}