using Board.Domain.Entities;
using Board.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Board.Infrastructure.Persistence.Repositories;

internal sealed class ProjectRepository(BoardDbContext context)
    : Repository<Project>(context), IProjectRepository
{
    
    public Task<Project?> GetByNameAndOwnerAsync(
        string name,
        Guid ownerId,
        CancellationToken cancellationToken = default)
        => _table
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.Name == name && p.OwnerId == ownerId, cancellationToken);
    
    public Task<Project[]> GetByOwnerAsync(Guid ownerId, CancellationToken cancellationToken = default)
        => _table
            .Where(p => p.OwnerId == ownerId)
            .ToArrayAsync(cancellationToken);
}
