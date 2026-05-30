using Board.Domain.Entities;
using Board.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Board.Infrastructure.Persistence.Repositories;

internal sealed class UserRepository(BoardDbContext context)
    : Repository<User>(context), IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => await _table
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Email == email, cancellationToken);
}
