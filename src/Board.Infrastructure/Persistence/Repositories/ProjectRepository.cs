using Board.Domain.Entities;
using Board.Domain.Repositories;

namespace Board.Infrastructure.Persistence.Repositories;

internal sealed class ProjectRepository(BoardDbContext context)
    : Repository<Project>(context), IProjectRepository;
