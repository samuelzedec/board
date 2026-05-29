using Board.Domain.Abstractions;
using Board.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Board.Infrastructure.Persistence.Repositories;

internal abstract class Repository<T>(BoardDbContext context)
    : IRepository<T> where T : Entity
{
    protected readonly DbSet<T> _table = context.Set<T>();

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _table.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        _table.Update(entity);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        _table.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _table.SingleOrDefaultAsync(t => t.Id == id, cancellationToken);
}
