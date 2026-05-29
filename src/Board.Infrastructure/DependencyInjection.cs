using Board.Infrastructure.Persistence;
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
        }

        private void ConfigureDbContext()
        {
            var isDevelopment = builder.Environment.IsDevelopment();

            builder.Services.AddDbContext<BoardDbContext>(options => options
                .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
                .EnableSensitiveDataLogging(isDevelopment)
                .EnableDetailedErrors(isDevelopment));
        }

        private void ConfigureHealthChecks()
        {
            builder.Services
                .AddHealthChecks()
                .AddNpgSql(builder.Configuration.GetConnectionString("DefaultConnection")!);
        }
    }
}