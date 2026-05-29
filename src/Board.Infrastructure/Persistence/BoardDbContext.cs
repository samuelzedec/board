using Microsoft.EntityFrameworkCore;

namespace Board.Infrastructure.Persistence;

internal sealed class BoardDbContext(
    DbContextOptions<BoardDbContext> options)
    : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(typeof(BoardDbContext).Assembly);
}