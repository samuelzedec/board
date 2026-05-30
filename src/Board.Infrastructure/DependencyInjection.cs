using Board.Application.Abstractions;
using Board.Domain.Repositories;
using Board.Infrastructure.Persistence;
using Board.Infrastructure.Persistence.Repositories;
using Board.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Board.Infrastructure;

public static class DependencyInjection
{
    extension(IHost app)
    {
        public async Task ApplyMigrationsAsync()
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<BoardDbContext>();
            await context.Database.MigrateAsync();
        }
    }

    extension(IHostApplicationBuilder builder)
    {
        public void ConfigureInfrastructureLayer()
        {
            builder.ConfigureDbContext();
            builder.ConfigureHealthChecks();
            builder.ConfigureRepositories();
        }

        private void ConfigureDbContext()
        {
            var isDevelopment = builder.Environment.IsDevelopment();

            builder.Services.AddDbContext<BoardDbContext>(options => options
                .UseNpgsql(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    npgsql => npgsql
                        .MigrationsAssembly(typeof(BoardDbContext).Assembly.FullName)
                        .MigrationsHistoryTable("__ef_migrations_history", "public"))
                .EnableSensitiveDataLogging(isDevelopment)
                .EnableDetailedErrors(isDevelopment));
        }

        private void ConfigureHealthChecks()
        {
            builder.Services
                .AddHealthChecks()
                .AddNpgSql(builder.Configuration.GetConnectionString("DefaultConnection")!);
        }

        private void ConfigureRepositories()
        {
            builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
            builder.Services.AddScoped<IColumnRepository, ColumnRepository>();
            builder.Services.AddScoped<ICardRepository, CardRepository>();
            builder.Services.AddScoped<ICommentRepository, CommentRepository>();
        }
    }
}