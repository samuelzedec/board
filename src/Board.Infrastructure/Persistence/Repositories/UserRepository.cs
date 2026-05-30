using Board.Domain.Entities;
using Board.Domain.Repositories;

namespace Board.Infrastructure.Persistence.Repositories;

internal sealed class UserRepository(BoardDbContext context)
    : Repository<User>(context), IUserRepository;