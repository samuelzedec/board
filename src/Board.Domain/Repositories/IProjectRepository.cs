using Board.Domain.Entities;

namespace Board.Domain.Repositories;

public interface IProjectRepository : IRepository<Project>
{
    Task<Project?> GetByNameAndOwnerAsync(
        string name,
        Guid ownerId,
        CancellationToken cancellationToken = default);
    Task<Project[]> GetByOwnerAsync(Guid ownerId, CancellationToken cancellationToken = default);
}
