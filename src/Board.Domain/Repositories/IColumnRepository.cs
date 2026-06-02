using Board.Domain.Entities;

namespace Board.Domain.Repositories;

public interface IColumnRepository : IRepository<Column>
{
    Task<IReadOnlyList<Column>> GetByProjectIdAsync(
        Guid projectId,
        CancellationToken cancellationToken = default);

    Task<Column?> GetByProjectIdAndOrderAsync(
        Guid projectId,
        int order,
        CancellationToken cancellationToken = default);
}