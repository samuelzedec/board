using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Board.Infrastructure.Persistence.Factories;

internal sealed class BoardDbContextFactory
    : IDesignTimeDbContextFactory<BoardDbContext>
{
    private const string UserSecretsId = "a739a8dc-0f56-4d1a-8a43-ed86448e01de";

    public BoardDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets(UserSecretsId)
            .Build();

        var dbContextOptionsBuilder = new DbContextOptionsBuilder<BoardDbContext>()
            .UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                npgsql => npgsql.MigrationsAssembly(typeof(BoardDbContext).Assembly.FullName)
            );

        return new BoardDbContext(dbContextOptionsBuilder.Options);
    }
}